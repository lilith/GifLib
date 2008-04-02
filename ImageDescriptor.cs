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
using System.IO;

namespace Jillzhang.GifUtility
{
    #region ��ImageDescriptor
    /// <summary>
    /// ͼ���ʶ��(Image Descriptor)һ��GIF�ļ��ڿ��԰������ͼ��
    /// һ��ͼ�����֮�����������һ��ͼ��ı�ʶ����ͼ���ʶ����0x2C(',')
    /// �ַ���ʼ���������������ͼ������ʣ�����ͼ��������߼���Ļ�߽��ƫ������
    /// ͼ���С�Լ����޾ֲ���ɫ�б����ɫ�б��С����10���ֽ����
    /// </summary>
    internal class ImageDescriptor
    {
        #region �ṹ�ֶ�      

        /// <summary>
        /// X����ƫ����
        /// </summary>
        internal short XOffSet;

        /// <summary>
        /// X����ƫ����
        /// </summary>
        internal short YOffSet;

        /// <summary>
        /// ͼ����
        /// </summary>
        internal short Width;

        /// <summary>
        /// ͼ��߶�
        /// </summary>
        internal short Height;     

        /// <summary>
        /// packed
        /// </summary>
        internal byte Packed;

        /// <summary>
        /// �ֲ���ɫ�б��־(Local Color Table Flag)
        /// ��λʱ��ʶ������ͼ���ʶ��֮����һ���ֲ���ɫ�б�����������֮���һ��ͼ��ʹ�ã�
        /// ֵ��ʱʹ��ȫ����ɫ�б�����pixelֵ��
        /// </summary>
        internal bool LctFlag;    

        /// <summary>
        /// ��֯��־(Interlace Flag)����λʱͼ������ʹ��������ʽ���У�����ʹ��˳�����С�
        /// </summary>
        internal bool InterlaceFlag;

        /// <summary>
        ///  �����־(Sort Flag)�������λ��ʾ�����ŵľֲ���ɫ�б��������.
        /// </summary>
        internal bool SortFlag;

        /// <summary>
        ///  pixel - �ֲ���ɫ�б��С(Size of Local Color Table)��pixel+1��Ϊ��ɫ�б��λ��
        /// </summary>
        internal int LctSize;
        #endregion     

        #region ��������
        internal byte[] GetBuffer()
        {
            List<byte> list = new List<byte>();
            list.Add(GifExtensions.ImageDescriptorLabel);
            list.AddRange(BitConverter.GetBytes(XOffSet));
            list.AddRange(BitConverter.GetBytes(YOffSet));
            list.AddRange(BitConverter.GetBytes(Width));
            list.AddRange(BitConverter.GetBytes(Height));
            byte packed = 0;
            int m = 0;
            if (LctFlag)
            {
                m = 1;
            }
            int i = 0;
            if (InterlaceFlag)
            {
                i = 1;
            }
            int s = 0;
            if (SortFlag)
            {
                s = 1;
            }
            byte pixel = (byte)(Math.Log(LctSize,2) - 1);
            packed = (byte)(pixel | (s << 5) | (i << 6) | (m << 7));
            list.Add(packed);          
            return list.ToArray();
        }
        #endregion
    }
    #endregion
}
