using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageSelector : MonoBehaviour
{
    public MazePassage[] availablePassages;
    public int[] passagesChance;

    public MazePassage PickRandomPassage()
    {
        for(var i = 0; i < availablePassages.Length; i++)
        {
            var chance = Random.Range(0, 100);
            if (chance < passagesChance[i])
            {
                return availablePassages[i];
            }
        }
        return availablePassages[availablePassages.Length - 1];

    }
}
