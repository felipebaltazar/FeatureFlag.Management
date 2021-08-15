using Xamarin.Forms;

namespace FeatureFlag.Management.Demo.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            var manager = new FeatureFlagManager(new MyResolver());
            var myFeature = manager.Get<MyFeature>();

            if (myFeature.IsEnabled)
            {
                // do somethng
            }
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
