using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;

    // Win Game Panel
    [SerializeField]
    private GameObject winGamePanel;
    // In Game Panel
    [SerializeField]
    private GameObject inGamePanel;
    // Setting
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject settingBoard;
    // Lose Game Panel
    [SerializeField]
    private GameObject loseGamePanel;

    public int score;
    public int scoreWin;
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
        if (score >= scoreWin) scoreText.text = scoreWin.ToString() + "/" + scoreWin.ToString();
        else scoreText.text = score.ToString() + "/" + scoreWin.ToString();
        //CheckWinGame();
    }

    // In Game
    void InGamePanel()
    {
        Time.timeScale = 1f;
        inGamePanel.SetActive(true);
        winGamePanel.SetActive(false);
        loseGamePanel.SetActive(false);
        settingPanel.SetActive(false);
    }


    // Win Game
    void WinGamePanel()
    {
        WinGameUI.instance.WinGamePanel();
        inGamePanel.SetActive(false);
        winGamePanel.SetActive(true);
        loseGamePanel.SetActive(false);

    }
    public void CheckWinGame()
    {
        if (score >= scoreWin)
        {
            Invoke("CheckWinGameDelay", 0.5f);
        }
    }
    void CheckWinGameDelay()
    {
        WinGamePanel();
    }

    // Lose Game
    public void LoseGamePanel()
    {
        LoseGameUI.instance.LoseGamePanel();
        inGamePanel.SetActive(false);
        winGamePanel.SetActive(false);
        loseGamePanel.SetActive(true);
    }

    // Setting Panel
    public void OnSettingPanel()
    {
        SettingUI.Instance.SettingPanel();
        settingPanel.GetComponent<CanvasGroup>().alpha = 0;
        settingPanel.SetActive(true);
        settingPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        inGamePanel.SetActive(false);
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

    public void ReturnLevel1()
    {
        SceneManager.LoadScene(0);
    }
}
