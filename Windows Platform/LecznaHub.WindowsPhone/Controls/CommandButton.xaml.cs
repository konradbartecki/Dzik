using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LecznaHub.Controls
{
    public sealed partial class CommandButton : UserControl
    {
        public CommandButton()
        {
            this.InitializeComponent();
        }

        // IsSpinningProperty is the dependency property identifier
        // no need for info in the last PropertyMetadata parameter, so we pass null
        //public static readonly DependencyProperty ItemNameProperty =
        //    DependencyProperty.Register(
        //        "ItemName", typeof(string), typeof(CommandButton), null
        //    );
        //// The property wrapper, so that callers can use this property through a simple ExampleClassInstance.IsSpinning usage rather than requiring property system APIs
        //public string ItemName
        //{
        //    get { return (string)GetValue(ItemNameProperty); }
        //    set { SetValue(ItemNameProperty, value); }
        //}



        public string ItemName
        {
            get { return ItemNameBox.Text; }
            set { ItemNameBox.Text = value; }
        }


        public string Title
        {
            get { return this.TitleBox.Text; }
            set { this.TitleBox.Text = value; }
        }

        //public string ItemName { get; set; }
    }
}
