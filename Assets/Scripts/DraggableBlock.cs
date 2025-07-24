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
        List<GridCell> targetCells = new List<GridCell>();

        // Kiểm tra tất cả cube con xem có đủ ô trống để snap không
        foreach (Transform cube in transform)
        {
            Vector3 worldPos = cube.position;
            GridCell cell = GridManager.Instance.GetClosestCell(worldPos);

            if (cell == null || !cell.IsEmpty())
            {
                doTweenAnim.ZoomOut(); // Trả về vị trí mặc định nếu không hợp lệ
            }
            else
            {
                targetCells.Add(cell); // Lưu lại cell hợp lệ
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
            KK();
            if (FindObjectOfType<DeleteBlock>().isExplode == false) break;
            FindObjectOfType<DeleteBlock>().cnt = 0;
            yield return new WaitForSeconds(1.1f);
        }
        FindObjectOfType<DeleteBlock>().isDo = false;
        Debug.Log("Coroutine kết thúc");
        Invoke(nameof(CheckEndGameDelay), (j - 1f) * 1.3f + 0.5f);
    }
    void KK()
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

