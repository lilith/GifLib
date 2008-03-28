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
    /// 图像标识符
    /// </summary>
    public class ImageDescriptor
    {
        private byte _startFlag;

        public byte StartFlag
        {
            get { return _startFlag; }
            set { _startFlag = value; }
        }

        private short _xOffSet;

        public short XOffSet
        {
            get { return _xOffSet; }
            set { _xOffSet = value; }
        }

        private short _yOffSet;

        public short YOffSet
        {
            get { return _yOffSet; }
            set { _yOffSet = value; }
        }

        private short _width;

        public short Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private short _height;

        public short Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private bool _lctFlag;

        public bool LctFlag
        {
            get { return _lctFlag; }
            set { _lctFlag = value; }
        }

        private bool _interlaceFlag;

        /// <summary>
        /// 交织标识
        /// </summary>
        public bool InterlaceFlag
        {
            get { return _interlaceFlag; }
            set { _interlaceFlag = value; }
        }

        private bool _sortFlag;


        public bool SortFlag
        {
            get { return _sortFlag; }
            set { _sortFlag = value; }
        }

        private int _lctSize;

        public int LctSize
        {
            get { return _lctSize; }
            set { _lctSize = value; }
        }
        public ImageDescriptor()
        {

        }
        public ImageDescriptor(Stream stream)
        {
            StreamHelper streamHelper = new StreamHelper(stream);
            _xOffSet = streamHelper.ReadShort();
            _yOffSet = streamHelper.ReadShort();
            _width = streamHelper.ReadShort();         
            _height = streamHelper.ReadShort();
           
            int packed = streamHelper.Read();
            _lctFlag = ((packed & 0x80) >> 7)==1;
            _interlaceFlag = ((packed & 0x40) >> 6)==1;
            _sortFlag = ((packed & 0x20) >> 5)==1;
            _lctSize =(2 << (packed & 0x07));
        }

        public byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(0x2C);
            list.AddRange(BitConverter.GetBytes(_xOffSet));
            list.AddRange(BitConverter.GetBytes(_yOffSet));
            list.AddRange(BitConverter.GetBytes(_width));
            list.AddRange(BitConverter.GetBytes(_height));
            byte packed = 0;
            int m = 0;
            if (_lctFlag)
            {
                m = 1;
            }
            int i = 0;
            if (_interlaceFlag)
            {
                i = 1;
            }
            int s = 0;
            if (_sortFlag)
            {
                s = 1;
            }
            byte pixel = (byte)(Math.Log(_lctSize,2) - 1);
            packed = (byte)(pixel | (s << 5) | (i << 6) | (m << 7));
            list.Add(packed);          
            return list.ToArray();
        }
	
    }
}
