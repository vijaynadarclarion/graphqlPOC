// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneNumbers.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using PhoneNumbers;

namespace Adf.Core.PhoneNumbers
{
    public interface IPhoneNumberService
    {
        string FormatPhoneNumber(string phoneNumber, string countryCode);
        bool IsValidNumber(string phoneNumber, PhoneNumberType checkNumType, string countryCode = "");
    }
}