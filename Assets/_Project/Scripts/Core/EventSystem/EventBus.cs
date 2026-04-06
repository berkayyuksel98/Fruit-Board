using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _listeners = new();

    public static void Subscribe<T>(Action<T> listener)
    {
        var type = typeof(T);
        
        if (!_listeners.ContainsKey(type))
            _listeners[type] = new List<Delegate>();
            
        _listeners[type].Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        var type = typeof(T);
        
        if (_listeners.ContainsKey(type))
            _listeners[type].Remove(listener);
    }

    public static void Raise<T>(T eventData)
    {
        var type = typeof(T);
        
        if (!_listeners.ContainsKey(type)) return;
        
        foreach (var listener in _listeners[type])
            (listener as Action<T>)?.Invoke(eventData);
    }

    public static void Clear()
    {
        _listeners.Clear();
    }
}

