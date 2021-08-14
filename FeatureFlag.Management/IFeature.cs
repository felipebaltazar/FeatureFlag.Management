namespace FeatureFlag.Management
{
    /// <summary>
    /// Defines that some object is a feature
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// returns if feature is enabled or not
        /// </summary>
        bool IsEnabled { get; }
    }

    /// <summary>
    /// Defines that some object is a feature with complex data
    /// </summary>
    /// <typeparam name="T">Feature data type</typeparam>
    public interface IFeature<T> : IFeature
    {
        /// <summary>
        /// Returns the feature complex data
        /// </summary>
        T Data { get; }
    }
}
