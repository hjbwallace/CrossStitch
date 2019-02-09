using System;

namespace CrossStitch.Core.Extensions
{
    public static class IntegerExtensions
    {
        public static int DivideAndRoundUp(this int source, int divideBy)
        {
            if (divideBy == 0)
                throw new ArgumentException("Cannot divide by zero");

            if (source == 0)
                return 0;

            return (source - 1) / divideBy + 1;
        }

        public static int DivideAndRoundUp(this int? source, int divideBy)
        {
            return DivideAndRoundUp(source.GetValueOrDefault(), divideBy);
        }
    }
}