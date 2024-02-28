using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BPMAnalyzer : MonoBehaviour
{
    public AudioSource audioSource;
    public float bpm;

    private const int SampleSize = 2048;
    private float[] samples;
    private float sampleRate;
    private float nextBeatTime;
    private float beatInterval;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource found on the GameObject or provided.");
                return;
            }
        }

        sampleRate = AudioSettings.outputSampleRate;
        samples = new float[SampleSize];
        nextBeatTime = 0;
        beatInterval = 0;
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetOutputData(samples, 0);

            // Analyze the audio data for beat detection
            DetectBeats(samples);

            if (beatInterval > 0)
            {
                bpm = 60f / beatInterval;
            }
        }
    }

    private void DetectBeats(float[] samples)
    {
        // Calculate the average amplitude of the audio signal
        float sum = 0;
        for (int i = 0; i < SampleSize; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        float averageAmplitude = sum / SampleSize;

        // Define a threshold for beat detection
        float thresholdMultiplier = 2.7f; // Adjust this value
        float threshold = averageAmplitude * thresholdMultiplier;

        // Check if the current amplitude exceeds the threshold
        if (Mathf.Abs(samples[0]) > threshold && Time.time > nextBeatTime)
        {
            // Calculate the time between beats
            beatInterval = Time.time - nextBeatTime;
            nextBeatTime = Time.time + beatInterval; // Schedule the next beat
        }
    }
}
