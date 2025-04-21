using MinhasReceitasApp.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new PasswordEncripter("abc1234");
}
