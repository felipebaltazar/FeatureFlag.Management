using FeatureFlag.Management.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;

namespace FeatureFlag.Management.Tests
{
    public abstract class BaseTests
    {
        protected readonly Mock<IFeatureResolver> _featureResolver;
        protected readonly Mock<IFeatureFlagManager> _featureFlagManager;

        protected BaseTests()
        {
            _featureResolver = new Mock<IFeatureResolver>();
            _featureFlagManager = new Mock<IFeatureFlagManager>();

            _featureResolver.Setup(r => r.GetStringForKey(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns<string, string>((key, defaultValue) => defaultValue);

            _featureResolver.Setup(r => r.GetBooleanForKey(It.IsAny<string>(), It.IsAny<bool>()))
                            .Returns<string, bool>((key, defaultValue) => defaultValue);

            _featureResolver.Setup(r => r.GetDoubleForKey(It.IsAny<string>(), It.IsAny<double>()))
                            .Returns<string, double>((key, defaultValue) => defaultValue);

            _featureResolver.Setup(r => r.GetNumberForKey(It.IsAny<string>(), It.IsAny<long>()))
                            .Returns<string, long>((key, defaultValue) => defaultValue);

            _featureResolver.Setup(r => r.DeserializeForKey(It.IsAny<string>(), It.IsAny<Func<List<string>>>()))
                            .Returns<string, Func<List<string>>>((key, defaultValue) => defaultValue());

            _featureFlagManager.Setup(m => m.Get<FakeFeature>())
                               .Returns(new FakeFeature(_featureResolver.Object));
        }

    }
}
