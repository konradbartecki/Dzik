using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LecznaHub.Common;
using LecznaHub.Core.Model;
using LecznaHub.Core.ViewModel;
using LecznaHub.Data;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Store;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using LecznaHub.BackgroundTasks;
using LecznaHub.Controls;
using OpenLeczna.DTOs;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace LecznaHub
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private AutoSuggestBox _myAutoSuggestBox;
        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public StationsViewModel stationsVM;

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Helpers.BackgroundTasksHelper.RegisterLiveTileUpdaterTask();
            
            //if (e.PageState != null)
           // {
                //this.DefaultViewModel["Groups"] = e.PageState["Groups"] as ObservableCollection<NewsCollection>;
                //this.DefaultViewModel["Stations"] = e.PageState["Stations"] as ObservableCollection<StationDto>;
               // return;
           // }


            //stationsVM = svm;
            ProgressIndicator.ShowLoader("Pobieranie wiadomości...", true);
            this.DefaultViewModel["Groups"] = await MainViewModel.GetGroupsAsync();
   
            ProgressIndicator.ShowLoader("Pobieranie rozkładów jazdy...", true);

            var stations = await StationsViewModel.GetStationsAsync();
            var cities = await StationsViewModel.GetCitiesAsync();
            var carriers = await StationsViewModel.GetCarriersAsync();

            this.DefaultViewModel["Stations"] = stations;
            this.DefaultViewModel["Cities"] = cities;
            this.DefaultViewModel["Carriers"] = carriers;
            ProgressIndicator.ShowLoader("", false);
            this.DefaultViewModel["ChosenStation"] = stations[0];
            this.DefaultViewModel["ChosenCity"] = cities[1];
        }


        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["Groups"] = DefaultViewModel["Groups"];
            //e.PageState["Stations"] = DefaultViewModel["Stations"];
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Details about the click event.</param>
        private void GroupSection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(SectionPage), groupId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((NewsItemBase)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //review
            var uri = new Uri($"ms-windows-store:reviewapp?appid={CurrentApp.AppId}");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void Grid_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //About
            if (!Frame.Navigate(typeof(AboutPage)))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async void moreApps_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var uri = new Uri($@"ms-windows-store:search?keyword={"Konrad Bartecki"}");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void Share_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var uri = new Uri($"ms-windows-store:navigate?appid={CurrentApp.AppId}");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        //private enum SuggestBoxMode
        //{
        //    Station,
        //    City
        //}

        //private SuggestBoxMode CurrentSuggestBoxMode;

        //private void MyAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{
        //    if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;

        //    //var svm = this.DefaultViewModel["Stations"] as StationsViewModel;
        //    //if (svm == null)
        //       // return;
 
        //    // You can set a threshold when to start looking for suggestions
        //    if (sender.Text.Length >= 1)
        //    {
        //        if (CurrentSuggestBoxMode == SuggestBoxMode.Station)
        //            sender.ItemsSource = stationsVM.GetStationsSuggestions(sender.Text);
        //        else
        //            sender.ItemsSource = stationsVM.GetCitiesSuggestions(sender.Text);
        //    }
        //    else
        //    {
        //        sender.ItemsSource = new List<string> { };
        //    }
        //}

        //private void DestinationGridView_Tapped(object sender, TappedRoutedEventArgs e)
        //{

        //}

        private void ShowMoreDeparturesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void StartingStationGridView_Tapped(object sender, TappedRoutedEventArgs e)
        //{

        //}

        //private void MyAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    sender.Visibility = Visibility.Collapsed;
        //    sender.Text = "";
        //    if (this.CurrentSuggestBoxMode == SuggestBoxMode.City)
        //        this.DestinationGridView.DataContext = args.SelectedItem as CityDTO;
        //    else
        //        this.StartingStationGridView.DataContext = args.SelectedItem as StationDto;

        //}

        //private void MyAutoSuggestBox_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    this._myAutoSuggestBox = (AutoSuggestBox) sender;
        //}

        //private void CommandButton_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    _myAutoSuggestBox.Visibility = Visibility.Visible;
        //    //starting station select button
        //    _myAutoSuggestBox.Text = "";
        //    _myAutoSuggestBox.PlaceholderText = "Wyszukaj przystanek";
        //    CurrentSuggestBoxMode = SuggestBoxMode.Station;
        //    _myAutoSuggestBox.Focus(FocusState.Programmatic);
        //}

        //private void CommandButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        //{
        //    _myAutoSuggestBox.Visibility = Visibility.Visible;
        //    //City select button
        //    _myAutoSuggestBox.Text = "";
        //    _myAutoSuggestBox.PlaceholderText = "Wyszukaj miasto";
        //    CurrentSuggestBoxMode = SuggestBoxMode.City;
        //    _myAutoSuggestBox.Focus(FocusState.Programmatic);
        //}

        private void GridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var stations = DefaultViewModel["Stations"] as ObservableCollection<StationDto>;
            if (stations == null) return;

            if (!Frame.Navigate(typeof(ListPickerView), stations))
             {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }
    }
}