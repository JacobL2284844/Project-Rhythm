using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Metronome : MonoBehaviour
{
    public float tempo = 100f; // Default tempo
    public float offsetSeconds = 0f; // Default offset in seconds
    public bool isPlaying = true;

    public Text offsetText; // Reference to the UI Text component
    public Text accuracyText; // Reference to the UI Text component for accuracy
    public Text JudgementText;
    public AudioSource tickSound; // Reference to the Tick Sound Audio
    public KeyCode hitKey = KeyCode.Mouse0;

    public float perfectTimingThreshold = 0.1f;
    public float goodTimingThreshold = 0.2f;
    public float mehTimingThreshold = 0.3f;

    public Image beatIndicator; // Reference to the beat indicator image

    private float nextBeatTime;
    private float lastBeatTime;
    private bool playerHitOnBeat = false;
    private Vector3 defaultScale; // The default scale of the beat indicator image

    private string offsetPrefsKey = "Offset"; // PlayerPrefs key for storing offset

    void Start()
    {
        // Load offset from PlayerPrefs
        if (PlayerPrefs.HasKey("Offset"))
        {
            offsetSeconds = PlayerPrefs.GetFloat("Offset");
        }

        nextBeatTime = Time.time;
        lastBeatTime = Time.time;

        // Store the default scale of the beat indicator
        if (beatIndicator != null)
        {
            defaultScale = beatIndicator.transform.localScale;
        }
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

                // Animate beat indicator
                if (beatIndicator != null)
                {
                    StartCoroutine(PulseBeatIndicator(0.2f)); // Start the pulsating animation
                }

                // Calculate the time for the next beat
                nextBeatTime += 60f / tempo;

                // Apply the offset
                nextBeatTime += offsetSeconds;

                lastBeatTime = nextBeatTime - offsetSeconds;
            }
        }

        // Update Offset Text
        if (offsetText != null)
        {
            offsetText.text = "Offset: " + offsetSeconds.ToString("F2") + "s";
        }

        // Check for player input
        if (Input.GetKeyDown(hitKey))
        {
            playerHitOnBeat = true;
            float timeSinceLastBeat = Time.time - lastBeatTime;

            float timingDifference = Mathf.Abs(timeSinceLastBeat);
            float accuracy = timingDifference; // Timing difference is already in seconds

            // Display accuracy text
            if (accuracyText != null)
                accuracyText.text = "Accuracy: " + accuracy.ToString("F2") + "s";

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

        PlayerPrefs.SetFloat("Offset", offsetSeconds); // Save the offset to PlayerPrefs whenever it changes
    }

    // Method to start/stop the metronome
    public void ToggleMetronome()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            // Set the next beat time to the current time plus the offset
            nextBeatTime = Time.time + offsetSeconds;
            lastBeatTime = Time.time;
        }
    }


    // Method to adjust the offset in seconds
    public void AdjustOffset(float offsetChange)
    {
        offsetSeconds += offsetChange;

        PlayerPrefs.SetFloat("Offset", offsetSeconds);// Save the offset to PlayerPrefs whenever it changes
    }

    IEnumerator PulseBeatIndicator(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            // Calculate the scale factor using a sinusoidal function to create a pulsating effect
            float scale = Mathf.Lerp(1f, 1.2f, Mathf.PingPong(timer / duration * 2f, 1f));

            // Apply the scale factor to the beat indicator
            beatIndicator.transform.localScale = defaultScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        // Reset the scale to the default scale when the animation is finished
        beatIndicator.transform.localScale = defaultScale;
    }
}
