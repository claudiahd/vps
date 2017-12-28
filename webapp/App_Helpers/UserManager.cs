﻿#region Using

using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Cryptography;
using System.Text;
using VPS.Models;

#endregion

namespace VPS
{
    public class UserManager : UserManager<IdentityUser>
    {
        static VPSEntities context = new VPSEntities();

        private static readonly UserStore<IdentityUser> UserStore = new UserStore<IdentityUser>();
        private static readonly UserManager Instance = new UserManager();

        private UserManager()
            : base(UserStore)
        {
        }

        public static UserManager Create()
        {
            // We have to create our own user manager in order to override some default behavior:
            //
            //  - Override default password length requirement (6) with a length of 4
            //  - Override user name requirements to allow spaces and dots
            var passwordValidator = new MinimumLengthValidator(4);
            var userValidator = new UserValidator<IdentityUser, string>(Instance)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            Instance.UserValidator = userValidator;
            Instance.PasswordValidator = passwordValidator;

            return Instance;
        }

      

    }
}