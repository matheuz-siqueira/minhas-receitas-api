using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Domain.Services.ServiceBus;

public interface IDeleteUserQueue
{
    public Task SendMessage(User user);
}
