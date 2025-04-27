using AutoMapper;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()); //Não faz mapeamento da senha pois tem que criptografar 
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUseProfileJson>();
    }

}
