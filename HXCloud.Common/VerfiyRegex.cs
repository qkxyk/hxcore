using System.Text.RegularExpressions;

namespace HXCloud.Common
{
    public class VerfiyRegex
    {
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="mess">要验证的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool PatternRegex(string mess, string pattern)
        {
            bool bRet = Regex.IsMatch(mess, pattern);
            return bRet;
        }
        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <param name="Email">电子邮件</param>
        /// <returns></returns>
        public static bool EmailRegex(string Email)
        {
            string patern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return PatternRegex(Email, patern);
        }
        /// <summary>
        /// 手机号码验证
        /// </summary>
        /// <param name="Mobile">手机号码</param>
        /// <returns></returns>
        public static bool MobileRegex(string Mobile)
        {
            string pattern = @"^1[3-9]\d{9}$";
            return PatternRegex(Mobile, pattern);
        }
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="Identity">身份证</param>
        /// <returns></returns>
        public static bool IdentityRegex(string Identity)
        {
            string pattern = @"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}[0-9Xx]$)";
            return PatternRegex(Identity, pattern);
        }
        /// <summary>
        /// 邮编验证
        /// </summary>
        /// <param name="Identity">邮编</param>
        /// <returns></returns>
        public static bool PostcodeRegex(string postcode)
        {
            string pattern = @"^\d{6}$";
            return PatternRegex(postcode, pattern);
        }
    }

}