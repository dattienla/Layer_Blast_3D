using UnityEngine;
using DG.Tweening;

public class DoTweenAnim : MonoBehaviour
{
    public Vector3 targetPosition;   // vị trí đích    // thời gian bay lên
    public float startYOffset = -15f; // điểm xuất phát bên dưới màn hình

    void Start()
    {
        AnimateIn();
    }

    public void AnimateIn()
    {
        // Bắt đầu từ bên dưới màn hình
        transform.position = new Vector3(targetPosition.x, targetPosition.y, startYOffset);
        transform.localScale = Vector3.one; // hoặc lớn hơn nếu bạn muốn phóng to rồi thu nhỏ

        // Dùng DOTween để di chuyển và thu nhỏ 
        transform.DOScale(0.5f, 0.1f).OnComplete(() =>
        {
            transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutBack);
        });
    }

    public void ZoomIn()
    {
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
    public void ZoomOut()
    {
        transform.DOScale(0.5f, 0.2f).SetEase(Ease.InBack);
        transform.DOMove(targetPosition, 0.3f).SetEase(Ease.InBack);
    }

}
