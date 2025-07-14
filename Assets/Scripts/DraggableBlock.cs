using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    private Vector3 posDefault;

    void Start()
    {
        cam = Camera.main;
        posDefault = transform.position; // Lưu vị trí mặc định của block
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mousePos = GetMouseWorldPosition();
        transform.position = mousePos + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        // Snap block to grid here
        SnapToGrid();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = cam.transform.position.y; // distance từ camera đến block
        return cam.ScreenToWorldPoint(mouseScreenPos);
    }

    void SnapToGrid()
    {
        // foreach (Transform cube in transform) // Duyệt từng cube con trong block
        // {
        Vector3 worldPos = transform.position;

        // tìm GridCell gần nhất với mỗi cube con
        GridCell cell = GridManager.Instance.GetClosestCell(worldPos);

        if (cell == null || !cell.IsEmpty())
        {
            transform.position = posDefault; // Trả về vị trí mặc định nếu không tìm thấy ô trống
            Debug.Log("Không thể đặt block tại đây!");
            return; // Hủy đặt
        }

        // Snap vị trí cube vào đúng ô
        transform.position = cell.transform.position;
        cell.AddLayer(transform.gameObject);
        //  }

        // Sau khi đặt xong toàn bộ block
        Destroy(this); // huỷ script kéo thả
    }
}
