using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class TypeMovementController : BaseMovement
    {
        public Transform model;
        protected ITypeMovement[] typeMovements;

        protected virtual void Awake()
        {
            typeMovements = GetComponents<ITypeMovement>();
        }

        protected virtual void Update()
        {
            Vector3 direction = Vector3.zero;
            foreach (ITypeMovement type in typeMovements)
            {
                direction += type.GetDirectionMove();
            }
            if (model) model.up = direction;
            base.Move(direction);
        }
    }
}
