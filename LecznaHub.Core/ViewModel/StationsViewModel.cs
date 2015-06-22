using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Model;
using Newtonsoft.Json;
using OpenLeczna.DTOs;

namespace LecznaHub.Core.ViewModel
{
    public class StationsViewModel : MvxViewModel
    {

        public async void Init()
        {

        }

        public static async Task<ObservableCollection<StationDto>> GetStationsAsync()
        {
            Downloader stationsDownloader = new Downloader(
                new Uri(Config.OpenLecznaApiEndpoint + "Stations/"));

            var json = await stationsDownloader.GetPageAsync();
            var objects = JsonConvert.DeserializeObject<ObservableCollection<StationDto>>(json);
            return objects;

        }

        public static async Task<ObservableCollection<CityDTO>> GetCitiesAsync()
        {
            Downloader stationsDownloader = new Downloader(
                new Uri(Config.OpenLecznaApiEndpoint + "Cities/"));

            var json = await stationsDownloader.GetPageAsync();
            var objects = JsonConvert.DeserializeObject<ObservableCollection<CityDTO>>(json);
            return objects;
        }

        public static async Task<ObservableCollection<CarrierDTO>> GetCarriersAsync()
        {
            Downloader stationsDownloader = new Downloader(
                new Uri(Config.OpenLecznaApiEndpoint + "Carriers/"));

            var json = await stationsDownloader.GetPageAsync();
            var objects = JsonConvert.DeserializeObject<ObservableCollection<CarrierDTO>>(json);
            return objects;
        }
    }
}
