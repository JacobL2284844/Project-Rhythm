using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeManager : MonoBehaviour
{
    public Animator hudAnimator;
    public Image quickTimeEvent;

    public Image blockEvent;//called in enemy attack

    public void PlayQuickTimeEvent(Image eventImage)
    {
        quickTimeEvent.sprite = eventImage.sprite;
        hudAnimator.SetTrigger("QuickTimeEvent");
    }
}
