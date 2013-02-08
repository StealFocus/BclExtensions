// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the DateTimeExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions
{
    using System;
    using System.Data.SqlTypes;

    public static class DateTimeExtensions
    {
        private static readonly DateTime SqlDateTimeMinValueEquivalent = new DateTime(1753, 1, 1, 0, 0, 0);

        public static bool LessThanSqlDateTimeMinValue(this DateTime dateTime)
        {
            return dateTime < SqlDateTimeMinValueEquivalent;
        }

        /// <remarks>
        /// <para>
        /// When doing something like the following:
        /// </para>
        /// <code>
        /// SqlParameter myDateParameter = new SqlParameter("@MyParamName", SqlDbType.DateTime);
        /// myDateParameter.AssignValue(DateTime.MinValue);
        /// </code>
        /// <para>
        /// You will get an exception because <see cref="DateTime.MinValue" /> is less than <see cref="SqlDateTime.MinValue" />.
        /// </para>
        /// <para>
        /// Use this method to safely get the value:
        /// </para>
        /// <code>
        /// SqlParameter myDateParameter = new SqlParameter("@MyParamName", SqlDbType.DateTime);
        /// myDateParameter.AssignValue(myDateTimeVariable.GetValueSafelyForSqlDateTimeParameter());
        /// </code>
        /// </remarks>
        public static DateTime GetValueSafelyForSqlDateTimeParameter(this DateTime dateTime)
        {
            if (dateTime.LessThanSqlDateTimeMinValue())
            {
                return SqlDateTimeMinValueEquivalent;
            }
            
            return dateTime;
        }
    }
}
