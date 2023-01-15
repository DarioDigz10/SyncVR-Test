using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private class TimedEvent
    {
        // The time at which the event is scheduled to execute
        public float timeToexecute;
        // The method or code to be executed when the event is triggered
        public Callback method;
        // A unique identifier for the event, used for canceling
        public string id;
    }
    // A list to store all scheduled events
    private List<TimedEvent> events;
    // A delegate to define the callback method
    public delegate void Callback();

    private void Awake() {
        events = new List<TimedEvent>();
    }
    /// <summary>
    /// Adds "method" to a list to be executed within "inSecons" seconds.
    /// </summary>
    /// <param name="method">The method or code to be executed</param>
    /// <param name="inSeconds">The number of seconds until the method is executed</param>
    /// <param name="id">A unique identifier for the event</param>
    public void Add(Callback method, float inSeconds, string id) {
        if (inSeconds < 0) {
            Debug.LogError("inSeconds parameter cannot be negative");
            return;
        }

        if (string.IsNullOrEmpty(id)) {
            Debug.LogError("id parameter cannot be null or empty");
            return;
        }

        // Check if an event with the same id already exists, remove it if it does
        for (int i = 0; i < events.Count; i++) {
            TimedEvent timedEvent = events[i];
            if (timedEvent.id == id) {
                events.Remove(timedEvent);
                break;
            }
        }

        events.Add(new TimedEvent {
            method = method,
            timeToexecute = Time.time + inSeconds,
            id = id
        });
    }

    /// <summary>
    /// Clears the list of scheduled events
    /// </summary>
    public void Clear() => events.Clear();

    /// <summary>
    /// Cancels a scheduled event based on its id
    /// </summary>
    /// <param name="id">The unique identifier of the event to cancel</param>
    public void Cancel(string id) {
        if (string.IsNullOrEmpty(id)) {
            Debug.LogError("id parameter cannot be null or empty");
            return;
        }

        for (int i = 0; i < events.Count; i++) {
            TimedEvent timedEvent = events[i];
            if (timedEvent.id == id) {
                events.Remove(timedEvent);
                break;
            }
        }
    }

    /// <summary>
    /// Iterates through the list of scheduled events and triggers the ones that have reached their execution time
    /// </summary>
    public void Update() {
        if (events.Count == 0) return;

        for (int i = events.Count - 1; i >= 0; i--) {
            TimedEvent timedEvent = events[i];
            if (timedEvent.timeToexecute <= Time.time) {
                timedEvent.method();
                events.Remove(timedEvent);
            }
        }
    }
}

