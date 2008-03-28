/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang ��Ȩ���С� 
//  
// �ļ�����LogicalScreenDescriptor.cs
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
using System.IO;

namespace Jillzhang.GifUtility
{
    /// <summary>
    /// �߼���Ļ��ʶ��(Logical Screen Descriptor)
    /// </summary>
    public class LogicalScreenDescriptor
    {
        private short _width;
        /// <summary>
        /// �߼���Ļ��� ������������GIFͼ��Ŀ��
        /// </summary>
        public short Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private short _height;

        /// <summary>
        /// �߼���Ļ�߶� ������������GIFͼ��ĸ߶�
        /// </summary>
        public short Height
        {
            get { return _height; }
            set { _height = value; }
        }


        private byte _packed;

        public byte Packed
        {
            get { return _packed; }
            set { _packed = value; }
        }

        private byte _bgIndex;
        /// <summary>
        /// ����ɫ,������ɫ(��ȫ����ɫ�б��е����������û��ȫ����ɫ�б���ֵû������)
        /// </summary>
        public byte BgColorIndex
        {
            get { return _bgIndex; }
            set { _bgIndex = value; }
        }


        private byte _pixelAspect;
        /// <summary>
        /// ���ؿ�߱�,���ؿ�߱�(Pixel Aspect Radio)
        /// </summary>
        public byte PixcelAspect
        {
            get { return _pixelAspect; }
            set { _pixelAspect = value; }
        }
        private bool _globalColorTableFlag;
        /// <summary>
        /// m - ȫ����ɫ�б��־(Global Color Table Flag)������λʱ��ʾ��ȫ����ɫ�б�pixelֵ������.
        /// </summary>
        public bool GlobalColorTableFlag
        {
            get { return _globalColorTableFlag; }
            set { _globalColorTableFlag = value; }
        }

        private byte _colorResoluTion;
        /// <summary>
        /// cr - ��ɫ���(Color ResoluTion)��cr+1ȷ��ͼ�����ɫ���.
        /// </summary>
        public byte ColorResoluTion
        {
            get { return _colorResoluTion; }
            set { _colorResoluTion = value; }
        }

        private int _sortFlag;

        /// <summary>
        /// s - �����־(Sort Flag)�������λ��ʾȫ����ɫ�б��������.
        /// </summary>
        public int SortFlag
        {
            get { return _sortFlag; }
            set { _sortFlag = value; }
        }

        private int _globalColorTableSize;
        /// <summary>
        /// ȫ����ɫ�б��С��pixel+1ȷ����ɫ�б����������2��pixel+1�η���.
        /// </summary>
        public int GlobalColorTableSize
        {
            get { return _globalColorTableSize; }
            set { _globalColorTableSize = value; }
        }

        public LogicalScreenDescriptor()
        {

        }

        public LogicalScreenDescriptor(Stream stream)
        {
            StreamHelper streamHelper = new StreamHelper(stream);
            _width = streamHelper.ReadShort();
            _height = streamHelper.ReadShort();
            _packed = (byte)streamHelper.Read();
            _globalColorTableFlag =((_packed & 0x80) >> 7)==1;
            _colorResoluTion = (byte)((_packed & 0x60) >> 5);
            _sortFlag = (byte)(_packed & 0x10) >> 4;
            _globalColorTableSize = 2 << (_packed & 0x07);
            _bgIndex = (byte)streamHelper.Read();
            _pixelAspect = (byte)streamHelper.Read();
        }

        public byte[] GetBuffer()
        {
            byte[] buffer = new byte[7];
            Array.Copy(BitConverter.GetBytes(_width), 0, buffer, 0, 2);
            Array.Copy(BitConverter.GetBytes(_height), 0, buffer, 2, 2);
            int m = 0;
            if (_globalColorTableFlag)
            {
                m = 1;
            }
            byte pixel = (byte)(Math.Log(_globalColorTableSize,2) - 1);
            _packed = (byte)(pixel | (_sortFlag << 4)|(_colorResoluTion<<5)|(m<<7));
            buffer[4] = _packed;
            buffer[5] = _bgIndex;
            buffer[6] = _pixelAspect;
            return buffer;
        }
    }
}
