using CrossStitch.Core.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrossStitch.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T source)
        {
            return GetDescription(typeof(T));
        }

        public static string GetDescription(this Type type)
        {
            var descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptions.FirstOrDefault()?.Description;
        }

        public static string GetDisplay<T>(this T source)
        {
            return GetDisplay(typeof(T));
        }

        public static string GetDisplay(this Type type)
        {
            var displays = (DisplayAttribute[])type.GetCustomAttributes(typeof(DisplayAttribute), false);
            return displays.FirstOrDefault()?.Name;
        }

        public static string GetTitle<T>(this T source)
        {
            return GetTitle(typeof(T));
        }

        public static string GetTitle(this Type type)
        {
            var displays = (TitleAttribute[])type.GetCustomAttributes(typeof(TitleAttribute), false);
            return displays.FirstOrDefault()?.Title;
        }
    }
}