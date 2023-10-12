namespace DevelopersHub.RealtimeNetworking.Client
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Threading : MonoBehaviour
    {
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        private void Update()
        {
            UpdateMain();
        }

        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null)
            {
                Debug.Log("No action to execute on main thread!");
                return;
            }
            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMain()
        {
            if (actionToExecuteOnMainThread)
            {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread)
                {
                    executeCopiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }
                for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
                {
                    executeCopiedOnMainThread[i]();
                }
            }
        }

    }
}