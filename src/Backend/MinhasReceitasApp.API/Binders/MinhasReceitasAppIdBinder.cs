using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace MinhasReceitasApp.API.Binders;

public class MinhasReceitasAppIdBinder : IModelBinder
{
    private readonly SqidsEncoder<long> _idEncoder;
    public MinhasReceitasAppIdBinder(SqidsEncoder<long> idEncoder) => _idEncoder = idEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        var id = _idEncoder.Decode(value).Single();

        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
