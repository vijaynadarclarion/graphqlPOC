// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberToWord.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Adf.Core.Localization;

namespace Adf.Core.Numbers
{
    public interface INumberToWordService
    {
        string ArabicPrefixText { get; set; }
        string ArabicSuffixText { get; set; }
        CurrencyInfo Currency { get; set; }
        string EnglishPrefixText { get; set; }
        string EnglishSuffixText { get; set; }
        decimal Number { get; set; }

        string ConvertToArabic();
        string ConvertToEnglish();
    }
}
