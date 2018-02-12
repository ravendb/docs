#region writing_key
using System;
using System.Security.Cryptography;

namespace GiveMeKey
{
    class Program
    {
        static void Main(string[] args)
        {
            var buffer = new byte[256 / 8];
            using (var cryptoRandom = new RNGCryptoServiceProvider())
            {
                cryptoRandom.GetBytes(buffer);
            }
            var stream = Console.OpenStandardOutput();
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
#endregion
