using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Model;
using Newtonsoft.Json;
using OpenLeczna.DTOs;

namespace LecznaHub.Core.ViewModel
{
    public class StationsViewModel : MvxViewModel
    {

        //public async void Init()
        //{

        //}

        public ObservableCollection<StationDto> StationsCollection { get; set; }
        public ObservableCollection<CityDTO> CitiesCollection { get; set; }
        public ObservableCollection<CarrierDTO> CarriersCollection { get; set; }

        public CityDTO ChosenCity { get; set; }
        public StationDto ChosenStation { get; set; }

        public StationsViewModel()
        {
            //Task.Run((Func<Task>)DownloadAllAsync).Wait();
        }

        public async Task DownloadAllAsync()
        {
            this.StationsCollection = await GetStationsAsync();
            this.CarriersCollection = await GetCarriersAsync();
            this.CitiesCollection = await GetCitiesAsync();
            ChosenStation = StationsCollection[0];
        }

        public static async Task<ObservableCollection<StationDto>> GetStationsAsync()
        {
            Downloader stationsDownloader = new Downloader(
                new Uri(Config.OpenLecznaApiEndpoint + "Stations/"));

            var json = await stationsDownloader.GetPageAsync();
            var objects = JsonConvert.DeserializeObject<ObservableCollection<StationDto>>(json);
            return objects;

        }
        public static async Task<StationDetailsDto> GetStationDetailsAsync(string stationName)
        {
            Downloader stationsDownloader = new Downloader(
                new Uri(String.Format("{0}/{1}/",
                Config.OpenLecznaApiEndpoint,
                stationName)));

            var json = await stationsDownloader.GetPageAsync();
            var objects = JsonConvert.DeserializeObject<StationDetailsDto>(json);
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

        public List<CityDTO> GetCitiesSuggestions(string text)
        {
            if (text.Length > 0)
            {
                text = Diacritics.Remove(text);

                var myList = CitiesCollection.Where(x => Diacritics.Remove(x.Name).Contains(text)).ToList();
                if (myList.Count > 0)
                    return myList;
                //else
                //{
                //    return CitiesCollection.ToList();
                //}
            }
            return new List<CityDTO>() { };
        } 

        public List<StationDto> GetStationsSuggestions(string text)
        {
            if (text.Length > 0)
            {
                text = Diacritics.Remove(text);

                var myList = StationsCollection.Where(x => Diacritics.Remove(x.ToString()).Contains(text)).ToList();
                if (myList.Count > 0)
                    return myList;
                //else
                //{
                //    return StationsCollection.ToList();
                //}
            }
            return new List<StationDto>() {};

        }
    }
}
