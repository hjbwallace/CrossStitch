using CrossStitch.Core.Attributes;
using CrossStitch.Core.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrossStitch.Core.Models
{
    public enum PatternMaterialType
    {
        [Ui("")] None,
        [Ui("Linen")] Linen,
        [Ui("Aida")] Aida,
        [Ui("Other")] Other
    }

    public class Pattern : NotifyPropertyChanged, IEntityWithThreads<PatternThread>
    {
        [Required(AllowEmptyStrings = false)]
        [Ui("Material Colour")]
        public string MaterialColour { get; set; }

        [Required]
        [Ui("Material Count")]
        [Range(1, 100)]
        public int? MaterialCount { get; set; }

        [Ui("Material Height")]
        [Range(1, 100)]
        public int? MaterialHeight { get; set; }

        [RequiredEnum]
        [Ui("Material Type")]
        [Lookup(typeof(PatternMaterialType))]
        public PatternMaterialType MaterialType { get; set; }

        [Ui("Material Width")]
        [Range(1, 100)]
        public int? MaterialWidth { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Ui("Name")]
        public string Name { get; set; }

        public int NumberOfRequiredThreads => Threads.Count(t => !t.HasInventory);

        [Key]
        public int PatternId { get; set; }

        public ObservableCollection<PatternProject> PatternProjects { get; set; } = new ObservableCollection<PatternProject>();

        [Ui("Reference")]
        public string Reference { get; set; }

        [Ui("Stitches Per Metre")]
        [Range(1, 100)]
        public int? StitchesPerMetre { get; set; }

        public int ThreadCount => Threads.Count();
        public ICollection<PatternThread> Threads { get; set; } = new ObservableCollection<PatternThread>();
    }

    public class PatternProject : NotifyPropertyChanged
    {
        public bool IsCompleted { get; set; }

        public Pattern Pattern { get; set; }

        public int PatternId { get; set; }

        [Key]
        public int PatternProjectId { get; set; }

        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}