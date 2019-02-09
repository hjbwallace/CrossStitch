using CrossStitch.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CrossStitch.Core.Models
{
    public class OrderThread : ThreadBase
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }

    public class PatternThread : ThreadBase
    {
        public Pattern Pattern { get; set; }
        public int PatternId { get; set; }
    }

    public class ThreadBase : NotifyPropertyChanged
    {
        public bool HasInventory => (ThreadReference?.OwnedLength ?? 0) - (Length) >= 0;

        [Required]
        public int? Length { get; set; }

        public int Skiens => Length.DivideAndRoundUp(8);

        [Key]
        public int ThreadId { get; set; }

        public ThreadReference ThreadReference { get; set; }

        public int ThreadReferenceId { get; set; }
    }
}