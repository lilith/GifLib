#region Copyright & License
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
    #region ��GifExtensions
    /// <summary>
    /// ��չ���е�һЩ����
    /// </summary>
    internal class GifExtensions
    {
        /// <summary>
        /// Extension Introducer - ��ʶ����һ����չ�飬�̶�ֵ0x21
        /// </summary>          
        internal const byte ExtensionIntroducer = 0x21;

        /// <summary>
        /// lock Terminator - ��ʶע�Ϳ�������̶�ֵ0
        /// </summary>
        internal const byte Terminator = 0;


        /// <summary>
        /// Application Extension Label - ��ʶ����һ��Ӧ�ó�����չ�飬�̶�ֵ0xFF 
        /// </summary>
        internal const byte ApplicationExtensionLabel = 0xFF;


        /// <summary>
        /// Comment Label - ��ʶ����һ��ע�Ϳ飬�̶�ֵ0xFE
        /// </summary>
        internal const byte CommentLabel = 0xFE;


        /// <summary>
        /// ͼ���ʶ����ʼ���̶�ֵΪ','
        /// </summary>
        internal const byte ImageDescriptorLabel = 0x2C;

        /// <summary>
        /// Plain Text Label - ��ʶ����һ��ͼ���ı���չ�飬�̶�ֵ0x01
        /// </summary>
        internal const byte PlainTextLabel = 0x01;

        /// <summary>
        /// Graphic Control Label - ��ʶ����һ��ͼ�ο�����չ�飬�̶�ֵ0xF9
        /// </summary>
        internal const byte GraphicControlLabel = 0xF9;

        /// <summary>
        /// ͼ��ı�ʾ
        /// </summary>
        internal const byte ImageLabel = 0x2C;

        /// <summary>
        /// �ļ���β
        /// </summary>
        internal const byte EndIntroducer = 0x3B;
    }
    #endregion
}
