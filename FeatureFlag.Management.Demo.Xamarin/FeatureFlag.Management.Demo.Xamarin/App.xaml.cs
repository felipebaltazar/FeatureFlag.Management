using FeatureFlag.Management.Xamarin.Forms;
using Xamarin.Forms;

namespace FeatureFlag.Management.Demo.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var myResolver = new MyResolver();
            var myFeature = new MyFeature(myResolver);
            var manager = new FeatureFlagManager(myResolver);

            FeatureFlagManager.SetDefaultFeatureBuilder((t) => myFeature);
            FeatureView.SetFeatureManagerResolver(() => manager);

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
