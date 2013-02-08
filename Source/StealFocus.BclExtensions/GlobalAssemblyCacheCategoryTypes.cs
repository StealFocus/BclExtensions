// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="GlobalAssemblyCacheCategoryTypes.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the GlobalAssemblyCacheCategoryTypes type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions
{
    using System;

    /// <summary>
    /// GlobalAssemblyCacheCategoryTypes Enumeration.
    /// </summary>
    [Flags]
    public enum GlobalAssemblyCacheCategoryTypes
    {
        /// <summary>
        /// The None Type.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The Zap Type.
        /// </summary>
        Zap = 0x1,

        /// <summary>
        /// The Global Assembly Cache Type.
        /// </summary>
        Gac = 0x2,

        /// <summary>
        /// The Download Type.
        /// </summary>
        Download = 0x4
    }
}
