using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(object data)
    {
        for(int i = 0; i < listeners.Count; i++) 
        {
            listeners[i].onEventRaised(data);
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if( !listeners.Contains(listener) )
        {
            listeners.Add(listener);
        }
    }
    public void RemoveListener(GameEventListener listener) 
    { 
        if(listeners.Contains(listener) ) 
        { 
            listeners.Remove(listener);
        }
    }
}
