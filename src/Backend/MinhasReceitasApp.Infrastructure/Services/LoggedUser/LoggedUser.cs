using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Infrastructure.DataAccess;

namespace MinhasReceitasApp.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly MinhasReceitasAppDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider; 
    public LoggedUser(MinhasReceitasAppDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext; 
        _tokenProvider = tokenProvider;
    }
    public async Task<User> User()
    {
        var token = _tokenProvider.Value();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token); 

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value; 
        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext.Users.AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
    
}
