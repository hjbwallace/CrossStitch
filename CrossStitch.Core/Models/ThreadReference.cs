using CrossStitch.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrossStitch.Core.Models
{
    public class ThreadReference : NotifyPropertyChanged
    {
        [Required]
        [Ui("Brand Id")]
        [Search]
        public string BrandId { get; set; }

        [Required]
        [Ui("Brand Name")]
        public string BrandName { get; set; }

        [Required]
        [Ui("Colour")]
        public string Colour { get; set; }

        [Required]
        [Ui("Description")]
        [Search]
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        public bool Owned => OwnedLength > 0;

        [Ui("Owned Length (m)")]
        public int OwnedLength { get; set; }

        public ICollection<ThreadBase> Threads { get; set; } = new List<ThreadBase>();

        public override string ToString()
        {
            return $"{BrandName ?? "-"}-{BrandId ?? "-"} {Description}";
        }
    }
}