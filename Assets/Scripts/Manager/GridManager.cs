using ISN.Entity;
using ISN.Character;
using ISN.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ISN.Manager
{
    /// <summary>
    /// Manage tiles on a grid
    /// </summary>
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

        public void ClearMap()
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
        }

        public void LoadMap(TextAsset map)
        {
            ClearMap();

            var mapInfo = map.text.Split('=').Select(x => x.Trim()).ToArray();

            var entityData = JsonConvert.DeserializeObject<Dictionary<string, MapInfo>>(mapInfo[1], new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

            // Parse map from file
            var lines = mapInfo[0]
                .Replace("\r", "")
                .Split('\n')
                .Reverse()
                .ToArray();

            Vector2Int? _spawnPos = null;
            for (int y = 0; y < lines.Length; y++)
            {
                List<Tile> tiles = new();
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var c = lines[y][x];
                    var info = c switch
                    {
                        'S' => _defaultTile,
                        _ when c >= '0' && c <= '9' => _defaultTile,
                        _ => _tiles.First(x => x.Character == c)
                    };

                    var tile = SpawnTile(new Vector2Int(x, y), info);

                    if (c == 'S') // Player spawn
                    {
                        Assert.IsTrue(_spawnPos == null, "Spawn pos was already set");
                        _spawnPos = new Vector2Int(x, y);
                    }
                    else if (c >= '0' && c <= '9') // Special events
                    {
                        var data = entityData[c.ToString()];
                        // Init resource from file
                        var targetResource = ResourceManager.Instance.GetMapResource<MapResourceInfo>(data.Key);
                        var speEntity = Instantiate(targetResource.Prefab, _mapContainer);
                        targetResource.InitSelf(speEntity);

                        // Place it on the grid
                        tile.ContainedEntity = speEntity.GetComponent<IGridEntity>();
                        tile.ContainedEntity.CurrentPosition = new Vector2Int(x, y);
                        speEntity.transform.position = new Vector2(x, y) * TileSizeUnit;
                    }
                    tiles.Add(tile);
                }
                _map.Add(tiles);
            }

            Assert.IsTrue(_spawnPos != null, "Spawn pos not set");
            SpawnParty(_spawnPos.Value);
        }

        public void SpawnParty(Vector2Int pos)
        {
            var tile = GetTile(pos);
            Assert.IsNotNull(tile, "Attempted to spawn party out of bounds");
            var player = PartyManager.Instance.Party.First();
            tile.ContainedEntity = player;
            tile.ContainedEntity.CurrentPosition = pos;
            player.GameObject.transform.position = (Vector2)pos * TileSizeUnit;

            var adjacents = new Vector2Int[] { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
            foreach (var charac in PartyManager.Instance.Party.Skip(1))
            {
                foreach (var a in adjacents)
                {
                    tile = GetTile(pos + a);
                    if (IsWalkable(pos + a, out var w))
                    {
                        tile.ContainedEntity = charac;
                        tile.ContainedEntity.CurrentPosition = pos + a;
                        player.GameObject.transform.position = (Vector2)(pos + a) * TileSizeUnit;
                        return;
                    }
                }
            }
        }

        public Tile SpawnTile(Vector2Int pos, TileInfo info)
        {
            var obj = Instantiate(_tilePrefab, _mapContainer);
            obj.transform.position = new Vector2(pos.x, pos.y) * TileSizeUnit;
            obj.GetComponent<SpriteRenderer>().sprite = info.Sprite;
            return new Tile()
            {
                TileInfo = info,
                TileObject = obj
            };
        }

        private Tile GetTile(Vector2Int position)
        {
            if (position.y >= _map.Count || position.x >= _map[position.y].Count)
            {
                return null; // Out of bounds
            }
            return _map[position.y][position.x];
        }

        public IGridEntity Get(Vector2Int position)
        {
            var tile = GetTile(position);
            return tile?.ContainedEntity;
        }

        /// <summary>
        /// Returns if the tile can be walked on
        /// </summary>
        public bool IsWalkable(Vector2Int pos, out IWalkable walkable)
        {
            var target = GetTile(pos);
            if (target == null || !target.TileInfo.IsWalkable)
            {
                walkable = null;
                return false;
            }

            var entity = target.ContainedEntity;
            if (entity != null)
            {
                walkable = entity as IWalkable;
                if (walkable == null)
                {
                    return false;
                }
            }
            else walkable = null;
            return true;
        }

        /// <summary>
        /// Try to move an entity from <paramref name="current"/> to <paramref name="direction"/>
        /// </summary>
        public bool TryMove(Vector2Int current, Vector2Int direction, out IWalkable walkable)
        {
            var tile = _map[current.y][current.x];
            Assert.IsNotNull(tile.ContainedEntity);

            var dest = current + direction;
            var destTile = GetTile(dest);
            if (!IsWalkable(dest, out walkable))
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

    public class MapInfo
    {
        public string Key { set; get; }
    }
}
