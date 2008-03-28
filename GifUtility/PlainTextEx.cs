/*----------------------------------------------------------------
// Copyright (C) 2008 jillzhang 版权所有。 
//  
// 文件名：PlainTextEx.cs   使用请保留版权信息  
// 文件功能描述： 更多信息请访问 http://jillzhang.cnblogs.com
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
    /// <summary>
    /// 图形文本扩展(Plain Text Extension)
    /// </summary>
    public class PlainTextEx : ExData
    {
        private static readonly byte _label = 0x01;
        private static readonly byte _blockSize = 12;

        /// <summary>
        /// Plain Text Label - 标识这是一个图形文本扩展块，固定值0x01
        /// </summary>
        public byte Lable
        {
            get { return _label; }
        }


        /// <summary>
        /// Block Size - 块大小，固定值12
        /// </summary>
        public int BlockSize
        {
            get { return _blockSize; }
        }


        private short _xOffSet;

        /// <summary>
        /// Text Glid Left Posotion - 像素值，文本框离逻辑屏幕的左边界距离
        /// </summary>
        public short XOffSet
        {
            get { return _xOffSet; }
            set { _xOffSet = value; }
        }

        private short _yOffSet;
        /// <summary>
        /// Text Glid Top Posotion - 像素值，文本框离逻辑屏幕的上边界距离
        /// </summary>
        public short YOffSet
        {
            get { return _yOffSet; }
            set { _yOffSet = value; }
        }

        private short _width;
        /// <summary>
        /// 文本框高度 Text Glid Width -像素值
        /// </summary>
        public short Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private short _hight;

        /// <summary>
        /// 文本框高度 Text Glid Height - 像素值
        /// </summary>
        public short Height
        {
            get { return _hight; }
            set { _hight = value; }
        }

        private byte _characterCellWidth;
        /// <summary>
        /// 字符单元格宽度 Character Cell Width - 像素值，单个单元格宽度
        /// </summary>
        public byte CharacterCellWidth
        {
            get { return _characterCellWidth; }
            set { _characterCellWidth = value; }
        }

        private byte _characterCellHeight;
        /// <summary>
        /// 字符单元格高度 Character Cell Height- 像素值，单个单元格高度
        /// </summary>
        public byte CharacterCellHeight
        {
            get { return _characterCellHeight; }
            set { _characterCellHeight = value; }
        }

        private byte _textForegroundColorIndex;
        /// <summary>
        /// 文本前景色索引 Text Foreground Color Index - 前景色在全局颜色列表中的索引
        /// </summary>
        public byte ForegroundColorIndex
        {
            get { return _textForegroundColorIndex; }
            set { _textForegroundColorIndex = value; }
        }

        private byte _bgColorIndex;
        /// <summary>
        /// 文本背景色索引 Text Blackground Color Index - 背景色在全局颜色列表中的索引
        /// </summary>
        public byte BgColorIndex 
        {
            get { return _bgColorIndex; }
            set { _bgColorIndex = value; }
        }

        private List<string> _textDatas = new List<string>();

        /// <summary>
        /// 文本数据块集合Plain Text Data - 一个或多个数据块(Data Sub-Blocks)组成，保存要在显示的字符串。
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
                throw new Exception("数据格式错误！");
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
