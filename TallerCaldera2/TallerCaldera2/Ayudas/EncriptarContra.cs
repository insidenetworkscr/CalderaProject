using System;
using System.Security.Cryptography;
using System.Text;

namespace TallerCaldera2.Ayudas
{
    public static class EncriptarContra
    {
        public static string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
