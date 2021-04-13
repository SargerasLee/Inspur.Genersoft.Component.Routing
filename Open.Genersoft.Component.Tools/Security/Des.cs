using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.Tools.Security
{
	public class Des
	{
        //密钥
        public static byte[] _KEY = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        //向量
        public static byte[] _IV = new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };

        /// <summary>
        /// DES加密操作
        /// </summary>
        /// <param name="normalTxt"></param>
        /// <returns></returns>
        public static string DesEncrypt(string normalTxt)
        {
            //byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(_KEY);
            //byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(_IV);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(_KEY, _IV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(normalTxt);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();

            string strRet = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            return strRet;
        }

        /// <summary>
        /// DES解密操作
        /// </summary>
        /// <param name="securityTxt">加密字符串</param>
        /// <returns></returns>
        public static string DesDecrypt(string securityTxt)//解密  
        {
            //byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(_KEY);
            //byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(_IV);
            byte[] byEnc;
            try
            {
                securityTxt.Replace("_%_", "/");
                securityTxt.Replace("-%-", "#");
                byEnc = Convert.FromBase64String(securityTxt);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(_KEY, _IV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
    }
}
