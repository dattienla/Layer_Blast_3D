using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseGameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject coinOfPlayerImage;
    public GameObject coinOfPlayerText;
    public static LoseGameUI instance;
    [SerializeField]
    private GameObject levelFailText;
    [SerializeField]
    private GameObject loseGameTitle;
    [SerializeField]
    private GameObject cubeImage;
    [SerializeField]
    private GameObject cubeText;
    [SerializeField]
    private GameObject adsButton;
    [SerializeField]
    private GameObject resetButton;
    [SerializeField]
    private GameObject coinPrefab;
    bool isClickAds;
    bool isClickReset;

    private void Awake()
    {
        isClickAds = true;
        isClickReset = true;
        instance = this;
    }

    public void LoseGamePanel()
    {
        int levelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        levelFailText.GetComponent<TextMeshProUGUI>().text = "Level " + levelIndex.ToString() + " Failed";
        levelFailText.transform.localScale = Vector3.zero;
        loseGameTitle.transform.localScale = Vector3.zero;
        cubeImage.transform.localScale = Vector3.one * 0.3f;
        adsButton.transform.localScale = Vector3.one * 0.3f;
        resetButton.transform.localScale = Vector3.one * 0.3f;

        coinOfPlayerText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coin", 0).ToString();
        cubeText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.score.ToString();
        loseGameTitle.transform.DOScale(1f, 1.2f).SetEase(Ease.OutBack);
        levelFailText.transform.DOScale(1f, 1.2f).SetEase(Ease.OutBack);
        cubeImage.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
        adsButton.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
        resetButton.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.8f).OnComplete(() =>
        {
            isClickReset = false;
            isClickAds = false;
        });


    }
    public void SpawnAndFlyCoins()
    {
        for (int i = 0; i < 10; i++)
        {
            // Tạo coin
            GameObject coin = Instantiate(coinPrefab, adsButton.transform);

            // Vị trí random trong khu vực
            RectTransform coinRT = coin.GetComponent<RectTransform>();
            coinRT.anchoredPosition = new Vector2(
                Random.Range(-100f, 100f),
                Random.Range(-50f, 50f)
            );

            // Di chuyển coin bay lên (Y tăng)
            coinRT.transform.DOMove(new Vector3(50f, -100f, 0) + coinRT.transform.position, 0.3f).OnComplete(() =>
            {
                coinRT.transform.DOMove(coinOfPlayerImage.transform.position, 0.5f).OnComplete(() =>
                {
                    Destroy(coin);
                });
            });
        }
    }
    public void CoinUpdate()
    {
        int c = PlayerPrefs.GetInt("Coin", 0) + 10;
        PlayerPrefs.SetInt("Coin", c);
        PlayerPrefs.Save();
        coinOfPlayerText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coin", 0).ToString();
    }
    public void Ads()
    {
        if (isClickAds == false)
        {
            SpawnAndFlyCoins();
            Invoke("CoinUpdate", 0.8f);
            isClickAds = true;
        }
    }
    public void Reset()
    {
        if (isClickReset == false)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
