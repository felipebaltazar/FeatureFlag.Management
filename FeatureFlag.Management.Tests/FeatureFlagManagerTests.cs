using FeatureFlag.Management.Tests.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace FeatureFlag.Management.Tests
{
    [Category("Manager")]
    public class FeatureFlagManagerTests : BaseTests
    {
        public FeatureFlagManagerTests() : base()
        {
        }

        [Fact(DisplayName = "Manager deve conseguir resolver nomes customizados para propriedades")]
        public void Manager_Should_Resolve_Custom_Properties_Names()
        {
            var manager = new FeatureFlagManager(_featureResolver.Object);
            var fakefeature = manager.Get<FakeFeature>();
            var deserializedValue = JsonConvert.DeserializeObject<List<string>>(Constants.FAKE_FEATURE_DATA_VALUE);

            Assert.Equal(deserializedValue.Count, fakefeature.Data.Count);
            Assert.True(fakefeature.Data.All(d => deserializedValue.Contains(d)));
            Assert.Equal(Constants.FAKE_FEATURE_IS_ENABLED_VALUE, fakefeature.IsEnabled);
        }
    }
}
