using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    #region AI

    private int aiCount;
    private int maxDepth = 100;

    private void Start()
    {
        BoardManager.Instance.OncurrentEvent.AddListener(HandleGamePlay);
    }

    private void HandleGamePlay(PiceType arg0)
    {
      if(arg0 == PiceType.White)
        {
            Debug.Log("watching");
        }
        else if( arg0 == PiceType.Black)
        {
            Debug.Log("its Me");
        }
    }


    #endregion
}
