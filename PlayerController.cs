using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public bool walking = false;
    public bool canMove = true;  // 控制玩家是否可以移动

    public Transform currentCube;
    public Transform clickedCube;
    public Transform indicator;

    public List<Transform> finalPath = new List<Transform>();

    private float blend;

    public playercdeff playercdeff;
    public Contentmanagement contentmanagement;
    public HandManager handmanager;
    public PlayerStats stats;
    
    void Start()
    {
        RayCastDown();       
    }

    void Update()
    {
        // 檢測 Backpack 是否開啟，如果是，設置 canMove 為 false
        // 否則，根據玩家的輸入來控制 Backpack 的開啟與關閉
        // HandleBackpackInput();

        // 如果玩家不能移动，直接返回
        if (!canMove)
        {
            return;
        }

        // 获取当前所在的地块
        RayCastDown();

        if (currentCube != null && currentCube.GetComponent<Walkable>().movingGround)
        {
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        // 点击地块进行移动
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    clickedCube = mouseHit.transform;

                    DOTween.Kill(gameObject.transform);
                    finalPath.Clear();
                    FindPath();

                    blend = transform.position.y - clickedCube.position.y > 0 ? -1 : 1;

                   // Debug.Log("Clicked Cube: " + clickedCube.name);
                }
            }
        }
    }

    /// <summary>
    /// 路径查找方法
    /// </summary>
    void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();
    }

    /// <summary>
    /// 递归探索路径
    /// </summary>
    void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        if (nextCubes.Count == 0)
            return;

        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        if (current == clickedCube)
        {
            return;
        }

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }

        visitedCubes.Add(current);

        ExploreCube(nextCubes, visitedCubes);
    }

    /// <summary>
    /// 构建最终路径
    /// </summary>
    void BuildPath()
    {
        Transform cube = clickedCube;
        while (cube != currentCube)
        {
            finalPath.Add(cube);
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;
        }

        finalPath.Insert(0, clickedCube);

        FollowPath();
    }

    /// <summary>
    /// 移动到目标地块的方法
    /// </summary>
    public void MoveToTargetWalkable() // 點擊場景中的3D物件移動到目標
    {
        DOTween.Kill(gameObject.transform);
        finalPath.Clear();
        FindPath();
        FollowPath();
    }

    /// <summary>
    /// 跟随路径移动玩家
    /// </summary>
    void FollowPath()
    {
        Sequence s = DOTween.Sequence();
        walking = true;

        for (int i = finalPath.Count - 1; i >= 0; i--)
        {
            Walkable walkable = finalPath[i].GetComponent<Walkable>();
            float time = walkable.isStair ? 1.5f : 1f;

            // 移动到当前地块
            s.Append(transform.DOMove(walkable.GetWalkPoint(), .2f * time).SetEase(Ease.Linear));

            // 如果需要，旋转玩家
            if (!walkable.dontRotate)
                s.Join(transform.DOLookAt(finalPath[i].position, .1f, AxisConstraint.Y, Vector3.up));

            playercdeff playercdeff = FindObjectOfType<playercdeff>();
            if (playercdeff != null)
            {
                playercdeff.FindWalkableTileTags();
            }

            // 檢查踩到標籤地板效果
            s.AppendCallback(() =>
            {
                if (walkable.iscardTile)
                {
                    Debug.Log("踩到了 iscardTile 地块: " + walkable.name);
                    walkable.iscardTile = false;
                    contentmanagement.CheckiscardTile();
                    handmanager.Drawattackcard();
                }               
            });
        }

        if (clickedCube.GetComponent<Walkable>().isButton)
        {
            s.AppendCallback(() => GameManager.instance.RotateRightPivot());
        }

        s.AppendCallback(() => Clear());
    }


    /// <summary>
    /// 清除路径和状态
    /// </summary>
    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
        walking = false;
    }

    /// <summary>
    /// 射线检测，获取当前所在地块
    /// </summary>
    public void RayCastDown()
    {
        if (transform.childCount > 0)
        {
            Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
            RaycastHit playerHit;

            if (Physics.Raycast(playerRay, out playerHit))
            {
                Walkable walkable = playerHit.transform.GetComponent<Walkable>();
                if (walkable != null)
                {
                    currentCube = playerHit.transform;

                    // 处理移动地面
                    if (walkable.movingGround)
                    {
                        transform.parent = currentCube.parent;
                    }
                    else
                    {
                        transform.parent = null;
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (transform.childCount > 0)
        {
            Ray ray = new Ray(transform.GetChild(0).position, -transform.up);
            Gizmos.DrawRay(ray);
        }
    }

    /// <summary>
    /// 设置动画混合参数
    /// </summary>
    /// <param name="x">混合值</param>
    void SetBlend(float x)
    {
        GetComponentInChildren<Animator>().SetFloat("Blend", x);
    }
}
