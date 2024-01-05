using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TT;
using System;

public class CellController : TTMonoBehaviour
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
        if (ID == -1)
        {
            Hide(() =>
            {
                spriteRenderer.sprite = AnimalController.Instance.GetAnimalByID(id);
            });
        }
        else
        {
            spriteRenderer.sprite = AnimalController.Instance.GetAnimalByID(id);
        }
        return true;
    }

    private void OnEnable()
    {
        Vector3 value = transform.localScale;
        this.transform.localScale = value * 0.3f;
        this.ScalceTo(value);
    }

    void Hide(Action callback)
    {
        Vector3 value = this.transform.localScale * 0.3f;
        this.ScalceTo(value, callback);
    }
}
