// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.



//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace System.Data.Common
{
    public abstract class DbProviderFactories
    {
        public static DbProviderFactory GetFactory(string providerInvariantName)
        {
            ADP.CheckArgumentLength(providerInvariantName, nameof(providerInvariantName));

            if (DbProviderFactoriesConfiguration.Dictionary.ContainsKey(providerInvariantName))
            {
                return DbProviderFactoriesConfiguration.Dictionary[providerInvariantName]();
            }

            throw ADP.ConfigProviderNotFound();
        }
        
        public static DbProviderFactory GetFactory(DbConnection connection)
        {
            ADP.CheckArgumentNull(connection, nameof(connection));
            return connection.ProviderFactory;
        }
    }

    public static class DbProviderFactoriesConfiguration
    {
        internal static readonly Dictionary<string, Func<DbProviderFactory>> Dictionary = new Dictionary<string, Func<DbProviderFactory>>();

        public static void Add(string providerInvariantName, Func<DbProviderFactory> constructorDelegate)
        {
            ADP.CheckArgumentNull(providerInvariantName, nameof(providerInvariantName));
            ADP.CheckArgumentNull(constructorDelegate, nameof(constructorDelegate));

            if (Dictionary.ContainsKey(providerInvariantName))
            {
                throw ADP.ConfigProviderKeyAlreadyExists();
            }

            Dictionary.Add(providerInvariantName, constructorDelegate);
        }

        public static string[] GetProviderInvariantNames()
        {
            var keys = new string[Dictionary.Count];
            Dictionary.Keys.CopyTo(keys, 0);
            return keys;
        }
    }
}

