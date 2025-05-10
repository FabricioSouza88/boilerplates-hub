namespace Core.Lib.EventHandler.Interfaces;
public interface IEventListenerRegistration
{
    IEventListener Listener { get; }
    EventListenerOptions Options { get; }
}