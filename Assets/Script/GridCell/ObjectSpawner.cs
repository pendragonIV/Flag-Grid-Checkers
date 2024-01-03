using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BallType
{
    Player,
    Enemy
}   

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;

    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject playerBall;
    [SerializeField] private GameObject enemyBall;
    [SerializeField] private Transform ballContainer;

    [SerializeField] private bool isPlayerTurn = true;  

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;    
        }
    }

    private void Update()
    {
        ArrowController();

        if (isPlayerTurn)
        {
            EnableArrow();
        }
        else
        {
            DisableArrow();
        }

        if(Input.GetMouseButtonDown(0) && isPlayerTurn)
        {
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                Debug.Log("EventSystem");
                return;
            }
            if(SpawnBall(arrow.transform.position, playerBall))
            {
                isPlayerTurn = false;
            }
        }
    }

    public void DisableArrow() 
    {
        if (!arrow.activeSelf)
        {
            return;
        }
        arrow.SetActive(false);
    }

    public void EnableArrow()
    {
        if (arrow.activeSelf)
        {
            return;
        }
        arrow.SetActive(true);
    }

    public void PlayerTurn()
    {
        isPlayerTurn = true;
    }

    private void ArrowController()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos.y = arrow.transform.position.y;
        Vector3Int mouseCellPos = GridCellManager.instance.GetObjCell(mousePos);

        if(mouseCellPos.x > GridCellManager.instance.GetEdgeCellRight().x - 1 
            || mouseCellPos.x < GridCellManager.instance.GetEdgeCellLeft().x + 1)
        {
            return;
        }

        Vector3 arrowPos = GridCellManager.instance.PositonToMove(mouseCellPos);
        arrow.transform.DOMove(arrowPos, .3f).SetUpdate(true);
    }

    public void SpawnEnemyBall(int spawnX)
    {
        Vector3 spawnPos = arrow.transform.position;
        Vector3Int spawnCellPos = GridCellManager.instance.GetObjCell(spawnPos);
        spawnCellPos.x = spawnX;
        if (GridCellManager.instance.IsPlacedCell(spawnCellPos + Vector3Int.down))
        {
            EnemyController.instance.EnemyTurn();
            return;
        }
        spawnPos = GridCellManager.instance.PositonToMove(spawnCellPos);
        SpawnBall(spawnPos, enemyBall);
    }

    private bool SpawnBall(Vector3 spawnPos, GameObject ballPrefab)
    {
        bool isSpawned = false;
        Vector3Int spawnCellPos = GridCellManager.instance.GetObjCell(spawnPos);
        if(GridCellManager.instance.IsPlacedCell(spawnCellPos + Vector3Int.down))
        {
            return isSpawned;
        }
        Vector3 spawnBallPos = GridCellManager.instance.PositonToMove(spawnCellPos);
        GameObject newBall = Instantiate(ballPrefab, ballContainer);
        newBall.transform.position = spawnBallPos;
        isSpawned = true;
        return isSpawned;
    }
}
