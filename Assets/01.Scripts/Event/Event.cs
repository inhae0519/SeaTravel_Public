using UnityEngine;

public abstract class Event : MonoBehaviour
{
    public bool endTrigger;
    public void EventStart()
    {
        endTrigger = false;
        EventStartMethod();
    }

    public void EventStop()
    {
        EventStopMethod();
    }

    protected abstract void EventStartMethod();
    protected abstract void EventStopMethod();
}
