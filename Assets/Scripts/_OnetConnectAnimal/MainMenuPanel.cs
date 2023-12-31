using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] Button _resume, _newGame;
    [SerializeField] TextMeshProUGUI _bestScore;

    private void OnEnable()
    {
        _bestScore.text = "Best Score: " + User.Instance.Data.BestScore.ToString();
    }
}
