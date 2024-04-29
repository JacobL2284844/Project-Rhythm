using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Enemy")]
    public EventReference enemyFootstep;//event anim
    public EventReference enemyTakeDamage;

    [Header("UI")]
    public EventReference uiClickSound;//called in menu manager
    public EventReference uiPauseSound;
    public EventReference uiMoveSound;
    public EventReference uiBackSound;

    [Header("Main Menu")]
    public EventReference mainMenuMusic;

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

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            RuntimeManager.PlayOneShot(mainMenuMusic);
        }
    }
    public void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
    }
    public void PLayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
    public void ReleaseMainMenuAudio()
    {
        FMOD.Studio.Bus masterBus;
        masterBus = RuntimeManager.GetBus("Bus:/");

        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
