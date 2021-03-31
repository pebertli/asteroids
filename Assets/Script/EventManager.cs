using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void EventGameObject(GameObject sender);
    private Dictionary<string, EventGameObject> eventDictionary;
    private static EventManager mEventManager;

    public static EventManager instance
    {
        get
        {
            if (!mEventManager)
            {
                mEventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
            }

            if (!mEventManager)
                Debug.LogError("No event Manager");
            else
            {
                mEventManager.Init();
            }

            return mEventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, EventGameObject>();
        }
    }

    public static void StartListening(string eventName, EventGameObject listener)
    {
        EventGameObject delegateDictionary;
        if (instance.eventDictionary.TryGetValue(eventName, out delegateDictionary))
        {
            delegateDictionary += listener;
        }
        else
        {
            delegateDictionary = new EventGameObject(listener);
        }
        instance.eventDictionary[eventName] = delegateDictionary;
    }

    public static void StopListening(string eventName, EventGameObject listener)
    {
        if (mEventManager == null)
            return;

        EventGameObject delegateDictionary;
        if (instance.eventDictionary.TryGetValue(eventName, out delegateDictionary))
        {
            delegateDictionary = (EventGameObject)Delegate.Remove(delegateDictionary, listener);

            // If a delegate remains, set the new head else remove the EventKey
            if (delegateDictionary != null)
                instance.eventDictionary[eventName] = delegateDictionary;
            else
                instance.eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(string eventName, GameObject go)
    {
        EventGameObject thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(go);
        }
    }
}
