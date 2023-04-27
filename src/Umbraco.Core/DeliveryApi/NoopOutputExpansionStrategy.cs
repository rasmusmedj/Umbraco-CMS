﻿using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Cms.Core.DeliveryApi;

internal sealed class NoopOutputExpansionStrategy : IOutputExpansionStrategy
{
    public IDictionary<string, object?> MapElementProperties(IPublishedElement element)
        => MapProperties(element.Properties);

    public IDictionary<string, object?> MapProperties(IEnumerable<IPublishedProperty> properties)
        => properties.ToDictionary(p => p.Alias, p => p.GetDeliveryApiValue(true));

    public IDictionary<string, object?> MapContentProperties(IPublishedContent content)
        => MapProperties(content.Properties);
}