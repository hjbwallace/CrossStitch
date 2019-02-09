using System.ComponentModel.DataAnnotations;

namespace CrossStitch.Core.Attributes
{
    public class RequiredEnumAttribute : RangeAttribute
    {
        public RequiredEnumAttribute()
            : base(1, int.MaxValue)
        { }
    }
}