using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeManager : MonoBehaviour
{
    public Animator hudAnimator;
    public Image quickTimeEvent;

    public Image blockEvent;//called in enemy attack

    public GameObject[] uiForStages;

    public Image pefectImage;
    public Sprite[] iconsForPerfect;
    public Image goodImage;
    public Sprite[] iconsForGood;
    public Image mehImage;
    public Sprite[] iconsForMeh;
    public Image missImage;
    public Sprite[] iconsForMiss;
    public void PlayBeatHitTiming(string timing)
    {
        hudAnimator.SetTrigger(timing);
        SetIconForBeatTiming(timing);
    }
    void SetIconForBeatTiming(string timing)
    {
        switch (timing)
        {
            case "HitTimePerfect":
                pefectImage.sprite = iconsForPerfect[Random.Range(0, iconsForPerfect.Length)];
                break;
            case "HitTimeGood":
                goodImage.sprite = iconsForGood[Random.Range(0, iconsForGood.Length)];
                break;
            case "HitTimeMeh":
                mehImage.sprite = iconsForMeh[Random.Range(0, iconsForMeh.Length)];
                break;
            case "HitTimeMiss":
                missImage.sprite = iconsForMiss[Random.Range(0, iconsForMiss.Length)];
                break;
            default:
                break;
        }
    }
    public void SetStage(int stage)
    {
        string stageUITage = "Stage" + stage;

        foreach (GameObject stageUIItem in uiForStages)
        {
            if (stageUIItem.CompareTag(stageUITage))
            {
                stageUIItem.SetActive(true);
            }
            else
            {
                stageUIItem.SetActive(false);
            }
        }
    }
}
