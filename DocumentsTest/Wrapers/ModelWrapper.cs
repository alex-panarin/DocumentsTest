using DocumentsTest.Common;
using System;
using System.Runtime.CompilerServices;

namespace DocumentsTest.Wrapers
{
    public class ModelWrapper<TModel> : ModelNotifyProperty
    {
        public ModelWrapper(TModel model)
        {
            Model = model;
        }
        public TModel Model { get; }

        

        protected virtual void SetValue(object currentValue, [CallerMemberName] string propertyName = default)
        {
            var propertyInfo = Model?.GetType().GetProperty(propertyName);
            
            if (propertyInfo == null) return;

            var propType = Nullable.GetUnderlyingType(Model.GetType().GetProperty(propertyName).PropertyType);

            propertyInfo.SetValue(Model, Convert.ChangeType(currentValue, propType));

            OnPropertyChanged(propertyName);
        }

        protected virtual TResult GetValue<TResult>([CallerMemberName] string propertyName = default)
        {
            return (TResult)Model?.GetType().GetProperty(propertyName).GetValue(Model);
        }
    }
}
