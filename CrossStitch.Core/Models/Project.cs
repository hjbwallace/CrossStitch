using CrossStitch.Core.Attributes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrossStitch.Core.Models
{
    public class Project : NotifyPropertyChanged
    {
        public enum ProjectStatus
        {
            NoPatterns,
            Created,
            InProgress,
            Completed
        }

        public int CompletedPatterns => PatternProjects.Count(o => o.IsCompleted);

        public DateTime CreatedDate { get; private set; }

        public bool IsCreated => CreatedDate != new DateTime();

        public void Create(DateTime now)
        {
            CreatedDate = now;
        }

        [Ui("Description")]
        public string Description { get; set; }

        [Required]
        [Ui("Name")]
        [Search]
        public string Name { get; set; }

        public ObservableCollection<PatternProject> PatternProjects { get; set; } = new ObservableCollection<PatternProject>();

        [Key]
        public int ProjectId { get; set; }

        public int RemainingPatternCount => PatternProjects.Count(o => !o.IsCompleted);

        public ProjectStatus Status
        {
            get
            {
                if (!PatternProjects.Any())
                    return ProjectStatus.NoPatterns;

                if (RemainingPatternCount == TotalPatternCount)
                    return ProjectStatus.Created;

                if (RemainingPatternCount == 0)
                    return ProjectStatus.Completed;

                return ProjectStatus.InProgress;
            }
        }

        public int TotalPatternCount => PatternProjects.Count();
    }
}