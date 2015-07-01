using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;
using JumpListSample.Common.JumpList;
using OpenLeczna.DTOs;

namespace LecznaHub.ViewModels
{
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
        //public IList Data
        //{
        //    get
        //    {
        //        if (data == null)
        //        {
        //            var items = MovieModel.CreateSampleData();
        //            data = items.ToGroups(x => x.Name, x => x.Category);
        //        }
        //        return data;
        //    }
        //}
        //private CollectionViewSource collection;

        //public CollectionViewSource Collection
        //{
        //    get
        //    {
        //        if (collection == null)
        //        {

        //        }
        //        return collection;
        //    }
        //}

    }
}
