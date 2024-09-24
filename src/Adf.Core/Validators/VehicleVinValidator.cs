using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace Adf.Core.Validators
{
    /// <summary>
    /// A vehicle identification number(VIN), or chassis number, is a unique code including a serial number, used by the automotive industry to identify towed vehicles, individual motor vehicles, scooters, motorcycles, trucks, and mopeds.
    /// </summary>
    public class VehicleVinValidator<T> : PropertyValidator<T, string>, IChasisVinValidator
    {
        public override string Name => "ChasisVinValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            //return Localized(errorCode, Name);
            return "{PropertyName} is not valid: {ErrorMessages}";
        }

        public override bool IsValid(ValidationContext<T> context, string vin)
        {
            // The VIN is composed of the following sections:
            // 1 - The first three characters uniquely identify the manufacturer of the vehicle using the world manufacturer identifier or WMI code.
            // 2 - The 4th to 8th positions in the VIN are the vehicle descriptor section or VDS.
            // 3 - position 9 as a check digit,
            // 4 - The 10th to 17th positions are used as the 'vehicle identifier section'(VIS).

            if (vin == null)
            {
                return true;
            }

            vin = vin
                .Replace(" ", "")
                .Replace("-", "")
                .ToUpper();

            if (string.IsNullOrEmpty(vin))
            {
                context.MessageFormatter.AppendArgument("ErrorMessages", "VIN can not be empty string");
                return false;
            }

            if (vin.Length != 17)
            {
                context.MessageFormatter.AppendArgument("ErrorMessages", "VIN Length must equal 17 characters");
                return false;
            }

            if (!Regex.IsMatch(vin, @"^[0-9a-zA-Z]+$"))
            {
                context.MessageFormatter.AppendArgument("ErrorMessages", "Only alphanumeric characters are allowed in VIN");
                return false;
            }

            var invalidChars = GetInvalidChar(vin);
            if (invalidChars.Count > 0)
            {
                context.AddFailure(GetInvalidCharsMessage(invalidChars));
            }

            if (GetCheckDigit(vin) != vin[8])
            {
                context.AddFailure("The Check Digit (9th position) does not calculate properly");
            }

            return true;
        }

        private string GetInvalidCharsMessage(Dictionary<int, char> invalidChars)
        {
            if (invalidChars?.Count == 0)
            {
                return string.Empty;
            }

            var errorMessage = new StringBuilder("Illegal characters found in VIN.");

            var messages = invalidChars.Select(c => $" character {c.Value} at position {c.Key}.");
            foreach (var message in messages)
            {
                errorMessage.AppendLine(message);
            }

            return errorMessage.ToString();
        }

        private Dictionary<int, char> GetInvalidChar(string vin)
        {
            // TODO: check this code if it perform better
            // Test this code with errors 4,14 for VIN (JTMBD31V595224664) and 5,14 for VIN (JTEHJ09JX65155058) we get from NHTSA
            // https://github.com/shapemetrics/VinValidationAttribute/blob/master/VinValidationAttribute.cs

            var invalidChars = new Dictionary<int, char>();

            if (vin?.Length != 17)
            {
                return invalidChars;
            }

            int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 0, 1, 2, 3, 4, 5, 0, 7, 0, 9, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] weights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };

            var sum = 0;
            for (int i = 0; i < 17; i++)
            {
                var c = vin[i];
                int value = 0;
                var weight = weights[i];
                // letter
                if (c >= 'A' && c <= 'Z')
                {
                    value = values[c - 'A'];
                    if (value == 0)
                    {
                        invalidChars.Add(i, c);
                    }
                }
                else
                {
                    if (c >= '0' && c <= '9')
                    {
                        value = c - '0';
                    }
                    else
                    {
                        invalidChars.Add(i, c);
                    }
                }

                sum = sum + weight * value;
            }

            return invalidChars;
        }

        private char GetCheckDigit(string vin)
        {
            if (vin?.Length != 17)
            {
                return '-';
            }

            var map = "0123456789X";
            var weights = "8765432X098765432";
            int sum = 0;
            for (int i = 0; i < 17; ++i)
            {
                sum += Transliterate(vin[i]) * map.IndexOf(weights[i]);
            }
            return map[sum % 11];
        }

        private int Transliterate(char c)
        {
            return "0123456789.ABCDEFGH..JKLMN.P.R..STUVWXYZ".IndexOf(c) % 10;
        }
    }

    public interface IChasisVinValidator { }
}
