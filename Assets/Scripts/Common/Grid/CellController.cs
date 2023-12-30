using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public int ID;
    public Vector2Int CellPos;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual bool Init(int id)
    {
        this.ID = id;
        spriteRenderer.sprite = AnimalController.Instance.GetAnimalByID(id);
        return true;
    }
}
