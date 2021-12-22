using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataLevelManager : MonoBehaviour
{

    public LevelData lvl;

    public void AddDataValue(GameObject AddGameObject)
    {
        lvl.CurrentBlocks.Add(AddGameObject);
    }
    public void GetDataValue(List<GameObject> DataList)
    {


        
        if (lvl.CurrentBlocks.Count != 0)
        {
           
            for (int i = 0; i < lvl.CurrentBlocks.Count; i++)
            {
                DataList.Add(lvl.CurrentBlocks[i]);
            }
        }
          
    }
    
}
