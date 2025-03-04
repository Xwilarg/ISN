using ISN.SO;
using UnityEngine;

namespace ISN.Manager
{
    /// <summary>
    /// Manage a dungeon spawn
    /// </summary>
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance { private set; get; }

        [SerializeField]
        private TileInfo _floor, _wall;

        private void Awake()
        {
            Instance = this;
        }

        public void GoToDungeon()
        {
            GridManager.Instance.ClearMap();

            SpawnRoom();

            GridManager.Instance.SpawnParty(Vector2Int.zero);
        }

        private const int FloorSize = 2;
        private void SpawnRoom()
        {
            for (int y = -FloorSize; y <= FloorSize; y++)
            {
                GridManager.Instance.SpawnTile(new Vector2Int(y, FloorSize + 1), _wall);
                GridManager.Instance.SpawnTile(new Vector2Int(y, -FloorSize - 1), _wall);
                GridManager.Instance.SpawnTile(new Vector2Int(FloorSize + 1, y), _wall);
                GridManager.Instance.SpawnTile(new Vector2Int(-FloorSize - 1, y ), _wall);
                for (int x = -FloorSize; x <= FloorSize; x++)
                {
                    GridManager.Instance.SpawnTile(new Vector2Int(x, y), _floor);
                }
            }
            GridManager.Instance.SpawnTile(new Vector2Int(FloorSize + 1, FloorSize + 1), _wall);
            GridManager.Instance.SpawnTile(new Vector2Int(FloorSize + 1, -FloorSize - 1), _wall);
            GridManager.Instance.SpawnTile(new Vector2Int(-FloorSize - 1, -FloorSize - 1), _wall);
            GridManager.Instance.SpawnTile(new Vector2Int(-FloorSize - 1, FloorSize + 1), _wall);
        }
    }
}