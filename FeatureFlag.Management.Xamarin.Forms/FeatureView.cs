using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace FeatureFlag.Management.Xamarin.Forms
{
    public class FeatureView : ContentView
    {
        private static Func<IFeatureFlagManager> _featureFlagManagerResolver;
        private readonly IFeatureFlagManager _featureFlagManager;

        public static readonly BindableProperty EnabledViewProperty =
            BindableProperty.Create(
                nameof(EnabledView),
                typeof(View),
                typeof(FeatureView),
                null);

        public static readonly BindableProperty DisabledViewProperty =
            BindableProperty.Create(
                nameof(DisabledView),
                typeof(View),
                typeof(FeatureView),
                null);

        public static readonly BindableProperty FeatureTypeProperty =
            BindableProperty.Create(
                nameof(FeatureType),
                typeof(Type),
                typeof(FeatureView),
                null);

        /// <summary>
        /// View to be presented when feature is enabled
        /// </summary>
        public View EnabledView
        {
            get => (View)GetValue(EnabledViewProperty);
            set => SetValue(EnabledViewProperty, value);
        }

        /// <summary>
        /// View to be presented when feature is disabled
        /// </summary>
        public View DisabledView
        {
            get => (View)GetValue(DisabledViewProperty);
            set => SetValue(DisabledViewProperty, value);
        }

        /// <summary>
        /// Feature type
        /// </summary>
        public Type FeatureType
        {
            get => (Type)GetValue(FeatureTypeProperty);
            set => SetValue(FeatureTypeProperty, value);
        }

        public FeatureView() : base()
        {
            Margin = new Thickness(0);
            Padding = new Thickness(0);
            BackgroundColor = Color.Transparent;

            _featureFlagManager = _featureFlagManagerResolver?.Invoke()
                ?? throw new NotSupportedException("You should call View.SetFeatureManagerResolver() before start using FeatureView");
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == FeatureTypeProperty.PropertyName)
                OnFeatureTypeChanged(FeatureType);
            else if (propertyName == EnabledViewProperty.PropertyName)
                OnEnabledViewChanged(EnabledView, FeatureType);
            else if (propertyName == DisabledViewProperty.PropertyName)
                OnDisabledViewChanged(DisabledView, FeatureType);
        }

        /// <summary>
        /// Called when EnabledView changes
        /// </summary>
        /// <param name="newEnabledView">New view to be used when feature is enabled</param>
        /// <param name="featureType">Feature type</param>
        protected virtual void OnEnabledViewChanged(View newEnabledView, Type featureType)
        {
            var isEnabled = IsFeatureEnabled(featureType);
            if (isEnabled)
                Content = newEnabledView;
        }

        /// <summary>
        /// Called when DisabledView changes
        /// </summary>
        /// <param name="newDisabledView"></param>
        /// <param name="featureType"></param>
        protected virtual void OnDisabledViewChanged(View newDisabledView, Type featureType)
        {
            var isEnabled = IsFeatureEnabled(featureType);
            if (!isEnabled)
                Content = newDisabledView;
        }

        protected virtual void OnFeatureTypeChanged(Type featureType)
        {
            var isEnabled = IsFeatureEnabled(featureType);
            Content = isEnabled ? EnabledView : DisabledView;
        }

        private bool IsFeatureEnabled(Type featureType)
        {
            if (featureType is null)
                return false;

            var feature = _featureFlagManager.Get(featureType);

            if(feature != null)
            {
                feature.OnEnabledChanged -= OnEnabledChanged;
                feature.OnEnabledChanged += OnEnabledChanged;
            }

            return feature?.IsEnabled ?? false;
        }

        private void OnEnabledChanged(object sender, EventArgs e)
        {
            if(sender is IFeature feature)
            {
                var isEnabled = feature.IsEnabled;
                Content = isEnabled ? EnabledView : DisabledView;
            }
        }

        public static void SetFeatureManagerResolver(Func<IFeatureFlagManager> resolver) => _featureFlagManagerResolver = resolver;
    }
}
