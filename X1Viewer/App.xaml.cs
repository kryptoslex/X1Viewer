using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using X1Viewer.Services;
using X1Viewer.Views;

namespace X1Viewer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
