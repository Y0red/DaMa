using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlight : MonoBehaviour
{
    public static BoardHighlight Instance { get; set; }
    public GameObject hilightprigab;
    private List<GameObject> hilights;

    public void Start()
    {
        Instance = this;
        hilights = new List<GameObject>();
    }

    private GameObject GetHilightObject()
    {
        GameObject go = hilights.Find(g => !g.activeSelf);

        if(go == null)
        {
            go = Instantiate(hilightprigab);
            hilights.Add(go);
        }

        return go;
    }

    public void HilightAllowedMoves(bool[,] moves)
    {
        
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    Debug.Log(i + "," + j);
                    GameObject go = GetHilightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i+0.5f,0.05f,j+0.5f);
                }
            }
        }
    }

    public void HideHilights()
    {
        foreach (GameObject go in hilights)
            go.SetActive(false);
    }
}
