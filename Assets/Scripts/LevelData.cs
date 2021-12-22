using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Obstacle", menuName = "LevelGenerator")]
public class LevelData : ScriptableObject
{
    public new string name;
    public int BlockCount;
    public List<GameObject> CurrentBlocks = new List<GameObject>();

}

