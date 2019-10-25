using System.ComponentModel;
using System.Runtime.CompilerServices;
using Domain;
using ViewModel.Annotations;

namespace ViewModel.Data
{
    public class Pixel : INotifyPropertyChanged
    {
        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}