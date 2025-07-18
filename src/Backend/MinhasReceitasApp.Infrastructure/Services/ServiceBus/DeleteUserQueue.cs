using Azure.Messaging.ServiceBus;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Services.ServiceBus;

namespace MinhasReceitasApp.Infrastructure.Services.ServiceBus;

public class DeleteUserQueue : IDeleteUserQueue
{
    private readonly ServiceBusSender _serviceBusSender;
    public DeleteUserQueue(ServiceBusSender serviceBusSender)
    {
        _serviceBusSender = serviceBusSender;
    }
    public async Task SendMessage(User user)
    {
        await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(user.UserIdentifier.ToString()));
    }
}
