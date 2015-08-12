using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using LecznaHub.Core.Model;
using OpenLeczna.DTOs;

namespace LecznaHub.Core.ViewModel
{
    /// <summary>
    /// This ViewModel contains all stuff needed for tabbed application
    /// </summary>
    public class TabbedViewModel : MvxViewModel
    {
        public NewsViewModel News { get; private set; }
        public TransportViewModel Transport { get; private set; }
        public UniversalNewsViewModel UniversalNewsViewModel { get; private set; }

        public TabbedViewModel()
        {
            this.News = new NewsViewModel();
            this.Transport = new TransportViewModel();
            this.UniversalNewsViewModel = new UniversalNewsViewModel();
        }

        public async Task DownloadViewModelsDataAsync()
        {
            await this.News.GetNewsDataAsync();
            await this.UniversalNewsViewModel.InitializeAsync();
            await this.Transport.GetTransportDataAsync();
        }
    }
}
