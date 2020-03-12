using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace X1Viewer.Utils
{
    public class PopUpInput
    {
        public PopUpInput()
        {
        }

        public static Task<string[]> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string[]>();

            var lblTitle = new Label { Text = "New Device", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "NOTE: Device should be in the same network subnet." };
            var txtInputIP = new Entry { Text = "", Placeholder = "IP Address" };
            var txtInputName = new Entry { Text = "", Placeholder = "Device Name" };

            var btnOk = new Button
            {
                Text = "Register",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                string[] res = new string[]{
                    txtInputIP.Text,
                    txtInputName.Text
                };
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(res);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await navigation.PopModalAsync();
                // pass empty result
                string[] res = new string[]{
                    txtInputIP.Text,
                    txtInputName.Text
                };
                tcs.SetResult(res);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,

                Children = {btnCancel, btnOk},
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInputIP, txtInputName, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInputIP.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
    }
}
