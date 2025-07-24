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
    public AudioSource backgroundAudio;
    private void Awake()
    {
        score = 0;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InGamePanel();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString() + "/" + scoreWin.ToString();
        CheckWinGame();
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
    void CheckWinGame()
    {
        if (score >= scoreWin)
        {
            WinGamePanel();
        }
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
        SceneManager.LoadScene(0);
    }


    // Setting Panel
    public void OnSettingPanel()
    {
        settingPanel.GetComponent<CanvasGroup>().alpha = 0;
        settingPanel.SetActive(true);
        settingPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        settingBoard.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        inGamePanel.SetActive(false);
        //musicButtonOn.SetActive(!backgroundAudio.mute);
        //musicButtonOff.SetActive(backgroundAudio.mute);
        //soundButtonOn.SetActive(!AudioListener.volume.Equals(0f));
        //soundButtonOff.SetActive(AudioListener.volume.Equals(0f));
    }
    public void CloseSettingButton()
    {
        settingPanel.GetComponent<CanvasGroup>().alpha = 1;
        settingPanel.transform.localScale = Vector3.one;
        settingPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        //settingPanel.transform.DOMove(new Vector3(445f, -950f, 0f), 0.3f);
        settingBoard.transform.DOScale(0f, 0.3f).OnComplete(() =>
        {
            settingPanel.SetActive(false);
        });
        inGamePanel.SetActive(true);
    }
    public void SoundButtonOn()
    {
        AudioListener.volume = 1f;
        soundButtonOn.SetActive(!AudioListener.volume.Equals(0f));
        soundButtonOff.SetActive(AudioListener.volume.Equals(0f));
    }
    public void SoundButtonOff()
    {
        AudioListener.volume = 0f;
        soundButtonOn.SetActive(!AudioListener.volume.Equals(0f));
        soundButtonOff.SetActive(AudioListener.volume.Equals(0f));
    }
    public void MusicButtonOn()
    {
        backgroundAudio.mute = false;
        musicButtonOn.SetActive(!backgroundAudio.mute);
        musicButtonOff.SetActive(backgroundAudio.mute);
    }
    public void MusicButtonOff()
    {
        backgroundAudio.mute = true;
        musicButtonOn.SetActive(!backgroundAudio.mute);
        musicButtonOff.SetActive(backgroundAudio.mute);
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


}
