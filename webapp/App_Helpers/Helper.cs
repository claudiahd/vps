using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using VPS.Models;

namespace VPS
{
    public class Helper
    {

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static void Seed()
        {
            // Make sure we always have at least the demo user available to login with
            // this ensures the user does not have to explicitly register upon first use
            byte[] password;
            using (MD5 md5Hash = MD5.Create())
            {
                password = GetMd5Hash(md5Hash, "DTe@m28!");
            }

            var administrator = new Users
            {
                //UserID = "6bc8cee0-a03e-430b-9711-420ab0d6a596",
                EmailAddress = "admin@myemail.com",
                Name = "Administrator",
                Password = password,
                UserTypeID = 1,
                Active = true
            };

            VPSEntities context = new VPSEntities();
            Users adminUser = context.Users.Where(u => u.EmailAddress == administrator.EmailAddress).First();

            if (adminUser == null)
            {
                context.Users.Add(administrator);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {


                }
            }
        }

        public static byte[] GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            return md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }

    public enum UserTypes
    {
        Admin = 1,
        User = 2,
        Operator = 3
    }

}