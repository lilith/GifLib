#region Copyright & License
/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang ��Ȩ���С� 
//  
// �ļ�����GifDecoder.cs    ʹ���뱣����Ȩ��Ϣ  
// �ļ����������� ������Ϣ����� http://jillzhang.cnblogs.com
// 
// ������ʶ��jillzhang 
// �޸ı�ʶ��
// �޸�������
//
// �޸ı�ʶ��
// �޸�������
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
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace Jillzhang.GifUtility
{
    #region ��GifDecoder-gif�ļ�������
    /// <summary>
    /// GIFͼ���ļ��Ľ�����
    /// </summary>
    internal class GifDecoder
    {      
        #region ˽�з������������ڲ�ʹ��     
        static void ReadImage(StreamHelper streamHelper, Stream fs, GifImage gifImage, List<GraphicEx> graphics, int frameCount)
        {
            ImageDescriptor imgDes = streamHelper.GetImageDescriptor(fs);
            GifFrame frame = new GifFrame();
            frame.ImageDescriptor = imgDes;
            frame.LocalColorTable = gifImage.GlobalColorTable;
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
            GraphicEx graphicEx = graphics[frameCount];
            frame.GraphicExtension = graphicEx;        
            Bitmap img = GetImageFromPixel(piexel, frame.Palette, imgDes.InterlaceFlag, imgDes.Width, imgDes.Height);
            frame.Image = img;          
            gifImage.Frames.Add(frame);
        }
        static Bitmap GetImageFromPixel(byte[] pixel, Color32[] colorTable, bool interlactFlag, int iw, int ih)
        {
            Bitmap img = new Bitmap(iw, ih);           
            BitmapData bmpData = img.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, img.PixelFormat);
            unsafe
            {
                Color32* p = (Color32*)bmpData.Scan0.ToPointer();
                Color32* tempPointer = p;
                int offSet = 0;
                if (interlactFlag)
                {
                    #region ��֯�洢ģʽ
                    int i = 0;
                    int pass = 0;//��ǰͨ��            
                    while (pass < 4)
                    {
                        //�ܹ���4��ͨ��
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
                    #endregion
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
            return img;
        }
        #endregion

        #region ��gifͼ���ļ����н���
        /// <summary>
        /// ��gifͼ���ļ����н���
        /// </summary>
        /// <param name="gifPath">gif�ļ�·��</param>
        internal static GifImage Decode(string gifPath)
        {
            FileStream fs=null;
            StreamHelper streamHelper=null;
            GifImage gifImage = new GifImage();
            List<GraphicEx> graphics = new List<GraphicEx>();        
            int frameCount = 0;
            try
            {
                fs = new FileStream(gifPath,FileMode.Open);
                streamHelper = new StreamHelper(fs);
                //��ȡ�ļ�ͷ
                gifImage.Header = streamHelper.ReadString(6);
                //��ȡ�߼���Ļ��ʾ��
                gifImage.LogicalScreenDescriptor = streamHelper.GetLCD(fs);  
                if (gifImage.LogicalScreenDescriptor.GlobalColorTableFlag)
                {
                    //��ȡȫ����ɫ�б�
                    gifImage.GlobalColorTable = streamHelper.ReadByte(gifImage.LogicalScreenDescriptor.GlobalColorTableSize * 3);
                }
                int nextFlag = streamHelper.Read();
                while (nextFlag != 0)
                {
                    if (nextFlag == GifExtensions.ImageLabel)
                    {
                        ReadImage(streamHelper, fs, gifImage, graphics, frameCount);
                        frameCount++;
                    }
                    else if (nextFlag == GifExtensions.ExtensionIntroducer)
                    {
                        int gcl = streamHelper.Read();
                        switch (gcl)
                        {
                            case GifExtensions.GraphicControlLabel:
                                {
                                    GraphicEx graphicEx = streamHelper.GetGraphicControlExtension(fs);
                                    graphics.Add(graphicEx);
                                    break;
                                }
                            case GifExtensions.CommentLabel:
                                {
                                    CommentEx comment = streamHelper.GetCommentEx(fs);
                                    gifImage.CommentExtensions.Add(comment);
                                    break;
                                }
                            case GifExtensions.ApplicationExtensionLabel:
                                {
                                    ApplicationEx applicationEx = streamHelper.GetApplicationEx(fs);
                                    gifImage.ApplictionExtensions.Add(applicationEx);
                                    break;
                                }
                            case GifExtensions.PlainTextLabel:
                                {
                                    PlainTextEx textEx = streamHelper.GetPlainTextEx(fs);
                                    gifImage.PlainTextEntensions.Add(textEx);
                                    break;
                                }
                        }
                    }
                    else if (nextFlag == GifExtensions.EndIntroducer)
                    {
                        //�����ļ�β
                        break;
                    }
                    nextFlag = streamHelper.Read();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
            return gifImage;
        }     
        #endregion    
    }
    #endregion
}
