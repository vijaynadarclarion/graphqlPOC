﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace System
{

    #region usings


    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    ///     The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     The newline.
        /// </summary>
        private const string Newline = "\r\n";

        /// <summary>
        /// English text pattern
        /// </summary>
        private const string englishTextPattern = @"^[A-Za-z0-9\s!@#$%^&*()_+=-`~\\\]\[{}|';:/.,?]*$";

        /// <summary>
        /// Arabic text pattern
        /// </summary>
        private const string arabicTextPattern = @"^[\u0600-\u06FF\u003A\0-9s]{0,4000}$";

        /// <summary>
        ///     The random.
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Appends random chars and numeric.
        ///     Added logic to specify the format of the random string (# will be random string, 0 will be random numeric, other
        ///     characters remain)
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AppendRandomFormattedString(this string text, string format)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string Numbers = "0123456789";

            var result = new StringBuilder(text);
            for (var formatIndex = 0; formatIndex < format.Length; formatIndex++)
            {
                switch (format.ToUpper()[formatIndex])
                {
                    case '0':
                        result.Append(Numbers[Random.Next(Numbers.Length)]);
                        break;
                    case '#':
                        result.Append(Chars[Random.Next(Chars.Length)]);
                        break;
                    default:
                        result.Append(format[formatIndex]);
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// The append random numbers.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AppendRandomNumbers(this string text, int length)
        {
            var chars = "0123456789";
            return text + new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string CreateRandomPassword(this string text, int length = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return "P@ssw0rd" + new string(chars);
        }

        public static string GetDate(string date)
        {
            var year = date.Substring(0, 4);
            var month = date.Substring(4, 2);
            var day = date.Substring(6, 2);
            return $"{day}/{month}/{year}";
        }

        public static string GetTime(string date)
        {
            var hour = date.Substring(8, 2);
            var min = date.Substring(10, 2);
            var sec = date.Substring(12, 2);
            return $"{hour}:{min}:{sec}";
        }

        /// <summary>
        /// Creates a new random string of upper, lower case letters and digits.
        ///     Very useful for generating random data for storage in test data.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <param name="includeNumbers">
        /// if set to <c>true</c> [include numbers].
        /// </param>
        /// <returns>
        /// randomized string
        /// </returns>
        public static string AppendRandomString(this string text, int length, bool includeNumbers = false)
        {
            var chars = includeNumbers ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return text + new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// The as unicode string.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AsUnicodeString(this string s)
        {
            var stringBuilder = new StringBuilder();
            foreach (var t in s)
            {
                stringBuilder.Append($"\\u{Convert.ToString(t, 16).PadLeft(4, '0')}");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        ///     breaking words.
        /// </summary>
        /// <param name="text">
        /// String that contains line of text
        /// </param>
        /// <param name="pos">
        /// Index where line of text starts
        /// </param>
        /// <param name="max">
        /// Maximum line length
        /// </param>
        /// <returns>
        /// The modified line length
        /// </returns>
        public static int BreakLine(this string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max - 1;

            while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            if (i < 0)
            {
                return max; // No whitespace found; break at maximum length
            }

            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            // Return length of text before whitespace
            return i + 1;
        }

        /// <summary>
        /// Returns part of a string up to the specified number of characters, while maintaining full words
        /// </summary>
        /// <param name="s">
        /// </param>
        /// <param name="length">
        /// Maximum characters to be returned
        /// </param>
        /// <returns>
        /// String
        /// </returns>
        public static string Chop(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.Length <= length)
            {
                return s;
            }

            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var word in words.Where(word => sb.ToString().Length + word.Length <= length))
            {
                sb.Append(word + " ");
            }

            return sb.ToString().TrimEnd(' ') + "...";
        }

        /// <summary>
        /// Returns a line count for a string
        /// </summary>
        /// <param name="s">
        /// string to count lines for
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int CountLines(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            return s.Split('\n').Length;
        }

        /// <summary>
        /// The format url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatUrl(this string url)
        {
            url = url.Replace("http://", "http:").Replace("https://", "https:");
            var pattern = @"//";
            var rgx = new Regex(pattern);
            var output = rgx.Replace(url.Trim(), "/");
            output = output.Replace("http:", "http://").Replace("https:", "https://");

            if (output.Length > 0 && output.Last() != '/')
            {
                output += '/';
            }

            // int lastSlash = output.LastIndexOf('/');
            // output = (lastSlash > -1) ? output.Substring(0, lastSlash) : output;
            return output;
        }

        /// <summary>
        /// Parses a string into an array of lines broken
        ///     by \r\n or \n
        /// </summary>
        /// <param name="s">
        /// String to check for lines
        /// </param>
        /// <returns>
        /// array of strings, or null if the string passed was a null
        /// </returns>
        public static string[] GetLines(this string s)
        {
            if (s == null)
            {
                return null;
            }

            s = s.Replace("\r\n", "\n");
            return s.Split('\n');
        }

        /// <summary>
        /// The if null get other field.
        /// </summary>
        /// <param name="firstText">
        /// The first text.
        /// </param>
        /// <param name="otherText">
        /// The other text.
        /// </param>
        /// <param name="isArabic">
        /// The is arabic.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IfNullGetOtherField(this string firstText, string otherText, bool isArabic)
        {
            // if (isArabic == null)
            // isArabic = CultureHelper.IsArabic;
            if (isArabic)
            {
                return !string.IsNullOrEmpty(firstText) ? firstText : otherText;
            }

            return !string.IsNullOrEmpty(otherText) ? otherText : firstText;
        }

        /// <summary>
        /// Determines whether [is valid email].
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsValidEmail(this string text)
        {
            try
            {
                return new System.Net.Mail.MailAddress(text).Address == text;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The remove from end.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(suffix))
            {
                return s;
            }

            if (s.IndexOf(suffix, StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                return s;
            }

            return s.Substring(0, s.Length - suffix.Length);
        }

        /// <summary>
        /// The remove starting with.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="staringWith">
        /// The staring with.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveStartingWith(this string s, string staringWith)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(staringWith))
            {
                return s;
            }

            var index = s.IndexOf(staringWith, StringComparison.InvariantCultureIgnoreCase);

            if (index < 0)
            {
                return s;
            }

            return s.Substring(0, index);
        }

        /// <summary>
        /// The replace arabic by english numbers.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReplaceArabicByEnglishNumbers(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var englishNumber = str;

            try
            {
                foreach (var arn in str)
                {
                    switch (arn)
                    {
                        case '٠':
                            englishNumber = englishNumber.Replace('٠', '0');
                            break;
                        case '١':
                            englishNumber = englishNumber.Replace('١', '1');
                            break;
                        case '٢':
                            englishNumber = englishNumber.Replace('٢', '2');
                            break;
                        case '٣':
                            englishNumber = englishNumber.Replace('٣', '3');
                            break;
                        case '٤':
                            englishNumber = englishNumber.Replace('٤', '4');
                            break;
                        case '٥':
                            englishNumber = englishNumber.Replace('٥', '5');
                            break;
                        case '٦':
                            englishNumber = englishNumber.Replace('٦', '6');
                            break;
                        case '٧':
                            englishNumber = englishNumber.Replace('٧', '7');
                            break;
                        case '٨':
                            englishNumber = englishNumber.Replace('٨', '8');
                            break;
                        case '٩':
                            englishNumber = englishNumber.Replace('٩', '9');
                            break;
                    }
                }

                return englishNumber;
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// The string format.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string StringFormat(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        /// <summary>
        /// The strip html.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string StripHtml(this string input)
        {
            return input == null ? null : Regex.Replace(input, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Converts a string into bytes for storage in any byte[] types
        ///     buffer or stream format (like MemoryStream).
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="encoding">
        /// The character encoding to use. Defaults to Unicode
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] ToByteArray(this string text, Encoding encoding = null)
        {
            if (text == null)
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Convert the string to camel case.
        /// </summary>
        /// <param name="str">
        /// the string to turn into Camel case
        /// </param>
        /// <returns>
        /// a string formatted as Camel case
        /// </returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (!char.IsUpper(str[0]))
            {
                return str;
            }

            var camelCase = char.ToLower(str[0]).ToString();
            if (str.Length > 1)
            {
                camelCase += str.Substring(1);
            }

            return camelCase;
        }

        /// <summary>
        /// Convert the string to Pascal case.
        /// </summary>
        /// <param name="theString">
        /// the string to turn into Pascal case
        /// </param>
        /// <returns>
        /// a string formatted as Pascal case
        /// </returns>
        public static string ToPascalCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null)
            {
                return null;
            }

            if (theString.Length < 2)
            {
                return theString.ToUpper();
            }

            // Split the string into words.
            var words = theString.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            return words.Aggregate(
                string.Empty,
                (current, word) => current + word.Substring(0, 1).ToUpper() + word.Substring(1));
        }

        /// <summary>
        /// Capitalize the first character and add a space before
        ///     each capitalized letter (except the first character).
        /// </summary>
        /// <param name="theString">
        /// the string to turn into Proper case
        /// </param>
        /// <returns>
        /// a string formatted as Proper case
        /// </returns>
        public static string ToProperCase(this string theString)
        {
            // If there are 0 or 1 characters, just return the string.
            if (theString == null)
            {
                return null;
            }

            if (theString.Length < 2)
            {
                return theString.ToUpper();
            }

            // Start with the first character.
            var result = theString.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (var i = 1; i < theString.Length; i++)
            {
                if (char.IsUpper(theString[i]))
                {
                    result += " ";
                }

                result += theString[i];
            }

            return result;
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">
        /// Text to be word wrapped
        /// </param>
        /// <param name="width">
        /// Width, in characters, to which the text
        ///     should be word wrapped
        /// </param>
        /// <returns>
        /// The modified text
        /// </returns>
        /// <see cref="http://www.softcircuits.com/Blog/post/2010/01/10/Implementing-Word-Wrap-in-C.aspx"/>
        public static string WordWrap(this string text, int width)
        {
            int pos, next;
            var sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
            {
                return text;
            }

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf(Newline, pos, StringComparison.Ordinal);

                if (eol == -1)
                {
                    next = eol = text.Length;
                }
                else
                {
                    next = eol + Newline.Length;
                }

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;

                        if (len > width)
                        {
                            len = BreakLine(text, pos, width);
                        }

                        sb.Append(text, pos, len);
                        sb.Append(Newline);

                        // Trim whitespace following break
                        pos += len;

                        while (pos < eol && char.IsWhiteSpace(text[pos]))
                        {
                            pos++;
                        }
                    } while (eol > pos);
                }
                else
                {
                    sb.Append(Newline); // Empty line
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(this string str, char c,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            //Check.NotNull(str, nameof(str));

            if (str.EndsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            //Check.NotNull(str, nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Indicates whether this string is null or an System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string str, int len)
        {
            //Check.NotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Gets index of nth occurence of a char in a string.
        /// </summary>
        /// <param name="str">source string to be searched</param>
        /// <param name="c">Char to search in <see cref="str"/></param>
        /// <param name="n">Count of the occurence</param>
        public static int NthIndexOf(this string str, char c, int n)
        {
            //Check.NotNull(str, nameof(str));

            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            return str.RemovePostFix(StringComparison.Ordinal, postFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix, comparisonType))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
        public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix, comparisonType))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        public static string ReplaceFirst(this string str, string search, string replace,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            //Check.NotNull(str, nameof(str));

            var pos = str.IndexOf(search, comparisonType);
            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replace + str.Substring(pos + search.Length);
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            //Check.NotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
            }

            return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            //Check.NotNull(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            //Check.NotNull(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string ToMd5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>PascalCase of the string</returns>
        public static string ToPascalCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToUpper() : str.ToUpperInvariant();
            }

            return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from Ending of the string if it exceeds maximum length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateFromBeginning(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Right(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        /// Converts given string to a byte array using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts given string to a byte array using the given <paramref name="encoding"/>
        /// </summary>
        public static byte[] GetBytes( this string str,  Encoding encoding)
        {
            //Check.NotNull(str, nameof(str));
            //Check.NotNull(encoding, nameof(encoding));

            return encoding.GetBytes(str);
        }

        public static bool HasArabicCharacter(this string str)
        {
            Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
            return regex.IsMatch(str);
        }

        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string AdjustOpeningCloseBalance(this string balance)
        {
            if (balance.IsNullOrEmpty())
                return "0";

            var result = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"), "{0:c2}",
             Convert.ToDouble(balance.Replace(',', '.').Split("SAR".ToCharArray())[3]));

            return result.Replace("£", "") + "";
        }

        public static string FormatNumber(this object number)

        {
            if (number == null)
                number = 0;

            var ex = Convert.ToDecimal(number.ToString());
            return string.Format("{0:#,0.##}", ex);
        }

        public static string DoubleFormatNumber(this object number)

        {
            if (number == null)
                number = 0;

            var ex = Convert.ToDecimal(number.ToString());
            return string.Format("{0:#,0.##}", ex);
        }
        public static string ConvertCurrencyToSarFormat(this string amount)
        {
            if (amount.IsNullOrEmpty())
            {
                amount = "0";
            }
            var result = Convert.ToDecimal(amount).ToString("$0.00");

            return result.Replace("$", "") + " SAR ";
        }
        public static string ToCurrencyFormat(this string amount)
        {
            amount = amount.Replace('.', ',');
            var result = string.Format("{0:0,00}", amount);
            return result;
        }




        public static string RemoveChar(this string str, List<char> charsToRemove)
        {
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), String.Empty));
            return str;
        }
        public static string ReplaceCharWithSpace(this string str, List<char> charsToRemove)
        {
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), " "));
            return str;
        }

        public static string ToJsonFormat(this string str)
        {
            var stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("{ <br />");
            if (!str.IsNullOrEmpty())
            {
                var items = str.Split(",");
                for (int item = 0; item < items.Length; item++)
                {
                    if (item == 0)
                    {

                        stringBuilder.AppendLine($"<b>{(items[item].Split(":")[0].Split("{")[1])}:</b>");
                        var val = items[item].Substring(items[item].IndexOf(':') + 1).Replace("}", "");
                        stringBuilder.AppendLine($"<span style='{SetColor(val)}'>{val}</span> ,<br />");
                        continue;
                    }
                    if (item == items.Length - 1)
                    {

                        stringBuilder.AppendLine($"<b>{items[item].Split(":")[0] }:</b>");
                        var val = items[item].Split(":")[1].Split("}")[0];
                        stringBuilder.AppendLine($"<span style='{SetColor(val)}'>{val}</span>");
                        continue;
                    }
                    else
                    {

                        stringBuilder.AppendLine($"<b>{items[item].Split(":")[0] }:</b>");
                        var val = items[item].Split(":")[1];
                        stringBuilder.AppendLine($"<span style='{SetColor(val)}'>{val}</span>,");
                    }

                    stringBuilder.AppendLine($"<br />");
                }
                stringBuilder.AppendLine("<br/ >} <br/ >");
                return stringBuilder.ToString();
            }
            return stringBuilder.ToString();
        }
        private static string SetColor(string value)
        {
            value = Regex.Replace(value, "[@,\\.\";'\\\\]", string.Empty);
            const string _boolColor = "color: red; display: contents;";
            const string _dateColor = "color: #008000; display: contents;";
            const string _defaultColor = "display: contents;";

            var isBool = bool.TryParse(value, out _);
            var isDate = DateTime.TryParse(value, out _);
            if (isBool)
            {
                return _boolColor;
            }
            else if (isDate)
            {
                return _dateColor;
            }
            else
            {
                return _defaultColor;
            }

        }

        public static long[] ConvertStringToLongArray(this string stringBody)
        {
            var checkdIdList = stringBody.Split(",");
            long[] longList = new long[checkdIdList.Count()];
            for (int i = 0; i < checkdIdList.Count(); i++)
            {
                longList[i] = long.Parse(checkdIdList[i]);
            }
            return longList;
        }

        /// <summary>
        ///  Encode string with base 64
        /// </summary>
        /// <param name="plainText">plain text</param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decode the base64 string
        /// </summary>
        /// <param name="base64EncodedData">base64 string</param>
        /// <returns></returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Gets the randomn string from text, with the specified length
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="length">Length of random string</param>
        /// <returns></returns>
        public static string GetRandomString(this string text, int length)
        {
            return new string(Enumerable.Repeat(text, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public static string GeShortUrl(this string longUrl, int shortUrlLength)
        {
            longUrl = WebUtility.UrlDecode(longUrl).Trim();

            // USAMA: if many not unique results returned from this algorithm
            // Try to add GUID to long URL before .. HAHAHA .. NO THIS MAKE IT WORSE
            // longUrl = longUrl + Guid.NewGuid().ToString();

            // Long URL into MD5 Hash
            string longUrlMd5Hash = longUrl.ToMd5();

            // Encode the long URL MD5 Hash into base64 string
            // result is always 22 characters
            string base64Encoded = longUrlMd5Hash.Base64Encode().Replace("=", "");

            // random swap some characters
            string swappedString = new string(base64Encoded.ToCharArray().OrderBy(s => (Random.Next(2) % 2) == 0).ToArray());

            // pick any 7 characters randomly from the result
            string shortUrl = swappedString.GetRandomString(shortUrlLength);

            return shortUrl;

        }

        public static bool CheckStringIsEnglish(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
        
            Regex englishRegex = new Regex(englishTextPattern);

            // Match the regular expression pattern against a text string.
            return englishRegex.Match(value).Success;
        }

        public static bool CheckStringIsArabic(this string value)
        {
            if (string.IsNullOrEmpty(value))          
                return true;

            Regex arabicRegex = new Regex(arabicTextPattern);

            // Match the regular expression pattern against a text string.
            return  arabicRegex.Match(value).Success;
        }

        public static bool CheckStringIsFreeOfXss(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            // Following regex convers all the js events and html tags mentioned in followng links.
            //https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet                 
            //https://msdn.microsoft.com/en-us/library/ff649310.aspx

            var pattren = new StringBuilder();

            //Checks any js events i.e. onKeyUp(), onBlur(), alerts and custom js functions etc.             
            pattren.Append(@"((alert|on\w+|function\s+\w+)\s*\(\s*(['|""+\d\w](,?\s *['|""+\d\w]*)*)*\s*\))");

            //Checks any html tags i.e. <script, <embed, <object etc.
            pattren.Append(@"|(<(script|iframe|embed|frame|frameset|object|img|applet|body|html|style|layer|link|ilayer|meta|bgsound))");
           
            bool isXssFreeData = !Regex.IsMatch(value, pattren.ToString(), 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return isXssFreeData;

        }
    }
}
