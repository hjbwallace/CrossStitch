using CrossStitch.Core.Validation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Validar;

namespace CrossStitch.Core.Models
{
    [InjectValidation]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        protected ValidationTemplate _validationTemplate;

        public NotifyPropertyChanged()
        {
            _validationTemplate = new ValidationTemplate(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Validate()
        {
            _validationTemplate.Validate(this, null);
        }
    }
}