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
using System.Runtime.InteropServices;
using System.Drawing;

namespace Jillzhang.GifUtility
{
    /// <summary>
    /// ��װ����ɫ�ṹ
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color32
    {
        /// <summary>
        /// ��ɫ�е�B,λ�����λ
        /// </summary>
        [FieldOffset(0)]
        public byte Blue;

        /// <summary>
        /// ��ɫ�е�G,λ�ڵڶ�λ
        /// </summary>
        [FieldOffset(1)]
        public byte Green;

        /// <summary>
        /// ��ɫ�е�R,λ�ڵ���λ
        /// </summary>
        [FieldOffset(2)]
        public byte Red;

        /// <summary>
        /// ��ɫ�е�A,λ�ڵ���λ
        /// </summary>
        [FieldOffset(3)]
        public byte Alpha;

        /// <summary>
        /// ��ɫ������ֵ
        /// </summary>
        [FieldOffset(0)]
        public int ARGB;

        /// <summary>
        /// ��ɫ
        /// </summary>
        public Color Color
        {
            get
            {
                return Color.FromArgb(ARGB);
            }
        }
    }
}
