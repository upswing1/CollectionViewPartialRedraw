namespace CollectionViewPartialRedraw;

public partial class MainPage : ContentPage
{
    private MainViewModel _viewModel;
    public MainPage()
	{
		InitializeComponent();
        DependencyService.Register<FleetsDataStore>();
        BindingContext = new MainViewModel() { PageSender = this.GetType().Name };
        _viewModel = (MainViewModel)BindingContext;
    }

    private void SortButton_Clicked(object sender, EventArgs e)
    {
        _viewModel.SortData();
        //WeakReferenceMessenger.Default.Send(new WM_DataCommand("Sort", ""));
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        _viewModel.FilterData(searchBar.Text);
        //WeakReferenceMessenger.Default.Send(new WM_DataCommand("Filter", searchBar.Text));
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        
    }
}

