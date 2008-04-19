#region File License
/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang ��Ȩ���С� 
//  
// �ļ�����Frame.cs
// �ļ�����������
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

namespace Jillzhang.GifUtility
{
    #region ��GifFrame
    /// <summary>
    /// Gif�ļ��п��԰������ͼ��ÿ��ͼ�����ͼ���һЩ�������������֡:GifFrame
    /// </summary>
    internal class GifFrame
    {
        #region private fields
        private ImageDescriptor _imgDes;
        private System.Drawing.Bitmap _img;
        private int _colorSize = 3;
        private byte[] _lct;
        private GraphicEx _graphicEx;
        private byte[] _buffer;        
        #endregion

        #region internal property
        /// <summary>
        /// ����ı���ɫ
        /// </summary>
        public Color32 BgColor
        {
            get
            {
                Color32[] act = PaletteHelper.GetColor32s(LocalColorTable);
                return act[GraphicExtension.TranIndex];
            }            
        }
        /// <summary>
        /// ͼ���ʶ��(Image Descriptor)
        /// һ��GIF�ļ��ڿ��԰������ͼ��
        /// һ��ͼ�����֮�����������һ��ͼ��ı�ʶ����
        /// ͼ���ʶ����0x2C(',')�ַ���ʼ��
        /// �������������ͼ������ʣ�����ͼ��������߼���Ļ�߽��ƫ������
        /// ͼ���С�Լ����޾ֲ���ɫ�б����ɫ�б��С
        /// </summary>
        internal ImageDescriptor ImageDescriptor
        {
            get { return _imgDes; }
            set { _imgDes = value; }
        }
               
        /// <summary>
        /// Gif�ĵ�ɫ��
        /// </summary>
        internal Color32[] Palette
        {
            get
            {
                Color32[] act = PaletteHelper.GetColor32s(LocalColorTable);
                if (GraphicExtension != null && GraphicExtension.TransparencyFlag)
                {
                    act[GraphicExtension.TranIndex] = new Color32(0);
                }
                return act;
            }
        }

        /// <summary>
        /// ͼ��
        /// </summary>
        internal System.Drawing.Bitmap Image
        {
            get { return _img; }
            set { _img = value; }
        }
        
        /// <summary>
        /// ����λ��С
        /// </summary>
        internal int ColorDepth
        {
            get
            {
                return _colorSize;
            }
            set
            {
                _colorSize = value;
            }
        }
        
        /// <summary>
        /// �ֲ���ɫ�б�(Local Color Table)
        /// �������ľֲ���ɫ�б��־��λ�Ļ�������Ҫ�����������ͼ���ʶ��֮��
        /// ����һ���ֲ���ɫ�б��Թ�����������ͼ��ʹ�ã�ע��ʹ��ǰӦ�߱���ԭ������ɫ�б�
        /// ʹ�ý���֮��ظ�ԭ�������ȫ����ɫ�б����һ��GIF�ļ���û���ṩȫ����ɫ�б�
        /// Ҳû���ṩ�ֲ���ɫ�б������Լ�����һ����ɫ�б���ʹ��ϵͳ����ɫ�б�
        /// RGBRGB......
        /// </summary>
        internal byte[] LocalColorTable
        {
            get { return _lct; }
            set { _lct = value; }
        }
        
        /// <summary>
        /// ͼ�ο�����չ(Graphic Control Extension)��һ�����ǿ�ѡ�ģ���Ҫ89a�汾����
        /// ���Է���һ��ͼ���(����ͼ���ʶ�����ֲ���ɫ�б��ͼ������)���ı���չ���ǰ�棬
        /// �������Ƹ���������ĵ�һ��ͼ�󣨻��ı�������Ⱦ(Render)��ʽ
        /// </summary>
        internal GraphicEx GraphicExtension
        {
            get { return _graphicEx; }
            set { _graphicEx = value; }
        }

        /// <summary>
        /// �ӳ�-����һ֮֡���ʱ����
        /// </summary>
        internal short Delay
        {
            get { return _graphicEx.Delay; }
            set { _graphicEx.Delay = value; }
        }
       
        /// <summary>
        /// ����Ǿ���LZWѹ���㷨���������
        /// </summary>
        internal byte[] IndexedPixel
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
        #endregion
    }
    #endregion
}
