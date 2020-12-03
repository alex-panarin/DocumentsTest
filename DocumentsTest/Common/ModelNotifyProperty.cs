using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DocumentsTest.Common
{
    public class ModelNotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
