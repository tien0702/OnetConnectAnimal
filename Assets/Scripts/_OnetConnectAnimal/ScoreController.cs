using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int Score { private set; get; } = 0;
    TextMeshProUGUI _scoreTxt;

    GridAnimalController _gridAnimal;

    private void Awake()
    {
        _scoreTxt = GetComponent<TextMeshProUGUI>();
        _scoreTxt.text = "Score: " + Score.ToString();
    }

    void Start()
    {
        _gridAnimal = GameObject.FindObjectOfType<GridAnimalController>();
        _gridAnimal.Events.RegisterEvent(GridAnimalController.GridAnimalEvent.OnRemove, OnRemoveAnimal);
    }

    void OnRemoveAnimal(int id)
    {
        Score += 5;
        _scoreTxt.text = "Score: " + Score.ToString();
    }

    private void OnDestroy()
    {
        if (_gridAnimal != null)
        {
            _gridAnimal.Events.UnRegisterEvent(GridAnimalController.GridAnimalEvent.OnRemove, OnRemoveAnimal);
        }
    }
}
