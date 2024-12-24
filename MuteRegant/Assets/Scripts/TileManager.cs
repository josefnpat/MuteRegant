using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static TileScriptableObject;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab;

    [SerializeField]
    private GameObject _tileParent;

    [SerializeField]
    private TileScriptableObject _emptyTile;
    [SerializeField]
    private TileScriptableObject _destroyTile;

    [System.Serializable]
    public class SpawnTile
    {
        public TileScriptableObject tileData;
        public int init = 0;
    }

    [SerializeField]
    private List<SpawnTile> _spawnTiles;

    private Dictionary<string, Tile> _tiles = new Dictionary<string, Tile>();

    private ResourceManager _resourceManager;
    private DialogManager _dialogManager;

    [SerializeField]
    public TMP_Text _infoText;

    public bool debugShowBalance = false;

    public const int BOARD_CENTER_WIDTH = 8;
    public const int BOARD_CENTER_HEIGHT = 4;

    public void Start()
    {
        _resourceManager = FindAnyObjectByType<ResourceManager>();
        Debug.Assert(_resourceManager);
        _dialogManager = FindAnyObjectByType<DialogManager>();
        Debug.Assert(_dialogManager);
        Generate();
    }

    public void Generate()
    {

        foreach (KeyValuePair<string, Tile> tilePair in _tiles)
        {
            Destroy(tilePair.Value.gameObject);
        }
        _tiles = new Dictionary<string, Tile>();

        for (int x = -BOARD_CENTER_WIDTH; x <= BOARD_CENTER_WIDTH; x++)
        {
            for (int y = -BOARD_CENTER_HEIGHT; y <= BOARD_CENTER_HEIGHT; y++)
            {
                Tile tile = NewTile(x, y);
                _tiles.Add(tile.Hash, tile);
            }
        }

        foreach (SpawnTile spawnTile in _spawnTiles)
        {
            for (int i = 0; i < spawnTile.init; i++)
            {
                Tile tile = GetRandomEmptyTile();
                tile.SetTileData(spawnTile.tileData);
            }
        }
    }

    public void Update()
    {
        if (debugShowBalance)
        {
            debugShowBalance = false;
#if UNITY_EDITOR
            foreach (string tileDataGUID in AssetDatabase.FindAssets("t:TileScriptableObject"))
            {
                string tilePath = AssetDatabase.GUIDToAssetPath(tileDataGUID);
                TileScriptableObject tileData = (TileScriptableObject)AssetDatabase.LoadAssetAtPath(tilePath, typeof(TileScriptableObject));

                float avgTarget = 0;
                foreach (TileScriptableObject buildTile in tileData.build.tiles)
                {
                    avgTarget += buildTile.generateMoney / buildTile.generateMoneyT;
                }
                float val = (avgTarget / tileData.build.tiles.Count) / tileData.build.cost;
                Debug.Log($"{tileData.displayName}: {val}");
            }
#endif
        }
    }

    public Tile GetRandomEmptyTile()
    {
        List<Tile> emptyTiles = new List<Tile>();
        foreach (KeyValuePair<string, Tile> testTile in _tiles)
        {
            if (testTile.Value.TileData == _emptyTile)
            {
                emptyTiles.Add(testTile.Value);
            }
        }
        if (emptyTiles.Count > 0)
        {
            return emptyTiles[Random.Range(0, emptyTiles.Count)];
        }
        Debug.LogError("No more empty tiles.");
        return null;
    }

    public Tile NewTile(int x, int y)
    {
        GameObject newTileGameObject = Instantiate(_tilePrefab, _tileParent.transform);
        Tile newTile = newTileGameObject.GetComponent<Tile>();
        newTile.SetTile(x, y, _resourceManager);
        newTile.SetTileData(_emptyTile);
        newTile.OnClickBuild.AddListener(OnTileBuild);
        newTile.OnClickDestroy.AddListener(OnTileDestroy);
        newTile.OnInfoShow.AddListener(OnTileInfoShow);
        newTile.OnInfoHide.AddListener(OnTileInfoHide);
        return newTile;
    }

    private void OnTileBuild(Tile tile)
    {
        OnTileTarget(tile, tile.TileData.build);
    }

    private void OnTileDestroy(Tile tile)
    {
        OnTileTarget(tile, tile.TileData.destroy);
    }

    private void OnTileTarget(Tile tile, ActionTarget target)
    {
        if (target.tiles.Count > 0)
        {
            if (_resourceManager.CanAfford(target.cost))
            {
                TileScriptableObject tileToTarget = target.tiles[Random.Range(0, target.tiles.Count)];
                tile.SetTileData(tileToTarget);
                OnTileInfoShow(tile);
                _resourceManager.RemoveMoney(target.cost);
                foreach (TriggerScriptableObject trigger in tile.TileData.OnStartTriggers)
                {
                    trigger.Run(_dialogManager);
                }
            }
        }
    }

    private void OnTileInfoShow(Tile tile)
    {
        string displayName = SECAMColorPalette.FormatColorString(tile.TileData.color, tile.TileData.displayName);
        string generateMoney = string.Empty;
        if (tile.TileData.generateMoney != 0)
        {
            string sign = tile.TileData.generateMoney > 0 ? "+" : "";
            string val = ResourceManager.ToKiloFormat(tile.TileData.generateMoney);
            generateMoney = SECAMColorPalette.FormatColorString(SECAMColorPalette.Enum.yellow, $"({sign}{val}g/{tile.TileData.generateMoneyT}s)");
        }
        string build = string.Empty;
        if (tile.TileData.build.tiles.Count > 0)
        {
            string val = ResourceManager.ToKiloFormat(tile.TileData.build.cost);
            build = SECAMColorPalette.FormatColorString(SECAMColorPalette.Enum.green, $"({val}g)");
        }
        string destroy = string.Empty;
        if (tile.TileData.destroy.tiles.Count > 0)
        {
            string val = ResourceManager.ToKiloFormat(tile.TileData.destroy.cost);
            destroy = SECAMColorPalette.FormatColorString(SECAMColorPalette.Enum.red, $"({val}g)");
        }
        _infoText.text = $"{displayName} {generateMoney} {build} {destroy}";
    }

    private void OnTileInfoHide(Tile tile)
    {
        _infoText.text = string.Empty;
    }

    public void Show()
    {
        _tileParent.SetActive(true);
    }

    public void Hide()
    {
        _tileParent.SetActive(false);
    }

}
