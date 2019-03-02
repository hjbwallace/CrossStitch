using CrossStitch.Core.Data;
using CrossStitch.Core.Models;
using CrossStitch.Core.ViewModels;
using CrossStitch.Tests.Fixtures;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.ViewModels
{
    public class EditThreadReferencesViewModelTests : ViewModelTestBase<EditThreadReferenceViewModel>
    {
        private readonly ThreadReferenceRepository _threadReferenceRepository;

        public EditThreadReferencesViewModelTests() : base(new ThreadReference())
        {
            _threadReferenceRepository = Instance<ThreadReferenceRepository>();
        }

        private static void UpdateThreadReference(ThreadReference threadReference)
        {
            threadReference.BrandId = "Brand Id";
            threadReference.BrandName = "Brand Name";
            threadReference.OwnedLength = 5;
            threadReference.Colour = "Blue";
        }

        private static ThreadReference ValidThreadReference() => new ThreadReference
        {
            BrandId = "Brand Id",
            BrandName = "Brand Name",
            OwnedLength = 5,
            Colour = "Blue",
            Description = "Description",
        };

        [Theory]
        [InlineData("Id", "Name", 1, "Blue", "Description", true)]
        [InlineData("Id", "Name", 0, "Blue", "Desc", true)]
        [InlineData(null, "Name", 0, "Blue", null, false)]
        [InlineData("Id", null, 0, "Blue", null, false)]
        [InlineData("Id", "Name", 0, null, null, false)]
        private void CanSaveReferenceButton(string id, string name, int length, string colour, string description, bool expected)
        {
            UnderTest.ThreadReference = new ThreadReference
            {
                BrandId = id,
                BrandName = name,
                Colour = colour,
                OwnedLength = length,
                Description = description
            };

            var canSave = UnderTest.SaveCommand.CanExecute(null);
            canSave.Should().Be(expected);
        }

        [Fact]
        private void CanSaveNewThreadReference()
        {
            var reference = ValidThreadReference();
            UnderTest.ThreadReference = reference;

            UnderTest.SaveCommand.Execute(null);

            var dbEntity = _threadReferenceRepository.GetMostRecent();
            dbEntity.Should().BeEquivalentTo(reference);
        }

        [Fact]
        private void CanUpdateExistingThreadReference()
        {
            var expected = new
            {
                Id = 1,
                BrandName = "Updated Brand Name",
                BrandId = "Updated Brand Id",
                Description = "Updated Description",
                OwnedLength = 10,
                Colour = "Red" // Not updating
            };

            var existingEntity = CommonActions.ThreadReferences.Data[0];
            UnderTest.Initialise(existingEntity);

            UnderTest.ThreadReference.BrandName = expected.BrandName;
            UnderTest.ThreadReference.BrandId = expected.BrandId;
            UnderTest.ThreadReference.Description = expected.Description;
            UnderTest.ThreadReference.OwnedLength = expected.OwnedLength;

            UnderTest.SaveCommand.Execute(null);

            var dbEntity = _threadReferenceRepository.Get(1);
            dbEntity.Should().BeEquivalentTo(expected);
        }
    }
}