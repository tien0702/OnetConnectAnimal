using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _countTxt;
    GridAnimalController _gridAnimal;

    public int CountHint { private set; get; }

    private void Awake()
    {
        _countTxt = GetComponentInChildren<TextMeshProUGUI>();
        _gridAnimal  = GameObject.FindAnyObjectByType<GridAnimalController>();

        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => { UseHint(); });
    }

    public bool Init(int countHint)
    {
        this.CountHint = countHint;
        _countTxt.text = CountHint.ToString();
        return true;
    }

    void UseHint()
    {
        if(CountHint <= 0)
        {
            return;
        }
        CountHint -= 1;
        _countTxt.text = CountHint.ToString();
    }
}
