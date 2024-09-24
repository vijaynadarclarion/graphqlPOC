// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneNumbers.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Adf.Core.PhoneNumbers
{
    #region usings

    using global::PhoneNumbers;

    #endregion

    /// <summary>
    ///     The phone numbers.
    /// </summary>
    public class PhoneNumberService : IPhoneNumberService
    {
        /// <summary>
        /// Formats the phone number.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="countryCode">
        /// The country Code.
        /// </param>
        /// <returns>
        /// The formatted phone number in E164 international format.
        /// </returns>
        public string FormatPhoneNumber(string phoneNumber, string countryCode)
        {
            countryCode = countryCode.ToUpper();
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var isPossibleNumber = phoneNumberUtil.IsPossibleNumber(phoneNumber, countryCode);

            if (!isPossibleNumber)
            {
                return string.Empty;
            }

            var number = phoneNumberUtil.ParseAndKeepRawInput(phoneNumber, countryCode);
            var isValidNumber = phoneNumberUtil.IsValidNumber(number);
            if (!isValidNumber)
            {
                return string.Empty;
            }

            var formattedNumber = phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
            return formattedNumber;
        }

        /// <summary>
        /// Determines whether [is valid mobile number] [the specified phone number].
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="numType">
        /// The num Type.
        /// </param>
        /// <param name="countryCode">
        /// The country Code. value is empty string if not validating aganist specific country
        /// </param>
        /// <returns>
        /// True if the provider number is a valid mobile phone; otherwise false.
        /// </returns>
        /// <author>Usama Nada</author>
        public bool IsValidNumber(string phoneNumber, PhoneNumberType checkNumType, string countryCode = "")
        {
            countryCode = countryCode.ToUpper();
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var isPossibleNumber = phoneNumberUtil.IsPossibleNumber(phoneNumber, countryCode);
            if (!isPossibleNumber)
            {
                return false;
            }

            var number = phoneNumberUtil.ParseAndKeepRawInput(phoneNumber, countryCode);
            var isValidNumber = phoneNumberUtil.IsValidNumber(number);
            if (!isValidNumber)
            {
                return false;
            }

            var calculatedNumberType = phoneNumberUtil.GetNumberType(number);

            if ((calculatedNumberType == PhoneNumberType.FIXED_LINE || calculatedNumberType == PhoneNumberType.MOBILE))
            {
                return true;
            }

            return calculatedNumberType.ToString().Equals(checkNumType.ToString());
        }
    }
}
