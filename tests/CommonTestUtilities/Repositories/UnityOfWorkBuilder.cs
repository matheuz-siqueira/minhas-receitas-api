using MinhasReceitasApp.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class UnityOfWorkBuilder
{
    public static IUnityOfWork Build()
    {
        var mock = new Mock<IUnityOfWork>();
        return mock.Object;
    }
}
