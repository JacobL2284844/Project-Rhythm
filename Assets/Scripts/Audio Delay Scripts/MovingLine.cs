using UnityEngine;
using UnityEngine.UI;

public class MovingLine : MonoBehaviour
{
    public Image beatLine;
    public float tempo = 100f; // Beats per minute of the song
    public int beatsBeforeReset = 5; // Number of beats before resetting the beat line position

    private float beatInterval; // Time interval between each beat
    private float beatLineStartPosition;
    private float beatLineEndPosition;
    private int beatCounter = 0;

    void Start()
    {

        // Calculate the time interval between each beat
        beatInterval = 60f / tempo;

        // Calculate the starting and ending positions of the beat line
        beatLineStartPosition = beatLine.rectTransform.anchoredPosition.x;
        beatLineEndPosition = beatLineStartPosition + beatLine.rectTransform.rect.width;

        // Start moving the beat line
        MoveBeatLine();
    }

    void MoveBeatLine()
    {
        // Move the beat line to the starting position
        beatLine.rectTransform.anchoredPosition = new Vector2(beatLineStartPosition, beatLine.rectTransform.anchoredPosition.y);

        // Start moving the beat line
        InvokeRepeating("UpdateBeatLinePosition", 0f, beatInterval);
    }

    void UpdateBeatLinePosition()
    {
        beatCounter++;

        // Calculate the new position of the beat line
        float newXPosition = beatLine.rectTransform.anchoredPosition.x + (beatLineEndPosition - beatLineStartPosition) / 4; // Move the beat line to the right by 1/4th of its width

        // Update the position of the beat line
        beatLine.rectTransform.anchoredPosition = new Vector2(newXPosition, beatLine.rectTransform.anchoredPosition.y);

        // Check if it's time to reset the beat line position
        if (beatCounter >= beatsBeforeReset)
        {
            // Reset the beat counter
            beatCounter = 0;

            // Reset beat line position
            beatLine.rectTransform.anchoredPosition = new Vector2(beatLineStartPosition, beatLine.rectTransform.anchoredPosition.y);
        }
    }
}
