using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class BaseMovement : MonoBehaviour
    {
        [SerializeField] protected float speed;

        public float Speed
        {
            set
            {
                speed = value;
            }
            get { return speed; }
        }

        protected virtual void Move(Vector3 direction)
        {
            transform.position += direction.normalized * Time.deltaTime * speed;
        }
    }
}