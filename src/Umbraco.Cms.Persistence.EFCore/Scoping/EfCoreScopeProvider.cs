﻿using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;

namespace Umbraco.Cms.Persistence.EFCore.Scoping;

public class EfCoreScopeProvider : IEfCoreScopeProvider
{
    private readonly IAmbientEfCoreScopeStack _ambientEfCoreScopeStack;
    private readonly IUmbracoEfCoreDatabaseFactory _umbracoEfCoreDatabaseFactory;

    public EfCoreScopeProvider()
        : this(StaticServiceProvider.Instance.GetRequiredService<IAmbientEfCoreScopeStack>(), StaticServiceProvider.Instance.GetRequiredService<IUmbracoEfCoreDatabaseFactory>())
    {
    }
    internal EfCoreScopeProvider(IAmbientEfCoreScopeStack ambientEfCoreScopeStack, IUmbracoEfCoreDatabaseFactory umbracoEfCoreDatabaseFactory)
    {
        _ambientEfCoreScopeStack = ambientEfCoreScopeStack;
        _umbracoEfCoreDatabaseFactory = umbracoEfCoreDatabaseFactory;
    }

    public IEfCoreScope CreateScope()
    {
        var efCoreScope = new EfCoreScope(_umbracoEfCoreDatabaseFactory);
        _ambientEfCoreScopeStack.Push(efCoreScope);
        return efCoreScope;
    }
}