using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamAuth.Security
{
    public static class CryptoUtilities
    {
        const int IVSize = 16;

        public static byte[] GetAES256KeyMaterial() => WinRTCrypto.CryptographicBuffer.GenerateRandom(32);

        public static byte[] Get256BiteSalt() => WinRTCrypto.CryptographicBuffer.GenerateRandom(32);

        //TODO

        public static string ByteArrayToString(byte[] data) => Encoding.UTF8.GetString(data, 0, data.Length);

        public static byte[] StringToByteArray(string text) => Encoding.UTF8.GetBytes(text);
    }
}
