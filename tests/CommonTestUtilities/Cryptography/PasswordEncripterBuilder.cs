using MinhasReceitasApp.Domain.Security.Cryptography;
using MinhasReceitasApp.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Sha512Encripter("abc1234");
}
