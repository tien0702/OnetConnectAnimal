using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelDAL
{
    GameLevelInfo[] infos;
    public GameLevelDAL()
    {
        int countLevels = 3;
        infos = new GameLevelInfo[countLevels];
        for(int i = 0; i < countLevels; i++)
        {
            infos[i] = new GameLevelInfo() { Time = 1000 * (i + 1), AnimalIDs = new int[] { 1, 2, 3 } };
        }
    }

    public GameLevelInfo GetGameLevelInfo(int level)
    {
        return infos[level];
    }

    public GameLevelInfo[] GetGameLevelInfos()
    {
        return infos;
    }
}
