using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;

[System.Serializable]
public class GameLevelInfo
{
    public double Time;
    public int AnimalIDs;
    public int Hint;
    public int[] CountIDs;
}

public class GameLevelController : SingletonBehaviour<GameLevelController>
{
    GameLevelDAL _dal;

}
