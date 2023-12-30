using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        Dictionary<Button, int> buttonPropertiy = new Dictionary<Button, int>();

        for (int i = 0; i < gameLevelInfos.Length; ++i)
        {
            var levelBtn = Instantiate(_levelBtnPrefab, _content);
            levelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + (i + 1).ToString();
            buttonPropertiy.Add(levelBtn, i + 1);
            levelBtn.onClick.AddListener(() => {
                SelectGameLevel(buttonPropertiy[levelBtn]);
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
