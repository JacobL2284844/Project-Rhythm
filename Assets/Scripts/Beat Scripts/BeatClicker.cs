using UnityEngine;

public class BeatClicker : MonoBehaviour
{
    public float bpm = 135f; // Beats per minute
    private float beatInterval; // Time interval between beats
    private float beatTimer; // Timer to keep track of beats
    private int score = 0; // Player's score
    private int streakMultiplier = 1; // Streak multiplier
    private ScoreDisplay scoreDisplay; // Reference to the ScoreDisplay script
    private StreakText streakText; // Reference to the StreakText script

    void Start()
    {
        // Calculate the time interval between beats based on the BPM
        beatInterval = 60f / bpm;
        // Start the beat timer
        beatTimer = beatInterval;

        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        streakText = FindObjectOfType<StreakText>();
    }

    void Update()
    {
        beatTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            // Check if the click was on time with the beat
            if (beatTimer <= 0)
            {
                Debug.Log("Clicked on beat!");
                // Increment the score by the streak multiplier
                score += streakMultiplier;
                // Update the score display
                scoreDisplay.UpdateScore(score);
                // Increase streak multiplier if on beat
                IncreaseStreakMultiplier();
                // Reset the beat timer to the next beat
                beatTimer = beatInterval;
                // Add your beat action here, for example, playing a sound
            }
            else
            {
                // Reset streak multiplier if off-beat
                ResetStreakMultiplier();
                // Add your off-beat action here
                Debug.Log("Off beat!");
            }
        }

        // Update streak text
        streakText.SetStreakMultiplier(streakMultiplier);
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
