using System;
using System.ComponentModel;

namespace FeatureFlag.Management.Xamarin.Forms
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class PreserveFeatureFlagManagementXamarinFormsAttribute : Attribute
    {
    }
}