using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
public class BeatController : MonoBehaviour
{
    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private StudioEventEmitter _eventEmitter;
    [SerializeField] private Intervals[] _intervals;

    private void Update()
    {
        if (_eventEmitter.IsPlaying())
        {
            int timelinePosition = 0;
            _eventEmitter.EventInstance.getTimelinePosition(out timelinePosition);

            float eventSampleRate = 0f;

            // Get the FMOD System instance and retrieve the sample rate
            FMODUnity.RuntimeManager.CoreSystem.getSoftwareFormat(out int sampleRate, out _, out _);
            eventSampleRate = sampleRate;

            foreach (Intervals interval in _intervals)
            {
                float sampledTime = (timelinePosition / (eventSampleRate * interval.GetIntervalLength(_bpm)));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }
    public void TestBeat()
    {
        Debug.Log("Test Beat Working");
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
