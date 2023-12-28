using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Transform _mainMenuPanel;

    private void Start()
    {
        _mainMenuPanel.GetComponent<EffectController>().ShowEffects();

        User.Instance.SaveData();
    }
}
