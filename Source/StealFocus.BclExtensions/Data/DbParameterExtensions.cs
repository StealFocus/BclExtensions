// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbParameterExtensions.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the DbParameterExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Data
{
    using System;
    using System.Data.Common;

    /// <summary>
    /// Extension Methods for <see cref="DbParameter"/>.
    /// </summary>
    public static class DbParameterExtensions
    {
        /// <summary>
        /// Determines the value to assign to a <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="databaseParameter">The SQL Parameter.</param>
        /// <param name="value">The value to assign to the parameter.</param>
        public static void AssignValue(this DbParameter databaseParameter, string value)
        {
            if (databaseParameter == null)
            {
                throw new ArgumentNullException("databaseParameter");
            }

            if (string.IsNullOrEmpty(value))
            {
                databaseParameter.Value = DBNull.Value;
            }
            else
            {
                databaseParameter.Value = value;
            }
        }

        /// <summary>
        /// Determines the value to assign to a <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="databaseParameter">The SQL Parameter.</param>
        /// <param name="value">The value to assign to the parameter.</param>
        public static void AssignValue(this DbParameter databaseParameter, DateTime? value)
        {
            if (databaseParameter == null)
            {
                throw new ArgumentNullException("databaseParameter");
            }

            if (value.HasValue)
            {
                databaseParameter.Value = value;
            }
            else
            {
                databaseParameter.Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Determines the value to assign to a <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="databaseParameter">The SQL Parameter.</param>
        /// <param name="value">The value to assign to the parameter.</param>
        public static void AssignValue(this DbParameter databaseParameter, int? value)
        {
            if (databaseParameter == null)
            {
                throw new ArgumentNullException("databaseParameter");
            }

            if (value.HasValue)
            {
                databaseParameter.Value = value;
            }
            else
            {
                databaseParameter.Value = DBNull.Value;
            }
        }
    }
}
