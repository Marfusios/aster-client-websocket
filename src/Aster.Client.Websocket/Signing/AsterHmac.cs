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

            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}