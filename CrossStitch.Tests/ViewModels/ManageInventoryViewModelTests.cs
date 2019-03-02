using CrossStitch.Core.Data;
using CrossStitch.Core.Models;
using CrossStitch.Core.Models.SearchCriteria;
using CrossStitch.Core.ViewModels;
using CrossStitch.Tests.Fixtures;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.ViewModels
{
    public class ManageInventoryViewModelTests : ViewModelTestBase<ManageInventoryViewModel>
    {
        private readonly ThreadReferenceRepository _threadReferenceRepository;

        public ManageInventoryViewModelTests()
        {
            _threadReferenceRepository = Instance<ThreadReferenceRepository>();
        }

        [Fact]
        public void CanAddNewThreadReference()
        {
            UnderTest.Initialise(null);
            UnderTest.NewThreadReferenceCommand.Execute(null);

            NavigatesTo<EditThreadReferenceViewModel>(new ThreadReference());
        }

        [Fact]
        public void CanEditExistingThreadReference()
        {
            var reference = CommonActions.ThreadReferences.Data[0];

            UnderTest.EditThreadReferenceCommand.Execute(reference);

            NavigatesTo<EditThreadReferenceViewModel>(reference);
        }

        [Fact]
        public void CanSearchForAllThreadReferences()
        {
            UnderTest.Initialise(null);
            UnderTest.Criteria = new InventorySearchCriteria
            {
                SearchString = string.Empty,
                ShowAllThreads = true
            };

            UnderTest.SearchCommand.Execute(null);

            var expectedCount = _threadReferenceRepository.GetAll().Count();

            UnderTest.SearchResults.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void CanSearchForOwnedThreadReferences()
        {
            UnderTest.Initialise(null);
            UnderTest.Criteria = new InventorySearchCriteria
            {
                SearchString = string.Empty,
                ShowAllThreads = false
            };

            UnderTest.SearchCommand.Execute(null);

            var expectedCount = _threadReferenceRepository.Query(x => x.Owned).Count();

            UnderTest.SearchResults.Should()
                .HaveCount(expectedCount)
                .And.OnlyContain(x => x.Wrapped.Owned);
        }

        [Theory]
        [InlineData("Example", 1)]
        [InlineData("Different Brand", 1)]
        public void CanSearchForThreadReferencesBySearchString(string searchString, int expected)
        {
            UnderTest.Initialise(null);
            UnderTest.Criteria = new InventorySearchCriteria
            {
                SearchString = searchString,
                ShowAllThreads = true
            };

            UnderTest.SearchCommand.Execute(null);
            UnderTest.SearchResults.Should().HaveCount(expected);
        }

        [Fact]
        public void SearchResultsAreOrderedWithOwnedFirst()
        {
            UnderTest.Initialise(null);
            UnderTest.Criteria = new InventorySearchCriteria
            {
                SearchString = string.Empty,
                ShowAllThreads = true
            };

            UnderTest.SearchCommand.Execute(null);
            //UnderTest.SearchResults.Should().BeEquivalentTo(new object[]
            //{
            //    new { Id = 3 },
            //    new { Id = 2 },
            //    new { Id = 1 }
            //});
        }
    }
}