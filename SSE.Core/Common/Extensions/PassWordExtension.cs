using System;

namespace SSE.Core.Common.Extensions
{
    public class PassWordExtension
    {
        private Random random = new Random();

        private bool isAllowLowercase;
        private bool isAllowUppercase;
        private bool isAllowNumber;
        private bool isAllowSpecial;
        private bool isAllowUnderscore;
        private bool isAllowSpace;

        public PassWordExtension(bool isAllowUppercase = true, bool isAllowLowercase = true,
                                 bool isAllowNumber = true, bool isAllowSpecial = true,
                                 bool isAllowUnderscore = true, bool isAllowSpace = true)
        {
            this.isAllowUppercase = isAllowUppercase;
            this.isAllowLowercase = isAllowLowercase;
            this.isAllowNumber = isAllowNumber;
            this.isAllowSpecial = isAllowSpecial;
            this.isAllowUnderscore = isAllowUnderscore;
            this.isAllowSpace = isAllowSpace;
        }

        // Return a random character from a string.
        private string RandomChar(string str)
        {
            return str.Substring(random.Next(0, str.Length - 1), 1);
        }

        // Return a random permutation of a string.
        private string RandomizeString(string str)
        {
            string result = "";
            while (str.Length > 0)
            {
                // Pick a random character.
                int i = random.Next(0, str.Length - 1);
                result += str.Substring(i, 1);
                str = str.Remove(i, 1);
            }
            return result;
        }

        public string RandomPassword()
        {
            const string LOWER = "abcdefghijklmnopqrstuvwxyz";
            const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBER = "0123456789";
            const string SPECIAL = @"~!@#$%^&*():;[]{}<>,.?/\|";

            string allowed = "";
            if (isAllowLowercase) allowed += LOWER;
            if (isAllowUppercase) allowed += UPPER;
            if (isAllowNumber) allowed += NUMBER;
            if (isAllowSpecial) allowed += SPECIAL;
            if (isAllowUnderscore) allowed += "_";
            if (isAllowSpace) allowed += " ";

            // Pick the number of characters.
            int min_chars = 6;
            int max_chars = 8;
            int num_chars = random.Next(min_chars, max_chars);

            // Satisfy requirements.
            string password = "";
            if (password.IndexOfAny(LOWER.ToCharArray()) == -1)
                password += RandomChar(LOWER);
            if (password.IndexOfAny(UPPER.ToCharArray()) == -1)
                password += RandomChar(UPPER);
            if (password.IndexOfAny(NUMBER.ToCharArray()) == -1)
                password += RandomChar(NUMBER);
            if (password.IndexOfAny(SPECIAL.ToCharArray()) == -1)
                password += RandomChar(SPECIAL);

            // Add the remaining characters randomly.
            while (password.Length < num_chars)
                password += RandomChar(allowed);

            // Randomize (to mix up the required characters at the front).
            password = RandomizeString(password);

            return password;
        }
    }
}