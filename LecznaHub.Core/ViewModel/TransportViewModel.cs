﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Annotations;
using LecznaHub.Core.Helpers;
using LecznaHub.Core.Model;
using Newtonsoft.Json;
using OpenLeczna.DTOs;
using static LecznaHub.Core.Helpers.DateTimeHelper;

namespace LecznaHub.Core.ViewModel
{
    public class TransportViewModel : MvxViewModel, INotifyPropertyChanged
    {

        public TransportViewModel()
        {
            this.StationsCollection = new ObservableCollection<StationDto>();
            this.CitiesCollection = new ObservableCollection<CityDTO>();
            this.CarriersCollection = new ObservableCollection<CarrierDTO>();
            this.ChosenStation = new StationDto();
            this.ChosenCity = new CityDTO();
        }

        public ObservableCollection<StationDto> StationsCollection { get; set; }
        public ObservableCollection<CityDTO> CitiesCollection { get; set; }
        public ObservableCollection<CarrierDTO> CarriersCollection { get; set; }

        private CityDTO _chosenCity;
        private StationDto _chosenStation;
        private StationDetailsDto _chosenStationDetails;
        private string _busDeparturesGlanceString;

        public CityDTO ChosenCity
        {
            get { return _chosenCity; }
            set
            {
                if (Equals(value, _chosenCity)) return;
                _chosenCity = value;
                BuildBusDepartureStringAsync();
                OnPropertyChanged();
            }
        }

        public StationDto ChosenStation
        {
            get { return _chosenStation; }
            set
            {
                if (Equals(value, _chosenStation)) return;
                _chosenStation = value;
                BuildBusDepartureStringAsync();
                OnPropertyChanged();             
            }   
        }

        public string BusDeparturesGlanceString
        {
            get { return _busDeparturesGlanceString; }
            set
            {
                if (value == _busDeparturesGlanceString) return;
                _busDeparturesGlanceString = value;
                BuildBusDepartureStringAsync();
                OnPropertyChanged();               
            }
        }

        private async Task<StationDetailsDto> DownloadStationDetailsAsync(string stationName, string city)
        {
            Downloader stationsDownloader = new Downloader(
                new Uri(String.Format("{0}Stations?name={1}&city={2}",
                Config.OpenLecznaApiEndpoint,
                stationName, city)));

            var json = await stationsDownloader.GetPageAsync();
            var details = JsonConvert.DeserializeObject<StationDetailsDto>(json);
            return details;
        }

        private async void BuildBusDepartureStringAsync()
        {
            if (_chosenStation == null && _chosenCity == null)
                return;
            //Download station details
            if (string.IsNullOrWhiteSpace(ChosenStation.Name) || string.IsNullOrWhiteSpace(ChosenCity.Name)) return;
            StationDetailsDto stationDetails = await DownloadStationDetailsAsync(ChosenStation.Name, ChosenStation.City);

            if(stationDetails.Schedules.Count == 0)
                throw new ArgumentNullException("stationDetails", "This station contains no schedules");
            //Select departures only to chosen city
            //var schedulesToDestination = from s in stationDetails.Schedules
            //    where s.DestinationCity == ChosenCity.Name &&
            //          s.ApplicableDays == "Weekdays"
            //    select s;
            var schedulesToDestination = stationDetails.Schedules
                .Where(x => x.DestinationCity == ChosenCity.Name &&
                            x.ApplicableDays == "Weekdays").ToList();
            var departures = schedulesToDestination.SelectMany(x => x.Departures).ToList();
            var nowTime = DateTime.Now;

            var times =
                departures.Where(x => 
                CompareStringAndDateTime(x.Time, nowTime) == DateTimeCompared.SecondParamIsEarlier)
                .ToList();
            times = times.OrderBy(x => DateTime.Parse(x.Time)).ToList();

            BusDeparturesGlanceString = String.Format("Najbliższe busy odjeżdzają o {0}, {1}, {2}. Ostatni o {3}", 
                times[0], times[1], times[2], times.Last());


            //var schedulesToDestination = stationDetails.Schedules.SelectMany<IEnumerable<ScheduleDetailsDTO>>(x => x.DestinationCity == ChosenCity.Name);

            //var departuresToDestination = from s in stationDetails.Schedules
            //    where s.DestinationCity == ChosenCity.Name
            //          && s.ApplicableDays == "Weekdays"
            //    select s;
            //Select departures from

            //var departures = departuresToDestination.Where(
            //    sch => sch.Departures.Where(
            //        departure => 
            //            CompareStringAndDateTime(departure.Time, nowTime) == DateTimeCompared.SecondParamIsEarlier
            //            )
            //var departures = from schedule in departuresToDestination
            //    select schedule.Departures;
        }


        //public StationsViewModel()
        //{
        //    Task.Run((Func<Task>)DownloadAllAsync).Wait();
        //}


        /// <summary>
        /// Downloads transport data from OpenLeczna website
        /// </summary>
        /// <returns></returns>
        public async Task GetTransportDataAsync()
        {
            this.StationsCollection = await GetStationsAsync();
            this.CarriersCollection = await GetCarriersAsync();
            this.CitiesCollection = await GetCitiesAsync();
            //TODO: Load/Save chosen stuff
            ChosenStation = StationsCollection[0];
            ChosenCity = CitiesCollection[1];
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
