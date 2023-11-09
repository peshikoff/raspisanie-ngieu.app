using Android.Widget;
using Try1RASP.CustomControls;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(MessageAndroid))]
namespace Try1RASP.CustomControls
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}