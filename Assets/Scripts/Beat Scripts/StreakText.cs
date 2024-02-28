using UnityEngine;
using UnityEngine.UI;

public class StreakText : MonoBehaviour
{
    public Text streakText; // Reference to the UI Text component
    private int streakMultiplier = 1; // Streak multiplier

    void Start()
    {
        // Find the Text component
        streakText = GetComponent<Text>();
        UpdateStreakText();
    }

    // Method to update the streak text
    public void UpdateStreakText()
    {
        streakText.text = "Streak: " + streakMultiplier + "x";
    }

    // Method to set the streak multiplier
    public void SetStreakMultiplier(int multiplier)
    {
        streakMultiplier = multiplier;
        UpdateStreakText();
    }

    // Method to reset the streak multiplier
    public void ResetStreakMultiplier()
    {
        streakMultiplier = 1;
        UpdateStreakText();
    }
}
