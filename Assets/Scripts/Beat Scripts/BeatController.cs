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
    //[SerializeField] private StudioEventEmitter _eventEmitter;
    [SerializeField] private Intervals[] _intervals;

    //private FMOD.Studio.EventInstance fmodMusicEventInstance;
    //[SerializeField] private int sampleRate = 48000;

    private void Start()
    {
        //if (_eventEmitter != null)
        //{
        //    fmodMusicEventInstance = _eventEmitter.EventInstance;
        //}
    }
    //best solution ? https://qa.fmod.com/t/gettimelineposition-accuracy-for-rhythm-game/20202
    private void Update()
    {
        foreach (Intervals interval in _intervals)
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForNewInterval(sampledTime);
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
