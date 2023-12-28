using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TT;


public class ConnectController : MonoBehaviour, IOnTouchBegan, IOnTouchEnded
{
    static CellController cellSelected;

    CellController cell;

    private void Awake()
    {
        cell = GetComponent<CellController>();
    }
    public void OnTouchBegan()
    {
        Debug.Log("began");
    }

    public void OnTouchEnded()
    {

    }
}
