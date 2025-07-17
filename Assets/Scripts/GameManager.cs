using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject winGamePanel;
    [SerializeField]
    private GameObject inGamePanel;
    [SerializeField]
    private GameObject loseGamePanel;

    public int score;
    private int scoreWin;
    private void Awake()
    {
        score = 0;
        scoreWin = 50;
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

    void InGamePanel()
    {
        Time.timeScale = 1f;
        inGamePanel.SetActive(true);
        winGamePanel.SetActive(false);
        loseGamePanel.SetActive(false);
    }

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
}
