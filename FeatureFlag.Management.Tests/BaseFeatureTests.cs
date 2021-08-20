using FeatureFlag.Management.Tests.Helpers;
using System.ComponentModel;
using System.Threading;
using Xunit;

namespace FeatureFlag.Management.Tests
{
    [Category("BaseFeature")]
    public sealed class BaseFeatureTests : BaseTests
    {
        public BaseFeatureTests() : base()
        {
        }

        [Fact(DisplayName = "BaseFeature deve notificar quando ouver alteração na propriedade Enabled")]
        public void BaseFeature_Should_Notify_When_Enabled_Changes()
        {
            var eventWaiter = new ManualResetEvent(false);
            var fakefeature = new FakeFeature(_featureResolver.Object);
            fakefeature.OnEnabledChanged += (s, e) => eventWaiter.Set();


            Assert.Equal(Constants.FAKE_FEATURE_IS_ENABLED_VALUE, fakefeature.IsEnabled);

            fakefeature.UpdateIsEnabled(false);
            eventWaiter.WaitOne(20);

            Assert.False(fakefeature.IsEnabled);
        }
    }
}
