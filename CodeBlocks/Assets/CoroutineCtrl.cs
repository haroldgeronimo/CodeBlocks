using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CoroutineState
{
    Ready,
    Running,
    Paused,
    Finished
}
public class CoroutineController
{
    private IEnumerator _routine;

    public CoroutineState state;

    public CoroutineController(IEnumerator routine)
    {
        _routine = routine;
        state = CoroutineState.Ready;
    }

    public IEnumerator Start()
    {
        if (state != CoroutineState.Ready)
        {
            throw new System.InvalidOperationException("Unable to start coroutine in state: " + state);
        }

        state = CoroutineState.Running;
        while (_routine.MoveNext())
        {
            yield return _routine.Current;
            while (state == CoroutineState.Paused)
            {
                yield return null;
            }
            if (state == CoroutineState.Finished)
            {
                yield break;
            }
        }

        state = CoroutineState.Finished;
    }

    public void Stop()
    {
        if (state != CoroutineState.Running || state != CoroutineState.Paused)
        {
            throw new System.InvalidOperationException("Unable to stop coroutine in state: " + state);
        }

        state = CoroutineState.Finished;
    }

    public void Pause()
    {
        if (state != CoroutineState.Running)
        {
            throw new System.InvalidOperationException("Unable to pause coroutine in state: " + state);
        }

        state = CoroutineState.Paused;
    }

    public void Resume()
    {
        if (state != CoroutineState.Paused)
        {
            throw new System.InvalidOperationException("Unable to resume coroutine in state: " + state);
        }

        state = CoroutineState.Running;
    }
}
public static class CoroutineExtensions
{
    public static Coroutine StartCoroutineEx(this MonoBehaviour monoBehaviour, IEnumerator routine, out CoroutineController coroutineController)
    {
        if (routine == null)
        {
            throw new System.ArgumentNullException("routine");
        }

        coroutineController = new CoroutineController(routine);
        return monoBehaviour.StartCoroutine(coroutineController.Start());
    }
}
