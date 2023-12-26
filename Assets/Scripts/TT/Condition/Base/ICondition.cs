using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public interface ICondition
    {
        bool IsSuitable { get; }
        Action<ICondition> OnSuitable { set; }
    }
}