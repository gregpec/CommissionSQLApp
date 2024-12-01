namespace CommissionApp;

using CommissionApp.Services;
using CommissionApp.UserCommunication;
public class App : IApp
{
    private readonly IUserCommunication _userCommunication;
    private readonly IEventHandlerService _eventHandlerService;
    public App(
        IUserCommunication userCommunication,
        IEventHandlerService eventHandlerService
        )
    {
        _userCommunication = userCommunication;
       _eventHandlerService = eventHandlerService;
    }
    public void Run()
    {
        _userCommunication.UseUserCommunication();
        _eventHandlerService.Events();       
    }
}
    

