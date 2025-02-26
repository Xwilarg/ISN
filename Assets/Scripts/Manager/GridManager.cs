using SIN.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ISN.Manager
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { private set; get; }

        [SerializeField]
        private TileInfo[] _tiles;
        private TileInfo _defaultTile;

        [SerializeField]
        private GameObject _tilePrefab;

        private const float TileSize = 128;
        private const float TileSizeUnit = 1.28f;

        private Transform _mapContainer;
        private readonly List<List<Tile>> _map = new();

        private void Awake()
        {
            Instance = this;
            _defaultTile = _tiles.First(x => x.Character == '.');
        }

        public void LoadMap(TextAsset map)
        {
            if (_mapContainer != null) // Clear old map
            {
                for (int i = 0; i < _mapContainer.childCount; i++)
                {
                    Destroy(_mapContainer.GetChild(i).gameObject);
                }
                Destroy(_mapContainer.gameObject);
                _mapContainer = new GameObject("Map").transform;
                _map.Clear();
            }

            // Parse map from file
            var lines = map.text.Replace("\r", "").Split('\n');
            for (int y = 0; y < lines.Length; y++)
            {
                List<Tile> tiles = new();
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var c = lines[y][x];
                    var info = c switch
                    {
                        'S' => _defaultTile,
                        _ => _tiles.First(x => x.Character == c)
                    };
                    var obj = Instantiate(_tilePrefab, _mapContainer);
                    obj.transform.position = new Vector2(x, y) * TileSizeUnit;
                    obj.GetComponent<SpriteRenderer>().sprite = info.Sprite;
                    tiles.Add(new Tile()
                    {
                        TileInfo = info, 
                        TileObject = obj
                    });
                }
                _map.Add(tiles);
            }
        }
    }

    public class Tile
    {
        public TileInfo TileInfo { set; get; }
        public GameObject TileObject { set; get; }
    }
}
