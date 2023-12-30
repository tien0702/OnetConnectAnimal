using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TT;
using UnityEngine;

public class AnimalController : Singleton<AnimalController>
{
    Dictionary<int, Sprite> _animals = new Dictionary<int, Sprite>();
    public AnimalController()
    {
        var sprites = Resources.LoadAll<Sprite>("Animals/").ToList();

        foreach (var sprite in sprites)
        {
            _animals.Add(int.Parse(sprite.name), sprite);
        }
    }

    public Sprite GetAnimalByID(int id)
    {
        if (_animals.ContainsKey(id))
            return _animals[id];
        else return null;
    }
}
