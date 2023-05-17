using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionViewPartialRedraw
{
    internal class MainViewModel: ModelBase
    {
        internal string PageSender;
        public ICommand SelectionChangedCommand { get; }

        private bool _sortAscending = true;

        public MainViewModel()
        {
            SelectionChangedCommand = new Command<object>(OnSelectionChanged);

            Fleets = FleetsDataStore.GetItemsAsync().Result.ToObservableCollection(); ;

        }

        private void OnSelectionChanged(object obj)
        {
            //FleetSelectorViewModel vm = (FleetSelectorViewModel)obj;

            //if (vm.SelectedItem != null)
            //{
            //    WeakReferenceMessenger.Default.Send(vm.SelectedItem);
            //}
        }

        #region Properties
        private ObservableCollection<Fleet> _fleets = new();
        public ObservableCollection<Fleet> Fleets
        {
            get { return _fleets; }
            set
            {
                if (_fleets != value)
                {
                    _fleets = value;
                    OnPropertyChanged(nameof(Fleets));
                }
            }
        }

        private Fleet _selectedItem;
        public Fleet SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private string _emptyCollectionMessage = "No results matched your filter.";
        public string EmptyCollectionMessage { get { return _emptyCollectionMessage; } set { _emptyCollectionMessage = value; OnPropertyChanged(nameof(EmptyCollectionMessage)); } }

        private string _emptyCollectionSuggestion = "Try a broader filter?";
        public string EmptyCollectionSuggestion { get { return _emptyCollectionSuggestion; } set { _emptyCollectionSuggestion = value; OnPropertyChanged(nameof(EmptyCollectionSuggestion)); } }

        private bool _isVisibleSearch;
        public bool IsVisibleSearch { get { return _isVisibleSearch; } set { _isVisibleSearch = value; OnPropertyChanged(nameof(IsVisibleSearch)); } }

        private bool _isVisibleFilter;
        public bool IsVisibleFilter { get { return _isVisibleFilter; } set { _isVisibleFilter = value; OnPropertyChanged(nameof(IsVisibleFilter)); } }
        private string _filterCriterion;
        public string FilterCriterion
        {
            get { return _filterCriterion; }
            set
            {
                if (_filterCriterion != value)
                {
                    _filterCriterion = value;
                    OnPropertyChanged(nameof(FilterCriterion));
                }
            }
        }

        private string _searchCriterion;
        public string SearchCriterion
        {
            get { return _searchCriterion; }
            set
            {
                if (_searchCriterion != value)
                {
                    _searchCriterion = value;
                    OnPropertyChanged(nameof(SearchCriterion));
                }
            }
        }

        #endregion

        public class DisplayAlertEventArgs : EventArgs
        {
            public string Title { get; set; }
            public string Message { get; set; }
        }

        public delegate void DisplayAlertEventHandler(object sender, DisplayAlertEventArgs args);

        public event DisplayAlertEventHandler DisplayAlertEvent;

        protected virtual void OnDisplayAlertEvent(DisplayAlertEventArgs args)
        {
            DisplayAlertEvent?.Invoke(this, args);
        }

        //public async override Task LoadAsync()
        //{
        //    if (IsLoaded)
        //        return;

        //    IsLoaded = true;
        //    IsBusy = IsLoaderVisible = true;
        //    await Task.Delay(500);

        //    try
        //    {
        //        Fleets = (await FleetsDataStore.GetItemsAsync()).ToObservableCollection();
        //        HasNoData = !Fleets.Any();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = IsLoaderVisible = false;
        //    }
        //}

        internal void SortData()
        {
            if (_sortAscending == false)
            {
                // Sort the fleets by name in descending order
                var sortedFleets = new ObservableCollection<Fleet>(Fleets.OrderByDescending(f => f.Name));
                Fleets.Clear();
                foreach (var fleet in sortedFleets)
                {
                    Fleets.Add(fleet);
                }

                OnPropertyChanged(nameof(Fleets));


                _sortAscending = true;
            }
            else
            {
                // Sort the fleets by name in ascending order
                var sortedFleets = new ObservableCollection<Fleet>(Fleets.OrderBy(f => f.Name));
                Fleets.Clear();
                foreach (var fleet in sortedFleets)
                {
                    Fleets.Add(fleet);
                }
                OnPropertyChanged(nameof(Fleets));


                _sortAscending = false;
            }
        }

        internal void FilterData(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var filteredFleets = FleetsDataStore
                    .ReadLocal()
                    .Result
                    .Where(x => x.Name.ToLower().Contains(text.ToLower()))
                    .ToObservableCollection();

                if (filteredFleets.Any())
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Fleets.Clear();
                        foreach (var fleet in filteredFleets)
                        {
                            Fleets.Add(fleet);
                        }
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Fleets.Clear();
                        EmptyCollectionMessage = "No results matched your filter.";
                        EmptyCollectionSuggestion = "Try a broader filter?";
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Fleets.Clear();
                    var allFleets = FleetsDataStore.ReadLocal().Result.ToObservableCollection();
                    foreach (var fleet in allFleets)
                    {
                        Fleets.Add(fleet);
                    }
                });
            }
        }
    }
}
