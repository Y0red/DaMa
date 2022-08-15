using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Pawn : MonoBehaviour
{
    public bool isKIng, IsWhite;
    [SerializeField] GameObject crown;
    [SerializeField] int CurrentX, CurrentY;
    public bool ValidMove(Pawn[,] board, int x1, int y1, int x2, int y2)
    {
        if (board[x2, y2] != null) return false;

        int deltaMove = Mathf.Abs(x1 - x2);
        int deltaMoveY = y2 - y1;


        if(IsWhite || isKIng)
        {
            if(deltaMove == 1)
            {
                if(deltaMoveY == 1)
                {
                    return true;
                }
            }
            else if(deltaMove == 2)
            {
                if(deltaMoveY == 2)
                {
                    Pawn p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.IsWhite != IsWhite)
                        return true;
                }
            }
        }

        if (!IsWhite || isKIng)
        {
            if (deltaMove == 1)
            {
                if (deltaMoveY == -1)
                {
                    return true;
                }
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == -2)
                {
                    Pawn p = board [(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.IsWhite != IsWhite)
                        return true;
                }
            }
        }
        return false;
    }
    internal bool IsForcedToMove(Pawn[,] board, int x, int y)
    {
        if (IsWhite || isKIng)
        {
            // top left
            if(x >= 2 && y <= 5)
            {
                Pawn p = board[x - 1, y + 1];
                //if there is a pice and it is not he same color as ours
                if(p != null && p.IsWhite != IsWhite)
                {
                    //check if its possible to land after the jump
                    if(board[x - 2, y + 2] == null)
                    {
                        return true;
                    }
                }
            }

            //top right
            if (x <= 2 && y <= 5)
            {
                Pawn p = board[x + 1, y + 1];
                //if there is a pice and it is not he same color as ours
                if (p != null && p.IsWhite != IsWhite)
                {
                    //check if its possible to land after the jump
                    if (board[x + 2, y + 2] == null)
                    {
                        return true;
                    }
                }
            }
        }
        if(!IsWhite || isKIng)
        {
            // bot left
            if (x >= 2 && y >= 2)
            {
                Pawn p = board[x - 1, y - 1];
                //if there is a pice and it is not he same color as ours
                if (p != null && p.IsWhite != IsWhite)
                {
                    //check if its possible to land after the jump
                    if (board[x - 2, y - 2] == null)
                    {
                        return true;
                    }
                }
            }

            //bot right
            if (x <= 2 && y <= 5)
            {
                Pawn p = board[x + 1, y - 1];
                //if there is a pice and it is not he same color as ours
                if (p != null && p.IsWhite != IsWhite)
                {
                    //check if its possible to land after the jump
                    if (board[x + 2, y - 2] == null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }
    public  bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];
        Pawn c;
        int i, j;

        if (IsWhite)
        {
            //top left
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i--;
                j++;
                if (i > 0 && j <= 8)
                    break;

                c = BoardManager.Instance.Pices[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (IsWhite != c.IsWhite)
                        r[i, j] = true;

                    break;
                }
            }


            //top right
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i++;
                j++;
                if (i <= 8 && j <= 8)
                    break;

                c = BoardManager.Instance.Pices[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (IsWhite != c.IsWhite)
                        r[i, j] = true;

                    break;
                }
            }
        }
        else
        {
            //down left
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i--;
                j--;
                if (i >= 8 && j > 0)
                    break;

                c = BoardManager.Instance.Pices[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (IsWhite != c.IsWhite)
                        r[i, j] = true;

                    break;
                }
            }


            //down right
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i++;
                j--;
                if (i >= 8 && j < 0)
                    break;

                c = BoardManager.Instance.Pices[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (IsWhite != c.IsWhite)
                        r[i, j] = true;

                    break;
                }
            }
        }

        return r;
    }
    public void PromotToKing()
    {
        crown.SetActive(true);
    }
    public void DoScale(bool isDo)
    {
        bool isTrue = isDo;

        if (isTrue) DoScaleUpDown();
        else
        {
            transform.DOComplete();
            transform.Rotate(Vector3.zero);
        }
    }
    void DoScaleUpDown()
    {
      // transform.DOLocalRotate(transform.position , 1f).onComplete += delegate
      //  {
     //       DoScaleUpDown();
      //  };
    }
}
