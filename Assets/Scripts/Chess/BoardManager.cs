using MenuSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardManager : Manager<BoardManager>
{
    public EventGameState OncurrentEvent;
    
    public PiceType currentPice = PiceType.White;
    public Pawn[,] Pices = new Pawn[8, 8];
    [SerializeField] Pawn selectedPices;

    private const float TILE_SIZE = 1f;
    private const float TILE_OFFSET = 0.5f;

    public int selectionX = -1;
    public int selectionY = -1;

    [SerializeField] GameObject whitePrifabs, blackPrifabs;
    [SerializeField] Transform whitePicesTransform, blackPicesTransform;
    [SerializeField]private List<GameObject> whitePices, blackPices;

    bool selected = false;
    public bool isWhiteturn = true;
    bool hasKilled = false;
    Quaternion orientation = Quaternion.Euler(0, 0, 0);

    Vector2 startDrag, endDrag;

    public string turn = string.Empty;
    public string winner = string.Empty;

    public bool isAiPlayer = false;

    void Start()
    {
        //SpawnMyPices();
        // GameEvents.Instance.OnStartGame += StartGame;
        StartGame();
    }
    void StartGame()
    {
        //UiManager.Instance.LoadMenu("Game_Play_Menu");
        if (isAiPlayer) GetComponent<NPC>().enabled = true;
        SpawnMyPices();
        OncurrentEvent.Invoke(currentPice);
    }
    private void LateUpdate()
    {
        if (isWhiteturn)
        {
            turn = "White";
            currentPice = PiceType.White;
        }
        else if (!isWhiteturn)
        {
            turn = "Black";
            currentPice = PiceType.Black;
        }
    }
    void Update()
    {
        //DrawBoard();
        UpdteSelection();

        if (selectedPices != null) UpdatePiceDrag(selectedPices);

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0 && !selected)
            {
                SelectPice(selectionX, selectionY);
            }
            else if (selected == true && selectionX >= 0 && selectionY >= 0)
            {
                TryMove((int)startDrag.x, (int)startDrag.y, selectionX, selectionY);
            }
            //else if (selectionX < 0 && selectionY < 0)
            //{
            //    //Debug.Log("out of bounds");
            //   // selected = false;
            //   // BoardHighlight.Instance.HideHilights();
            //}
            //else
            //{
            //    //Debug.Log("bounds");
            //  //  selected = false;
            //  //  BoardHighlight.Instance.HideHilights();
            //}
        }   
    }
    public void TryMove(int x1, int y1, int x2, int y2)
    {
        startDrag = new Vector2(x1,y1);
        endDrag = new Vector2(x2, y2);

        selectedPices = Pices[x1, y1];

        if (x2 < 0 || x2 >= Pices.Length || y2 < 0 || y2 >= Pices.Length)
        {
            if(selectedPices != null)
            {
                MoveMyPices(selectedPices, x1, y1);
            }
            startDrag = Vector2.zero;
            selectedPices = null;
            selected = false;
            //Debug.Log("out of bounds");
            return;
        }
        if (selectedPices != null)
        {
            //if it has not moved
            if (endDrag == startDrag)
            {
                MoveMyPices(selectedPices, x1, y1);

                startDrag = Vector2.zero;
                selectedPices = null;
                selected = false;

                //Debug.Log("not Moved");
                return;
            }

            // check if its a valid move
            if (selectedPices.ValidMove(Pices, x1, y1, x2, y2))
            {
                //did we kill anything
                //if this is a jump
                if (Mathf.Abs(x2 - x1) == 2)
                {
                    Pawn p = Pices[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null)
                    {
                        Pices[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        p.gameObject.SetActive(false);
                        if (p.IsWhite) whitePices.Remove(p.gameObject);
                        else blackPices.Remove(p.gameObject);
                        hasKilled = true;
                    }
                }

                Pices[x2, y2] = selectedPices;
                Pices[x1, y1] = null;
                MoveMyPices(selectedPices, x2, y2);
                EndTurn();
                //Debug.Log("valid");
                return;
            }
            else
            {
                MoveMyPices(selectedPices, x1, y1);

                startDrag = Vector2.zero;
                selectedPices = null;
                selected = false;
                //Debug.Log("not valid");
                return;
            }
        }
    }
    private void MoveMyPices(Pawn p, int x, int y)
    {
        p.DoScale(false);
        p.transform.position = ChessCenter(x, y);
    }
    List<Pawn> ScanForPossibleMove(Pawn p, int x, int y)
    {
        List<Pawn> forcedPices = new List<Pawn>();

        if (Pices[x, y].IsForcedToMove(Pices, x, y))
        {
            forcedPices.Add(Pices[x, y]);
        }
        return forcedPices;
    }
    List<Pawn> ScanForPossibleMove()
    {
       List<Pawn> forcedPices = new List<Pawn>();
        //check all pices
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(Pices[i, j] != null && Pices[i, j].IsWhite == isWhiteturn)
                {
                    if (Pices[i, j].IsForcedToMove(Pices, i, j))
                    {
                        forcedPices.Add(Pices[i, j]);
                    }
                }
            }
        }

        return forcedPices;
    }
    void UpdatePiceDrag(Pawn p)
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("ChessPlane")))
        {
            p.transform.position = hit.point + Vector3.up;
            p.DoScale(true);
            //Debug.Log(hit.point);

        }
    }
    public void SelectPice(int x, int y)
    {
        if (Pices[x, y] == null)
            return;
        
        if (Pices [x,y].IsWhite != isWhiteturn)
           return;


        // allowedMoves = PicesManagers[x, y].PossibleMoves();
        // selectedChessManager = PicesManagers[x, y];
        // BoardHighlight.Instance.HilightAllowedMoves(allowedMoves);
        Pawn p = Pices[x, y];
        if(p != null)
        {
            selectedPices = p;
            startDrag = new Vector2(x, y);
            selected = true;

            bool[,] allowedMoves = Pices[x, y].PossibleMoves();

            //BoardHighlight.Instance.HilightAllowedMoves(allowedMoves);
        }
        else
        {
           // BoardHighlight.Instance.HideHilights();
        }
    }
    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        //draw selection Lline
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.right * (selectionX) + Vector3.forward * selectionY,
                Vector3.right * (selectionX + 1) + Vector3.forward * selectionY, Color.black);

            Debug.DrawLine(Vector3.right * (selectionX) + Vector3.forward * selectionY,
                Vector3.right * (selectionX) + Vector3.forward * (selectionY + 1), Color.black);

            Debug.DrawLine(Vector3.right * (selectionX) + Vector3.forward * (selectionY + 1),
                Vector3.right * (selectionX + 1) + Vector3.forward * (selectionY + 1), Color.black);

            Debug.DrawLine(Vector3.right * (selectionX + 1) + Vector3.forward * (selectionY),
                Vector3.right * (selectionX + 1) + Vector3.forward * (selectionY + 1), Color.black);

            Debug.DrawLine(
                           Vector3.forward * selectionY + Vector3.right * selectionX,
                          Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(
                           Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                           Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
    private void SpawnChesman(GameObject prifab, int x, int y)
    {
        Vector3 ro = new Vector3(-90, 0, 0);
        GameObject go = Instantiate(prifab, ChessCenter(x,y), orientation) as GameObject;
       
        go.transform.SetParent(transform);
        Pices[x, y] = go.GetComponent<Pawn>();
        Pices[x, y].SetPosition(x,y);
        if(go.TryGetComponent<Pawn>(out Pawn comp))
        {
            if (comp.IsWhite) { whitePices.Add(go); go.transform.SetParent(whitePicesTransform); }
            else { blackPices.Add(go); go.transform.SetParent(blackPicesTransform); }
        }
    }
    private Vector3 ChessCenter(int x, int y)
    {
        Vector3 orign = Vector3.zero;
        orign.x += (TILE_SIZE * x) + TILE_OFFSET;
        orign.z += (TILE_SIZE * y) + TILE_OFFSET;
        orign.y += 0.2f;
        return orign;
    }
    private void UpdteSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;

            //Debug.Log(hit.point);

        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    void SpawnMyPices()
    {
        whitePices = new List<GameObject>();
        blackPices = new List<GameObject>();

        Pices = new Pawn[8, 8];

        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate
                SpawnChesman(whitePrifabs, (oddRow) ? x : x + 1, y);
            }
        }

        //spawn black
      
        for (int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate
                SpawnChesman(blackPrifabs, (oddRow) ? x : x + 1, y);
            }
        }
    }
    private void EndTurn()
    {
        int x = (int)endDrag.x; 
        int y = (int)endDrag.y;

        //promotion
        DoKingPromotion(y);

        selectedPices = null;
        selected = false;

        //check for double move
        if(ScanForPossibleMove(selectedPices, x, y).Count != 0 && hasKilled)
        {
            return;
        }
        //else end turn
        CheckVictory();
        startDrag = Vector2.zero;
        isWhiteturn = !isWhiteturn;
        OncurrentEvent.Invoke(isWhiteturn ? PiceType.White : PiceType.Black);
        hasKilled = false;
        //GameEvents.Instance.UpdateGameText(turn);
    }
    private void CheckVictory()
    {
        //throw new NotImplementedException();
        if (blackPices.Count <= 0)
        {
            //white wins
            winner = "White Wins";
            EndGame();
            return;
        }
        else if(whitePices.Count <= 0)
        {
            //black wins
            winner = "Black Wins";
            EndGame();
            return;
        }
        return;
    }
    private void DoKingPromotion(int y)
    {
        if (selectedPices != null)
        {
            if (selectedPices.IsWhite && !selectedPices.isKIng && y == 7)
            {
                selectedPices.isKIng = true;
                selectedPices.PromotToKing();
            }
            if (!selectedPices.IsWhite && !selectedPices.isKIng && y == 0)
            {
                selectedPices.isKIng = true;
                selectedPices.PromotToKing();
            }
        }
    }
    public void EndGame()
    {
        OncurrentEvent.Invoke(PiceType.End);
        GameEvents.Instance.GameOver(winner);
    }
}
[Serializable]
public enum PiceType { Black, White, End }
[Serializable]
public class EventGameState : UnityEvent<PiceType> { }