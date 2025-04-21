using MinhasReceitasApp.Application.Services.AutoMapper; 
using AutoMapper;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        return new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper();
    }
}
