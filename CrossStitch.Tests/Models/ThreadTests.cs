using CrossStitch.Core.Models;
using Xunit;

namespace CrossStitch.Tests.Models
{
    public class ThreadTests
    {
        [Theory]
        [InlineData(20, 10, false)]
        [InlineData(10, 20, true)]
        [InlineData(20, 20, true)]
        public void HasInventory(int threadLength, int inventoryLength, bool hasInventory)
        {
            var thread = new PatternThread
            {
                Length = threadLength,
                ThreadReference = new ThreadReference
                {
                    OwnedLength = inventoryLength
                }
            };

            Assert.Equal(hasInventory, thread.HasInventory);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(10, false)]
        public void HasInventoryWithMissingThreadReference(int threadLength, bool hasInventory)
        {
            var thread = new PatternThread { Length = threadLength };

            Assert.Equal(hasInventory, thread.HasInventory);
        }
    }
}