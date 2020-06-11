using AppAzureSQL.Models;
using AppAzureSQL.Services;
using AppAzureSQL.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAzureSQL.ViewModels
{
    public class DriverDetailViewModel : BaseViewModel
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private int id;
        Command _saveCommand;
        public Command SaveCommand => _saveCommand ?? (_saveCommand = new Command(SaveAction));
        Command _deleteCommand;
        public Command DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(DeleteAction));

        Command _getLocationCommand;
        public Command GetLocationCommand => _getLocationCommand ?? (_getLocationCommand = new Command(GetLocationAction));

        Command _TakePictureCommand;
        public Command TakePictureCommand => _TakePictureCommand ?? (_TakePictureCommand = new Command(TakePictureAction));

        Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));



        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        string _Status;
        public string Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }

        double _Latitude;
        public double Latitude
        {
            get => _Latitude;
            set => SetProperty(ref _Latitude, value);
        }

        double _Longitude;
        public double Longitude
        {
            get => _Longitude;
            set => SetProperty(ref _Longitude, value);
        }

        string _Picture;
        public string Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }
        public DriverDetailViewModel()
        {

        }
        public DriverDetailViewModel(DriverModel driver)
        {
            id = driver.IDDriver;
            Name = driver.Name;
            Status = driver.Status;
            Latitude = double.Parse(driver.ActualPosition.Latitude);
            Longitude = double.Parse(driver.ActualPosition.Longitude);
            Picture = driver.Picture;

        }
        private async void SaveAction(object obj)
        {
            IsBusy = true;
            if (id == 0)
            {
                ApiResponse response = await new ApiService().PostDataAsync("driver", new DriverModel
                {
                    Name = this.Name,
                    Status = this.Status,
                    Picture = this.Picture,
                    ActualPosition = new PositionModel
                    {
                        Latitude = this.Latitude.ToString(),
                        Longitude = this.Longitude.ToString()
                    }
                });

                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTrips", "Error al crear el viaje", "OK");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTrips", response.Message, "OK");
                    return;
                }
                //await Task.Delay(5000);
                IsBusy = false;
                ItemsViewModel.GetInstance().RefreshTrips();
                await Application.Current.MainPage.DisplayAlert("Mensaje", "El viaje fue creado exitosamente", "ok");
                await RootPage.NavigateFromMenu(0);
            }
            else
            {

                
                ApiResponse response = await new ApiService().PutDataAsync("driver/2", new DriverModel
                {
                    IDDriver = id,
                    Name = this.Name,
                    Status = this.Status,
                    Picture = this.Picture,
                    ActualPosition = new PositionModel
                    {
                        Latitude = this.Latitude.ToString(),
                        Longitude = this.Longitude.ToString()
                    }
                });

                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTrips", "Error al crear el viaje", "OK");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTrips", response.Message, "OK");
                    return;
                }
                IsBusy = false;
                ItemsViewModel.GetInstance().RefreshTrips();
                await Application.Current.MainPage.DisplayAlert("Mensaje", "El viaje fue actualizado exitosamente", "ok");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }

        }

        private async void DeleteAction(object obj)
        {
            IsBusy = true;
            if (id != 0)
            {
                ApiResponse response = await new ApiService().DeleteDataAsync("driver", id);

                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTrips", "Error al eliminar el viaje", "OK");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTrips", response.Message, "OK");
                    return;
                }
                //await Task.Delay(5000);
                IsBusy = false;
                ItemsViewModel.GetInstance().RefreshTrips();
                await Application.Current.MainPage.DisplayAlert("Mensaje", "El viaje fue eliminado exitosamente", "ok");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("AppTrips", "Error al eliminar el viaje", "OK");
                return;
            }
        }


        private async void GetLocationAction()
        {
            string hola = "hola";
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private async void SelectPictureAction()
        {

            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("ERROR", ":( Seleccionar fotografia no esta disponible.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (file == null)
                return;

            Picture = file.Path;

            //await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");
        }

        private async void TakePictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            Picture = file.Path;
        }
    }
}
