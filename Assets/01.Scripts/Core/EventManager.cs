using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EventEnum
{
    None = 0,
    Raining = 1,
}

public class EventManager : MonoSingleton<EventManager>
{
    [Header("Setting")] 
    [SerializeField] private float maxDelayTime;
    private float delayTime;
    private int randomEvent;
    
    private Dictionary<Type, Event> _events;
    private List<Event> _eventList;
    private Event _currentEvent;

    private void Awake()
    {
        delayTime = maxDelayTime;
        _events = new Dictionary<Type, Event>();
        _eventList = new List<Event>();
        
        foreach (EventEnum eventEnum in Enum.GetValues(typeof(EventEnum)))
        {
            if(eventEnum == EventEnum.None) continue;

            Event eventCompo = GetComponent($"{eventEnum.ToString()}Event") as Event;
            Type type = eventCompo.GetType();
            _events.Add(type, eventCompo);
        }
    }

    private void Update()
    {
        ChooseRandomEvent();
        if (_currentEvent is not null&&_currentEvent.endTrigger)
        {
            _currentEvent.EventStop();
            _currentEvent = null;
            delayTime = maxDelayTime;
        }
    }

    private void ChooseRandomEvent()
    {
        if(_currentEvent is not null)
            return;
        
        delayTime -= Time.deltaTime;
        if (delayTime <= 0)
        {
            randomEvent = Random.Range(1, 2);
            _currentEvent = GetEvent((EventEnum)randomEvent);
            _currentEvent.EventStart();
        }
    }

    public Event GetEvent(EventEnum eventEnum)
    {
        Type type = Type.GetType($"{eventEnum.ToString()}Event");
        if (type == null) 
            return null;

        if (_events.TryGetValue(type, out Event target))
        {
            return target;
        }

        return null;
    }
}
