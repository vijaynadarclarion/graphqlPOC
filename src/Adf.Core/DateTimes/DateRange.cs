// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRange.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Adf.Core.DateTimes
{
    #region usings

    using System;

    #endregion

    /// <summary>
    /// The date range.
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> class.
        /// </summary>
        public DateRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> class.
        /// </summary>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="endDate">
        /// The end date.
        /// </param>
        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The total days.
        /// </summary>
        public double TotalDays => (EndDate - StartDate).TotalDays;

        /// <summary>
        /// The includes.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Includes(DateTime value)
        {
            return StartDate <= value && value <= EndDate;
        }
    }
}
