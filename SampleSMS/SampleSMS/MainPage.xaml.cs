using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SampleSMS
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string _lat = "";
        private string _lon = "";
        private string _phoneNumber = "";

        public MainPage()
        {
            InitializeComponent();
        }

        async void btnSendSms_Clicked(object sender, EventArgs e)
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Sms>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.Sms>();
            }

            await SendSms();
        }

        public async Task SendSms()
        {
            try
            {
                var message = new SmsMessage(
                    $@"http://www.google.com/maps/dir/?api=1&travelmode=driving&layer=traffic&destination={_lat},{_lon}",
                    $"{_phoneNumber}");
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Failed", "Sms is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", ex.Message, "OK");
            }
        }

        private async void btnSendSmsSilently_Clicked(object sender, EventArgs e)
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Sms>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.Sms>();
            }

            try
            {
                if (CrossMessaging.Current.SmsMessenger.CanSendSmsInBackground)
                {
                    CrossMessaging.Current.SmsMessenger.SendSmsInBackground(
                        $"{_phoneNumber}",
                        $@"http://www.google.com/maps/dir/?api=1&travelmode=driving&layer=traffic&destination={_lat},{_lon}");
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Failed", "Sms is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", ex.Message, "OK");
            }
        }
    }
}
