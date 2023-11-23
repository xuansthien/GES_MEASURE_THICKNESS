using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POLAR.DataService.Helper
{
    public class ConvertTime
    {
        const string TimeFormat = "dd/MM/yyyy HH:mm:ss.fff";

        public static long TimeToLong(DateTime dateTime)
        {

            long result = Convert.ToInt64((dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);

            if (result < 0)
            {
                result = 0;
            }
            return result;
        }
        //Ticks = 637414770852987673
        //result = 637414770389853512

        public static DateTime LongToTime(long longTime)
        {

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            try
            {
                dtDateTime = dtDateTime.AddMilliseconds(longTime).ToLocalTime();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return dtDateTime;
        }

        public static string LongToStrTime(long longTime, string format = TimeFormat)
        {
            string result;
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            try
            {
                result = LongToTime(longTime).ToString(format);
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }

        public static double CalculateTimeSpan(long current, long past)
        {

            long durationTick = 0;
            double resultDuration = 0;
            if (current < past)
            {
                resultDuration = -1;
            }
            else
            {
                durationTick = current - past;
                resultDuration = Math.Round(new TimeSpan(durationTick).TotalSeconds, 3);
            }

            return resultDuration;

        }

        public static double CalculateTimeSpan(DateTime current, DateTime past)
        {
            return CalculateTimeSpan(TimeToLong(current), TimeToLong(past));
        }

    }

    public class ConvertPath
    {
        public static string UserToSystem(string userPath)
        {
            string result = string.Empty;
            string ParentFolder = System.IO.Directory.GetCurrentDirectory();

            if (Regex.IsMatch(userPath, @"^\./*[\D\d]+"))
            {
                userPath = userPath.Replace("./", "\\");
                userPath = userPath.Replace("/", "\\");

            }

            result = ParentFolder + userPath + "\\";
            return result;

        }
    }

    public class CryptDecrypt
    {
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string Crypt(string text)
        {
            if (text != null)
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
                byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Convert.ToBase64String(outputBuffer);
            }
            else
            {
                return "";
            }

        }

        public static string Decrypt(string text)
        {
            if (text != null)
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
                byte[] inputbuffer = Convert.FromBase64String(text);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Encoding.Unicode.GetString(outputBuffer);
            }
            else
            {
                return "";
            }
        }
    }

    public static class DesciptionHelper
    {
        public static string ParseEnumToString<T>(T inputEnum)
        {

            string result = string.Empty;
            MemberInfo[] memberEnums = typeof(T).GetMembers();
            memberEnums.Where((w) => w.Name == inputEnum.ToString()).ToList().ForEach(i =>
            {
                var av = (DescriptionAttribute)i.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                result = av.Description;
            });
            return result;
        }

    }
}
