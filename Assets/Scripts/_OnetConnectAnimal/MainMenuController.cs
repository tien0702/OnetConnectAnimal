using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Transform _mainMenuPanel;

    private void Start()
    {
        _mainMenuPanel.GetComponent<EffectController>().ShowEffects();

        User.Instance.SaveData();

        AudioManager.Instance.PlayMusic("bg", true);
    }
}
