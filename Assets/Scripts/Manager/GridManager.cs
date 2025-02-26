using ISN.Entity;
using ISN.Character;
using SIN.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

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
                _map.Clear();
            }
            _mapContainer = new GameObject("Map").transform;

            // Parse map from file
            var lines = map.text
                .Replace("\r", "")
                .Split('\n')
                .Select(x => x.ToCharArray().Reverse().ToArray())
                .Reverse()
                .ToArray();
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
                    var tile = new Tile()
                    {
                        TileInfo = info,
                        TileObject = obj
                    };

                    if (c == 'S') // Player spawn
                    {
                        tile.ContainedEntity = PlayerController.Instance;
                        tile.ContainedEntity.CurrentPosition = new Vector2Int(x, y);
                        PlayerController.Instance.transform.position = new Vector2(x, y) * TileSizeUnit;
                    }
                    tiles.Add(tile);
                }
                _map.Add(tiles);
            }
        }

        public bool TryMove(Vector2Int current, Vector2Int direction)
        {
            var tile = _map[current.y][current.x];
            Assert.IsNotNull(tile.ContainedEntity);

            var dest = current + direction;
            if (dest.y >= _map.Count || dest.x >= _map[dest.y].Count)
            {
                return false; // Out of bounds
            }

            var destTile = _map[dest.y][dest.x];
            if (!destTile.TileInfo.IsWalkable)
            {
                return false;
            }

            destTile.ContainedEntity = tile.ContainedEntity;
            destTile.ContainedEntity.CurrentPosition = dest;
            destTile.ContainedEntity.GameObject.transform.position = (Vector2)dest * TileSizeUnit;
            tile.ContainedEntity = null;
            return true;
        }
    }

    public class Tile
    {
        public TileInfo TileInfo { set; get; }
        public GameObject TileObject { set; get; }
        public IGridEntity ContainedEntity { set; get; }
    }
}
