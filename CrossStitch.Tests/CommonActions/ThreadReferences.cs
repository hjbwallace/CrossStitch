using CrossStitch.Core.Models;

namespace CrossStitch.Tests.CommonActions
{
    public static class ThreadReferences
    {
        public static ThreadReference[] Data = new[]
        {
            new ThreadReference
            {
                Id = 1,
                BrandId = "1",
                BrandName = "Test Brand",
                Colour = "Red",
                Description = "First Example",
                OwnedLength = 0,
            },

            new ThreadReference
            {
                Id = 2,
                BrandId = "22",
                BrandName = "Test Brand",
                Colour = "Blue",
                Description = "Second Example",
                OwnedLength = 1,
            },

            new ThreadReference
            {
                Id = 3,
                BrandId = "1",
                BrandName = "Other Brand",
                Colour = "Green",
                Description = "Different Brand",
                OwnedLength = 10,
            },
        };
    }
}