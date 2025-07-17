using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    // private Vector3 posDefault;
    private List<GridCell> previewCells = new List<GridCell>();
    private DoTweenAnim doTweenAnim;

    void Start()
    {
        doTweenAnim = GetComponent<DoTweenAnim>();
        cam = Camera.main;
        //posDefault = doTweenAnim.targetPosition[doTweenAnim.index]; // Lưu vị trí mặc định của block
    }

    void Update()
    {
        // PC: xử lý bằng chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                DragBlock(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                EndDrag();
            }
        }

        // Mobile: xử lý bằng touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TryStartDrag(touch.position);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                    {
                        DragBlock(touch.position);
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging)
                    {
                        EndDrag();
                    }
                    break;
            }
        }
    }

    void TryStartDrag(Vector3 inputPos)
    {
        Ray ray = cam.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform) // chỉ bắt khi chạm đúng object này
            {
                doTweenAnim.ZoomIn();
                offset = transform.position - GetWorldPosition(inputPos);
                isDragging = true;
            }
        }
    }

    void DragBlock(Vector3 inputPos)
    {
        Vector3 worldPos = GetWorldPosition(inputPos);
        transform.position = worldPos + offset;
        PreviewSnapCells();
    }

    void EndDrag()
    {
        isDragging = false;
        if (previewCells != null)
        {
            foreach (var cell in previewCells)
            {
                cell.GetComponent<GridCell>().SetHighlight(false);
            }
        }
        SnapToGrid();
    }

    Vector3 GetWorldPosition(Vector3 screenPos)
    {
        screenPos.z = cam.WorldToScreenPoint(transform.position).z;
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.y = 0f;
        return worldPos;
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
            BlockManager placedBlock = GetComponent<BlockManager>();
            placedBlock.isUsed = true; // Đánh dấu block đã được sử dụng
            QueueBlockManager.Instance.DeleteBlockFromQueue();
            //Sau khi snap, kiểm tra xem có block nào trùng màu không thì xoá đi
            if (placedBlock != null)
            {
                FindObjectOfType<DeleteBlock>().ExplodeBlockAndNeighBors(placedBlock);
            }
            // Sau khi đặt xong, huỷ chức năng kéo thả
            Destroy(this);
            QueueBlockManager.Instance.CheckEndGame(); // Kiểm tra kết thúc game sau khi snap
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
/*
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
        return worldPos;*/
