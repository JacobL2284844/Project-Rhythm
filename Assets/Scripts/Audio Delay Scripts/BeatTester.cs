using UnityEngine;
using UnityEngine.UI;

public class BeatTester : MonoBehaviour
{
    public float bpm = 120f; // Beats per minute
    public KeyCode hitKey = KeyCode.Space; // Adjust this to the key you want to use for hitting
    public Text feedbackText; // Reference to the Text UI object for displaying feedback

    private float beatInterval;
    private float nextBeatTime;
    private float lastBeatTime;

    void Start()
    {
        beatInterval = 60f / bpm; // Calculate beat interval in seconds
        nextBeatTime = Time.time + beatInterval;
        lastBeatTime = Time.time;

        if (feedbackText == null)
            Debug.LogError("Text UI reference is missing!"); // Error check if Text UI is not assigned
        else
            feedbackText.text = "Hit Accuracy: ";
    }

    void Update()
    {
        if (Input.GetKeyDown(hitKey))
        {
            CheckHitAccuracy();
        }

        if (Time.time >= nextBeatTime)
        {
            Metronome();
        }
    }

    void Metronome()
    {
        lastBeatTime = nextBeatTime;
        nextBeatTime += beatInterval;
        // Here you would trigger some visual/audio cue for the beat
    }

    void CheckHitAccuracy()
    {
        float timeSinceLastBeat = Time.time - lastBeatTime;
        float accuracy = Mathf.Abs(timeSinceLastBeat);

        string accuracyText;
        if (accuracy < 0.05f) // Adjust these thresholds as necessary for your game
        {
            accuracyText = "Perfect!";
        }
        else if (accuracy < 0.1f)
        {
            accuracyText = "Great!";
        }
        else if (accuracy < 0.2f)
        {
            accuracyText = "Good";
        }
        else
        {
            accuracyText = "Miss";
        }

        feedbackText.text = "Hit Accuracy: " + accuracy.ToString("F3") + "s " + accuracyText;
    }
}
