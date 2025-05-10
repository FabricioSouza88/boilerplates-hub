namespace Core.Lib.EventHandler.Interfaces;
public interface IEventHandler
{
    void PublishEvent(dynamic payload, string process, string action);
}