using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instance;
    [SerializeField]
    private GameObject settingBoard;
    [SerializeField]
    private GameObject soundButtonOn;
    [SerializeField]
    private GameObject soundButtonOff;
    [SerializeField]
    private GameObject musicButtonOn;
    [SerializeField]
    private GameObject musicButtonOff;
    [SerializeField]
    private GameObject vibrateButtonOn;
    [SerializeField]
    private GameObject privacyButton;
    [SerializeField]
    private GameObject termsButton;
    [SerializeField]
    private GameObject restoreButton;
 
    public AudioSource backgroundSource;
    public AudioSource sfxSource;
    public AudioClip explodeAudio;
    public AudioClip winGameAudio;
    public AudioClip loseGameAudio;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        int music = PlayerPrefs.GetInt("Music", 1); // 1 = bật, 0 = tắt
        bool isMusicMuted = (music == 0);

        backgroundSource.mute = isMusicMuted;

        if (musicButtonOn != null) musicButtonOn.SetActive(!isMusicMuted);
        if (musicButtonOff != null) musicButtonOff.SetActive(isMusicMuted);

        int sound = PlayerPrefs.GetInt("Sound", 1); // 1 = bật, 0 = tắt
        bool isSoundMuted = (sound == 0);

        sfxSource.mute = isSoundMuted;

        if (soundButtonOn != null) musicButtonOn.SetActive(!isSoundMuted);
        if (soundButtonOff != null) musicButtonOff.SetActive(isSoundMuted);
    }

    public void SettingPanel()
    {
        settingBoard.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        musicButtonOn.SetActive(!backgroundSource.mute);
        musicButtonOff.SetActive(backgroundSource.mute);
        soundButtonOn.SetActive(!sfxSource.mute);
        soundButtonOff.SetActive(sfxSource.mute);
    }
    public void SoundButton()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            sfxSource.mute = true;
            PlayerPrefs.SetInt("Sound", 0);
            PlayerPrefs.Save();
        }
        else
        {
            sfxSource.mute = false;
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.Save();
        }
        soundButtonOn.SetActive(!sfxSource.mute);
        soundButtonOff.SetActive(sfxSource.mute);
    }
    public void MusicButton()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            backgroundSource.mute = true;
            PlayerPrefs.SetInt("Music", 0);
            PlayerPrefs.Save();
        }
        else
        {
            backgroundSource.mute = false;
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.Save();
        }
        musicButtonOn.SetActive(!backgroundSource.mute);
        musicButtonOff.SetActive(backgroundSource.mute);

    }
    public void PrivacyButton()
    {

    }
    public void TermsButton()
    {

    }
    public void RestoreButton()
    {

    }
}
