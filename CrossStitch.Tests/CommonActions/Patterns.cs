using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Models;
using CrossStitch.Core.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System.Linq;

namespace CrossStitch.Tests.CommonActions
{
    public static class Patterns
    {
        public static Pattern DbCreate(Pattern pattern, PatternThread[] threads = null)
        {
            pattern.Threads = threads ?? pattern.Threads;

            var viewModel = SimpleIoc.Default.GetInstance<EditPatternViewModel>();
            viewModel.Initialise(pattern);
            viewModel.SaveCommand.Execute(null);

            var repo = SimpleIoc.Default.GetInstance<PatternRepository>();
            return repo.GetAll().Last();
        }

        public static Pattern DbCreate()
        {
            return DbCreate(Valid(), Threads.PatternThreadWithReference().AsArray());
        }

        public static Pattern Valid() => new Pattern
        {
            MaterialColour = "Red",
            MaterialCount = 5,
            MaterialHeight = 10,
            MaterialType = PatternMaterialType.Aida,
            MaterialWidth = 20,
            Name = "Example Pattern",
            Reference = "Reference",
            StitchesPerMetre = 20,
            Threads = Threads.PatternThread().AsList(),
        };
    }
}