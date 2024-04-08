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

    public void PlayBeatHitTiming(string timing)
    {
        hudAnimator.SetTrigger(timing);
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
