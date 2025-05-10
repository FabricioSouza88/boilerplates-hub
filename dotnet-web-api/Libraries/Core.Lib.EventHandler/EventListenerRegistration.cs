﻿using Core.Lib.EventHandler.Interfaces;

namespace Core.Lib.EventHandler;

public class EventListenerRegistration<TListener> : IEventListenerRegistration where TListener : class, IEventListener
{
    public IEventListener Listener { get; }
    public EventListenerOptions Options { get; }

    public EventListenerRegistration(TListener listener, Action<EventListenerOptions> configure)
    {
        Listener = listener;
        Options = new EventListenerOptions();
        configure?.Invoke(Options);
    }
}