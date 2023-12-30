using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellItemInfo
{
    public int ID;
    public string Data;
}

public class CellItemController : MonoBehaviour
{
    [field: SerializeField] public CellItemInfo Info { private set; get; }

    public virtual bool Init(CellItemInfo info)
    {
        return true;
    }
}
