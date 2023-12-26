using System;
using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;

public abstract class BaseHandleBehaviour : MonoBehaviour, IHandle
{
    protected Action<IHandle> _onEndHandle;
    public Action<IHandle> OnEndHandle { set => _onEndHandle = value; }

    public abstract void Handle();

    protected virtual void EndHandle()
    {
        if(_onEndHandle != null)
        {
            _onEndHandle(this);
        }
    }
}
