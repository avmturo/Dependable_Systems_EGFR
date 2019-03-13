using System;

namespace DepSystems
{
    public static class OurExtensions
    {
        // Reference:
        // https://stackoverflow.com/a/11942
        public static int GetAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }

        /// <summary>
        /// Loops through each character in the string and checks whether it is alphanumeric
        /// </summary>
        /// <param name="check">The string to check</param>
        /// <returns>True if the string is alphanumeric, false otherwise</returns>
        public static bool IsAlphaNumeric(this string check)
        {
            foreach(char character in check)
            {
                if(!char.IsLetterOrDigit(character))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumeric(this string check)
        {
            foreach (char character in check)
            {
                if (!char.IsDigit(character))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
