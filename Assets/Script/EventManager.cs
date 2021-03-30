using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void EventGameObject(GameObject sender);
    private Dictionary<string, EventGameObject> eventDictionary;
    //private Dictionary<string, UnityEvent<GameObject>> eventDictionaryGameObject;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
            }

            if (!eventManager)
                Debug.LogError("No event Manager");
            else
            {
                eventManager.Init();
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, EventGameObject>();
        }

        //if (eventDictionaryGameObject == null)
        //{
        //    eventDictionaryGameObject = new Dictionary<string, UnityEvent<GameObject>>();
        //}
    }

    public static void StartListening(string eventName, EventGameObject listener)
    {
        //EventGameObject thisEvent;

        //if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        //{
        //    thisEvent += listener;
        //}
        //else
        //{
        //    var d = thisEvent + listener;
        //    instance.eventDictionary.Add(eventName, d);
        //    d += listener;
        //}

        //EventGameObject d;
        //instance.eventDictionary.TryGetValue(eventName, out d);
        //d = (EventGameObject)Delegate.Combine(d, listener);
        //instance.eventDictionary[eventName] = d;
        //Debug.Log(d.GetInvocationList().ToString());
        //for (int ctr = d.GetInvocationList().Length - 1; ctr >= 0; ctr--)
        //{
        //    var outputMsg = d.GetInvocationList()[ctr];
        //    Debug.Log(outputMsg.Method);
        //}

        EventGameObject d;
        if (instance.eventDictionary.TryGetValue(eventName, out d))
        {
            d += listener;
        }
        else
        {
            d = new EventGameObject(listener);
        }
        instance.eventDictionary[eventName] = d;
        //d = (EventGameObject)Delegate.Combine(d, listener);
        //instance.eventDictionary[eventName] = d;
        //Debug.Log(d.GetInvocationList().ToString());
        //for (int ctr = d.GetInvocationList().Length - 1; ctr >= 0; ctr--)
        //{
        //    var outputMsg = d.GetInvocationList()[ctr];
        //    Debug.Log(outputMsg.Method);
        //}
    }

    //public static void StartListening(string eventName, UnityAction<GameObject> listener)
    //{
    //    UnityEvent<GameObject> thisEvent = null;

    //    if (instance.eventDictionaryGameObject.TryGetValue(eventName, out thisEvent))
    //    {
    //        thisEvent.AddListener(listener);
    //    }
    //    else
    //    {
    //        thisEvent = new GameObjectUnityEvent();
    //        thisEvent.AddListener(listener);
    //        instance.eventDictionaryGameObject.Add(eventName, thisEvent);
    //    }
    //}

    //public static void StopListening(string eventName, UnityAction<GameObject> listener)
    //{
    //    if (eventManager == null)
    //        return;

    //    UnityEvent<GameObject> thisEvent = null;
    //    if (instance.eventDictionaryGameObject.TryGetValue(eventName, out thisEvent))
    //    {
    //        thisEvent.RemoveListener(listener);
    //    }
    //}

    public static void StopListening(string eventName, EventGameObject listener)
    {
        if (eventManager == null)
            return;

        //EventGameObject thisEvent = null;
        //if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        //{
        //    thisEvent -= (listener);
        //}

        EventGameObject d;
        if (instance.eventDictionary.TryGetValue(eventName, out d))
        {
            d = (EventGameObject)Delegate.Remove(d, listener);

            // If a delegate remains, set the new head else remove the EventKey
            if (d != null)
                instance.eventDictionary[eventName] = d;
            else
                instance.eventDictionary.Remove(eventName);
        }
    }

    //public static void TriggerEvent(string eventName)
    //{
    //    UnityEvent thisEvent = null;
    //    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    //    {
    //        thisEvent.Invoke();
    //    }
    //}

    public static void TriggerEvent(string eventName, GameObject go)
    {
        EventGameObject thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(go);
        }
    }
}
