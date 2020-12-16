﻿using EzDbSchema.Core.Extentions.Json;
using EzDbSchema.Core.Extentions.Strings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using JsonPair = System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>;
using JsonPairEnumerable = System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>>;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("EzDbSchema.MsSql")]
[assembly: InternalsVisibleTo("EzDbSchema.Cli")]
[assembly: InternalsVisibleTo("EzDbSchema.Tests")]

namespace EzDbSchema.Internal
{
	internal class AppSettings 
    {
		/// <summary></summary>
		public string ApplicationName { get; set; } = "";
        /// <summary></summary>
		public string ConnectionString { get; set; } = "";
        /// <summary></summary>
		public string SchemaName { get; set; } = "";
        /// <summary></summary>
		public string Version { get; set; } = "";
		/// <summary></summary>
		public bool VerboseMessages { get; set; } = false;
        private static AppSettings instance;
        
		private AppSettings()
        {
			//_configuration = configuration;
        }
        internal static AppSettings Instance
        {
            get
            {

                if (instance == null)
                {
                    var configFileName = "{ASSEMBLY_PATH}appsettings.json".ResolvePathVars();
                    try
                    {
                        //Complete ghetto way to deal with working around a Newtonsoft JSON bug 
                        var appsettingsText = File.ReadAllText(configFileName);
                        var items = JsonObject.Parse(appsettingsText);
                        instance = new AppSettings();
                        foreach (JsonPair jp in items )
                        {
                            var p = instance.GetType().GetProperty(jp.Key);
                            if (p != null) p.SetValue(instance, jp.Value.AsString());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception(string.Format("Error while parsing {0}. {1}", configFileName, ex.Message), ex);
                    }
				}
                return instance;
            }
        }
    }
}
