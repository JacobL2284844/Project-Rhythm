using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [Header("Player")]
    public EventReference attackSwingSound;//called in attack manager
    public EventReference footstepSound;//called in anim events
    public EventReference slideSound;//called in anim events
    public EventReference takeDamage;//called in anim events

    [Header("UI")]
    public EventReference uiClickSound;//called in menu manager
    public EventReference uiPauseSound;
    public EventReference uiMoveSound;
    public EventReference uiBackSound;

    [Header("Volume")]
    [Range(0, 1)]
    private Bus masterBus;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple audio managers !");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");

        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
    }
    public void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
    }
    public void PLayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
}
