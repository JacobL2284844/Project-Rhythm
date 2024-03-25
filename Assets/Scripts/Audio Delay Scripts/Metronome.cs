using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public float tempo = 100f; // Default tempo
    public float offsetMilliseconds = 0f; // Default offset in milliseconds
    public bool isPlaying = true;

    public Text offsetText; // Reference to the UI Text component
    public Text accuracyText; // Reference to the UI Text component for accuracy
    public Text JudgementText;
    public AudioSource tickSound; // Reference to the Tick Sound Audio
    public KeyCode hitKey = KeyCode.Mouse0;

    public float perfectTimingThreshold = 0.1f;
    public float goodTimingThreshold = 0.2f;
    public float mehTimingThreshold = 0.3f;

    private float nextBeatTime;
    private float lastBeatTime;
    private bool playerHitOnBeat = false;

    private string offsetPrefsKey = "Offset"; // PlayerPrefs key for storing offset

    void Start()
    {
        // Load offset from PlayerPrefs
        if (PlayerPrefs.HasKey("Offset"))
        {
            offsetMilliseconds = PlayerPrefs.GetFloat("Offset");
        }

        nextBeatTime = Time.time;
        lastBeatTime = Time.time;
    }

    void Update()
    {
        if (isPlaying)
        {
            if (Time.time >= nextBeatTime)
            {
                // Trigger beat feedback (visual or audio)
                if (tickSound != null)
                    tickSound.Play(); // Play the ticking sound

                // Calculate the time for the next beat
                nextBeatTime += 60f / tempo;

                // Apply the offset
                nextBeatTime += offsetMilliseconds / 1000f;

                // Check if the player hit on the beat
                if (playerHitOnBeat)
                {
                    float accuracy = Mathf.Abs((Time.time - lastBeatTime) * 1000f); // Calculate accuracy in milliseconds
                    if (accuracyText != null)
                        accuracyText.text = "Accuracy: " + accuracy.ToString("F2") + "ms";
                    playerHitOnBeat = false; // Reset player hit flag
                }

                lastBeatTime = nextBeatTime - (offsetMilliseconds / 1000f);
            }
        }

        // Update Offset Text
        if (offsetText != null)
        {
            offsetText.text = "Offset: " + offsetMilliseconds.ToString("F2") + "ms";
        }

        // Check for player input
        if (Input.GetKeyDown(hitKey))
        {

            playerHitOnBeat = true;
            float timeSinceLastBeat = Time.time - lastBeatTime;

            float timingDifference = Mathf.Abs(timeSinceLastBeat);

            if (timingDifference <= perfectTimingThreshold) // Perfect Timing Threshold
            {
                JudgementText.text = "Perfect";
            }
            else if (timingDifference <= goodTimingThreshold) // Good Timing Threshold
            {
                JudgementText.text = "Good";
            }
            else if (timingDifference <= mehTimingThreshold) // Meh Timing Threshold
            {
                JudgementText.text = "Meh";
            }
            else
            {
                JudgementText.text = "Offbeat";
            }

            lastBeatTime = Time.time; // Update the last beat time
        }

        PlayerPrefs.SetFloat("Offset", offsetMilliseconds);// Save the offset to PlayerPrefs whenever it changes
    }

    // Method to start/stop the metronome
    public void ToggleMetronome()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            // Set the next beat time to the current time plus the offset
            nextBeatTime = Time.time + (offsetMilliseconds / 1000f);
            lastBeatTime = Time.time;
        }
    }


    // Method to adjust the offset in milliseconds
    public void AdjustOffset(float offsetChange)
    {
        offsetMilliseconds += offsetChange;

        PlayerPrefs.SetFloat("Offset", offsetMilliseconds);// Save the offset to PlayerPrefs whenever it changes

    }

}
