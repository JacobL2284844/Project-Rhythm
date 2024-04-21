using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
public class BeatController : MonoBehaviour
{
    [SerializeField] private float _bpm;
    public BeatClicker beatClicker;
    private float _timeSinceStart;
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

        beatClicker.bgmEventInstance.getTimelinePosition(out int position);
        _timeSinceStart = position;
    }
    //best solution ? https://qa.fmod.com/t/gettimelineposition-accuracy-for-rhythm-game/20202
    private void Update()
    {
        _timeSinceStart += Time.deltaTime;

        float currentBeat = GetCurrentBeat();
        foreach (Intervals interval in _intervals)
        {
            interval.CheckForNewInterval(currentBeat);
        }
    }

    private float GetCurrentBeat()
    {
        return _timeSinceStart * (_bpm / 60f);
    }

    [System.Serializable]
    public class Intervals
    {
        [SerializeField] private float _steps;
        [SerializeField] private UnityEvent _trigger;
        private int _lastInterval;

        public void CheckForNewInterval(float interval)
        {
            if (Mathf.FloorToInt(interval) != _lastInterval)
            {
                _lastInterval = Mathf.FloorToInt(interval);
                _trigger.Invoke();
            }
        }
    }
}
