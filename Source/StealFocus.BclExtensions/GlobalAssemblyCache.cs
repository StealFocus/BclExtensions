﻿// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="GlobalAssemblyCache.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the GlobalAssemblyCache type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// GlobalAssemblyCache Class.
    /// </summary>
    /// <remarks>
    /// Manipulates the GAC.
    /// </remarks>
    public static class GlobalAssemblyCache
    {
        private const string DefaultFusionAssemblyName = "mscorcfg, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

        public static string FusionAssemblyName { get; set; }

        /// <summary>
        /// Gets a list of items in the GAC.
        /// </summary>
        /// <param name="globalAssemblyCacheCategory">The category.</param>
        /// <returns>An array of <see cref="GlobalAssemblyCacheItem"/>. The items in the GAC.</returns>
        public static GlobalAssemblyCacheItem[] GetAssemblyList(GlobalAssemblyCacheCategoryTypes globalAssemblyCacheCategory)
        {
            Type fusionType;
            if (string.IsNullOrEmpty(FusionAssemblyName))
            {
                fusionType = InitialiseFusion(DefaultFusionAssemblyName);
            }
            else
            {
                fusionType = InitialiseFusion(FusionAssemblyName);
            }
            
            ArrayList assemblyInfoList = new ArrayList();
            object[] args = new object[] { assemblyInfoList, (uint)globalAssemblyCacheCategory };
            const BindingFlags BindingFlags = (BindingFlags)314;
            fusionType.InvokeMember("ReadCache", BindingFlags, null, null, args, CultureInfo.CurrentCulture);
            GlobalAssemblyCacheItem[] gacAssemblies = new GlobalAssemblyCacheItem[assemblyInfoList.Count];
            for (int i = 0; i < assemblyInfoList.Count; i++)
            {
                string name = (string)GetField(assemblyInfoList[i], "Name");
                string locale = (string)GetField(assemblyInfoList[i], "Locale");
                string publicKeyToken = (string)GetField(assemblyInfoList[i], "PublicKeyToken");
                string version = (string)GetField(assemblyInfoList[i], "Version");
                gacAssemblies[i] = new GlobalAssemblyCacheItem(name, version, locale, publicKeyToken);
            }

            return gacAssemblies;
        }

        private static Type InitialiseFusion(string fusionAssemblyName)
        {
            Assembly assembly = Assembly.Load(fusionAssemblyName);
            return assembly.GetType("Microsoft.CLRAdmin.Fusion");
        }

        /// <summary>
        /// Gets an object.
        /// </summary>
        /// <param name="source">An <see cref="object"/>. The object.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="type">The type to get.</param>
        /// <returns>An <see cref="object"/>. The value.</returns>
        private static object Get(object source, string name, BindingFlags type)
        {
            Type sourceType = source as Type;
            bool isStatic = sourceType != null;
            Type t = isStatic ? sourceType : source.GetType();
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | (isStatic ? BindingFlags.Static : BindingFlags.Instance) | type;
            object target = isStatic ? null : source;
            return t.InvokeMember(name, bindingFlags, null, target, null, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets a field.
        /// </summary>
        /// <param name="source">An <see cref="object"/>. The item containing the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <returns>An <see cref="object"/>. The field value.</returns>
        private static object GetField(object source, string name)
        {
            return Get(source, name, BindingFlags.GetField);
        }
    }
}
