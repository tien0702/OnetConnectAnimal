using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;

public class GameLevelDAL : SingleDAL
{
    public GameLevelDAL()
    {
        this.LoadData("game-levels");
    }

    public GameLevelInfo GetGameLevelInfo(int level)
    {
        return this.GetData<GameLevelInfo>(level-1);
    }

    public GameLevelInfo[] GetGameLevelInfos()
    {
        return GetDatas<GameLevelInfo>();
    }
}
