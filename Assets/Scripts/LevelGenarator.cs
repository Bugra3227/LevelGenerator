using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class LevelGenerator : EditorWindow
{
    DataLevelManager DataManage;
    public List<GameObject> Obstacles = new List<GameObject>();
    public GameObject levldta;
    public List<GameObject> WhichObstacles = new List<GameObject>();
    List<GameObject> list = new List<GameObject>();
    GameObject AddingObstacles;
    public string LevelNumber;
    public GameObject AddBlocks;
    public int BlockSzie;
    public int LevelNumbers;
    [Header("LevelLength(Blocks Count)")]
    [Range(0, 30)]
    public int LevelLenght;
    [Header("DistanceOfBlocks")]
    [Range(20, 70)]
    public int DistanceOfBlocks;
    
    List<int> BlockCount = new List<int>();

    [MenuItem("LevelGenerator/Level Generator")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<LevelGenerator>("Level Generator");
        window.minSize = new Vector2(500, 200);
    }
    void GetObstacle()
    {
        
    }
    private void OnEnable()
    {
        DataManage = GameObject.Find("Data").GetComponent<DataLevelManager>();
        DataManage.GetDataValue(Obstacles);
        
        
        
        for (int i = 0; i < Obstacles.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            list[i] = EditorGUILayout.ObjectField("Obstacle" + " " + +i, Obstacles[i], typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();
        }
        
    }
    private void OnGUI()
    {

        EditorGUILayout.LabelField("LevelGenerator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();
        //Level Label And Textfield
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level Number", EditorStyles.label, GUILayout.MaxWidth(125));
        LevelNumbers=EditorGUILayout.IntField("", LevelNumbers, GUILayout.MaxWidth(125));
        EditorGUILayout.EndHorizontal();
        //End Label And TextField
        EditorGUILayout.Space();

       

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Add Obstacle", EditorStyles.label);
        EditorGUILayout.EndHorizontal();

        //



        //


        //Level Length;
        /*
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level Length", EditorStyles.label);
        EditorGUILayout.EndHorizontal();
        */
        //
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        AddingObstacles = EditorGUILayout.ObjectField("Obstacles", AddingObstacles, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();
        //

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        while (Obstacles.Count > list.Count)
        {
            list.Add(null);
        }
        while (Obstacles.Count < list.Count)
        {
            list.RemoveAt(list.Count - 1);
        }
        

        for (int i = 0; i < Obstacles.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            list[i] = EditorGUILayout.ObjectField("Obstacle" + " " + +i, Obstacles[i], typeof(GameObject), true) as GameObject;
           
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Obstacle"))
        {
            if(AddingObstacles!=null)
            {
                Obstacles.Add(AddingObstacles);
                DataManage.AddDataValue(AddingObstacles);
                
            }
            AddingObstacles = null;
        }

        EditorGUILayout.EndHorizontal();


        /*
     EditorGUILayout.BeginHorizontal();
     objectScale = EditorGUILayout.IntSlider("", objectScale, 0, 30);
     EditorGUILayout.EndHorizontal();
        */


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("How Many Block", EditorStyles.label, GUILayout.MaxWidth(125));
        BlockSzie = EditorGUILayout.IntField("", BlockSzie, GUILayout.MaxWidth(125));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        while (BlockCount.Count < BlockSzie)
        {
            BlockCount.Add(0);
        }
        while (BlockCount.Count > BlockSzie)
        {
            BlockCount.RemoveAt(BlockCount.Count - 1);
        }
        for (int i = 0; i < BlockSzie; i++)
        {
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i + "." + "Obstacle", EditorStyles.label, GUILayout.MaxWidth(125));
            BlockCount[i] = EditorGUILayout.IntField("", BlockCount[i] , GUILayout.MaxWidth(125));
            EditorGUILayout.EndHorizontal();
        }
        for (int i = 0; i < BlockSzie; i++)
        {

            EditorGUILayout.BeginHorizontal();
            if (BlockCount[i] < Obstacles.Count)
                EditorGUILayout.LabelField(Obstacles[BlockCount[i]].name, EditorStyles.label, GUILayout.MaxWidth(125));
            else
                GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height / 2), "Doesnt Exist"+ Obstacles[BlockCount[i]]);
            EditorGUILayout.EndHorizontal();
        }
        




        //End Level Length
        // distance between obstacles;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Distance Between Obstacles", EditorStyles.label);

        DistanceOfBlocks = EditorGUILayout.IntSlider("", DistanceOfBlocks, 0, 150);

        //End distance between obstacles
        EditorGUILayout.EndVertical();
    }
    private bool IsValidContent()
    {
        return true;
    }
}

