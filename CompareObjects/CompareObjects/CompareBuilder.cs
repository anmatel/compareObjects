using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CompareObjects
{
    public class CompareBuilder<T>
    {
        private readonly CompareSettings _settings;

        public CompareBuilder()
        {
            _settings = new CompareSettings
            {
                DefaultPropertyTexts = new Dictionary<string, string>(),
                IgnoredFields = new List<string>(),
                PropertiesDisplayNames = new Dictionary<string, string>()
            };
        }

        public CompareSettings Build()
        {
            return _settings;
        }

        public CompareBuilder<T> CaseSensitiveOn()
        {
            _settings.CaseSensitive = true;
            return this;
        }

        public CompareBuilder<T> RecursiveOn()
        {
            _settings.CompareChildren = true;
            return this;
        }

        public CompareBuilder<T> IgnoreField(Expression<Func<T, object>> expression)
        {
            var name = GetPropertyName(expression);
            _settings.IgnoredFields.Add(name);
            return this;
        }

        public CompareBuilder<T> SetDefaultText(string text)
        {
            _settings.CustomDefaultText = text;
            return this;
        }

        /// <summary>
        /// Use '[Name]', '[PreviousValue]' or '[NextValue]' in text to replace it for property name, previous value of the property or next value
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="text"></param>
        /// <returns>CompareBuilder<typeparamref name="T"/></returns>
        public CompareBuilder<T> SetTextForProperty(Expression<Func<T, object>> expression, string text)
        {
            var name = GetPropertyName(expression);
            _settings.DefaultPropertyTexts.Add(name, text);
            return this;
        }

        /// <summary>
        /// '[Name]' pattern in readable text will be replaced with specified display name instead of property name
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="text"></param>
        /// <returns>CompareBuilder<typeparamref name="T"/></returns>
        public CompareBuilder<T> SetDisplayNameForProperty(Expression<Func<T, object>> expression, string text)
        {
            var name = GetPropertyName(expression);
            _settings.PropertiesDisplayNames.Add(name, text);
            return this;
        }

        private string GetPropertyName(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression ?? (MemberExpression)((UnaryExpression)expression.Body).Operand;
            return memberExpression.Member.Name;
        }
    }
}
