using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    #region AI

    private int aiCount;
    private int maxDepth = 100;

    List<Pawn> pawns;

    List<Moves> allMOves = new List<Moves>();
   // [SerializeField]List<Pawn> selectedPawns = new List<Pawn>();
    Pawn selectedPawn;

    private void Start()
    {
        BoardManager.Instance.OncurrentEvent.AddListener(HandleGamePlay);
    }

    private void HandleGamePlay(PiceType type)
    {
      if(type == PiceType.White)
      {
         Debug.Log("watching");
      }
      else if( type == PiceType.Black)
      {
         Debug.Log("its Me");
         Play();
      }
    }

    void Play()
    {
        pawns = new List<Pawn>();
        pawns = BoardManager.Instance.GetAIPlayers();

        List<Pawn> preSelected = new List<Pawn>();
        bool foundEnemy = false;

        if (pawns != null)
        {
            foreach(Pawn p in pawns)
            {
                //check if ther are enemy pawn in front of all pawns that can move
                
               if(p.GetAllMoves().Count >= 1)
               {
                    if (IsEnemyInFront(p))
                    {
                        Debug.Log("enemy in front");
                        selectedPawn = p;

                        BoardManager.Instance.SelectPice((int)selectedPawn.GetCurrentPos().x, (int)selectedPawn.GetCurrentPos().y);

                        MM(selectedPawn);
                        foundEnemy = true;
                        break;
                    }
                    else preSelected.Add(p);
               }
            }

            if(!foundEnemy)SelectPawnToPlay(preSelected);
        }
    }
    void PlayAgain()
    {

       if(pawns == null)
       {
            pawns = new List<Pawn>();
            pawns = BoardManager.Instance.GetAIPlayers();
       }

        List<Pawn> preSelected = new List<Pawn>();
        bool foundEnemy = false;

        if (pawns != null)
        {
            foreach (Pawn p in pawns)
            {
                //check if ther are enemy pawn in front of all pawns that can move

                if (p.GetAllMoves().Count >= 1)
                {
                    if (IsEnemyInFront(p))
                    {
                        Debug.Log("enemy in front");
                        selectedPawn = p;

                        BoardManager.Instance.SelectPice((int)selectedPawn.GetCurrentPos().x, (int)selectedPawn.GetCurrentPos().y);

                        MM(selectedPawn);
                        foundEnemy = true;
                        break;
                    }
                    else preSelected.Add(p);
                }
            }

            if (!foundEnemy) SelectPawnToPlay(preSelected);
        }
    }

    void SelectPawnToPlay(List<Pawn> preSelected)
    {
        int length = preSelected.Count;

        int rand = UnityEngine.Random.Range(0, length);

        selectedPawn = preSelected[rand];

        BoardManager.Instance.SelectPice((int)selectedPawn.GetCurrentPos().x, (int)selectedPawn.GetCurrentPos().y);

        MM(selectedPawn);
    }
    void MM(Pawn selPawn)
    {
        Moves bestMove = new Moves();
        Moves justMove = new Moves();
        allMOves = selPawn.GetAllMoves();
       
        foreach(Moves m in allMOves)
        {
           //if there is enemy in front 
             //check if its protected
            if(Check(selPawn, m.X, m.Y))
            {
                if (m.Dir.Equals("DR"))
                {
                    //if its down right
                    if (Check(selPawn, m.X + 1, m.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DR1 cant eat player is protected{m.X + 1}:{m.Y - 1}");
                         pawns.Remove(selPawn);
                         PlayAgain();
                        break;
                    }
                    else
                    {
                        //can move enemy not protected
                        // StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, m.X + 1, m.Y - 1, 2f));
                        bestMove.Dir = m.Dir;
                        bestMove.X = m.X + 1;
                        bestMove.Y = m.Y - 1;
                        Debug.Log("best");
                    }
                }
                else
                {
                    //else its down left
                    if (Check(selPawn, m.X - 1, m.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DL1 cant eat player is protected{m.X - 1}:{m.Y - 1}");
                         pawns.Remove(selPawn);
                        PlayAgain();
                        break;
                    }
                    else
                    {
                        //can move enemy not protected
                        //StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, m.X - 1, m.Y - 1, 2f));
                        bestMove.Dir = m.Dir;
                        bestMove.X = m.X - 1;
                        bestMove.Y = m.Y - 1;
                        Debug.Log("best");
                    }
                }
            }
            else
            {
                justMove.Dir = m.Dir;
                justMove.X = m.X;
                justMove.Y = m.Y;
                Debug.Log(" just move");
            }
         }

        if (bestMove != null) StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, bestMove.X, bestMove.Y, 2f));
        if(justMove != null) StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, justMove.X, justMove.Y, 2f));
        //else PlayAgain();
    }
     void MakeMove(Pawn selPawn)
    {
        Moves moveOne, moveTwo;
        allMOves = selPawn.GetAllMoves();
        if(allMOves.Count > 1)
        {
            moveOne = allMOves[0];
            moveTwo = allMOves[1];

            Debug.Log("has 2 moves");

            if (Check(selPawn,moveOne.X, moveOne.Y))
            {
                ///////check if pawn has first move

                //has enemy in front
                //check enemy can be eaten
                if (moveOne.Dir.Equals("DR"))
                {
                    //if its down right
                    if (Check(selPawn, moveOne.X + 1, moveOne.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DR1 cant eat player is protected{moveOne.X + 1}:{moveOne.Y - 1}");
                        pawns.Remove(selPawn);
                        PlayAgain();
                    }
                    else
                    {
                        //can move enemy not protected
                        StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveOne.X + 1, moveOne.Y - 1, 2f));
                    }
                }
                else
                {
                    //else its down left
                    if (Check(selPawn, moveOne.X - 1, moveOne.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DL1 cant eat player is protected{moveOne.X - 1}:{moveOne.Y - 1}");
                        pawns.Remove(selPawn);
                        PlayAgain();
                    }
                    else
                    {
                        //can move enemy not protected
                        StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveOne.X - 1, moveOne.Y - 1, 2f));
                    }
                }

                ///////check if pawn has second move
                //has enemy in front
                //check enemy can be eaten
                if (moveTwo.Dir.Equals("DR"))
                {
                    //if its down right
                    if (Check(selPawn, moveTwo.X + 1, moveTwo.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DR2 cant eat player is protected{moveTwo.X + 1}:{moveTwo.Y - 1}");
                        pawns.Remove(selPawn);
                        PlayAgain();
                    }
                    else
                    {
                        //can move enemy not protected
                        StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveTwo.X + 1, moveTwo.Y - 1, 2f));
                    }
                }
                else
                {
                    //else its down left
                    if (Check(selPawn, moveTwo.X - 1, moveTwo.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DL2 cant eat player is protected{moveTwo.X - 1}:{moveTwo.Y - 1}");
                        pawns.Remove(selPawn);
                        PlayAgain();
                    }
                    else
                    {
                        //can move enemy not protected
                        StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveTwo.X - 1, moveTwo.Y - 1, 2f));
                    }
                }

            }
            else
            {
                ///checke before we move blindly
                ///
                Debug.Log("can move");
                StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveOne.X, moveOne.Y, 2f));
            }
        }
        else
        {
            moveOne = allMOves[0];

            Debug.Log("has 1 moves");


            if (Check(selPawn, moveOne.X, moveOne.Y))
            {
                ///////check if pawn has first move

                //has enemy in front
                //check enemy can be eaten
                if (moveOne.Dir.Equals("DR"))
                {
                    //if its down right
                    if (Check(selPawn, moveOne.X + 1, moveOne.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DR1 cant eat player is protected{moveOne.X + 1}:{moveOne.Y - 1}");
                        pawns.Remove(selPawn);
                        PlayAgain();
                    }
                    else
                    {
                        //can move enemy not protected
                        StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveOne.X + 1, moveOne.Y - 1, 2f));
                    }
                }
                else
                {
                    //else its down left
                    if (Check(selPawn, moveOne.X - 1, moveOne.Y - 1))
                    {
                        //cant move enemy protected
                        Debug.Log($"DL1 cant eat player is protected{moveOne.X - 1}:{moveOne.Y - 1}");
                        pawns.Remove(selPawn);
                        PlayAgain();
                    }
                    else
                    {
                        //can move enemy not protected
                        StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveOne.X - 1, moveOne.Y - 1, 2f));
                    }
                }
            }
            else
            {
                Debug.Log("can move");
                StartCoroutine(MoveSelected((int)selPawn.GetCurrentPos().x, (int)selPawn.GetCurrentPos().y, moveOne.X, moveOne.Y, 2f));
            }
        }

    }
     IEnumerator MoveSelected(int x1, int y1, int x2, int y2,float sec)
    {
        yield return new WaitForSeconds(sec);
        BoardManager.Instance.TryMove(x1, y1, x2, y2);
        yield return null;
    }
     bool Check(Pawn p,int x, int y)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return false;
        Pawn c = BoardManager.Instance.Pices[x, y];
        if (c == null) return false;
        else
        {
            if (p.IsWhite != c.IsWhite) return true;
            else return false;
        }
    }
     bool IsEnemyInFront(Pawn p)
    {
        List<Moves> mov = p.GetAllMoves();

        foreach(Moves m in mov)
        {
            if (Check(p, m.X, m.Y)) return true;
        }
        return false;
    }

    #endregion
}
