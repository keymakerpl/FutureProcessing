using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Infrastructure.Wrapper
{
    /// <summary>
    /// Box your business model into this class for Data Error Notifications
    /// </summary>
    /// <typeparam name="T">Model to box</typeparam>
    public class ModelWrapper<T> : NotifyDataErrorInfoBase, IModelWrapper<T>
    {
        public T Model { get; set; }

        public ModelWrapper(T model)
        {
            Model = model;
        }
        
        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        protected override bool SetProperty<TValue>(ref TValue storage, TValue value, [CallerMemberName] string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            ValidatePropertyInternal(propertyName, value);

            return base.SetProperty(ref storage, value, propertyName);
        }

        /// <summary>
        /// 2 step validation
        /// </summary>
        /// <param name="propertyName"></param>
        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);
            ValidateDataAnnotations(propertyName, currentValue);
            ValidateCustomErrors(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var context = new ValidationContext(Model) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(currentValue, context, results);
            results.ForEach(r => AddError(propertyName, r.ErrorMessage));
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors == null) return;
            foreach (var error in errors)
            {
                AddError(propertyName, error);
            }
        }

        /// <summary>
        /// Overload for custom errors
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}