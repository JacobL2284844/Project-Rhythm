using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BeatClicker : MonoBehaviour
{
    public bool inGameAsset = false;//allow ashleys tests ??

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

    public int failCounter = 0;
    public int failCounterThreshold = 3;
    public string recentHitState = "tbd";//most recent tag of hit
    public string perfectTag = "Perfect!";
    public string goodTag = "Good!";
    public string mehTag = "Meh!";
    public string failTag = "Offbeat!";

    public Text offsetText; // (Can remove if you dont want Offset Adjust UI)
    public float offsetMilliseconds = 0f;

    void Start()
    {

        // Load offset from PlayerPrefs when the game starts
        if (PlayerPrefs.HasKey("Offset"))
        {
            offsetMilliseconds = PlayerPrefs.GetFloat("Offset");
        }

        beatInterval = 60f / bpm + offsetMilliseconds;
        beatTimer = beatInterval; // Start the beat timer

        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        streakText = FindObjectOfType<StreakText>();
    }

    public void PerfromCheckBeat(InputAction.CallbackContext context)//from input provider
    {
        if(context.started)
        {
            CheckBeat();
        }
    }
    void Update()
    {

        beatTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && ! inGameAsset) // Mouse button input
        {
            CheckBeat();
        }

        streakText.SetStreakMultiplier(streakMultiplier); // Update streak text

        PlayerPrefs.SetFloat("Offset", offsetMilliseconds);// Save the offset to PlayerPrefs whenever it changes
        beatInterval = 60f / bpm + offsetMilliseconds; // Calculate the time interval between beats based on the BPM (With Offset)

        // Update Offset Text (Can remove if you dont want Offset Adjust)
        if (offsetText != null)
        {
            offsetText.text = "Offset: " + offsetMilliseconds.ToString("F2") + "ms";
        }

    }

    void CheckBeat()//ashleys code for beat ckeck
    {
        float timingDifference = Mathf.Abs(beatTimer);

        if (timingDifference <= perfectTimingThreshold) // Perfect Timing Threshold
        {
            recentHitState = perfectTag;
            //Debug.Log(perfectTag);

            score += streakMultiplier;
            IncreaseStreakMultiplier();
            scoreDisplay.UpdateScore(score);
        }
        else if (timingDifference <= goodTimingThreshold) // Good Timing Threshold
        {
            recentHitState = goodTag;
            //Debug.Log(goodTag);

            score += streakMultiplier;
            scoreDisplay.UpdateScore(score);
        }
        else if (timingDifference <= mehTimingThreshold) // Meh Timing Threshold
        {
            recentHitState = mehTag;
            //Debug.Log(mehTag);
            score += streakMultiplier;
            scoreDisplay.UpdateScore(score);
        }
        else
        {
            recentHitState = failTag;
            //Debug.Log(failTag); // Add your off-beat action here

            failCounter++;

            ResetStreakMultiplier();// Reset streak multiplier if off-beat
        }

        beatTimer = beatInterval; // Reset the beat timer for the next beat
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

    // Method to adjust the offset in milliseconds (Can remove if you dont want offset adjust)
    public void AdjustOffset(float offsetChange)
    {
        offsetMilliseconds += offsetChange;

        PlayerPrefs.SetFloat("Offset", offsetMilliseconds);// Save the offset to PlayerPrefs whenever it changes

    }
}
