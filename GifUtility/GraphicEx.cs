/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang 版权所有。 
//  
// 文件名：GraphicEx.cs
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

namespace Jillzhang.GifUtility
{
    public class GraphicEx:ExData
    {
        private static readonly byte _label = 0xF9;
        public byte Label
        {
            get
            {
                return _label;
            }
        }
        private static readonly byte _blockSize = 4;
        public byte BlockSize
        {
            get
            {
                return _blockSize;
            }
        }
        byte _packed;
        short _delay;
        byte _tranIndex;

        private bool _transFlag;

        public bool TransparencyFlag
        {
            get { return _transFlag; }
            set { _transFlag = value; }
        }
        private int _disposalMethod;

        public int DisposalMethod
        {
            get { return _disposalMethod; }
            set { _disposalMethod = value; }
        }	



        public byte Packed
        {
            get
            {
                return _packed;
            }
            set
            {
                _packed = value;
            }
        }

        public short Delay
        {
            get
            {
                return _delay;
            }
            set
            {
                _delay = value;
            }
        }

        public byte TranIndex
        {
            get
            {
                return _tranIndex;
            }
            set
            {
                _tranIndex = value;
            }
        }
        public GraphicEx()
        {
        }
        public GraphicEx(Stream stream)
        {
            StreamHelper streamHelper = new StreamHelper(stream);
            int  blockSize = streamHelper.Read();
            if (blockSize != _blockSize)
            {
                throw new Exception("数据格式错误！");
            }
            _packed = (byte)streamHelper.Read();
            _transFlag = (_packed & 0x01) == 1;
            _disposalMethod = (_packed&0x1C)>>2;        
            _delay = streamHelper.ReadShort();
            _tranIndex =(byte) streamHelper.Read();
            streamHelper.Read();
        }

        public byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(0x21);
            list.Add(0xF9);
            list.Add(4);
            int t = 0;
            if (_transFlag)
            {
                t = 1;
            }
            _packed = (byte)((_disposalMethod << 2) | t);
            list.Add(_packed);
            list.AddRange(BitConverter.GetBytes(_delay));
            list.Add(_tranIndex);
            list.Add(0);
            return list.ToArray();
        }
    }
}
