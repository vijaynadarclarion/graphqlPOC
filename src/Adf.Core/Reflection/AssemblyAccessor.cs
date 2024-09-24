// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyAccessor.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Adf.Core.Reflection
{
    #region usings

    using System.IO;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    ///     AssemblyAccessor
    /// </summary>
    public static class AssemblyAccessor
    {
        /// <summary>
        ///     Gets Assembly Company
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyCompany()
        {
            var attributes = Assembly.GetEntryAssembly() ////GetExecutingAssembly
                .GetCustomAttributes(typeof(AssemblyCompanyAttribute)).ToList();
            if (attributes.Count == 0)
            {
                return string.Empty;
            }

            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }

        /// <summary>
        ///     Gets Assembly Copyright
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyCopyright()
        {
            var attributes = Assembly.GetEntryAssembly() // GetExecutingAssembly
                .GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).ToList();
            if (attributes.Count == 0)
            {
                return string.Empty;
            }

            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        /// <summary>
        ///     Gets Assembly Description
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyDescription()
        {
            var attributes = Assembly.GetEntryAssembly() // GetExecutingAssembly
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute)).ToList();
            if (attributes.Count == 0)
            {
                return string.Empty;
            }

            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }

        /// <summary>
        ///     Gets Assembly FileVersion
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyFileVersion()
        {
            var attributes = Assembly.GetEntryAssembly() // GetExecutingAssembly
                .GetCustomAttributes(typeof(AssemblyFileVersionAttribute)).ToList();
            if (attributes.Count == 0)
            {
                return string.Empty;
            }

            return ((AssemblyFileVersionAttribute)attributes[0]).Version;
        }

        /// <summary>
        ///     Gets Assembly Product
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyProduct()
        {
            var attributes = Assembly.GetEntryAssembly() // GetExecutingAssembly
                .GetCustomAttributes(typeof(AssemblyProductAttribute)).ToList();
            if (attributes.Count == 0)
            {
                return string.Empty;
            }

            return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        /// <summary>
        ///     Gets Assembly Title
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyTitle()
        {
            var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute))
                .ToList(); // GetExecutingAssembly
            if (attributes.Count > 0)
            {
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != string.Empty)
                {
                    return titleAttribute.Title;
                }
            }

            return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase); ////GetExecutingAssembly
        }

        /// <summary>
        ///     Gets Assembly Version
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AssemblyVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString(); ////GetExecutingAssembly
        }
    }
}
