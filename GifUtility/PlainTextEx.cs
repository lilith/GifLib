/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang ��Ȩ���С� 
//  
// �ļ�����PlainTextEx.cs   ʹ���뱣����Ȩ��Ϣ  
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

namespace Jillzhang.GifUtility
{
    /// <summary>
    /// ͼ���ı���չ(Plain Text Extension)
    /// </summary>
    public class PlainTextEx : ExData
    {
        private static readonly byte _label = 0x01;
        private static readonly byte _blockSize = 12;

        /// <summary>
        /// Plain Text Label - ��ʶ����һ��ͼ���ı���չ�飬�̶�ֵ0x01
        /// </summary>
        public byte Lable
        {
            get { return _label; }
        }


        /// <summary>
        /// Block Size - ���С���̶�ֵ12
        /// </summary>
        public int BlockSize
        {
            get { return _blockSize; }
        }


        private short _xOffSet;

        /// <summary>
        /// Text Glid Left Posotion - ����ֵ���ı������߼���Ļ����߽����
        /// </summary>
        public short XOffSet
        {
            get { return _xOffSet; }
            set { _xOffSet = value; }
        }

        private short _yOffSet;
        /// <summary>
        /// Text Glid Top Posotion - ����ֵ���ı������߼���Ļ���ϱ߽����
        /// </summary>
        public short YOffSet
        {
            get { return _yOffSet; }
            set { _yOffSet = value; }
        }

        private short _width;
        /// <summary>
        /// �ı���߶� Text Glid Width -����ֵ
        /// </summary>
        public short Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private short _hight;

        /// <summary>
        /// �ı���߶� Text Glid Height - ����ֵ
        /// </summary>
        public short Height
        {
            get { return _hight; }
            set { _hight = value; }
        }

        private byte _characterCellWidth;
        /// <summary>
        /// �ַ���Ԫ���� Character Cell Width - ����ֵ��������Ԫ����
        /// </summary>
        public byte CharacterCellWidth
        {
            get { return _characterCellWidth; }
            set { _characterCellWidth = value; }
        }

        private byte _characterCellHeight;
        /// <summary>
        /// �ַ���Ԫ��߶� Character Cell Height- ����ֵ��������Ԫ��߶�
        /// </summary>
        public byte CharacterCellHeight
        {
            get { return _characterCellHeight; }
            set { _characterCellHeight = value; }
        }

        private byte _textForegroundColorIndex;
        /// <summary>
        /// �ı�ǰ��ɫ���� Text Foreground Color Index - ǰ��ɫ��ȫ����ɫ�б��е�����
        /// </summary>
        public byte ForegroundColorIndex
        {
            get { return _textForegroundColorIndex; }
            set { _textForegroundColorIndex = value; }
        }

        private byte _bgColorIndex;
        /// <summary>
        /// �ı�����ɫ���� Text Blackground Color Index - ����ɫ��ȫ����ɫ�б��е�����
        /// </summary>
        public byte BgColorIndex 
        {
            get { return _bgColorIndex; }
            set { _bgColorIndex = value; }
        }

        private List<string> _textDatas = new List<string>();

        /// <summary>
        /// �ı����ݿ鼯��Plain Text Data - һ���������ݿ�(Data Sub-Blocks)��ɣ�����Ҫ����ʾ���ַ�����
        /// </summary>
        public List<string> TextDatas
        {
            get { return _textDatas; }
            set { _textDatas = value; }
        }


        public PlainTextEx()
        {

        }

        public byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(0x21);
            list.Add(0x01);
            list.Add(12);
            list.AddRange(BitConverter.GetBytes(_xOffSet));
            list.AddRange(BitConverter.GetBytes(_yOffSet));
            list.AddRange(BitConverter.GetBytes(_width));
            list.AddRange(BitConverter.GetBytes(_hight));
            list.Add(_characterCellWidth);
            list.Add(_characterCellHeight);
            list.Add(_textForegroundColorIndex);
            list.Add(_bgColorIndex);
            foreach (string text in _textDatas)
            {
                list.Add((byte)text.Length);
                foreach (char c in text)
                {
                    list.Add((byte)c);
                }               
            }
        
           list.Add(0);
           return list.ToArray();
        }

        public PlainTextEx(Stream stream)
        {
            StreamHelper streamHelper = new StreamHelper(stream);
            int blockSize = streamHelper.Read();
            if (blockSize != _blockSize)
            {
                throw new Exception("���ݸ�ʽ����");
            }
            _xOffSet = streamHelper.ReadShort();
            _yOffSet = streamHelper.ReadShort();
            _width = streamHelper.ReadShort();
            _hight = streamHelper.ReadShort();
            _characterCellWidth = (byte)streamHelper.Read();
            _characterCellHeight = (byte)streamHelper.Read();
            _textForegroundColorIndex = (byte)streamHelper.Read();
            _bgColorIndex = (byte)streamHelper.Read();
            int nextFlag = streamHelper.Read();
            while (nextFlag != 0)
            {
                blockSize = nextFlag;
                string data = streamHelper.ReadString(blockSize);
                _textDatas.Add(data);
                nextFlag = streamHelper.Read();
            }
        }
    }
}
