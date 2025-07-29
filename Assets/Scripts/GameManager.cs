using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;

    // Win Game Panel
    [SerializeField]
    private GameObject winGamePanel;
    [SerializeField]
    private GameObject winGameTitle;
    ///////
    [SerializeField]
    private GameObject inGamePanel;

    // Setting
    [SerializeField]
    private GameObject settingPanel;
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
    private GameObject vibrateButtonOff;
    [SerializeField]
    private GameObject privacyButton;
    [SerializeField]
    private GameObject termsButton;
    [SerializeField]
    private GameObject restoreButton;
    [SerializeField]
    private GameObject settingBoard;
    /// 

    [SerializeField]
    private GameObject loseGamePanel;
    public int score;
    public int scoreWin;
    public AudioSource backgroundSource;
    public AudioSource sfxSource;
    public AudioClip explodeAudio;
    private void Awake()
    {
        score = 0;
        Instance = this;
    }
    // Start is called before the first frame update
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
        InGamePanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= scoreWin) scoreText.text = scoreWin.ToString() + "/" + scoreWin.ToString();
        else scoreText.text = score.ToString() + "/" + scoreWin.ToString();
        //CheckWinGame();
    }

    // InGamePanel
    void InGamePanel()
    {
        Time.timeScale = 1f;
        inGamePanel.SetActive(true);
        winGamePanel.SetActive(false);
        loseGamePanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    ////////

    void WinGamePanel()
    {
        Time.timeScale = 0f;
        inGamePanel.SetActive(false);
        winGamePanel.SetActive(true);
        loseGamePanel.SetActive(false);
    }
    public void CheckWinGame()
    {
        if (score >= scoreWin)
        {
            Invoke("CheckWinGameDelay", 0f);
        }
    }
    void CheckWinGameDelay()
    {
        WinGamePanel();
    }
    public void LoseGamePanel()
    {
        Time.timeScale = 0f;
        inGamePanel.SetActive(false);
        winGamePanel.SetActive(false);
        loseGamePanel.SetActive(true);
    }
    public void Reset()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }


    // Setting Panel
    public void OnSettingPanel()
    {
        settingPanel.GetComponent<CanvasGroup>().alpha = 0;
        settingPanel.SetActive(true);
        settingPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        settingBoard.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        inGamePanel.SetActive(false);
        musicButtonOn.SetActive(!backgroundSource.mute);
        musicButtonOff.SetActive(backgroundSource.mute);
        soundButtonOn.SetActive(!sfxSource.mute);
        soundButtonOff.SetActive(sfxSource.mute);
    }
    public void CloseSettingButton()
    {
        settingPanel.GetComponent<CanvasGroup>().alpha = 1;
        settingPanel.transform.localScale = Vector3.one;
        settingPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        settingBoard.transform.DOScale(0f, 0.3f).OnComplete(() =>
        {
            settingPanel.SetActive(false);
        });
        inGamePanel.SetActive(true);
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

    public void VibrateButtonOn()
    {

    }
    public void VibrateButtonOff()
    {

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
    ///////
    public void NextLevel()
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }
    public void ReturnLevel1()
    {
        SceneManager.LoadScene(0);
    }
}
