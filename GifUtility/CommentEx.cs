/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang 版权所有。 
//  
// 文件名：CommentEx.cs
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
    public class CommentEx:ExData
    {
        private static readonly byte _label = 0xFE;
        public byte Label
        {
            get
            {
                return _label;
            }
        }
    
        private List<string> _commentString;
        public List<string> Comments
        {
            get
            {
                return _commentString;
            }
            set
            {
                _commentString = value;
            }
        }

        public CommentEx(Stream stream)
        {
            StreamHelper streamHelper = new StreamHelper(stream);           
            _commentString = new List<string>();
            int nextFlag = streamHelper.Read();
            while (nextFlag != 0)
            {
                int blockSize = nextFlag;
                string data = streamHelper.ReadString(blockSize);
                _commentString.Add(data);
                nextFlag = streamHelper.Read();
            }
        }

        public byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(0x21);
            list.Add(0xFE);
            foreach (string coment in _commentString)
            {
                char[] commentCharArray = coment.ToCharArray();
                list.Add((byte)commentCharArray.Length);
                foreach (char c in commentCharArray)
                {
                    list.Add((byte)c);
                }
            }
            list.Add(0);
            return list.ToArray();
        }
    }
}
