using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using CrossStitch.Core.Models.SearchCriteria;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.Models
{
    public class InventorySearchCriteriaTests
    {
        [Theory]
        [InlineData("Id")]
        [InlineData("Desc")]
        public void ApplyToIdAndDescription(string searchString)
        {
            var source = new ThreadReference[] { GetExampleThreadReference() };

            var criteria = new InventorySearchCriteria
            {
                ShowAllThreads = true,
                SearchString = searchString
            };

            var results = source.ApplyCriteria(criteria).ToArray();
            results.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("Id2")]
        [InlineData("Name")]
        [InlineData("Desc2")]
        public void ApplyWithInvalidTerm(string searchString)
        {
            var source = new ThreadReference[] { GetExampleThreadReference() };

            var criteria = new InventorySearchCriteria
            {
                ShowAllThreads = true,
                SearchString = searchString
            };

            var results = source.ApplyCriteria(criteria).ToArray();

            Assert.False(results.Any());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TracksShowAllThreadsForOwnedThreads(bool showAllThreads)
        {
            var source = new ThreadReference[] { GetExampleThreadReference(1) };

            var criteria = new InventorySearchCriteria
            {
                ShowAllThreads = showAllThreads,
                SearchString = "Id"
            };

            source.ApplyCriteria(criteria).Should().HaveCount(1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TracksShowAllThreadsForUnownedThreads(bool showAllThreads)
        {
            var source = new ThreadReference[] { GetExampleThreadReference(0) };

            var criteria = new InventorySearchCriteria
            {
                ShowAllThreads = showAllThreads,
                SearchString = "Id"
            };

            source.ApplyCriteria(criteria).Should().HaveCount(showAllThreads ? 1 : 0);
        }

        private static ThreadReference GetExampleThreadReference(int ownedLength = 1)
        {
            return new ThreadReference
            {
                BrandId = "Id",
                BrandName = "Name",
                Description = "Description",
                OwnedLength = ownedLength
            };
        }
    }
}