using AutoMapper;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Services.LoggedUser;

namespace MinhasReceitasApp.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser; 
        _mapper = mapper; 
    }
    public async Task<ResponseUseProfileJson> Execute()
    {
        var user = await _loggedUser.User(); 
        return _mapper.Map<ResponseUseProfileJson>(user); 
    }
}
