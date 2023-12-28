using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellInfo
{
    public int ID;
}

public class CellController : MonoBehaviour
{
    public CellInfo Info;
    public Vector2Int CellPos;

    public virtual bool Init(CellInfo info)
    {
        return true;
    }
}
