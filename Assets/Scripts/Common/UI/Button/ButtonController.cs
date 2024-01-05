using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfig
{
    public string ClickName;
}

public class ButtonController : MonoBehaviour
{
    public static readonly string DefaultConfigPath = "Data/UI/Button/button-config";
    protected static ButtonConfig _config;
    public static ButtonConfig Config
    {
        get
        {
            if(_config == null)
                _config = LoadConfig();
            return _config;
        }
    }
    public static ButtonConfig LoadConfig()
    {
        TextAsset configData = Resources.Load<TextAsset>(DefaultConfigPath);
        if(configData == null ) return null;

        string data = configData.text;
        return JsonUtility.FromJson<ButtonConfig>(data);
    }


    protected Button _button;
    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySound);
    }

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(PlaySound);
    }

    protected virtual void OnDisable()
    {
        if (_button != null) _button.onClick.RemoveListener(PlaySound);
    }

    protected virtual void OnDestroy()
    {
        if(_button != null) _button.onClick.RemoveListener(PlaySound);
    }

    protected virtual void PlaySound()
    {
        AudioManager.Instance.PlaySFX(ButtonController.Config.ClickName);
    }
}
