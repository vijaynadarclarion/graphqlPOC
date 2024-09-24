using Adf.Core.Localization;
using FluentValidation;
using System;

namespace Adf.Core.Validators
{
    /// <summary>
    /// Validator extensions
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// A vehicle identification number(VIN), or chassis number, is a unique code including a serial number, used by the automotive industry to identify towed vehicles, individual motor vehicles, scooters, motorcycles, trucks, and mopeds.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> VehicleVin<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new VehicleVinValidator<T>());
        }
    }
}
