using System;

namespace FeatureFlag.Management
{
    /// <summary>
    /// Resolver that will get values from cloud services like Azure or RemoteConfig
    /// </summary>
    public interface IFeatureResolver
    {
        /// <summary>
        /// Resolve a number value for a specified key
        /// </summary>
        /// <param name="key">key of feature</param>
        /// <param name="defaultValue">default value to use if needed</param>
        /// <returns>Value number</returns>
        long GetNumberForKey(string key, long defaultValue);

        /// <summary>
        /// Resolve a boolean value for a specified key
        /// </summary>
        /// <param name="key">key of feature</param>
        /// <param name="defaultValue">default value to use if needed</param>
        /// <returns>Value number</returns>
        bool GetBooleanForKey(string key, bool defaultValue);

        /// <summary>
        /// Resolve a double value for a specified key
        /// </summary>
        /// <param name="key">key of feature</param>
        /// <param name="defaultValue">default value to use if needed</param>
        /// <returns>Value double</returns>
        double GetDoubleForKey(string key, double defaultValue);

        /// <summary>
        /// Resolve a string value for a specified key
        /// </summary>
        /// <param name="key">key of feature</param>
        /// <param name="defaultValue">default value to use if needed</param>
        /// <returns>Value string</returns>
        string GetStringForKey(string key, string defaultValue);

        /// <summary>
        /// Resolve a complex value for a specified key
        /// </summary>
        /// <param name="key">key of feature</param>
        /// <param name="defaultValue">Function that will create a default value to use if needed</param>
        /// <returns>Complex value deserialized</returns>
        TResult DeserializeForKey<TResult>(string key, Func<TResult> defaultValue) where TResult : class;
    }
}
