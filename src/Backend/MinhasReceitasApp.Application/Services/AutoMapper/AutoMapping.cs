using AutoMapper;
using MinhasReceitasApp.Communication.Requests;

namespace MinhasReceitasApp.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain(); 
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()); //Não faz mapeamento da senha pois tem que criptografar 
    }

}