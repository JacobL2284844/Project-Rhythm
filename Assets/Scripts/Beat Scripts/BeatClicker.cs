using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatClicker : MonoBehaviour
{
    public float bpm = 135f; // Beats per minute
    public float perfectTimingThreshold = 0.03f; // Threshold for perfect timing (in seconds)
    public float goodTimingThreshold = 0.07f; // Threshold for good timing (in seconds)
    public float mehTimingThreshold = 0.1f; // Threshold for meh timing (in seconds)

    private float beatInterval; // Time interval between beats
    private float beatTimer; // Timer to keep track of beats

    private int score = 0; // Player's score
    private int streakMultiplier = 1; // Streak multiplier
    private ScoreDisplay scoreDisplay; // Reference to the ScoreDisplay script
    private StreakText streakText; // Reference to the StreakText script

    void Start()
    {
        beatInterval = 60f / bpm; // Calculate the time interval between beats based on the BPM
        beatTimer = beatInterval; // Start the beat timer

        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        streakText = FindObjectOfType<StreakText>();
    }

    void Update()
    {
        beatTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) // Mouse button input
        {
            float timingDifference = Mathf.Abs(beatTimer);

            if (timingDifference <= perfectTimingThreshold) // Perfect Timing Threshold
            {
                Debug.Log("Perfect!");
                score += streakMultiplier;
                IncreaseStreakMultiplier();
                scoreDisplay.UpdateScore(score);
            }
            else if (timingDifference <= goodTimingThreshold) // Good Timing Threshold
            {
                Debug.Log("Good!");
                score += streakMultiplier;
                scoreDisplay.UpdateScore(score);
            }
            else if (timingDifference <= mehTimingThreshold) // Meh Timing Threshold
            {
                Debug.Log("Meh!");
                score += streakMultiplier;
                scoreDisplay.UpdateScore(score);
            }
            else
            {
                Debug.Log("Offbeat!"); // Add your off-beat action here
                ResetStreakMultiplier();// Reset streak multiplier if off-beat
            }

            beatTimer = beatInterval; // Reset the beat timer for the next beat
        }

        streakText.SetStreakMultiplier(streakMultiplier); // Update streak text
    }

    // Method to increase the streak multiplier, up to a maximum of 8x
    void IncreaseStreakMultiplier()
    {
        streakMultiplier = Mathf.Min(streakMultiplier + 1, 8);
    }

    // Method to reset the streak multiplier
    void ResetStreakMultiplier()
    {
        streakMultiplier = 1;
    }
}
