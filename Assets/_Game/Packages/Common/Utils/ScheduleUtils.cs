using System;
using System.Collections;
using UnityEngine;

public class ScheduleUtils
{
    class MonoBehaviourHook : MonoBehaviour { }

    static MonoBehaviourHook scheduler;

    static void InitSchedulerIfNeeded()
    {
        if (scheduler == null)
        {
            GameObject obj = new GameObject("Scheduler");
            scheduler = obj.AddComponent<MonoBehaviourHook>();
            UnityEngine.Object.DontDestroyOnLoad(obj);
        }
    }

    public static void StopAllCoroutines()
    {
        if (scheduler != null) scheduler.StopAllCoroutines();
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
        if (scheduler != null && coroutine != null) scheduler.StopCoroutine(coroutine);
    }

    public static Coroutine DelayTask(float delay, Action task)
    {
        if (task == null) return null;
        InitSchedulerIfNeeded();
        return scheduler.StartCoroutine(DelayRoutine(delay, task));
    }

    static IEnumerator DelayRoutine(float delay, Action task)
    {
        if (delay < Time.deltaTime)
        {
            try { task?.Invoke(); } catch { }
        }
        else
        {
            yield return new WaitForSeconds(delay);
            try { task?.Invoke(); } catch { }
        }
        
    }
}
