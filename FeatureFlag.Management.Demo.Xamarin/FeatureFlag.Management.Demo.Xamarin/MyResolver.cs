using System;
using RemoteConfig = FeatureFlag.Management.Firebase.RemoteConfig;

namespace FeatureFlag.Management.Demo.Xamarin
{
    // Resolver as example for RemoteConfig
    public class MyResolver : IFeatureResolver
    {
        public MyResolver()
        {
            // use this static class to set RemoteConfig default values
            var defaultValues = RemoteConfig.DefaultValues;
        }

        public TResult DeserializeForKey<TResult>(string key, Func<TResult> defaultValue) where TResult : class
        {
            try
            {
                //RemoteConfig example
                //var serializedValue = RemoteConfig.GetString(key);
                //JsonConvert.Deserialize<TResult>(serializedValue);
            }
            catch (Exception ex)
            {
                // log
            }

            return defaultValue();
        }

        public bool GetBooleanForKey(string key, bool defaultValue)
        {
            try
            {
                //RemoteConfig example
                // return RemoteConfig.GetBoolean(key);
            }
            catch (Exception ex)
            {
                // log
            }

            return defaultValue;
        }

        public double GetDoubleForKey(string key, double defaultValue)
        {
            try
            {
                //RemoteConfig example
                // return RemoteConfig.GetDouble(key);
            }
            catch (Exception ex)
            {
                // log
            }

            return defaultValue;
        }

        public long GetNumberForKey(string key, long defaultValue)
        {
            try
            {
                //RemoteConfig example
                // return RemoteConfig.GetNumber(key);
            }
            catch (Exception ex)
            {
                // log
            }

            return defaultValue;
        }

        public string GetStringForKey(string key, string defaultValue)
        {
            try
            {
                //RemoteConfig example
                // return RemoteConfig.GetString(key);
            }
            catch (Exception ex)
            {
                // log
            }

            return defaultValue;
        }
    }
}
