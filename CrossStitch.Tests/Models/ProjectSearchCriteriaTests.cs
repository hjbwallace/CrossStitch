using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using CrossStitch.Core.Models.SearchCriteria;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.Models
{
    public class ProjectSearchCriteriaTests
    {
        [Theory]
        [InlineData("Na")]
        [InlineData("Name")]
        public void ApplyToName(string searchString)
        {
            var source = new Project[] { GetExampleProject() };

            var criteria = new ProjectSearchCriteria
            {
                SearchString = searchString
            };

            var results = source.ApplyCriteria(criteria).ToArray();
            results.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("Name2")]
        [InlineData("Description")]
        public void ApplyWithInvalidTerm(string searchString)
        {
            var source = new Project[] { GetExampleProject() };

            var criteria = new ProjectSearchCriteria
            {
                SearchString = searchString
            };

            var results = source.ApplyCriteria(criteria).ToArray();

            results.Should().BeEmpty();
        }

        private static Project GetExampleProject(int ownedLength = 1)
        {
            return new Project
            {
                Name = "Name",
                Description = "Description"
            };
        }
    }
}