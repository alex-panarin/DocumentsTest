using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DocumentsTest.Wrapers
{
    public class ValidateModelWrapper<TModel> : ModelWrapper<TModel>, INotifyDataErrorInfo
    {
        public ValidateModelWrapper(TModel model)
            :base(model)
        {

        }

        private readonly Dictionary<string, List<string>> _propertyErrors
         = new Dictionary<string, List<string>>();
        
        public bool HasErrors => _propertyErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return 
                _propertyErrors.ContainsKey(propertyName)
               ? _propertyErrors[propertyName]
               : null;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            OnPropertyChanged(nameof(HasErrors));
        }

        protected void ClearErrors(string propertyName)
        {
            if (_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Remove(propertyName);

                OnErrorsChanged(propertyName);
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName, object currentValue)
        {
            return null;
        }

        protected override void SetValue(object value, [CallerMemberName] string propertyName = null)
        {
            ValidatePropertyInternal(propertyName, value);
            
            if (HasErrors) return;

            base.SetValue(value, propertyName);
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors[propertyName] = new List<string>();
            }

            if (!_propertyErrors[propertyName].Contains(error))
            {
                _propertyErrors[propertyName].Add(error);

                OnErrorsChanged(propertyName);
            }
        }
        protected void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            try
            {
                ValidateCustomErrors(propertyName, currentValue);

                ValidateDataAnnotations(propertyName, currentValue);
            }
            catch(FormatException x)
            {
                AddError(propertyName, x.Message);
            }
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();

            var context = new ValidationContext(Model) { MemberName = propertyName };

            var propType = Nullable.GetUnderlyingType(Model.GetType().GetProperty(propertyName).PropertyType);

            var value = Convert.ChangeType(currentValue, propType);

            Validator.TryValidateProperty(value, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName, object currentValue)
        {
            var errors = ValidateProperty(propertyName, currentValue);

            if (errors == null) return;

            foreach (var error in errors)
            {
                AddError(propertyName, error);
            }
        }

        


    }
}
