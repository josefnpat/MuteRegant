using System;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _art;
    private Vector3 _artOriginalPosition;

    public int _positionX;
    public int PositionX { get { return _positionX; } }
    public int _positionY;
    private ResourceManager _resourceManager;

    public int PositionY { get { return _positionY; } }

    public string Position { get { return Hash; } }
    public string Hash { get { return $"{_positionX},{_positionY}"; } }

    private TileScriptableObject _tileData;
    public TileScriptableObject TileData { get { return _tileData; } }

    public class TileEvent : UnityEvent<Tile> { };
    public TileEvent OnClickBuild = new TileEvent();
    public TileEvent OnClickDestroy = new TileEvent();
    public TileEvent OnInfoShow = new TileEvent();
    public TileEvent OnInfoHide = new TileEvent();

    private float _generateMoneyDt = 0;

    private static float _bounceT = 0.5f;
    private float _bounceDt = _bounceT;
    private float _bounceDistance = 0.1f;
    

    public void Start()
    {
        _artOriginalPosition = _art.transform.localPosition;
    }

    public void Update()
    {
        if (_tileData.generateMoney > 0)
        {
            _generateMoneyDt += Time.deltaTime;
            if (_generateMoneyDt >= _tileData.generateMoneyT)
            {
                _generateMoneyDt -= _tileData.generateMoneyT;
                if (_tileData.generateMoney != 0)
                {
                    if (_tileData.generateMoney < 0)
                    {
                        _resourceManager.RemoveMoney((long)(-_tileData.generateMoney));
                    }
                    else
                    {
                        _resourceManager.AddMoney((long)_tileData.generateMoney);
                    }
                }
                _bounceDt = 0;
            }
        }
        _bounceDt = Mathf.Min(_bounceT, _bounceDt + Time.deltaTime);
        float offset = Mathf.Sin(_bounceDt / _bounceT * Mathf.PI * 2) * _bounceDistance;
        _art.transform.localPosition = new Vector3(
            _artOriginalPosition.x,
            _artOriginalPosition.y + offset,
            _artOriginalPosition.z
        );
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickBuild.Invoke(this); 
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnClickDestroy.Invoke(this);
        }
        // todo: add something else for middle click?
        /*
        if (Input.GetMouseButtonDown(2))
        {
            OnClickSilly.Invoke(this);
        }
        */
    }
    private void OnMouseEnter()
    {
        OnInfoShow.Invoke(this);
    }

    private void OnMouseExit()
    {
        OnInfoHide.Invoke(this);
    }

    public void SetTile(int positionX, int positionY, ResourceManager resourceManager)
    {
        _positionX = positionX;
        _positionY = positionY;
        _resourceManager = resourceManager;

        transform.position = new Vector3(positionX, positionY, 0);
    }

    public void SetTileData(TileScriptableObject tile)
    {
        _tileData = tile;

        _art.sprite = tile.art;
        _art.color = SECAMColorPalette.GetColor(tile.color);
    }

}
