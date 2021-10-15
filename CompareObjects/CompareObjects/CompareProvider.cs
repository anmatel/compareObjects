using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CompareObjects;
using CompareObjects.Contracts;
using InnerDifference = CompareObjects.Difference;

namespace CompareNetObjects
{
    public class CompareProvider<T> : ICompareProvider<T>
    {
        private readonly CompareLogic _compareLogic;
        private readonly CompareSettings _compareSettings;
        private readonly string _defaultText;

        private const string DefaultText = "[Name] changed from [PreviousValue] to [NextValue]";

        public CompareProvider(ICompareRule<T> rule, CompareBuilder<T> builder)
        {
            var compareRule = rule ?? throw new ArgumentNullException(nameof(rule));
            var compareBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
            _compareSettings = compareRule.OnConfigure(compareBuilder);

            _compareLogic = new CompareLogic
            {
                Config =
                {
                    CaseSensitive = _compareSettings.CaseSensitive,
                    CompareChildren = _compareSettings.CompareChildren
                }
            };
            if (_compareSettings.IgnoredFields.Any())
                _compareLogic.Config.MembersToIgnore = _compareSettings.IgnoredFields;
            _defaultText = _compareSettings.CustomDefaultText ?? DefaultText;
        }

        public List<InnerDifference> GetDifferences(T object1, T object2)
        {
            return GetDifferenceList(object1, object2);
        }

        public string GetDifferencesText(T object1, T object2)
        {
            var result=string.Empty;
            var diffList = GetDifferenceList(object1, object2);
            return diffList.Aggregate(result, (current, item) => current + $"{item.ReadableText}\n");
        }

        public bool IsEqual(T object1, T object2)
        {
            return _compareLogic.Compare(object1, object2).AreEqual;
        }

        private List<InnerDifference> GetDifferenceList(T object1, T object2)
        {
            _compareLogic.Config.MaxDifferences = object1.GetType().GetProperties().Length;
            var result = new List<InnerDifference>();
            var differences = _compareLogic.Compare(object1, object2).Differences;

            const string namePattern = "\\[Name\\]";
            const string previousValuePattern = "\\[PreviousValue\\]";
            const string nextValuePattern = "\\[NextValue\\]";

            foreach (var item in differences)
            {
                var difference = new InnerDifference
                {
                    PropertyName = item.PropertyName,
                    PreviousPropertyValue = item.Object1Value,
                    NextPropertyValue = item.Object2Value,
                    ReadableText =
                        _compareSettings.DefaultPropertyTexts.TryGetValue(item.PropertyName, out var defaultPropValue)
                            ? defaultPropValue
                            : _defaultText
                };


                item.PropertyName = _compareSettings.PropertiesDisplayNames.TryGetValue(item.PropertyName, out var displayName) ? displayName : item.PropertyName;
                    difference.ReadableText = Regex.Replace(difference.ReadableText, namePattern, item.PropertyName);
                difference.ReadableText = Regex.Replace(difference.ReadableText, previousValuePattern, difference.PreviousPropertyValue.ToString() ?? string.Empty);
                difference.ReadableText = Regex.Replace(difference.ReadableText, nextValuePattern, difference.NextPropertyValue.ToString() ?? string.Empty);

                result.Add(difference);
            }
            return result;
        }
    }
}
