using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Common
{
    public class NewTea
    {
        private const uint Delta = 0x9E3779B9;
        private const int DefaultRound = 32;
        #region 加密解密
        public static void Code(uint[] v, uint[] k)
        {
            if (v == null || k == null) return;
            uint v0 = v[0], v1 = v[1];
            uint k0 = k[0], k1 = k[1], k2 = k[2], k3 = k[3];
            uint sum = 0;
            uint delta = 0x9e3779b9;
            for (int i = 0; i < 32; i++)
            {
                sum += delta;
                v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
            }
            v[0] = v0; v[1] = v1;
            return;
        }

        public static void Decode(uint[] v, uint[] k)
        {
            uint n = 32;
            uint sum = 0xC6EF3720;
            uint y = v[0];
            uint z = v[1];
            uint delta = 0x9e3779b9;

            //sum = delta << 4;//左移4位即乘以2的4次方
            while (n-- > 0)
            {
                z -= ((y << 4) + k[2]) ^ (y + sum) ^ ((y >> 5) + k[3]);
                y -= ((z << 4) + k[0]) ^ (z + sum) ^ ((z >> 5) + k[1]);
                sum -= delta;
            }
            v[0] = y;
            v[1] = z;
        }
        #endregion
        #region 数据转换
        private uint[] ConvertBytesToUint(byte[] by)
        {
            uint[] ret = new uint[by.Length / 4];
            for (int i = 0; i < by.Length; i += 4)
            {
                uint temp = (uint)(by[i] | by[i + 1] << 8 | by[i + 2] << 16 | by[i + 3] << 24);
                ret[i / 4] = temp;
            }
            return ret;
        }
        private byte[] ConvertUintsToBytes(uint[] u)
        {
            byte[] by = new byte[u.Length * 4];
            for (int i = 0; i < u.Length; i++)
            {
                by[i * 4] = (byte)u[i];
                by[i * 4 + 1] = (byte)(u[i] >> 8);
                by[i * 4 + 2] = (byte)(u[i] >> 16);
                by[i * 4 + 3] = (byte)(u[i] >> 24);
            }
            return by;
        }
        // <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion
        /// <summary>
        /// 加密字符
        /// </summary>
        /// <param name="s">要加密的内容</param>
        /// <param name="key">密钥</param>
        /// <param name="keyEncoding">字符编码</param>
        /// <param name="bFlag">是否只计算前8位</param>
        /// <returns></returns>
        public byte[] EncrypString(string s, string key, Encoding keyEncoding, bool bFlag = false)
        {
            //加密的字符串和密钥不能为空
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            byte[] byContent = keyEncoding.GetBytes(s);
            byte[] byKey = keyEncoding.GetBytes(key);
            return EncryptByte(byContent, byKey);
        }
        /// <summary>
        /// 加密byte数组
        /// </summary>
        /// <param name="by">要加密的内容,加密的数组长度位8的倍数，不足的补零</param>
        /// <param name="byKey">加密的key，key值为长度是16个字节，不足补零</param>
        /// <param name="bFlag">是否只计算前8位</param>
        /// <returns>返回加密的byte数组</returns>
        public byte[] EncryptByte(byte[] by, byte[] Key, bool bFlag = false)
        {
            int len = by.Length;
            int lenKey = Key.Length;
            //加密的数据和密钥不能为空
            if (len <= 0 || lenKey <= 0)
            {
                return null;
            }
            //内容长度必须是8的倍数，不够需要补齐，填充0
            byte[] byContent;
            if (len % 8 == 0)
            {
                byContent = new byte[len];
            }
            else
            {
                byContent = new byte[(len / 8) * 8 + 8];
            }
            Array.Copy(by, byContent, len);
            byte[] byKey = new byte[16];//密钥长度为16
            if (lenKey <= 16)
            {
                Array.Copy(Key, byKey, lenKey);//长度不足16位则补零，建议密钥长度位16位
            }
            else
            {
                Array.Copy(Key, byKey, 16);//长度不足16位则补零，建议密钥长度位16位
            }
            //byte转换为uint
            var uintContent = ConvertBytesToUint(byContent);
            var uintKey = ConvertBytesToUint(byKey);
            if (!bFlag)
            {
                //8位8位参与运算
                for (int i = 0; i < uintContent.Length; i += 2)
                {
                    uint[] temp = new uint[2];
                    Array.Copy(uintContent, i, temp, 0, 2);
                    Code(temp, uintKey);
                    Array.Copy(temp, 0, uintContent, i, 2);
                }
            }
            else
            {
                Code(uintContent, uintKey);
            }
            byte[] byRet = ConvertUintsToBytes(uintContent);
            return byRet;
        }

        /// <summary>
        /// 解密加密的字符串
        /// </summary>
        /// <param name="s">要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="keyEncoding">字符编码</param>
        /// <param name="bFlag">是否只计算前8位,true表示只计算前8位,false是所有位都参与运算</param>
        /// <returns></returns>
        public byte[] DecryptString(string s, string key, Encoding keyEncoding, bool bHex = false, bool bFlag = false)
        {
            //解密的字符串和密钥不能为空
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            byte[] byContent;
            if (bHex)//是否16进制的字符串
            {
                byContent = strToToHexByte(s);
            }
            else
            {
                byContent = keyEncoding.GetBytes(s);
            }
            byte[] byKey = keyEncoding.GetBytes(key);
            return DecryptByte(byContent, byKey, bFlag);
        }

        public byte[] DecryptByte(byte[] by, byte[] Key, bool bFlag = false)
        {
            int len = by.Length;
            int lenKey = Key.Length;
            //解密的数据和密钥不能为空
            if (len <= 0 || lenKey <= 0)
            {
                return null;
            }
            //内容长度必须是8的倍数，不够需要补齐，填充0
            byte[] byContent;
            if (len % 8 == 0)
            {
                byContent = new byte[len];
            }
            else
            {
                byContent = new byte[(len / 8) * 8 + 8];
            }
            Array.Copy(by, byContent, len);
            byte[] byKey = new byte[16];//密钥长度为16
            if (lenKey <= 16)
            {
                Array.Copy(Key, byKey, lenKey);//长度不足16位则补零，建议密钥长度位16位
            }
            else
            {
                Array.Copy(Key, byKey, 16);//长度不足16位则补零，建议密钥长度位16位
            }
            //byte转换为uint
            var uintContent = ConvertBytesToUint(byContent);
            var uintKey = ConvertBytesToUint(byKey);
            if (!bFlag)
            {
                //8位8位参与运算
                for (int i = 0; i < uintContent.Length; i += 2)
                {
                    uint[] temp = new uint[2];
                    Array.Copy(uintContent, i, temp, 0, 2);
                    Decode(temp, uintKey);
                    Array.Copy(temp, 0, uintContent, i, 2);
                }
            }
            else
            {
                Decode(uintContent, uintKey);
            }
            byte[] byRet = ConvertUintsToBytes(uintContent);
            return byRet;
        }
    }
}
