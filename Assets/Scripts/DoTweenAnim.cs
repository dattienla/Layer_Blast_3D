using UnityEngine;
using DG.Tweening;

public class DoTweenAnim : MonoBehaviour
{
    private Vector3[] targetPosition;   // vị trí đích  
    public int index; // chỉ số của block trong hàng đợi

    private Vector3 startPosition = new Vector3(0f, 0f, -15f); // vị trí bắt đầu


    void Start()
    {
        transform.position = startPosition;
        targetPosition = new Vector3[10];
        targetPosition[1] = new Vector3(-3.22f, 0, -3); // vị trí slot 1
        targetPosition[2] = new Vector3(-0.62f, 0, -3);  // vị trí slot 2
        targetPosition[3] = new Vector3(1.98f, 0, -3);  // vị trí slot 3
    }
    public void BlockStart()
    {
        // Dùng DOTween để di chuyển và thu nhỏ 
        transform.DOScale(new Vector3(0.6f, 0.35f, 0.6f), 0.1f).OnComplete(() =>
        {
            transform.DOMove(targetPosition[index], 0.3f).SetEase(Ease.OutBack);
        });
    }

    public void ZoomIn()
    {
        transform.DOScale(new Vector3(1f, 0.8f, 1f), 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
        });
        Vector3 pos = transform.position;
        pos.y = 0.2f;
        transform.position = pos;
    }
    public void ZoomOut()
    {
        transform.DOScale(new Vector3(0.6f, 0.35f, 0.6f), 0.2f).SetEase(Ease.InBack);
        transform.DOMove(targetPosition[index], 0.3f)
                 .SetEase(Ease.InBack);

    }
}
