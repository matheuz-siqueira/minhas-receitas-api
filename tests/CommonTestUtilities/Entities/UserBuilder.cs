using Bogus;
using CommonTestUtilities.Cryptography;
using MinhasReceitasApp.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var password = new Faker().Internet.Password();

        var user = new Faker<User>()
            .RuleFor(user => user.Id, () => 1)
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f) => f.Internet.Email())
            .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(user => user.Password, (f) => passwordEncripter.Encrypt(password));

        return (user, password);
    }

}
