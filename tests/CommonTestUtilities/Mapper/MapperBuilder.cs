using MinhasReceitasApp.Application.Services.AutoMapper;
using AutoMapper;
using CommonTestUtilities.IdEncryption;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncripter = IdEncripterBuilder.Build();

        var mapper = new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping(idEncripter));
        }).CreateMapper();

        return mapper;
    }
}
