﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace ItemChanger
{
    public static class Finder
    {
        private static Dictionary<string, AbstractItem> Items;
        private static Dictionary<string, AbstractLocation> Locations;

        public static AbstractItem GetItem(string name)
        {
            return Items.TryGetValue(name, out AbstractItem item) ? item.Clone() : null;
        }

        public static AbstractLocation GetLocation(string name)
        {
            return Locations.TryGetValue(name, out AbstractLocation loc) ? loc.Clone() : null;
        }

        internal static void Load()
        {
            JsonSerializer js = new JsonSerializer
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,

            };

            js.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            using (Stream s = ItemJson)
            using (StreamReader sr = new StreamReader(s))
            using (var jtr = new JsonTextReader(sr))
            {
                Items = js.Deserialize<Dictionary<string, AbstractItem>>(jtr);
            }

            using (Stream s = LocationJson)
            using (StreamReader sr = new StreamReader(s))
            using (var jtr = new JsonTextReader(sr))
            {
                Locations = js.Deserialize<Dictionary<string, AbstractLocation>>(jtr);
            }
        }

        private static Stream ItemJson => typeof(Finder).Assembly.GetManifestResourceStream("ItemChanger.Resources.items.json");
        private static Stream LocationJson => typeof(Finder).Assembly.GetManifestResourceStream("ItemChanger.Resources.locations.json");
    }
}
