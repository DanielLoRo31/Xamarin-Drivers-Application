using AppAzureSQL.Models;
using AppAzureSQL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAzureSQL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriversDetailPage : ContentPage
    {
        public DriversDetailPage()
        {
            InitializeComponent();
            BindingContext = new DriverDetailViewModel();
        }
        public DriversDetailPage(DriverModel driver)
        {
            InitializeComponent();
            BindingContext = new DriverDetailViewModel(driver);
        }
    }
}