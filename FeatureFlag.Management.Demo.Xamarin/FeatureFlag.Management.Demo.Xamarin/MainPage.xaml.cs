using Xamarin.Forms;

namespace FeatureFlag.Management.Demo.Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void Disable_Clicked(System.Object sender, System.EventArgs e)
        {
            MyMocker.Instance.SetFeatureEnabledValue(false);
        }

        public void Enable_Clicked(System.Object sender, System.EventArgs e)
        {
            MyMocker.Instance.SetFeatureEnabledValue(true);
        }
    }
}
