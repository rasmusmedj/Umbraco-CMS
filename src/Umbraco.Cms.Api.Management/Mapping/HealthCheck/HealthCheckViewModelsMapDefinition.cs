using Umbraco.Cms.Api.Common.ViewModels.Pagination;
using Umbraco.Cms.Api.Management.ViewModels.HealthCheck;
using Umbraco.Cms.Core.HealthChecks;
using Umbraco.Cms.Core.Mapping;

namespace Umbraco.Cms.Api.Management.Mapping.HealthCheck;

public class HealthCheckViewModelsMapDefinition : IMapDefinition
{
    public void DefineMaps(IUmbracoMapper mapper)
    {
        mapper.Define<HealthCheckActionViewModel, HealthCheckAction>((source, context) => new HealthCheckAction(), Map);
        mapper.Define<HealthCheckAction, HealthCheckActionViewModel>((source, context) => new HealthCheckActionViewModel() { ValueRequired = false }, Map);
        mapper.Define<HealthCheckStatus, HealthCheckResultViewModel>((source, context) => new HealthCheckResultViewModel() { Message = string.Empty }, Map);
        mapper.Define<Core.HealthChecks.HealthCheck, HealthCheckViewModel>((source, context) => new HealthCheckViewModel() { Name = string.Empty }, Map);
        mapper.Define<IGrouping<string?, Core.HealthChecks.HealthCheck>, HealthCheckGroupViewModel>((source, context) => new HealthCheckGroupViewModel() { Checks = new List<HealthCheckViewModel>() }, Map);
        mapper.Define<IEnumerable<IGrouping<string?, Core.HealthChecks.HealthCheck>>, PagedViewModel<HealthCheckGroupViewModel>>((source, context) => new PagedViewModel<HealthCheckGroupViewModel>(), Map);
    }

    // Umbraco.Code.MapAll -ActionParameters
    private static void Map(HealthCheckActionViewModel source, HealthCheckAction target, MapperContext context)
    {
        target.Alias = source.Alias;
        target.HealthCheckId = source.HealthCheckKey;
        target.Name = source.Name;
        target.Description = source.Description;
        target.ValueRequired = source.ValueRequired;
        target.ProvidedValueValidation = source.ProvidedValueValidation;
        target.ProvidedValueValidationRegex = source.ProvidedValueValidationRegex;
        target.ProvidedValue = source.ProvidedValue;
    }

    // Umbraco.Code.MapAll
    private static void Map(HealthCheckAction source, HealthCheckActionViewModel target, MapperContext context)
    {
        if (source.HealthCheckId is not null)
        {
            target.HealthCheckKey = (Guid)source.HealthCheckId;
        }

        target.Alias = source.Alias;
        target.Name = source.Name;
        target.Description = source.Description;
        target.ValueRequired = source.ValueRequired;
        target.ProvidedValue = source.ProvidedValue;
        target.ProvidedValueValidation = source.ProvidedValueValidation;
        target.ProvidedValueValidationRegex = source.ProvidedValueValidationRegex;
    }

    // Umbraco.Code.MapAll
    private static void Map(HealthCheckStatus source, HealthCheckResultViewModel target, MapperContext context)
    {
        target.Message = source.Message;
        target.ResultType = source.ResultType;
        target.ReadMoreLink = source.ReadMoreLink;
        target.Actions = context.MapEnumerable<HealthCheckAction, HealthCheckActionViewModel>(source.Actions);
    }

    // Umbraco.Code.MapAll
    private static void Map(Core.HealthChecks.HealthCheck source, HealthCheckViewModel target, MapperContext context)
    {
        target.Key = source.Id;
        target.Name = source.Name;
        target.Description = source.Description;
    }

    // Umbraco.Code.MapAll
    private static void Map(IGrouping<string?, Core.HealthChecks.HealthCheck> source, HealthCheckGroupViewModel target, MapperContext context)
    {
        target.Name = source.Key;
        target.Checks = context.MapEnumerable<Core.HealthChecks.HealthCheck, HealthCheckViewModel>(source.OrderBy(x => x.Name));
    }

    // Umbraco.Code.MapAll
    private static void Map(IEnumerable<IGrouping<string?, Core.HealthChecks.HealthCheck>> source, PagedViewModel<HealthCheckGroupViewModel> target, MapperContext context)
    {
        target.Items = context.MapEnumerable<IGrouping<string?, Core.HealthChecks.HealthCheck>, HealthCheckGroupViewModel>(source);
        target.Total = source.Count();
    }
}