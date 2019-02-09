namespace CrossStitch.Core.Models
{
    public class PatternSelection : NotifyPropertyChanged
    {
        public bool IsUpdated { get; set; }
        public Pattern[] Patterns { get; set; } = new Pattern[0];
        public string ProjectName { get; set; }
    }
}