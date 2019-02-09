using CrossStitch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrossStitch.Core.Attributes
{
    public class LookupAttribute : Attribute
    {
        public LookupAttribute(Type type)
        {
            Type = type;

            Lookups = Enum.GetValues(type)
                        .Cast<object>()
                        .Select(o => o.ToString())
                        .ToArray();

            LookupOptions = Lookups.Select(o => new LookupOption
            {
                Display = type.GetField(o)?.GetCustomAttribute<UiAttribute>()?.Display ?? o,
                Value = o
            }).ToList();

            if (Type.IsNullable())
                LookupOptions.Insert(0, new LookupOption { Display = "", Value = null });
        }

        public List<LookupOption> LookupOptions { get; }
        public string[] Lookups { get; }
        public Type Type { get; }

        public class LookupOption
        {
            public object Display { get; set; }
            public object Value { get; set; }
        }
    }
}