﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QL_DoAnThucTap.Handler
{
    public class Validate
    {
        public static bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(84|0[35789])[0-9]{8}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
        public static bool IsValidPassWord(string password)
        {
            if (password == null || !(password is string))
            {
                return false;
            }
            if (password.Length < 8 || password.Length > 16)
            {
                return false;
            }
            if (!password.Any(char.IsUpper) || !password.Any(IsSpecialCharacter))
            {
                return false;
            }

            return true;
        }
        private static bool IsSpecialCharacter(char c)
        {
            string specialCharacters = "!@#$%^&*()-_+={}[]:\";'<>?,./|\\";
            return specialCharacters.Contains(c);
        }
    }
}
