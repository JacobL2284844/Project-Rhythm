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
    public QuickTimeManager quickTimeUIManager;
    public enum Stage { Stage1, Stage2, Stage3, Stage4 }
    public Stage currentStage = Stage.Stage1; // Current stage
    [SerializeField] int current_beatHitsToProgress = 10; // Number of beats needed to progress to the next stage
    [SerializeField] int beatHitsToProgress_Stage2 = 4;
    [SerializeField] int beatHitsToProgress_Stage3 = 8;
    [SerializeField] int beatHitsToProgress_Stage4 = 12;

    public int missesToReset = 3; // Number of misses allowed before stage decrease
    public int missesToDecreaseStage = 5; // Number of misses to decrease the stage
    public int beatsHit = 0; // Number of beats hit by the player
    private float misses = 0;


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

        current_beatHitsToProgress = beatHitsToProgress_Stage2;
    }

    public void PerfromCheckBeat(InputAction.CallbackContext context)//from input provider
    {
        if (context.started)
        {
            CheckBeat();
        }
    }
    void Update()
    {

        beatTimer -= Time.deltaTime;

        //if (Input.GetMouseButtonDown(0) && ! inGameAsset) // Mouse button input
        //{
        //    CheckBeat();
        //}

        streakText.SetStreakMultiplier(streakMultiplier); // Update streak text

        PlayerPrefs.SetFloat("Offset", offsetMilliseconds);// Save the offset to PlayerPrefs whenever it changes
        beatInterval = 60f / bpm + offsetMilliseconds; // Calculate the time interval between beats based on the BPM (With Offset)

        // Update Offset Text (Can remove if you dont want Offset Adjust)
        if (offsetText != null)
        {
            offsetText.text = "Offset: " + offsetMilliseconds.ToString("F2") + "ms";
        }

        // Update stage based on performance
        UpdateStage();
    }

    void UpdateStage()
    {

        if (misses >= missesToDecreaseStage && currentStage != Stage.Stage1)
        {
            // Decrease stage if player misses enough beats and not at Stage1
            currentStage--;
            Debug.Log("Stage Down: " + currentStage); // Remove (Only For Testing)
            beatsHit = 0;
            misses = 0;

            CheckNewStage();
        }
        else if (beatsHit >= current_beatHitsToProgress && currentStage != Stage.Stage4)
        {
            // Increase stage if player hits beats enough times and not at Stage4
            currentStage++;
            Debug.Log("Stage Up: " + currentStage);// Remove (Only For Testing
            beatsHit = 0;
            misses = 0;

            CheckNewStage();
        }
        else if (misses == missesToReset)
        {
            // Resets BeatHit if player reaches misses Threshold
            beatsHit = 0;
            misses = misses + 0.1f;

            CheckNewStage();
        }
    }
    void CheckNewStage()
    {
        if(currentStage == Stage.Stage1)
        {
            quickTimeUIManager.SetStage(1);//update ui
            current_beatHitsToProgress = beatHitsToProgress_Stage2;
            return;
        }
        else if (currentStage == Stage.Stage2)
        {
            quickTimeUIManager.SetStage(2);//update ui
            current_beatHitsToProgress = beatHitsToProgress_Stage3;
            return;
        }
        else if (currentStage == Stage.Stage3)
        {
            quickTimeUIManager.SetStage(3);//update ui
            current_beatHitsToProgress = beatHitsToProgress_Stage4;
            return;
        }
        else if (currentStage == Stage.Stage4)
        {
            quickTimeUIManager.SetStage(4);//update ui
            return;
        }
    }
    void CheckBeat()//ashleys code for beat ckeck
    {
        float timingDifference = Mathf.Abs(beatTimer);

        if (timingDifference <= perfectTimingThreshold) // Perfect Timing Threshold
        {
            recentHitState = perfectTag;
            beatsHit++;
            score += streakMultiplier;
            IncreaseStreakMultiplier();
            scoreDisplay.UpdateScore(score);

            quickTimeUIManager.PlayBeatHitTiming("HitTimePerfect");
        }
        else if (timingDifference <= goodTimingThreshold) // Good Timing Threshold
        {
            recentHitState = goodTag;
            beatsHit++;
            score += streakMultiplier;
            scoreDisplay.UpdateScore(score);

            quickTimeUIManager.PlayBeatHitTiming("HitTimeGood");
        }
        else if (timingDifference <= mehTimingThreshold) // Meh Timing Threshold
        {
            recentHitState = mehTag;
            beatsHit++;
            score += streakMultiplier;
            scoreDisplay.UpdateScore(score);

            quickTimeUIManager.PlayBeatHitTiming("HitTimeGood");
        }
        else  // Add your off-beat action here
        {
            recentHitState = failTag;
            failCounter++;
            misses++;
            ResetStreakMultiplier();// Reset streak multiplier if off-beat

            quickTimeUIManager.PlayBeatHitTiming("HitTimeMiss");
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
