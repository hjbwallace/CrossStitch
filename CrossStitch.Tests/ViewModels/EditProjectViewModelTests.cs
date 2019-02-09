using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using CrossStitch.Core.ViewModels;
using CrossStitch.Tests.CommonActions;
using CrossStitch.Tests.Fixtures;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.ViewModels
{
    public class EditProjectViewModelTests : ViewModelTestBase<EditProjectViewModel>
    {
        private readonly OrderRepository _orderRepository;
        private readonly PatternProjectRepository _patternProjectRepository;
        private readonly ProjectRepository _projectRepository;

        public EditProjectViewModelTests()
        {
            _projectRepository = Instance<ProjectRepository>();
            _orderRepository = Instance<OrderRepository>();
            _patternProjectRepository = Instance<PatternProjectRepository>();

            _projectRepository.Clear();
            _patternProjectRepository.Clear();
        }

        [Fact]
        public void CanAddPatternsToProject()
        {
            var project = GenerateTestProject();
            Shell.Navigate<EditProjectViewModel>(project);

            UnderTest.SaveCommand.Execute(null);

            HasEntries<Project>(1);
            HasEntries<PatternProject>(0);

            var dbProject = _projectRepository.GetAll().Last();
            Shell.Navigate<EditProjectViewModel>(dbProject);

            UnderTest.UpdatePatternsCommand.Execute(null);
            NavigatesTo<UpdateProjectPatternsViewModel>(UnderTest.PatternSelectionDto);

            var updateProjectPatternsVm = Instance<UpdateProjectPatternsViewModel>();
            updateProjectPatternsVm.PatternSelections = updateProjectPatternsVm.PatternSelections.Select(ps =>
            {
                ps.IsSelected = true;
                return ps;
            }).AsObservable();

            updateProjectPatternsVm.ConfirmCommand.Execute(null);

            var hasProjects = UnderTest.Project.PatternProjects.Any();

            UnderTest.SaveCommand.Execute(null);

            HasEntries<Project>(1);
            HasEntries<PatternProject>(1);
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

        [Fact]
        public void GenerateOrderCommand()
        {
            var expectedOrder = new
            {
                Description = "Order for TEST PROJECT",
                Threads = new { Length = 5 }.AsArray(),
            };

            var dbPattern = Patterns.DbCreate();

            var project = new Project
            {
                Name = "TEST PROJECT",
                Description = "Some Description",
                PatternProjects = new[]
                {
                    new PatternProject
                    {
                        Pattern = dbPattern
                    }
                }.AsObservable(),
            };

            UnderTest.Initialise(project);
            UnderTest.GenerateOrderCommand.Execute(null);

            NavigatesTo<EditOrderViewModel>(expectedOrder);
        }

        private Project GenerateTestProject()
        {
            var dbPattern = Patterns.DbCreate();

            var project = new Project
            {
                Name = "TEST PROJECT",
                Description = "Some Description",
            };

            //var patternProject = new PatternProject
            //{
            //    PatternProjectId = 0,
            //    //Pattern = dbPattern,
            //    PatternId = dbPattern.PatternId,
            //    Project = project,
            //};

            //project.PatternProjects = new[] { patternProject }.AsObservable();

            return project;
        }
    }
}