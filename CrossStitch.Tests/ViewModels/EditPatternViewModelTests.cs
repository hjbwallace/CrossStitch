using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using CrossStitch.Core.ViewModels;
using CrossStitch.Tests.CommonActions;
using CrossStitch.Tests.Fixtures;
using CrossStitch.Tests.Mocks;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.ViewModels
{
    public class EditPatternViewModelTests : ViewModelTestBase<EditPatternViewModel>
    {
        private readonly PatternRepository _patternRepository;
        private readonly ThreadRepository _threadRepository;

        public EditPatternViewModelTests() : base(new Pattern())
        {
            _patternRepository = SimpleIoc.Default.GetInstance<PatternRepository>();
            _threadRepository = SimpleIoc.Default.GetInstance<ThreadRepository>();

            _patternRepository.Clear();
            _threadRepository.Clear();
        }

        [Theory]
        [InlineData("", "", false)]
        [InlineData("Some Id", null, false)]
        [InlineData(null, "Some Brand", false)]
        [InlineData("Some Id", "Some Brand", true)]
        public void CanAddThreadCommand(string brandId, string brandName, bool expected)
        {
            UnderTest.AddThreadModel = new AddThreadModel
            {
                BrandId = brandId,
                BrandName = brandName
            };

            UnderTest.AddThreadCommand.CanExecute(null).Should().Be(expected);
        }

        [Fact]
        public void CanGenerateOrderCommand()
        {
            var tests = new[]
            {
                new Pattern { Threads = new PatternThread[0] },
                new Pattern { Threads = new PatternThread { Length = 0 }.AsArray() }
            };

            foreach (var test in tests)
            {
                UnderTest.Initialise(test);
                UnderTest.GenerateOrderCommand.CanExecute(null).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(123, 2, 100, true)]
        [InlineData(0, 1, 2, false)]
        [InlineData(-1, 1, 2, false)]
        [InlineData(null, 1, 2, false)]
        public void CanSavePatternWithMultipleThreadLength(int? length1, int? length2, int? length3, bool expected)
        {
            UnderTest.Model.Threads = new List<PatternThread>
            {
                new PatternThread { Length = length1 },
                new PatternThread { Length = length2 },
                new PatternThread { Length = length3 },
            };

            var canSave = UnderTest.SaveCommand.CanExecute(null);
            canSave.Should().Be(expected);
        }

        [Theory]
        [InlineData(123, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(null, false)]
        public void CanSavePatternWithThreadLength(int? length, bool expected)
        {
            UnderTest.Model.Threads = new PatternThread { Length = length }.AsArray();

            var canSave = UnderTest.SaveCommand.CanExecute(null);
            canSave.Should().Be(expected);
        }

        [Fact]
        public void DeleteThread()
        {
            var patternThread = new PatternThread
            {
                ThreadReference = ThreadReferences.Data[0]
            };

            UnderTest.Model.Threads = patternThread.AsList();

            UnderTest.DeleteThreadCommand.Execute(patternThread);

            UnderTest.Model.Threads.Should().BeEmpty();
        }

        [Fact]
        public void DeleteThreadFromDatabase()
        {
            var pattern = UnderTest.Model;
            UpdatePattern(pattern);

            var expectedThread = new
            {
                Length = 2,
                ThreadReferenceId = 1
            };

            var thread = new PatternThread
            {
                Length = expectedThread.Length,
                ThreadReferenceId = expectedThread.ThreadReferenceId
            };

            pattern.Threads = thread.AsList();
            UnderTest.SaveCommand.Execute(null);

            HasEntries<Pattern>(1);
            HasEntries<ThreadBase>(1);

            //var dbPattern = _patternRepository.Get(pattern.PatternId);
            //UnderTest.Initialise(dbPattern);

            UnderTest.DeleteThreadCommand.Execute(thread);
            UnderTest.SaveCommand.Execute(null);

            HasEntries<Pattern>(1);
            HasEntries<ThreadBase>(0);
        }

        [Fact]
        public void DialogShownWhenValidatingModel()
        {
            var pattern = ValidPattern();
            pattern.MaterialColour = null;

            UnderTest.Model = pattern;

            UnderTest.SaveCommand.Execute(null);
            Dialog.LastDialogWas(DialogType.Warning, "Validation Issues");

            // And the pattern isnt saved
            var patterns = SimpleIoc.Default.GetInstance<PatternRepository>().GetAll();
            patterns.Should().BeEmpty();

            HasEntries<Pattern>(0);
        }

        [Fact]
        public void GenerateOrderCommand()
        {
            var expectedOrder = new
            {
                Description = "Order for TEST PATTERN",
                Threads = new { Length = 20 }.AsArray(),
            };

            var pattern = new Pattern
            {
                Name = "TEST PATTERN",
                Threads = new PatternThread { Length = 20 }.AsArray()
            };

            UnderTest.Initialise(pattern);
            UnderTest.GenerateOrderCommand.Execute(null);

            NavigatesTo<EditOrderViewModel>(expectedOrder);
        }

        [Theory]
        [InlineData("1", "Other Brand")]
        public void InformationDialogShownWhenAddingDuplicateThread(string brandId, string brandName)
        {
            UnderTest.Model.Threads = new PatternThread
            {
                ThreadReference = new ThreadReference
                {
                    BrandId = brandId,
                    BrandName = brandName
                }
            }.AsArray();

            UnderTest.AddThreadModel = new AddThreadModel
            {
                BrandId = brandId,
                BrandName = brandName
            };

            UnderTest.AddThreadCommand.Execute(null);
            Dialog.LastDialogWas(DialogType.Info, "Duplicate Thread Reference");
        }

        [Theory]
        [InlineData("22", "Other Brand")]
        public void InformationDialogShownWhenCannotFindThreadToAdd(string brandId, string brandName)
        {
            UnderTest.AddThreadModel = new AddThreadModel
            {
                BrandId = brandId,
                BrandName = brandName
            };

            UnderTest.AddThreadCommand.Execute(null);
            Dialog.LastDialogWas(DialogType.Info, "Missing Thread Reference");
        }

        [Fact]
        public void SavingNewPattern()
        {
            var pattern = UnderTest.Model;
            UpdatePattern(pattern);

            UnderTest.SaveCommand.Execute(null);
            Dialog.NoDialogShown();

            // And the pattern isnt saved
            var patterns = SimpleIoc.Default.GetInstance<PatternRepository>().GetAll();
            patterns.Last().Should().BeEquivalentTo(pattern);

            HasEntries<Pattern>(1);
        }

        [Fact]
        public void SavingNewPatternWithThread()
        {
            var pattern = UnderTest.Model;
            UpdatePattern(pattern);

            var expectedThread = new
            {
                Length = 2,
                ThreadReferenceId = 1
            };

            var thread = new PatternThread
            {
                Length = expectedThread.Length,
                ThreadReferenceId = expectedThread.ThreadReferenceId
            };

            pattern.Threads = thread.AsList();
            UnderTest.SaveCommand.Execute(null);

            // And the pattern isnt saved
            var dbPattern = _patternRepository.GetAll().Last();
            dbPattern.Should().BeEquivalentTo(pattern, opt => opt.Excluding(o => o.Threads));
            dbPattern.Threads.Should().HaveCount(1);
            dbPattern.Threads.Should().BeEquivalentTo(expectedThread);

            HasEntries<Pattern>(1);
            HasEntries<ThreadBase>(1);
        }

        private static void UpdatePattern(Pattern pattern)
        {
            pattern.PatternId = default(int);
            pattern.MaterialColour = "Red";
            pattern.MaterialCount = 5;
            pattern.MaterialHeight = 10;
            pattern.MaterialType = PatternMaterialType.Aida;
            pattern.MaterialWidth = 20;
            pattern.Name = "Example Pattern";
            pattern.Reference = "Reference";
            pattern.StitchesPerMetre = 20;
            //pattern.Threads = ValidTHread().AsList();
        }

        private static Pattern ValidPattern() => new Pattern
        {
            //PatternId = default(int),
            MaterialColour = "Red",
            MaterialCount = 5,
            MaterialHeight = 10,
            MaterialType = PatternMaterialType.Aida,
            MaterialWidth = 20,
            Name = "Example Pattern",
            Reference = "Reference",
            StitchesPerMetre = 20,
            Threads = ValidThread().AsList(),
        };

        private static PatternThread ValidThread() => new PatternThread
        {
            Length = 5,
        };
    }
}