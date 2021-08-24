using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace FeatureFlag.Management
{
    /// <inheritdoc/>
    public abstract class BaseFeature<TFeature> : IFeature where TFeature : BaseFeature<TFeature>
    {
        /// <summary>
        /// Service that will resolve feature properties in cloud like Azure or RemoteConfig
        /// </summary>
        protected virtual IFeatureResolver FlagResolver { get; }

        /// <inheritdoc/>
        public event EventHandler OnEnabledChanged;

        private bool _isEnabled;

        /// <inheritdoc/>
        public virtual bool IsEnabled
        {
            get => _isEnabled;
            protected set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                OnEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="flagResolver">Service that will resolve feature properties in cloud like Azure or RemoteConfig</param>
        protected BaseFeature(IFeatureResolver flagResolver) => FlagResolver = flagResolver;


        #region Protected methods

        /// <summary>
        /// Parse the default value from attribute or based on type
        /// </summary>
        /// <param name="getter">Expression that contains the property to extract default value</param>
        /// <returns>Default value for specified property</returns>
        protected virtual bool GetDefaultValueFor(Expression<Func<TFeature, bool>> getter)
        {
            (string key, bool defaultValue) = ExtractStructFeatureSettings(getter);
            return FlagResolver.GetBooleanForKey(key, defaultValue);
        }

        /// <summary>
        /// Parse the default value from attribute or based on type
        /// </summary>
        /// <param name="getter">Expression that contains the property to extract default value</param>
        /// <returns>Default value for specified property</returns>
        protected virtual long GetDefaultValueFor(Expression<Func<TFeature, long>> getter)
        {
            (string key, long defaultValue) = ExtractStructFeatureSettings(getter);
            return FlagResolver.GetNumberForKey(key, defaultValue);
        }

        /// <summary>
        /// Parse the default value from attribute or based on type
        /// </summary>
        /// <param name="getter">Expression that contains the property to extract default value</param>
        /// <returns>Default value for specified property</returns>
        protected virtual double GetDefaultValueFor(Expression<Func<TFeature, double>> getter)
        {
            (string key, double defaultValue) = ExtractStructFeatureSettings(getter);
            return FlagResolver.GetDoubleForKey(key, defaultValue);
        }

        /// <summary>
        /// Parse the default value from attribute or based on type
        /// </summary>
        /// <param name="getter">Expression that contains the property to extract default value</param>
        /// <returns>Default value for specified property</returns>
        protected virtual string GetDefaultValueFor(Expression<Func<TFeature, string>> getter)
        {
            (string key, string defaultValue) = ExtractClassFeatureSettings(getter);
            return FlagResolver.GetStringForKey(key, defaultValue);
        }

        /// <summary>
        /// Parse the default value from attribute or based on type
        /// </summary>
        /// <typeparam name="TResult">Complex data object type</typeparam>
        /// <param name="getter">Expression that contains the property to extract default value</param>
        /// <returns>Deserialized default value for specified property</returns>
        protected virtual TResult GetDefaultValueFor<TResult>(Expression<Func<TFeature, TResult>> getter) where TResult : class, new()
        {
            var memberExpression = AssertExpressionBody(getter);
            var property = AssertExpressionMember(memberExpression);
            var key = ExtractFeatureKey(property);
            var serializedValue = ExtractDefaultValue(property) as string;

            return FlagResolver.DeserializeForKey(key, () => JsonConvert.DeserializeObject<TResult>(serializedValue));
        }

        #endregion

        #region Private Mathods

        private (string Key, TResult defaultValue) ExtractStructFeatureSettings<TResult>(Expression<Func<TFeature, TResult>> getter) where TResult : struct
        {
            var memberExpression = AssertExpressionBody(getter);
            var property = AssertExpressionMember(memberExpression);
            return (Key: ExtractFeatureKey(property), (ExtractDefaultValue(property) as TResult?) ?? default(TResult));
        }

        private (string Key, TResult defaultValue) ExtractClassFeatureSettings<TResult>(Expression<Func<TFeature, TResult>> getter) where TResult : class
        {
            var memberExpression = AssertExpressionBody(getter);
            var property = AssertExpressionMember(memberExpression);
            return (Key: ExtractFeatureKey(property), (ExtractDefaultValue(property) as TResult) ?? default(TResult));
        }

        private static object ExtractDefaultValue(PropertyInfo property)
        {
            var defaultValueAttr = property.GetCustomAttribute<DefaultValueAttribute>(false);
            if (!(defaultValueAttr is null))
                return defaultValueAttr.Value;

            return null;
        }

        private string ExtractFeatureKey(PropertyInfo property)
        {
            var key = $"{GetType().Name}_{property.Name}";
            var flagNameAttr = property.GetCustomAttribute<DisplayNameAttribute>(false);
            if (!(flagNameAttr is null))
                key = flagNameAttr.DisplayName;

            return key;
        }

        private PropertyInfo AssertExpressionMember(MemberExpression memberExpression)
        {
            if (!(memberExpression.Member is PropertyInfo property))
                throw new InvalidCastException("Expression member must be a Property!");

            return GetType().GetProperty(property.Name);
        }

        private static MemberExpression AssertExpressionBody<TResult>(Expression<Func<TFeature, TResult>> getter)
        {
            if (!(getter.Body is MemberExpression memberExpression))
                throw new InvalidOperationException("Expression must be a MemberExpression");

            return memberExpression;
        }

        #endregion
    }
}