using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;
    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>(); 
    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveWithEmail(email)).ReturnsAsync(true);
    }

    public void GetByEmailAndPassword(User user)
    {
        _repository.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user); 
    }
    public IUserReadOnlyRepository Build() => _repository.Object;
    
}
