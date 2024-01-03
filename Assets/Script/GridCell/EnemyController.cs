using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    public Transform ballContainer;
    private bool isSpawned = false;
    private int playerCount;
    private Vector3Int playerPos;
    private int enemyCount;
    private Vector3Int enemyPos;

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

    public void EnemyTurn()
    {
        SetDefault();
        int maxX = GridCellManager.instance.GetEdgeCellRight().x - 1;
        int minX = GridCellManager.instance.GetEdgeCellLeft().x + 1;

        CheckEnemy(minX, maxX);
        playerPos = CheckPlayer(minX, maxX);

        if(playerCount > enemyCount)
        {
            if(playerPos.y != 3)
            {
                ObjectSpawner.instance.SpawnEnemyBall(playerPos.x);
                isSpawned = true;
                return;
            }
            else
            {
                Debug.Log("No player");
            }

        }
        else
        {
            if(enemyPos.y != 3)
            {
                ObjectSpawner.instance.SpawnEnemyBall(enemyPos.x);
                isSpawned = true;
                return;
            }
            else
            {
                Debug.Log("No enemy");
            }
        }
        
        CheckRandom(minX, maxX);

    }

    private void SetDefault()
    {
        enemyCount = 0;
        playerCount = 0;
        playerPos = new Vector3Int(0, 3, 0);
        enemyPos = new Vector3Int(0, 3, 0);
        isSpawned = false;
    }

    private void CheckRandom(int minX, int maxX)
    {
        if (!isSpawned)
        {
            int randomX = UnityEngine.Random.Range(minX, maxX + 1);
            Vector3Int startPos = new Vector3Int(randomX, 3, 0);
            while (GridCellManager.instance.IsPlacedCell(startPos))
            {
                randomX = UnityEngine.Random.Range(minX, maxX + 1);
                startPos = new Vector3Int(randomX, 3, 0);
            }
            ObjectSpawner.instance.SpawnEnemyBall(randomX);
        }
    }

    private Vector3Int GetLastCellAvailable(Vector3Int startPos, Direction direction)
    {
        Vector3Int dir = GridCellManager.instance.GetDirection(direction);
        Vector3Int nextPos = startPos + dir;
        Vector3Int lastPos = startPos;
        while (GridCellManager.instance.IsPlaceableArea(nextPos) 
            && !GridCellManager.instance.IsPlacedCell(nextPos))
        {
            lastPos = nextPos;
            nextPos += dir;
        }
        return lastPos;
    }

    private int CheckDirection(Vector3Int startPos, Direction direction)
    {
        Vector3Int dir = GridCellManager.instance.GetDirection(direction);
        Vector3Int nextPos = startPos + dir;

        int count = 0;

        while (GridCellManager.instance.IsPlacedCell(nextPos))
        {
            if (GridCellManager.instance.GetPlacedCell(nextPos).GetComponent<Ball>().GetBallType() != BallType.Enemy)
            {
                break;
            }
            nextPos += dir;
            count++;
        }
        return count;
    }

    private void CheckEnemy(int minX, int maxX)
    {
        for (int x = minX; x <= maxX; x++)
        {
            Vector3Int startPos = new Vector3Int(x, 3, 0);
            Vector3Int spawnPos = GetLastCellAvailable(startPos, Direction.Down);
            if (spawnPos.y != 3)
            {
                foreach (Direction d in Enum.GetValues(typeof(Direction)))
                {
                    int tmpEnemyCount = CheckDirection(spawnPos, d);
                    if (tmpEnemyCount > enemyCount)
                    {
                        enemyCount = tmpEnemyCount;
                        enemyPos = spawnPos;
                    }
                }
            }
        }
    }

    private Vector3Int CheckPlayer(int minX, int maxX)
    {
        playerCount = 0;
        Vector3Int spawnPos = new Vector3Int(0, 3, 0);

        for (int x = minX; x <= maxX; x++)
        {
            Vector3Int startPos = new Vector3Int(x, 3, 0);
            Vector3Int lastAvailable = GetLastCellAvailable(startPos, Direction.Down);
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                int count = CheckPlayerDirection(lastAvailable, d);
                if (count > playerCount)
                {
                    playerCount = count;
                    spawnPos = lastAvailable;
                }
            }
        }
        if(playerCount < 2)
        {
            spawnPos = new Vector3Int(0, 3, 0);
        }
        return spawnPos;
    }

    private int CheckPlayerDirection(Vector3Int startPos, Direction direction)
    {
        Vector3Int dir = GridCellManager.instance.GetDirection(direction);
        Vector3Int nextPos = startPos + dir;

        int count = 0;

        while (GridCellManager.instance.IsPlacedCell(nextPos))
        {
            if (GridCellManager.instance.GetPlacedCell(nextPos).GetComponent<Ball>().GetBallType() == BallType.Enemy)
            {
                break;
            }
            nextPos += dir;
            count++;
        }
        return count;
    }
}
