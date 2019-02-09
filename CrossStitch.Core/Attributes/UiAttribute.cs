using System;

namespace CrossStitch.Core.Attributes
{
    public class UiAttribute : Attribute
    {
        public UiAttribute(string display, string tooltip = null)
        {
            Display = display;
            Tooltip = tooltip;
        }

        public string Display { get; }
        public string Tooltip { get; }
    }
}