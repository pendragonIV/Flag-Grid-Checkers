using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private BallType ballType; 
    
    private List<GameObject> matchListHorizontal = new List<GameObject>();
    private List<GameObject> matchListVertical = new List<GameObject>();
    private List<GameObject> matchListLeftDiagonal = new List<GameObject>();
    private List<GameObject> matchListRightDiagonal = new List<GameObject>();

    private void Start()
    {
        BouncingBall();
    }

    private void BouncingBall()
    {
        Vector3Int ballPos = GridCellManager.instance.GetObjCell(transform.position);
        Vector3Int nextPos = ballPos;
        int count = 0;

        while (GridCellManager.instance.IsPlaceableArea(nextPos + Vector3Int.down)
            && !GridCellManager.instance.IsPlacedCell(nextPos + Vector3Int.down))
        {
            nextPos += Vector3Int.down;
            count++;
        }

        if (!GridCellManager.instance.IsPlaceableArea(nextPos))
        {
            Destroy(gameObject);
        }

        Vector3 nextBallPos = GridCellManager.instance.PositonToMove(nextPos);
        GridCellManager.instance.SetPlacedCell(nextPos, gameObject);
        transform.DOMove(nextBallPos, .2f * count).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(() =>
        {
            CheckMatch(nextPos);
            if(ballType == BallType.Enemy)
            {
                ObjectSpawner.instance.PlayerTurn();
                GameManager.instance.gameScene.ChangeTurn(1);
            }
            else
            {
                EnemyController.instance.EnemyTurn();
                GameManager.instance.gameScene.ChangeTurn(2);
            }
        });

    }

    private void CheckMatch(Vector3Int checkPos)
    {
        SetDefaultList();

        foreach (Direction d in Enum.GetValues(typeof(Direction)))
        {
            CheckDirection(checkPos, d);
        }

        CheckMatchList();
    }

    private void SetDefaultList()
    {
        matchListRightDiagonal.Add(gameObject);
        matchListLeftDiagonal.Add(gameObject);
        matchListHorizontal.Add(gameObject);
        matchListVertical.Add(gameObject);
    }

    private void CheckMatchList()
    {
        int player;
        if(ballType == BallType.Enemy)
        {
            player = 2;
        }
        else
        {
            player = 1;
        }

        if (matchListHorizontal.Count >= 4 || matchListVertical.Count >= 4 || matchListLeftDiagonal.Count >= 4 || matchListRightDiagonal.Count >= 4)
        {
            GameManager.instance.Win(player);
            return;
        }

        if(GridCellManager.instance.IsFull())
        {
            GameManager.instance.ShowDrawPanel();
        }

    }

    private void CheckDirection(Vector3Int startPos, Direction direction)
    {
        List<GameObject> matchList = new List<GameObject>();
        Vector3Int dir = GridCellManager.instance.GetDirection(direction);
        Vector3Int nextPos = startPos + dir;

        while (GridCellManager.instance.IsPlacedCell(nextPos))
        {
            if(GridCellManager.instance.GetPlacedCell(nextPos).GetComponent<Ball>().GetBallType() != ballType)
            {
                break;
            }
            matchList.Add(GridCellManager.instance.GetPlacedCell(nextPos));
            nextPos += dir;
        }

        if(direction == Direction.RightTop || direction == Direction.LeftDown)
        {
            matchListRightDiagonal.AddRange(matchList);
        }else if(direction == Direction.LeftTop || direction == Direction.RightDown)
        {
            matchListLeftDiagonal.AddRange(matchList);
        }
        else if(direction == Direction.Left || direction == Direction.Right)
        {
            matchListHorizontal.AddRange(matchList);
        }
        else if(direction == Direction.Down)
        {
            matchListVertical.AddRange(matchList);
        }
    }


    public BallType GetBallType()
    {
        return ballType;
    }
}
