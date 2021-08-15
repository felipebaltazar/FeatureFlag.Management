
using System.Collections.Generic;
using System.ComponentModel;

namespace FeatureFlag.Management.Tests.Helpers
{
    public class FakeFeature : BaseFeature<FakeFeature>, IFeature<List<string>>
    {
        [DisplayName(Constants.FAKE_FEATURE_IS_ENABLED_KEY)]
        [DefaultValue(Constants.FAKE_FEATURE_IS_ENABLED_VALUE)]
        public override bool IsEnabled
        {
            get;
            protected set;
        }

        [DisplayName(Constants.FAKE_FEATURE_DATA_KEY)]
        [DefaultValue(Constants.FAKE_FEATURE_DATA_VALUE)]
        public List<string> Data
        {
            get;
            private set;
        }

        public FakeFeature(IFeatureResolver featureResolver) : base(featureResolver)
        {
            IsEnabled = GetDefaultValueFor(f => f.IsEnabled);
            Data = GetDefaultValueFor(f => f.Data);
        }
    }
}
