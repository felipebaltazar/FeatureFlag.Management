using System;

namespace FeatureFlag.Management
{
    /// <inheritdoc/>
    public class FeatureFlagManager : IFeatureFlagManager
    {
        private static Func<Type, object> _featureBuilder;
        private readonly IFeatureResolver _flagResolver;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="flagResolver">Resolver that implements flag services like Azure or RemoteConfig</param>
        public FeatureFlagManager(IFeatureResolver flagResolver) => _flagResolver = flagResolver;

        /// <inheritdoc/>
        public TFeature Get<TFeature>() where TFeature : class, IFeature
        {
            var featureType = typeof(TFeature);
            return ResolveForType<TFeature>(featureType);
        }

        /// <summary>
        /// Resolve the instance for a specified feature
        /// </summary>
        /// <typeparam name="TFeature">Feature result type</typeparam>
        /// <param name="featureType">Feature type to resolve</param>
        /// <returns></returns>
        protected virtual TFeature ResolveForType<TFeature>(Type featureType) where TFeature : class, IFeature
        {
            return (_featureBuilder?.Invoke(featureType)
                    ?? Activator.CreateInstance(featureType, new[] { _flagResolver })) as TFeature;
        }

        /// <summary>
        /// Set a default resolver to build (or get) instances of features objects
        /// You can use it as resolver for your dependency injection container
        /// </summary>
        /// <param name="featureBuilder">Method that you resolve the instance of feature objects</param>
        public static void SetDefaultFeatureBuilder(Func<Type, object> featureBuilder) =>
            _featureBuilder = featureBuilder;
    }
}
