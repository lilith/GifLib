/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang 版权所有。 
//  
// 文件名：GifDecoder.cs    使用请保留版权信息  
// 文件功能描述： 更多信息请访问 http://jillzhang.cnblogs.com
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
    /// <summary>
    /// GIF图像文件的解码器
    /// </summary>
    public class GifDecoder : IDisposable
    {
        #region 私有变量
        FileStream fs;
        StreamHelper streamHelper;
        int frameCount = 0;
        int bgIndex;
        Bitmap lastImage;
        List<GraphicEx> graphics = new List<GraphicEx>();
        int lastDisposal;
        int[] act;
        Hashtable table = new Hashtable();
        #endregion

        #region 背景图片的长度
        short width = 0;
        /// <summary>
        /// 背景图片的长度
        /// </summary>
        public short Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        #endregion

        #region 背景图片的高度
        short height = 0;
        /// <summary>
        /// 背景图片的高度
        /// </summary>
        public short Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        #endregion

        #region gif文件头，可能情况有两种:GIF89a或者GIF87a
        string header = "";
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        #endregion

        #region 全局颜色列表
        private byte[] gct;
        /// <summary>
        /// 全局颜色列表
        /// </summary>
        public byte[] GlobalColorTable
        {
            get
            {
                return gct;
            }
            set
            {
                gct = value;
            }
        }
        #endregion

        #region Gif的调色板
        /// <summary>
        /// Gif的调色板
        /// </summary>
        public int[] Palette
        {
            get
            {
                return act;
            }
        }
        #endregion

        #region 全局颜色的索引表
        /// <summary>
        /// 全局颜色的索引表
        /// </summary>
        public Hashtable GlobalColorIndexedTable
        {
            get { return table; }
        }
        #endregion

        #region 注释扩展块集合
        List<CommentEx> comments = new List<CommentEx>();
        /// <summary>
        /// 注释块集合
        /// </summary>
        public List<CommentEx> CommentExtensions
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
            }
        }
        #endregion

        #region 应用程序扩展块集合
        List<ApplicationEx> applictions = new List<ApplicationEx>();
        /// <summary>
        /// 应用程序扩展块集合
        /// </summary>
        public List<ApplicationEx> ApplictionExtensions
        {
            get
            {
                return applictions;
            }
            set
            {
                applictions = value;
            }
        }
        #endregion

        #region 图形文本扩展集合
        List<PlainTextEx> texts = new List<PlainTextEx>();
        /// <summary>
        /// 图形文本扩展集合
        /// </summary>
        public List<PlainTextEx> PlainTextEntensions
        {
            get
            {
                return texts;
            }
            set
            {
                texts = value;
            }
        }
        #endregion

        #region 逻辑屏幕描述
        LogicalScreenDescriptor lcd;
        /// <summary>
        /// 逻辑屏幕描述
        /// </summary>
        public LogicalScreenDescriptor LogicalScreenDescriptor
        {
            get
            {
                return lcd;
            }
            set
            {
                lcd = value;
            }
        }
        #endregion

        #region 解析出来的帧集合
        List<GifFrame> frames = new List<GifFrame>();
        /// <summary>
        /// 解析出来的帧集合
        /// </summary>
        public List<GifFrame> Frames
        {
            get
            {
                return frames;
            }
            set
            {
                frames = value;
            }
        }
        #endregion

        #region 私有方法，用于类内部使用
        public int[] GetColotTable(byte[] table)
        {
            int[] tab = new int[table.Length / 3];
            int i = 0;
            int j = 0;
            while (i < table.Length)
            {
                int r = table[i++];
                int g = table[i++];
                int b = table[i++];
                int a = 255;
                tab[j++] = (int)(a << 24 | (r << 16) | (g << 8) | b);
            }
            return tab;
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
        void ReadHeader()
        {
            header = streamHelper.ReadString(6);
        }

        void ReadImage()
        {
            ImageDescriptor imgDes = new ImageDescriptor(fs);
            GifFrame frame = new GifFrame();
            frame.ImageDescriptor = imgDes;
            frame.LocalColorTable = gct;
            if (imgDes.LctFlag)
            {
                frame.LocalColorTable = streamHelper.ReadByte(imgDes.LctSize*3);
            }
            LZWDecoder lzwDecoder = new LZWDecoder(fs);
            int dataSize = streamHelper.Read();
            frame.ColorDepth = dataSize;        
            byte[] piexel = lzwDecoder.DecodeImageData(imgDes.Width, imgDes.Height, dataSize);
            frame.IndexedPixel = piexel;
            int blockSize = streamHelper.Read();
            DataStruct data = new DataStruct(blockSize, fs);
            act = GetColotTable(frame.LocalColorTable);
            GraphicEx graphicEx = graphics[frameCount];
            frame.GraphicExtension = graphicEx;
            if (graphicEx != null)
            {
                if (graphicEx.TransparencyFlag)
                {
                    act[graphicEx.TranIndex] = 0;
                }
            }       
            Bitmap img = GetImageFromPixel(piexel, act, imgDes.InterlaceFlag, imgDes.XOffSet, imgDes.YOffSet, graphicEx.TransparencyFlag, imgDes.Width, imgDes.Height);
            //img.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, frameCount + "_0.png"));
            lastDisposal = graphicEx.DisposalMethod;
            frame.Image = img;           
            frameCount++;
            frames.Add(frame);
        }
        Bitmap GetImageFromPixel(byte[] pixel, int[] colorTable, bool interlactFlag, short xOffSet, short yOffSet, bool transparencyFlag, int iw, int ih)
        {
            Bitmap img = new Bitmap(iw, ih);
            Rectangle rect = new Rectangle(xOffSet, yOffSet, iw, ih);

            BitmapData bmpData = img.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, img.PixelFormat);
            unsafe
            {
                int* p = (int*)bmpData.Scan0.ToPointer();            
                int* tempPointer = p;
                int offSet = 0;
                if (interlactFlag)
                {
                    int i = 0;
                    int pass = 0;//当前通道            
                    while (pass < 4)
                    {
                        //总共有4个通道
                        if (pass == 1)
                        {
                            p = tempPointer;
                            p += (4 * iw );
                            offSet += 4 * iw;
                        }
                        else if (pass == 2)
                        {
                            p = tempPointer;
                            p += (2 * iw );
                            offSet += 2 * iw;
                        }
                        else if (pass == 3)
                        {
                            p = tempPointer;
                            p += (1 * iw);
                            offSet += 1 * iw;
                        }
                        int rate = 2;
                        if (pass == 0 | pass == 1)
                        {
                            rate = 8;
                        }
                        else if (pass == 2)
                        {
                            rate = 4;
                        }
                        while (i < pixel.Length)
                        {                         
                            *p++ = colorTable[pixel[i++]];                          
                            offSet++;
                            if (i % (iw) == 0)
                            {
                                p += (iw * (rate - 1));
                                offSet += (iw * (rate - 1));
                                if ( offSet  >= pixel.Length)
                                {
                                    pass++;
                                    offSet = 0;
                                    break;
                                }                               
                            }                        
                        }
                    }
                }
                else
                {
                    int i = 0;
                    for (i = 0; i < pixel.Length; )
                    {                        
                        *p++ = colorTable[pixel[i++]];   
                    }
                }
            }
            img.UnlockBits(bmpData);         
            lastImage = img;
            return img;
        }
        #endregion

        #region 对gif图像文件进行解码
        /// <summary>
        /// 对gif图像文件进行解码
        /// </summary>
        /// <param name="gifPath">gif文件路径</param>
        public void Decode(string gifPath)
        {
            fs = new FileStream(gifPath, FileMode.Open);
            streamHelper = new StreamHelper(fs);
            ReadHeader();
            lcd = new LogicalScreenDescriptor(fs);
            width = lcd.Width;
            height = lcd.Height;
            int sortFlag = lcd.SortFlag;
            int gctSize = lcd.GlobalColorTableSize;
            bgIndex = lcd.BgColorIndex;
            int pixelAspect = lcd.PixcelAspect;
            if (lcd.GlobalColorTableFlag)
            {
                gct = streamHelper.ReadByte(gctSize * 3);
            }
            int nextFlag = streamHelper.Read();
            while (nextFlag != 0)
            {
                if (nextFlag == 0x2C)
                {
                    ReadImage();
                }
                else if (nextFlag == 0x21)
                {
                    int gcl = streamHelper.Read();
                    switch (gcl)
                    {
                        case 0xF9:
                            {
                                GraphicEx graphicEx = new GraphicEx(fs);
                                graphics.Add(graphicEx);
                                break;
                            }
                        case 0xFE:
                            {
                                CommentEx comment = new CommentEx(fs);
                                comments.Add(comment);
                                break;
                            }
                        case 0xFF:
                            {
                                ApplicationEx applicationEx = new ApplicationEx(fs);
                                applictions.Add(applicationEx);
                                break;
                            }
                        case 0x01:
                            {
                                PlainTextEx textEx = new PlainTextEx(fs);
                                texts.Add(textEx);
                                break;
                            }
                    }
                }
                else if (nextFlag == 0x3B)
                {
                    //到了文件尾
                    break;
                }
                nextFlag = streamHelper.Read();
            }
            fs.Close();
        }
        #endregion

        public void Dispose()
        {
            foreach (GifFrame f in frames)
            {
                if (f.Image != null)
                {
                    f.Image.Dispose();
                }
            }
            if (fs != null)
            {
                fs.Close();
            }
        }
    }
}
