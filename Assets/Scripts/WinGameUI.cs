using CrazyLabsExtension;
using DG.Tweening;
using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject coinOfPlayerImage;
    public GameObject coinOfPlayerText;
    public static WinGameUI instance;
    [SerializeField]
    private GameObject levelPassText;
    [SerializeField]
    private GameObject winGameTitle;
    [SerializeField]
    private GameObject cubeImage;
    [SerializeField]
    private GameObject cubeText;
    [SerializeField]
    private GameObject coinImage;
    [SerializeField]
    private GameObject coinText;
    [SerializeField]
    private GameObject nextLevelButton;
    [SerializeField]
    private GameObject CoinCollectImage;
    [SerializeField]
    private GameObject coinPrefab;
    bool isClick;

    private void Awake()
    {
        isClick = true;
        instance = this;
    }

    public void WinGamePanel()
    {
        int levelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        levelPassText.GetComponent<TextMeshProUGUI>().text = "Level " + levelIndex.ToString() + " Passed";
        levelPassText.transform.localScale = Vector3.zero;
        winGameTitle.transform.localScale = Vector3.zero;
        cubeImage.transform.localScale = Vector3.one * 0.3f;
        coinImage.transform.localScale = Vector3.one * 0.3f;
        nextLevelButton.transform.localScale = Vector3.one * 0.3f;
        CoinCollectImage.transform.localScale = Vector3.one * 0.3f;

        coinOfPlayerText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coin", 0).ToString();
        cubeText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.scoreWin.ToString();
        coinText.GetComponent<TextMeshProUGUI>().text = "10";
        winGameTitle.transform.DOScale(1f, 1.2f).SetEase(Ease.OutBack);
        levelPassText.transform.DOScale(1f, 1.2f).SetEase(Ease.OutBack);
        cubeImage.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
        coinImage.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
        CoinCollectImage.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
        CoinCollectImage.transform.DOMove(new Vector3(0, -40f, 0) + CoinCollectImage.transform.position, 3f).SetLoops(-1, LoopType.Yoyo);
        nextLevelButton.transform.DOScale(1f, 1f).SetEase(Ease.OutBack).SetDelay(0.8f).OnComplete(() =>
        {
            isClick = false;
        });
    }
    IEnumerator SpawnCoin(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Tạo coin
        GameObject coin = Instantiate(coinPrefab, CoinCollectImage.transform);

        // Vị trí random trong khu vực
        RectTransform coinRT = coin.GetComponent<RectTransform>();
        coinRT.anchoredPosition = new Vector2(
            Random.Range(-100f, 100f),
            Random.Range(-50f, 50f)
        );
        coinRT.transform.DOMove(new Vector3(50f, -100f, 0) + coinRT.transform.position, 0.3f).OnComplete(() =>
        {
            coinRT.transform.DOMove(coinOfPlayerImage.transform.position, 0.5f).OnComplete(() =>
            {
                Destroy(coin);
            });
        });

        // Di chuyển coin bay lên (Y tăng)
    }
    public void SpawnAndFlyCoins()
    {
        for (int i = 0; i < 10; i++)
        {
            float delay = 0.05f * i;
            StartCoroutine(SpawnCoin(delay));
        }
    }
    void NextLevelDelay()
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 5)
        {
            SceneManager.LoadScene(0);
        }
        else SceneManager.LoadScene(sceneIndex + 1);
    }
    public void CoinUpdate()
    {
        int c = PlayerPrefs.GetInt("Coin", 0) + 10;
        PlayerPrefs.SetInt("Coin", c);
        PlayerPrefs.Save();
        coinOfPlayerText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coin", 0).ToString();
    }
    public void NextLevel()
    {
        if (isClick == false)
        {
            SpawnAndFlyCoins();
            Invoke("NextLevelDelay", 1.6f);
            Invoke("CoinUpdate", 1.3f);
            Invoke("Haptic", 0.9f);
            isClick = true;
        }
    }
    void Haptic()
    {
        for (int loop = 0; loop < 5; loop++)
        {
            float delay = 0.1f * loop;
            StartCoroutine(PlayHapticAfterDelay(delay)); // Delay nhỏ
        }
    }
    private IEnumerator PlayHapticAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HapticFeedbackController.TriggerHaptics(HapticPatterns.PresetType.Selection);
    }
}
