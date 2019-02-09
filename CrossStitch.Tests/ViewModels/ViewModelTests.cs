using CrossStitch.Core.Attributes;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.ViewModels;
using System.Linq;
using Xunit;

namespace CrossStitch.Tests.ViewModels
{
    public class ViewModelTests
    {
        [Fact]
        private void AllViewModelsHaveTitles()
        {
            var types = typeof(ViewModelBase).Assembly.GetLoadableTypes().GetConcreteImplementations<ViewModelBase>();
            Assert.All(types, t => t.GetCustomAttributes(typeof(TitleAttribute), true).Single());
        }
    }
}