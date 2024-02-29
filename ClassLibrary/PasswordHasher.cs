using System;
using System.Text;
using System.Security.Cryptography;

public class PasswordHasher
{
    public string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] sourceByPassw = Encoding.UTF8.GetBytes(password);
            byte[] hashSourceBytePassw = sha256Hash.ComputeHash(sourceByPassw);
            string hashPassw = BitConverter.ToString(hashSourceBytePassw).Replace("-", String.Empty);
            return hashPassw;
        }
    }
}