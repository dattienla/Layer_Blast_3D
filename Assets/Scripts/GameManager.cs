using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject winGamePanel;
    [SerializeField]
    private GameObject inGamePanel;
    public int score;
    private int scoreWin;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreWin = 50;
    }

    // Update is called once per frame
    void Update()
    {
        InGamePanel();
        CheckWinGame();
    }

    void InGamePanel()
    {
        scoreText.text = score.ToString() + "/" + scoreWin.ToString();
        inGamePanel.SetActive(true);
        winGamePanel.SetActive(false);
    }

    void WinGamePanel()
    {
        inGamePanel.SetActive(false);
        winGamePanel.SetActive(true);
    }
    void CheckWinGame()
    {
        if (score >= scoreWin)
        {
            WinGamePanel();
        }
    }
    void EndGamePanel()
    {

    }

}
