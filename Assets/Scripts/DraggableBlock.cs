using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    bool isDraggable = true; // Biến này có thể được điều chỉnh từ bên ngoài để chặn việc kéo block
    // private Vector3 posDefault;
    private List<GridCell> previewCells = new List<GridCell>();
    private DoTweenAnim doTweenAnim;
    public List<BlockManager> BlockQ;
    private void Awake()
    {
        BlockQ = new List<BlockManager>();
    }
    void Start()
    {
        doTweenAnim = GetComponent<DoTweenAnim>();
        cam = Camera.main;
        //posDefault = doTweenAnim.targetPosition[doTweenAnim.index]; // Lưu vị trí mặc định của block
    }

    void Update()
    {
        if (FindObjectOfType<DeleteBlock>().isDo == true)
        {
            return; // Nếu đang xử lý nổ block thì không cho kéo
        }
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
        if (!isDraggable) return; //  chặn bắt đầu kéo

        Ray ray = cam.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
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
        // Kiểm tra tất cả cube con xem có đủ ô trống để snap không
        List<GridCell> targetCells = new List<GridCell>();
        Vector3 worldPos0 = transform.GetChild(0).position;
        GridCell cell0 = GridManager.Instance.GetClosestCell(worldPos0);

        if (cell0 == null || !cell0.IsEmpty())
        {
            doTweenAnim.ZoomOut(); // Trả về vị trí mặc định nếu không hợp lệ
        }
        else
        {
            targetCells.Add(cell0); // Lưu lại cell hợp lệ
        }

        foreach (Transform cube in transform)
        {
            if (cube == transform.GetChild(0)) continue; // Bỏ qua cube đầu tiên đã kiểm tra
            Vector2Int cubeAbs = new Vector2Int(Mathf.Abs((int)cube.localPosition.z), Mathf.Abs((int)cube.localPosition.x));
            Vector2Int cube0Abs = new Vector2Int(Mathf.Abs((int)transform.GetChild(0).localPosition.z), Mathf.Abs((int)transform.GetChild(0).localPosition.x));
            Vector2Int dis = cubeAbs - cube0Abs;
            if (cell0 != null)
            {
                Vector2Int posCell = new Vector2Int(cell0.x + dis.x, cell0.y + dis.y); // tính toán vị trí của ô trong grid
                GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
                if (cell == null || !cell.IsEmpty())
                {
                    doTweenAnim.ZoomOut(); // Trả về vị trí mặc định nếu không hợp lệ
                }
                else
                {
                    targetCells.Add(cell); // Lưu lại cell hợp lệ
                }
            }

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
                if (cube.tag == "cubeMatOut" || cube.tag == "cubeMatIn") continue;
                cell.layers.Enqueue(cube.gameObject); // Tăng số lượng layer trong cell
            }
            int cnt = 0;
            BlockManager placedBlock = GetComponent<BlockManager>();
            placedBlock.isUsed = true; // Đánh dấu block đã được sử dụng
            QueueBlockManager.Instance.DeleteBlockFromQueue();

            if (!BlockQ.Contains(placedBlock)) BlockQ.Add(placedBlock);

            while (cnt != BlockQ.Count)
            {
                cnt = BlockQ.Count;
                Queue<BlockManager> temp = new Queue<BlockManager>(BlockQ);
                foreach (var dat in temp)
                {
                    HashSet<BlockManager> H = FindObjectOfType<DeleteBlock>().GetBlockNeighbor(dat);
                    foreach (var h in H)
                    {
                        if (!BlockQ.Contains(h)) BlockQ.Add(h);
                    }
                }
            }
            StartCoroutine(CallKKMultipleTimes());
            isDraggable = false;
        }
    }
    void CheckEndGameDelay()
    {
        QueueBlockManager.Instance.CheckEndGame();
    }
    IEnumerator CallKKMultipleTimes()
    {
        int j = 0;
        while (j < 10)
        {
            j++;
            FindObjectOfType<DeleteBlock>().isDo = true;
            Debug.Log("Coroutine Loop: " + j + " | Time: " + Time.time);
            ExpldeBlock();
            if (FindObjectOfType<DeleteBlock>().isExplode == false) break;
            FindObjectOfType<DeleteBlock>().cnt = 0;
            yield return new WaitForSeconds(1.1f);
        }
        FindObjectOfType<DeleteBlock>().isDo = false;
        Debug.Log("Coroutine kết thúc");
        Invoke(nameof(CheckEndGameDelay), (j - 1f) * 1.3f + 0.3f);
    }
    void ExpldeBlock()
    {
        Queue<BlockManager> temp = new Queue<BlockManager>(BlockQ);

        foreach (var dat in temp)
        {
            FindObjectOfType<DeleteBlock>().ExplodeBlockAndNeighBors(dat);
            if (dat.GetColorOutSite() == Color.white) BlockQ.Remove(dat);
        }
        foreach (var pre in FindObjectOfType<DeleteBlock>().PreBlockExplode)
        {
            pre.Explode();
        }
        FindObjectOfType<DeleteBlock>().PreBlockExplode.Clear();
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
        Vector3 worldPos0 = transform.GetChild(0).position;
        GridCell cell0 = GridManager.Instance.GetClosestCell(worldPos0);

        if (cell0 == null || !cell0.IsEmpty())
        {
            cellValid = false;
        }
        else
        {
            previewCells.Add(cell0); // Lưu lại cell hợp lệ
        }
        foreach (Transform cube in transform)
        {
            if (cube == transform.GetChild(0)) continue; // Bỏ qua cube đầu tiên đã kiểm tra
            Vector2Int cubeAbs = new Vector2Int(Mathf.Abs((int)cube.localPosition.z), Mathf.Abs((int)cube.localPosition.x));
            Vector2Int cube0Abs = new Vector2Int(Mathf.Abs((int)transform.GetChild(0).localPosition.z), Mathf.Abs((int)transform.GetChild(0).localPosition.x));
            Vector2Int dis = cubeAbs - cube0Abs;
            if (cell0 != null)
            {
                Vector2Int posCell = new Vector2Int(cell0.x + dis.x, cell0.y + dis.y); // tính toán vị trí của ô trong grid
                GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
                if (cell == null || !cell.IsEmpty())
                {
                    cellValid = false;
                }
                else
                {
                    previewCells.Add(cell); // Lưu lại cell hợp lệ
                }
            }
        }
        // Highlight màu khác tùy theo hợp lệ
        if (previewCells.Count == transform.childCount)
        {
            foreach (var cell in previewCells)
            {
                cell.SetHighlight(true, cellValid);
            }
        }
    }
}

