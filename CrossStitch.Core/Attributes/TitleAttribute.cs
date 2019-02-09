using System;

namespace CrossStitch.Core.Attributes
{
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}