// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace System
{
    #region usings

    using System.ComponentModel;
    using System.Reflection;

    #endregion

    /// <summary>
    ///     The enum extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// The has.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return ((int)(object)type & (int)(object)value) == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The is.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// gets description attribute of the enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>decription attribute of the enum value</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute) ? value.ToString() : attribute.Description;
        }

        //public static T ToEnum<T>(this string value)
        //{
        //    return (T)Enum.Parse(typeof(T), value, true);
        //}


        //public static string GetDescription(this Enum enumValue)
        //{
        //    FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

        //    DescriptionAttribute[] attributes =
        //        (DescriptionAttribute[])fi.GetCustomAttributes(
        //            typeof(DescriptionAttribute),
        //            false);

        //    if (attributes != null && attributes.Length > 0)
        //        return attributes[0].Description;
        //    else
        //        return enumValue.ToString();
        //}

    }
}
