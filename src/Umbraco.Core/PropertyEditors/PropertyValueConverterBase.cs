﻿using System.Collections.Generic;
using System.Xml;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Cms.Core.PropertyEditors;

/// <summary>
///     Provides a default implementation for <see cref="IPropertyValueConverter" />.
/// </summary>
/// <seealso cref="IPropertyValueConverter" />
public abstract class PropertyValueConverterBase : IPropertyValueConverter
{
    /// <inheritdoc />
    public virtual bool IsConverter(IPublishedPropertyType propertyType)
        => false;

    /// <inheritdoc />
    public virtual bool? IsValue(object? value, PropertyValueLevel level)
    {
        switch (level)
        {
            case PropertyValueLevel.Source:
                // the default implementation uses the old magic null & string comparisons,
                // other implementations may be more clever, and/or test the final converted object values
                return value != null && (!(value is string stringValue) || !string.IsNullOrWhiteSpace(stringValue));
            case PropertyValueLevel.Inter:
                return null;
            case PropertyValueLevel.Object:
                return null;
            default:
                throw new NotSupportedException($"Invalid level: {level}.");
        }
    }

    /// <inheritdoc />
    public virtual Type GetPropertyValueType(IPublishedPropertyType propertyType)
        => typeof(object);

    /// <inheritdoc />
    public virtual PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        => PropertyCacheLevel.Snapshot;

    /// <inheritdoc />
    public virtual object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview)
        => source;

    /// <inheritdoc />
    public virtual object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        => inter;

    /// <inheritdoc />
    public virtual object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
    {
        var d = new XmlDocument();
        XmlElement e = d.CreateElement("values");
        d.AppendChild(e);

        if (inter is IEnumerable<string> collection)
        {
            foreach (var value in collection)
            {
                XmlElement ee = d.CreateElement("value");
                ee.InnerText = value;
                e.AppendChild(ee);
            }
        }
        else
        {
            XmlElement ee = d.CreateElement("value");
            ee.InnerText = inter?.ToString() ?? string.Empty;
            e.AppendChild(ee);
        }

        return d.CreateNavigator();
    }

    [Obsolete(
        "This method is not part of the IPropertyValueConverter contract, therefore not used and will be removed in future versions; use IsValue instead.")]
    public virtual bool HasValue(IPublishedProperty property, string culture, string segment)
    {
        var value = property.GetSourceValue(culture, segment);
        return value != null && (!(value is string stringValue) || !string.IsNullOrWhiteSpace(stringValue));
    }
}
