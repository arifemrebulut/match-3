using UnityEngine;

namespace Agave
{
    public class BoardDataSO : MonoBehaviour
    {
        private Vector2Int _dimensions;

        public int[] spawnerColumns;

        public void SetDimensions(Vector2Int dimensions)
        {
            _dimensions = dimensions;

            spawnerColumns = new int[_dimensions.x];
        }

        public int[] GetSpawnerColumns()
        {
            return spawnerColumns;
        }
    }
}