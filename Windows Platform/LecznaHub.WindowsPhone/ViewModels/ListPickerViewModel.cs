using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;
using JumpListSample.Common.JumpList;
using OpenLeczna.DTOs;

namespace LecznaHub.ViewModels
{

    /// <summary>
    /// This one is limited to WinRT only because I was unable to find 
    /// proper crossplatform alternative for CollectionViewSource
    /// </summary>
    public class ListPickerViewModel
    {
        public IList Data { get; private set; }
        public CollectionViewSource Collection { get; private set; }

        public ListPickerViewModel(ObservableCollection<StationDto> stations)
        {
            var stationsList = stations.ToList();
            Data = stationsList.ToGroups(x => x.Name, x => x.City);
            Collection = new CollectionViewSource();
            Collection.Source = Data;
            Collection.IsSourceGrouped = true;
        }

    }
}
