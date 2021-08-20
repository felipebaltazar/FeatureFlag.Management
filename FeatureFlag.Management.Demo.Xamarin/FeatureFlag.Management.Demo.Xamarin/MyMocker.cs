using System;
using System.Collections.Generic;

namespace FeatureFlag.Management.Demo.Xamarin
{
    public class MyMocker
    {
        private static readonly Lazy<MyMocker> _instance = new Lazy<MyMocker>(() => new MyMocker(), true);

        public static MyMocker Instance
        {
            get => _instance.Value;
        }

        private readonly List<MyFeature> _instances = new List<MyFeature>();

        internal void SetFeatureEnabledValue(bool isFeatureEnabled)
        {
            foreach(var feature in _instances)
            {
                feature.SetIsEnabled(isFeatureEnabled);
            }
        }

        internal void Observe(MyFeature myFeature)
        {
            _instances.Add(myFeature);
        }
    }
}
