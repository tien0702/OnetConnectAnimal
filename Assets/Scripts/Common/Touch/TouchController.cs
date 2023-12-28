using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField] protected LayerMask layerTarget;
        GameObject hitGameObject = null;
        IOnTouchBegan[] onTouchBegans;
        IOnTouchMoved[] onTouchMoved;
        IOnTouchEnded[] onTouchEnded;

        GameObject GetHitGameObject()
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(touchPos.x, touchPos.y), Vector2.zero, Mathf.Infinity, layerTarget.value);
            return (hit.transform != null && hit.transform.Equals(this.transform)) ? hit.transform.gameObject : null;
        }

        protected virtual void Update()
        {
            if (Input.touchCount == 0) return;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                hitGameObject = GetHitGameObject();
                if (hitGameObject != null)
                {
                    onTouchBegans = hitGameObject.GetComponents<IOnTouchBegan>();
                    if (onTouchBegans != null)
                    {
                        foreach (var onTouch in onTouchBegans)
                        {
                            onTouch.OnTouchBegan();
                        }
                        onTouchBegans = null;
                        onTouchMoved = hitGameObject.GetComponents<IOnTouchMoved>();
                        onTouchEnded = hitGameObject.GetComponents<IOnTouchEnded>();
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && hitGameObject)
            {
                foreach (var movedTouch in onTouchMoved)
                {
                    movedTouch.OnTouchMoved();
                }
            }
            else if ((Input.GetTouch(0).phase == TouchPhase.Ended) && (hitGameObject != null))
            {
                foreach (var endTouch in onTouchEnded)
                {
                    endTouch.OnTouchEnded();
                }

                hitGameObject = null;
                onTouchBegans = null;
                onTouchMoved = null;
                onTouchEnded = null;
            }
        }
    }
}
