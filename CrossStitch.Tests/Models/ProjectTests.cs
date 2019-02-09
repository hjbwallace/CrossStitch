using CrossStitch.Core.Models;
using System.Collections.ObjectModel;
using Xunit;

namespace CrossStitch.Tests.Models
{
    public class ProjectTests
    {
        [Fact]
        public void CompletedWhenAllPatternsCompleted()
        {
            var project = new Project
            {
                PatternProjects = new ObservableCollection<PatternProject>
                {
                    new PatternProject { IsCompleted = true },
                    new PatternProject { IsCompleted = true },
                }
            };

            Assert.Equal(Project.ProjectStatus.Completed, project.Status);
        }

        [Fact]
        public void CreatedWhenAllPatternsNotCompleted()
        {
            var project = new Project
            {
                PatternProjects = new ObservableCollection<PatternProject>
                {
                    new PatternProject { IsCompleted = false },
                    new PatternProject { IsCompleted = false },
                }
            };

            Assert.Equal(Project.ProjectStatus.Created, project.Status);
        }

        [Fact]
        public void InProgressWhenAnyPatternCompleted()
        {
            var project = new Project
            {
                PatternProjects = new ObservableCollection<PatternProject>
                {
                    new PatternProject { IsCompleted = false },
                    new PatternProject { IsCompleted = true },
                }
            };

            Assert.Equal(Project.ProjectStatus.InProgress, project.Status);
        }

        [Fact]
        public void NoPatternsWhenNoPatterns()
        {
            var project = new Project();
            Assert.Equal(Project.ProjectStatus.NoPatterns, project.Status);
        }
    }
}