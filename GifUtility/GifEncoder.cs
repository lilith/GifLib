/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang 版权所有。 
//  
// 文件名：GifEncoder.cs
// 文件功能描述：
// 
// 创建标识：jillzhang 
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
/*-------------------------New BSD License ------------------
 Copyright (c) 2008, jillzhang
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of jillzhang nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace Jillzhang.GifUtility
{
    public class GifEncoder : IDisposable
    {
        FileStream fs;
        StreamHelper streamHelper;
        byte[] gct;
        short width;
        short heigt = 0;
        Hashtable table = new Hashtable();

        public GifEncoder(string gifPath)
        {
            fs = new FileStream(gifPath, FileMode.Create);
            streamHelper = new StreamHelper(fs);
        }

        #region 写文件头
        /// <summary>
        /// 写文件头
        /// </summary>
        /// <param name="header">文件头</param>
        public void WriteHeader(string header)
        {
            streamHelper.WriteString(header);
        }
        #endregion

        #region 写逻辑屏幕标识符
        /// <summary>
        /// 写逻辑屏幕标识符
        /// </summary>
        /// <param name="lsd"></param>
        public void WriteLSD(LogicalScreenDescriptor lsd)
        {
            streamHelper.WriteBytes(lsd.GetBuffer());
            this.width = lsd.Width;
            this.heigt = lsd.Height;
        }
        #endregion

        #region 写全局颜色表
        /// <summary>
        /// 写全局颜色表
        /// </summary>
        /// <param name="buffer">全局颜色表</param>
        public void SetGlobalColorTable(byte[] buffer)
        {
            streamHelper.WriteBytes(buffer);
            gct = buffer;
        }
        #endregion

        #region 写入注释扩展集合
        /// <summary>
        /// 写入注释扩展集合
        /// </summary>
        /// <param name="comments">注释扩展集合</param>
        public void SetCommentExtensions(List<CommentEx> comments)
        {
            foreach (CommentEx ce in comments)
            {
                streamHelper.WriteBytes(ce.GetBuffer());
            }
        }
        #endregion

        #region 写入应用程序展集合
        /// <summary>
        /// 写入应用程序展集合
        /// </summary>
        /// <param name="comments">写入应用程序展集合</param>
        public void SetApplicationExtensions(List<ApplicationEx> applications)
        {
            foreach (ApplicationEx ap in applications)
            {
                streamHelper.WriteBytes(ap.GetBuffer());
            }
        }
        #endregion


        public void SetFrames(List<GifFrame> frames)
        {
            foreach (GifFrame f in frames)
            {
                List<byte> list = new List<byte>();
                if (f.GraphicExtension != null)
                {
                    list.AddRange(f.GraphicExtension.GetBuffer());
                }
                f.ImageDescriptor.SortFlag = false;
                f.ImageDescriptor.InterlaceFlag = false;
                list.AddRange(f.ImageDescriptor.GetBuffer());
                if (f.ImageDescriptor.LctFlag)
                {
                    list.AddRange(f.LocalColorTable);
                }
                streamHelper.WriteBytes(list.ToArray());
                int transIndex = -1;
                
                if (f.GraphicExtension.TransparencyFlag)
                {
                    transIndex = f.GraphicExtension.TranIndex;
                }             

                byte[] indexedPixel = GetImagePixels(f.Image, f.LocalColorTable,transIndex);               

                LZWEncoder lzw = new LZWEncoder(indexedPixel, (byte)f.ColorDepth);
                lzw.Encode(fs);

                streamHelper.WriteBytes(new byte[] { 0 });
            }
            streamHelper.WriteBytes(new byte[] { 0x3B });
        }

        public void Encode(GifDecoder gifDecoder)
        {
            using (this)
            {
                this.WriteHeader(gifDecoder.Header);               
                this.WriteLSD(gifDecoder.LogicalScreenDescriptor);
                if (gifDecoder.LogicalScreenDescriptor.GlobalColorTableFlag)
                {
                    this.SetGlobalColorTable(gifDecoder.GlobalColorTable);
                }
                this.SetApplicationExtensions(gifDecoder.ApplictionExtensions);
                this.SetCommentExtensions(gifDecoder.CommentExtensions);
                this.SetFrames(gifDecoder.Frames);
                gifDecoder.Dispose();
            }
        }

        Hashtable GetColotTable(byte[] table, int transIndex)
        {
            int[] tab = new int[table.Length / 3];
            Hashtable hashTab = new Hashtable();
            int i = 0;
            int j = 0;
            while (i < table.Length)
            {
                int color = 0;
                if (j == transIndex)
                {
                    i += 3;
                }
                else
                {
                    int r = table[i++];
                    int g = table[i++];
                    int b = table[i++];
                    int a = 255;
                    color = (int)(a << 24 | (r << 16) | (g << 8) | b);
                }
                if (!hashTab.ContainsKey(color))
                {
                    hashTab.Add(color, j);
                }
                tab[j++] = color;
            }
            return hashTab;
        }
        /**
         * Extracts image pixels into byte array "pixels"
         */
        protected byte[] GetImagePixels(Bitmap image, byte[] colorTab, int transIndex)
        {
            int iw = image.Width;
            int ih = image.Height;

            byte[] pixels = new byte[iw * ih];
            Hashtable table = GetColotTable(colorTab, transIndex);
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadOnly, image.PixelFormat);
            unsafe
            {
                int* p = (int*)bmpData.Scan0.ToPointer();
                for (int i = 0; i < iw * ih; i++)
                {
                    int color = p[i];                 
                    byte index = Convert.ToByte(table[color]);
                    pixels[i] = index;
                }
            }
            image.UnlockBits(bmpData);
            return pixels;
        }

        #region 释放资源
        public void Dispose()
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
        #endregion
    }
}
