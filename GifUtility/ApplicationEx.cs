/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang 版权所有。 
//  
// 文件名：ApplicationEx.cs
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
    public class ApplicationEx : ExData
    {
        public ApplicationEx()
        {
            DataStruct ds = new DataStruct();
            ds.BlockSize = 3;
            ds.Data = new byte[] { 1,0,0};
            _dataStructs.Add(ds);
        }
        private static readonly byte _label = 0xFF;
        public byte Label
        {
            get
            {
                return _label;
            }
        }
        private static readonly byte _blockSize=11;
        public byte BlockSize
        {
            get
            {
                return _blockSize;
            }
        }
        private string _applicationIdentifie = "NETSCAPE";
        public string ApplicationIdentifie
        {
            get
            {
                return _applicationIdentifie;
            }

            set
            {
                _applicationIdentifie = value;
            }
        }

        private string _applicationAuthenticationCode = "2.0";
        public string ApplicationAuthenticationCode
        {
            get
            {
                return _applicationAuthenticationCode;
            }

            set
            {
                _applicationAuthenticationCode = value;
            }
        }

        private List<DataStruct> _dataStructs = new List<DataStruct>();
        public List<DataStruct> Datas
        {
            get
            {
                return _dataStructs;
            }
            set
            {
                _dataStructs = value;
            }
        }

        public ApplicationEx(Stream stream)
        {
            StreamHelper streamHelper = new StreamHelper(stream);
            int blockSize =streamHelper. Read();
            if (blockSize != _blockSize)
            {
                throw new Exception("数据格式错误！");
            }
            _applicationIdentifie  = streamHelper.ReadString(8);
            _applicationAuthenticationCode = streamHelper.ReadString(3);
            int nextFlag = streamHelper.Read();
            while (nextFlag != 0)
            {
                DataStruct data = new DataStruct(nextFlag,stream);
                _dataStructs.Add(data);
                nextFlag = streamHelper.Read();
            }
        }

        public byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(0x21);
            list.Add(0xFF);
            list.Add(11);
            foreach (char c in _applicationIdentifie)
            {
                list.Add((byte)c);
            }
            foreach (char c in _applicationAuthenticationCode)
            {
                list.Add((byte)c);
            }
            foreach (DataStruct ds in _dataStructs)
            {                
                list.AddRange(ds.GetBuffer());
            }
            list.Add(0);
            return list.ToArray();
        }
    }
}
