using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    private Vector3 posDefault;
    private List<GridCell> previewCells = new List<GridCell>();
    private DoTweenAnim doTweenAnim;

    void Start()
    {
        doTweenAnim = GetComponent<DoTweenAnim>();
        cam = Camera.main;
        posDefault = doTweenAnim.targetPosition; // Lưu vị trí mặc định của block
    }

    void OnMouseDown()
    {
        doTweenAnim.ZoomIn(); // Phóng to block về trạng thái ban đầu
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mousePos = GetMouseWorldPosition();
        transform.position = mousePos + offset;
        PreviewSnapCells();
    }

    void OnMouseUp()
    {
        isDragging = false;
        foreach (var cell in previewCells)
        {
            cell.SetHighlight(false);
        }
        SnapToGrid();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        worldPos.y = 0f; // ép xuống mặt đất (trục y = 0)
        return worldPos;
        //Vector3 mouseScreenPos = Input.mousePosition;
        //mouseScreenPos.z = cam.transform.position.y; // distance từ camera đến block
        //return cam.ScreenToWorldPoint(mouseScreenPos);
    }
    /// <summary>
    /// snap block vào grid
    /// </summary>
    void SnapToGrid()
    {
        List<GridCell> targetCells = new List<GridCell>();

        // Kiểm tra tất cả cube con xem có đủ ô tróng để snap không
        foreach (Transform cube in transform)
        {
            Vector3 worldPos = cube.position;
            GridCell cell = GridManager.Instance.GetClosestCell(worldPos);

            if (cell == null || !cell.IsEmpty())
            {
                Debug.Log("Không thể đặt block vì có ô bị chiếm hoặc nằm ngoài lưới.");
                doTweenAnim.ZoomOut(); // Trả về vị trí mặc định nếu không hợp lệ
            }
            else
                targetCells.Add(cell); // Lưu lại cell hợp lệ
        }

        // Snap toàn bộ cùng lúc
        if (targetCells.Count == transform.childCount)
        {
            for (int i = 0; i < targetCells.Count; i++)
            {
                Transform cube = transform.GetChild(i);
                GridCell cell = targetCells[i];

                // Snap vị trí và add vào cell
                cube.position = cell.transform.position;
                cell.layers.Push(cube.gameObject); // Tăng số lượng layer trong cell
            }
            //Sau khi snap, kiểm tra xem có block nào trùng màu không thì xoá đi
            BlockManager placedBlock = GetComponent<BlockManager>();
            if (placedBlock != null)
            {
                FindObjectOfType<DeleteBlock>().ExplodeBlockAndNeighBors(placedBlock);
            }
            // Sau khi đặt xong, huỷ chức năng kéo thả
            Destroy(this);
        }
    }
    /// <summary>
    /// Highlight các ô mà block sẽ snap vào
    /// </summary>
    void PreviewSnapCells()
    {
        // Xóa highlight cũ
        foreach (var cell in previewCells)
        {
            cell.SetHighlight(false);
        }

        previewCells.Clear();

        bool cellValid = true;

        foreach (Transform cube in transform)
        {
            Vector3 worldPos = cube.position;
            GridCell cell = GridManager.Instance.GetClosestCell(worldPos);

            if (cell == null || !cell.IsEmpty())
            {
                cellValid = false;
            }

            if (cell != null && !previewCells.Contains(cell))
                previewCells.Add(cell);
        }

        // Highlight màu khác tùy theo hợp lệ
        foreach (var cell in previewCells)
        {
            cell.SetHighlight(true, cellValid);
        }
    }


}
