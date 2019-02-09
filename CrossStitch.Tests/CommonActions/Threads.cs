using CrossStitch.Core.Models;

namespace CrossStitch.Tests.CommonActions
{
    public static class Threads
    {
        public static PatternThread PatternThread() => new PatternThread
        {
            Length = 5,
        };

        public static PatternThread PatternThreadWithReference() => new PatternThread
        {
            Length = 5,
            ThreadReferenceId = ThreadReferences.Data[0].Id,
        };
    }
}