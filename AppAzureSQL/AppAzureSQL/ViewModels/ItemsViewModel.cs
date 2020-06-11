using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAzureSQL.Models;
using AppAzureSQL.Services;
using AppAzureSQL.Views;

namespace AppAzureSQL.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        static ItemsViewModel _instance;

        ObservableCollection<DriverModel> drivers;
        public ObservableCollection<DriverModel> Drivers {
            get => drivers;
            set => SetProperty(ref drivers, value);
        }
        public Command LoadDriversCommand { get; set; }

        Command _modifyCommand;
        public Command ModifyCommand => _modifyCommand ?? (_modifyCommand = new Command(ModifyAction));

        DriverModel _DriverSelected;
        public DriverModel DriverSelected
        {
            get => _DriverSelected;
            set => SetProperty(ref _DriverSelected, value);
        }



        public ItemsViewModel()
        {
            Title = "Inicio";
            Drivers = new ObservableCollection<DriverModel>();
            LoadDriversCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                
                LoadTrips();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadTrips()
        {
            Drivers.Clear();
            ApiResponse response = await new ApiService().GetDataAsync<DriverModel>("driver");
            if (response != null || response.IsSuccess)
            {
                Debug.WriteLine("response.Result: " + response.Result.ToString());
                Drivers = (ObservableCollection<DriverModel>)response.Result;
            }
        }
        private void ModifyAction()
        {
            if (DriverSelected != null)
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DriversDetailPage(DriverSelected as DriverModel)));
            }
            
        }
        public static ItemsViewModel GetInstance()
        {
            if (_instance == null) _instance = new ItemsViewModel();
            return _instance;
        }
        public void RefreshTrips()
        {
            LoadTrips();
        }

    }
}