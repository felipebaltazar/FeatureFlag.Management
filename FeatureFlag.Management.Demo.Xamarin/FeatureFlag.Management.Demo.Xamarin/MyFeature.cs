using System.Collections.Generic;
using System.ComponentModel;

namespace FeatureFlag.Management.Demo.Xamarin
{
    public class MyFeature : BaseFeature<MyFeature>, IFeature<List<string>>
    {
        [DefaultValue(true)]
        [DisplayName("myfeature_enabled")]
        public override bool IsEnabled
        {
            get;
            protected set;
        }

        [DefaultValue("[\"1\", \"2\", \"3\"]")]
        [DisplayName("myfeature_data")]
        public List<string> Data
        {
            get;
            protected set;
        }

        public MyFeature(IFeatureResolver flagResolver) : base(flagResolver)
        {
            // Here you can set others dependencies to mock or extend this functionality
            // eg.:  IMockService myMockService or IEventAggregatoor eventAggregator
            IsEnabled = GetDefaultValueFor(@this => @this.IsEnabled);
            Data = GetDefaultValueFor(@this => @this.Data);
        }
    }
}
