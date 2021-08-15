# FeatureFlag.Management
Control your features with this escalable, mockable and decoupled sdk

[![NuGet](https://img.shields.io/nuget/v/FeatureFlag.Management.svg)](https://www.nuget.org/packages/FeatureFlag.Management/)
[![NuGet](https://img.shields.io/nuget/v/FeatureFlag.Management.Firebase.svg)](https://www.nuget.org/packages/FeatureFlag.Management.Firebase/)


## Getting started

- Install the FeatureFlag.Management package

 ```
 Install-Package FeatureFlag.Management -Version 0.0.1-pre
 ```

- You can get your features using the FeatureFlagManager (building new instance or DI)

```csharp
var manager = new FeatureFlagManager(new MyResolver());
var myFeature = manager.Get<MyFeature>();

if (myFeature.IsEnabled)
{
    // do somethng
}
```