using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrossStitch.Core.Validation
{
    //https://github.com/Fody/Validar
    public class ValidationTemplate : IDataErrorInfo, INotifyDataErrorInfo
    {
        private INotifyPropertyChanged _target;

        private ValidationContext _validationContext;

        private List<ValidationResult> _validationResults;

        public ValidationTemplate(INotifyPropertyChanged target)
        {
            this._target = target;
            _validationContext = new ValidationContext(target, null, null);
            _validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(target, _validationContext, _validationResults, true);
            target.PropertyChanged += Validate;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string Error
        {
            get
            {
                var strings = _validationResults.Select(x => x.ErrorMessage)
                                               .ToArray();
                return string.Join(Environment.NewLine, strings);
            }
        }

        public bool HasErrors
        {
            get { return _validationResults.Count > 0; }
        }

        public string this[string propertyName]
        {
            get
            {
                var strings = _validationResults.Where(x => x.MemberNames.Contains(propertyName))
                                               .Select(x => x.ErrorMessage)
                                               .ToArray();
                return string.Join(Environment.NewLine, strings);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationResults.Where(x => x.MemberNames.Contains(propertyName))
                                    .Select(x => x.ErrorMessage);
        }

        public void Validate(object sender, PropertyChangedEventArgs e)
        {
            _validationResults.Clear();
            Validator.TryValidateObject(_target, _validationContext, _validationResults, true);
            var hashSet = new HashSet<string>(_validationResults.SelectMany(x => x.MemberNames));
            foreach (var error in hashSet)
            {
                RaiseErrorsChanged(error);
            }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}