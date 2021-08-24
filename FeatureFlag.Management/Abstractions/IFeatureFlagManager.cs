using System;

namespace FeatureFlag.Management
{
    /// <summary>
    /// Manages the registred features
    /// </summary>
    public interface IFeatureFlagManager
    {
        /// <summary>
        /// Returns a specific feature
        /// </summary>
        /// <typeparam name="TFeature">Feature type</typeparam>
        /// <returns>Instance of feature</returns>
        TFeature Get<TFeature>() where TFeature : class, IFeature;

        /// <summary>
        /// Returns a specific feature
        /// </summary>
        /// <param name="featureType">Feature type</param>
        /// <returns>Instance of feature</returns>
        IFeature Get(Type featureType);
    }
}
