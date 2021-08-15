using FeatureFlag.Management.Tests.Helpers;
using RemoteConfig = FeatureFlag.Management.Firebase.RemoteConfig;
using Xunit;
using System.ComponentModel;

namespace FeatureFlag.Management.Tests
{
    [Category("Generator")]
    public class GeneratorTests : BaseTests
    {

        public GeneratorTests() : base()
        {
        }

        [Fact(DisplayName = "Generator deve conseguir resolver nomes customizados para propriedades")]
        public void Generator_Should_Resolve_Custom_Properties_Names()
        {
            var defaultValues = RemoteConfig.DefaultValues;

            Assert.Equal(Constants.FAKE_FEATURE_IS_ENABLED_VALUE, defaultValues[Constants.FAKE_FEATURE_IS_ENABLED_KEY]);
            Assert.Equal(Constants.FAKE_FEATURE_DATA_VALUE, defaultValues[Constants.FAKE_FEATURE_DATA_KEY]);
        }
    }
}
