using System;
using UnityEngine;

    public enum EventName {
        LockPlayer,
        PlayerDeath,
        PlayerHealthUpdated,
        StartDialogue,
        PlayerXPUpdated, 
        DialoguePerformed,
        PossessionExecuted
    }

    public class EventArgs {
        public object[] variables;
    }

    public static class GlobalEventSystem {

        private static Action<EventArgs>[] gameEvents;

        static GlobalEventSystem() {
            gameEvents = new Action<EventArgs>[Enum.GetValues(typeof(EventName)).Length];
        }

        public static void AddListener(EventName eventToListen, Action<EventArgs> listener) {
            gameEvents[(int)eventToListen] += listener;
        }

        public static void RemoveListener(EventName eventListened, Action<EventArgs> listenerToRemove) {
            gameEvents[(int)eventListened] -= listenerToRemove;
        }

        public static void CastEvent(EventName eventName, EventArgs message) {
            gameEvents[(int)eventName]?.Invoke(message);
        }

    }