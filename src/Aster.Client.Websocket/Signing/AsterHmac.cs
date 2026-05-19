using System;
using System.Security.Cryptography;
using System.Text;

namespace Aster.Client.Websocket.Signing
{
    /// <summary>
    /// Aster HMAC signature signing.
    /// </summary>
    public class AsterHmac : IAsterSignatureService
    {
        private readonly byte[]? _secret;

        public AsterHmac(string? secret)
        {
            _secret = secret != null ? Encoding.UTF8.GetBytes(secret) : 
                null;
        }

        public string Sign(string payload)
        {
            using var hmacSha256 = new HMACSHA256(_secret ?? throw new InvalidOperationException("Secret cannot be null"));
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            var hash = hmacSha256.ComputeHash(payloadBytes);

            return ToLowerHex(hash);
        }

        private static string ToLowerHex(byte[] bytes)
        {
            var chars = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var value = bytes[i];
                chars[i * 2] = GetLowerHexChar(value >> 4);
                chars[i * 2 + 1] = GetLowerHexChar(value & 0xF);
            }

            return new string(chars);
        }

        private static char GetLowerHexChar(int value)
        {
            return (char)(value < 10 ? '0' + value : 'a' + value - 10);
        }
    }
}
