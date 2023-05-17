using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CollectionViewPartialRedraw
{
    public class ModelBase : INotifyPropertyChanged
    {
        public IDataStore<Fleet> FleetsDataStore => DependencyService.Get<IDataStore<Fleet>>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
