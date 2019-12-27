using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using Sef.Interfaces;

namespace Sef.Common
{
    public abstract class BaseEntity : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region  Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(String property = null)
        {
            var handler = Volatile.Read(ref PropertyChanged);
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        #region  Implementation of INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(String propertyName)
        {
            List<Func<String>> errors;
            return !String.IsNullOrEmpty(propertyName) && propertyErrors.TryGetValue(propertyName, out errors)
                ? errors.Select(messageGetter => messageGetter())
                : null;
        }

        public Boolean HasErrors
        { get { return propertyErrors.Count > 0; } }

        private readonly Dictionary<String, List<Func<String>>> propertyErrors = new Dictionary<string, List<Func<string>>>();

        protected void SetErrors(String property, List<Func<String>> errors)
        {
            if (!String.IsNullOrEmpty(property))
            {
                propertyErrors[property] = errors;
            }
            else
            {
                throw new ArgumentNullException("property");
            }
            raiseErrorsChanged(property);
        }

        protected void ClearErrors(String property)
        {
            if (!String.IsNullOrEmpty(property))
            {
                propertyErrors.Remove(property);
            }
            else
            {
                propertyErrors.Clear();
            }
            raiseErrorsChanged(property);
        }

        private void raiseErrorsChanged(String property = null)
        {
            var handler = Volatile.Read(ref ErrorsChanged);
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(property));
            }
        }

        #endregion
    }

    public abstract class EditableBase<T> : BaseEntity, IEditable<T>
        where T : EditableBase<T>, new()
    {
        #region Implementation of IEditable<T>

        public abstract void UpdateFrom(T other);

        #endregion
    }
}
