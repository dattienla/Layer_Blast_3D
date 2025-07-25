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
        MusicButtonOff();
        SoundButtonOff();
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= scoreWin) scoreText.text = scoreWin.ToString() + "/" + scoreWin.ToString();
        else scoreText.text = score.ToString() + "/" + scoreWin.ToString();
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
            Invoke("CheckWinGameDelay", 1.3f);
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
        // AudioListener.volume = 1f;
        //soundButtonOn.SetActive(!AudioListener.volume.Equals(0f));
        //soundButtonOff.SetActive(AudioListener.volume.Equals(0f));
        soundButtonOff.SetActive(true);
        soundButtonOn.SetActive(false);
    }
    public void SoundButtonOff()
    {
        //AudioListener.volume = 0f;
        //soundButtonOn.SetActive(!AudioListener.volume.Equals(0f));
        //soundButtonOff.SetActive(AudioListener.volume.Equals(0f));
        soundButtonOn.SetActive(true);
        soundButtonOff.SetActive(false);
    }
    public void MusicButtonOn()
    {
        //backgroundAudio.mute = false;
        //musicButtonOn.SetActive(!backgroundAudio.mute);
        //musicButtonOff.SetActive(backgroundAudio.mute);
        musicButtonOff.SetActive(true);
        musicButtonOn.SetActive(false);
    }
    public void MusicButtonOff()
    {
        // backgroundAudio.mute = true;
        //musicButtonOn.SetActive(!backgroundAudio.mute);
        //musicButtonOff.SetActive(backgroundAudio.mute);
        musicButtonOn.SetActive(true);
        musicButtonOff.SetActive(false);
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
