using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Common
{
    public class EncryptData
    {
        /// <summary>
        /// 加密用户密码
        /// </summary>
        /// <param name="strPassword">用户输入的密码</param>
        /// <param name="strSalt">密钥</param>
        /// <returns>加密后的密码</returns>
        public static string EncryptPassword(string strPassword, string strSalt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strPassword + strSalt));
            string byte2String = null;
            for (int i = 0; i < result.Length; i++)
            {
                byte2String += result[i].ToString("x");
            }
            return byte2String;
        }

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="length">随机数的长度</param>
        /// <returns></returns>
        public static string CreateRandom(int length = 4)
        {
            char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder newRandom = new StringBuilder(length);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        public static string CreateUUID()
        {
            string guid = Guid.NewGuid().ToString("N");
            return guid;
        }
    }
}
