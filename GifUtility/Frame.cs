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

using System;
using System.Collections.Generic;
using System.Text;

namespace Jillzhang.GifUtility
{
    public class GifFrame
    {
        private ImageDescriptor _imgDes;

        public ImageDescriptor ImageDescriptor
        {
            get { return _imgDes; }
            set { _imgDes = value; }
        }

        private System.Drawing.Bitmap _img;

        public System.Drawing.Bitmap Image
        {
            get { return _img; }
            set { _img = value; }
        }
        private int _colorSize = 3;
        public int ColorDepth
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

        private byte[] _lct;

        public byte[] LocalColorTable
        {
            get { return _lct; }
            set { _lct = value; }
        }

        private GraphicEx _graphicEx;


        public GraphicEx GraphicExtension
        {
            get { return _graphicEx; }
            set { _graphicEx = value; }
        }


        public short Delay
        {
            get { return _graphicEx.Delay; }
            set { _graphicEx.Delay = value; }
        }
        private byte[] _buffer;
        public byte[] IndexedPixel
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
    }
}
