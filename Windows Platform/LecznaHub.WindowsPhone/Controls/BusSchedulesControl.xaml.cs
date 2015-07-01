using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LecznaHub.Core.ViewModel;
using OpenLeczna.DTOs;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LecznaHub.Controls
{
    public sealed partial class BusSchedulesControl : UserControl
    {
        public BusSchedulesControl()
        {            
            this.InitializeComponent();
            //this.ViewModel = new StationsViewModel();
            //Task.Run(new Func<Task>(ViewModel.DownloadAllAsync));
        }


    }
}
