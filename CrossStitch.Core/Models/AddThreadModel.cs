using CrossStitch.Core.Attributes;
using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace CrossStitch.Core.Models
{
    public class AddThreadModel : ObservableObject
    {
        [Required]
        [Ui("Brand Id")]
        public string BrandId { get; set; }

        [Required]
        [Ui("Brand Name")]
        public string BrandName { get; set; } = "DMC";
    }
}