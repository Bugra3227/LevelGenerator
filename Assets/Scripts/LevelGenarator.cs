#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Basit bir seviye editörü penceresi.
/// - Mevcut engel (obstacle) prefab'larını bir listede tutar
/// - Bu engellerden blok dizisi oluşturmak için indeks seçmeye izin verir
/// - Obstacles listesini DataLevelManager üzerinden kaydedip/okur
/// 
/// Menüden: LevelGenerator/Level Generator ile açılır.
/// </summary>
public class LevelGenerator : EditorWindow
{
    // Sahnedeki "Data" objesi üzerinden alınan seviye verisi yöneticisi
    private DataLevelManager _dataManager;

    /// <summary>Kaydedilen tüm engel prefab listesi.</summary>
    public List<GameObject> Obstacles = new List<GameObject>();

    // Şu an bu alanlar kullanılmıyor gibi; yine de silmeyip bırakıyorum.
    public GameObject levldta;
    public List<GameObject> WhichObstacles = new List<GameObject>();

    /// <summary>Inspector'da gösterilen ve düzenlenebilen geçici obstacle alanı listesi.</summary>
    private List<GameObject> _obstacleEditorList = new List<GameObject>();

    /// <summary>Yeni eklenecek obstacle prefab referansı.</summary>
    private GameObject _addingObstacle;

    public string LevelNumber;
    public GameObject AddBlocks;
    public int BlockSzie; // Kaç blok/engel seçilecek
    public int LevelNumbers;

    [Header("LevelLength(Blocks Count)")]
    [Range(0, 30)]
    public int LevelLenght;

    [Header("DistanceOfBlocks")]
    [Range(20, 70)]
    public int DistanceOfBlocks;

    /// <summary>
    /// Her blok için, Obstacles listesinden hangi indexteki prefab'ı kullanılacağını tutar.
    /// Örneğin BlockCount[0] = 2 ise, ilk blokta Obstacles[2] kullanılır.
    /// </summary>
    private List<int> _blockCount = new List<int>();

    [MenuItem("LevelGenerator/Level Generator")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<LevelGenerator>("Level Generator");
        window.minSize = new Vector2(500, 200);
    }

    private void OnEnable()
    {
        // Sahnedeki "Data" isimli objeyi bulup DataLevelManager bileşenini alıyoruz.
        var dataObj = GameObject.Find("Data");
        if (dataObj != null)
        {
            _dataManager = dataObj.GetComponent<DataLevelManager>();
        }

        if (_dataManager != null)
        {
            // DataLevelManager'dan mevcut obstacle listesini doldur.
            _dataManager.GetDataValue(Obstacles);
        }

        // Editor listesi boyutunu Obstacles listesiyle eşitle
        SyncEditorObstacleList();
    }

    /// <summary>
    /// _obstacleEditorList listesinin, Obstacles listesi ile aynı boyutta olmasını sağlar.
    /// </summary>
    private void SyncEditorObstacleList()
    {
        while (Obstacles.Count > _obstacleEditorList.Count)
        {
            _obstacleEditorList.Add(null);
        }

        while (Obstacles.Count < _obstacleEditorList.Count)
        {
            _obstacleEditorList.RemoveAt(_obstacleEditorList.Count - 1);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("LevelGenerator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();

        #region Level Numarası

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level Number", EditorStyles.label, GUILayout.MaxWidth(125));
        LevelNumbers = EditorGUILayout.IntField("", LevelNumbers, GUILayout.MaxWidth(125));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        #endregion

        #region Obstacle Ekleme

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Add Obstacle", EditorStyles.label);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Yeni eklenecek obstacle prefab alanı
        EditorGUILayout.BeginHorizontal();
        _addingObstacle = EditorGUILayout.ObjectField(
            "Obstacles",
            _addingObstacle,
            typeof(GameObject),
            true
        ) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Listelerin boyutlarını eşitle
        SyncEditorObstacleList();

        // Mevcut Obstacles listesini editörde göster
        for (int i = 0; i < Obstacles.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _obstacleEditorList[i] = EditorGUILayout.ObjectField(
                "Obstacle " + i,
                Obstacles[i],
                typeof(GameObject),
                true
            ) as GameObject;
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Obstacle"))
        {
            if (_addingObstacle != null)
            {
                Obstacles.Add(_addingObstacle);

                // Veriyi DataLevelManager'a kaydet
                if (_dataManager != null)
                {
                    _dataManager.AddDataValue(_addingObstacle);
                }
            }

            _addingObstacle = null;
            SyncEditorObstacleList();
        }

        EditorGUILayout.EndHorizontal();

        #endregion

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        #region Block / Level Dizisi

        // Kaç blok seçilecek?
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("How Many Block", EditorStyles.label, GUILayout.MaxWidth(125));
        BlockSzie = EditorGUILayout.IntField("", BlockSzie, GUILayout.MaxWidth(125));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // BlockCount listesinin boyutunu BlockSzie ile eşitle
        while (_blockCount.Count < BlockSzie)
        {
            _blockCount.Add(0);
        }
        while (_blockCount.Count > BlockSzie)
        {
            _blockCount.RemoveAt(_blockCount.Count - 1);
        }

        // Her blok için hangi obstacle index'inin seçileceğini göster
        for (int i = 0; i < BlockSzie; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i + ". Obstacle", EditorStyles.label, GUILayout.MaxWidth(125));
            _blockCount[i] = EditorGUILayout.IntField("", _blockCount[i], GUILayout.MaxWidth(125));
            EditorGUILayout.EndHorizontal();
        }

        // Seçilen index'e göre obstacle isimlerini göster
        for (int i = 0; i < BlockSzie; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (_blockCount[i] < Obstacles.Count && _blockCount[i] >= 0)
            {
                EditorGUILayout.LabelField(Obstacles[_blockCount[i]].name, EditorStyles.label,
                    GUILayout.MaxWidth(200));
            }
            else
            {
                EditorGUILayout.LabelField("Doesn't Exist (Index: " + _blockCount[i] + ")", EditorStyles.helpBox);
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        EditorGUILayout.Space();

        #region Engel Arası Mesafe

        EditorGUILayout.LabelField("Distance Between Obstacles", EditorStyles.label);
        DistanceOfBlocks = EditorGUILayout.IntSlider("", DistanceOfBlocks, 0, 150);

        #endregion

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// İleride seviye validasyonu yapılacaksa buradan kontrol edilebilir.
    /// Şimdilik her zaman true dönüyor.
    /// </summary>
    private bool IsValidContent()
    {
        return true;
    }
}
#endif
