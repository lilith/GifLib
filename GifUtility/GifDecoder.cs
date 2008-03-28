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
    /// GIFͼ���ļ��Ľ�����
    /// </summary>
    public class GifDecoder : IDisposable
    {
        #region ˽�б���
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

        #region ����ͼƬ�ĳ���
        short width = 0;
        /// <summary>
        /// ����ͼƬ�ĳ���
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

        #region ����ͼƬ�ĸ߶�
        short height = 0;
        /// <summary>
        /// ����ͼƬ�ĸ߶�
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

        #region gif�ļ�ͷ���������������:GIF89a����GIF87a
        string header = "";
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        #endregion

        #region ȫ����ɫ�б�
        private byte[] gct;
        /// <summary>
        /// ȫ����ɫ�б�
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

        #region Gif�ĵ�ɫ��
        /// <summary>
        /// Gif�ĵ�ɫ��
        /// </summary>
        public int[] Palette
        {
            get
            {
                return act;
            }
        }
        #endregion

        #region ȫ����ɫ��������
        /// <summary>
        /// ȫ����ɫ��������
        /// </summary>
        public Hashtable GlobalColorIndexedTable
        {
            get { return table; }
        }
        #endregion

        #region ע����չ�鼯��
        List<CommentEx> comments = new List<CommentEx>();
        /// <summary>
        /// ע�Ϳ鼯��
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

        #region Ӧ�ó�����չ�鼯��
        List<ApplicationEx> applictions = new List<ApplicationEx>();
        /// <summary>
        /// Ӧ�ó�����չ�鼯��
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

        #region ͼ���ı���չ����
        List<PlainTextEx> texts = new List<PlainTextEx>();
        /// <summary>
        /// ͼ���ı���չ����
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

        #region �߼���Ļ����
        LogicalScreenDescriptor lcd;
        /// <summary>
        /// �߼���Ļ����
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

        #region ����������֡����
        List<GifFrame> frames = new List<GifFrame>();
        /// <summary>
        /// ����������֡����
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

        #region ˽�з������������ڲ�ʹ��
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

        #region ��gifͼ���ļ����н���
        /// <summary>
        /// ��gifͼ���ļ����н���
        /// </summary>
        /// <param name="gifPath">gif�ļ�·��</param>
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
                    //�����ļ�β
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
