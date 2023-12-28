using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameLevelInfo
{
    public double Time;
    public int[] AnimalIDs;
}

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] Button _levelBtnPrefab;
    [SerializeField] Transform _content;

    GameLevelDAL _dal;

    private void Awake()
    {
        if(_dal == null)
        {
            _dal = new GameLevelDAL();
            Init();
        }
    }

    bool Init()
    {
        GameLevelInfo[] gameLevelInfos = _dal.GetGameLevelInfos();

        for (int i = 0; i < gameLevelInfos.Length; ++i)
        {
            var levelBtn = Instantiate(_levelBtnPrefab, _content);
            levelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + (i + 1).ToString();
            levelBtn.onClick.AddListener(() => {
                SelectGameLevel(i);
            });
        }

        return true;
    }

    public void SelectGameLevel(int gameLevel)
    {
        GlobalData.GameLevel = gameLevel;
        AsynSceneLoader.Instance.LoadSceneByName("GameScene");
    }
}
