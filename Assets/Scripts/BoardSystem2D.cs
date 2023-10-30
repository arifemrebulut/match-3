using System.Text;
using UnityEngine;

namespace Agave
{
    public class BoardSystem2D<T> : MonoBehaviour
    {
        private T[,] _data;

        public Vector2Int Dimensions { get; private set; }
        public bool Initialized { get; private set; }

        protected void CreateBoard(Vector2Int dimensions)
        {
            if (dimensions.x < 1 || dimensions.y < 1)
            {
                Debug.LogError("Board dimensions should be positive numbers.");
            }

            Dimensions = dimensions;

            _data = new T[dimensions.x, dimensions.y];

            Initialized = true;
        }

        public void Clear()
        {
            _data = new T[Dimensions.x, Dimensions.y];
        }

        public bool CheckBounds(int x, int y)
        {
            if (!Initialized)
            {
                Debug.LogError("Board has not been initialized.");
                return false;
            }

            return (x >= 0 && x < Dimensions.x) && (y >= 0 && y < Dimensions.y);
        }

        public bool IsTileEmpty(int x, int y)
        {
            if (!CheckBounds(x, y))
            {
                Debug.LogError($"({x}, {y}) this position is not in bounds of the board.");
                return false;
            }

            return Equals(_data[x, y], default);
        }

        public bool PutItemAt(T item, int x, int y, bool allowOverwrite = false)
        {
            if (!CheckBounds(x, y))
            {
                Debug.LogError($"({x}, {y}) this position is not in bounds of the board.");
                return false;
            }

            if (!allowOverwrite && !IsTileEmpty(x, y))
            {
                return false;
            }

            _data[x, y] = item;
            return true;
        }

        public T GetItemAt(int x, int y)
        {
            if (!CheckBounds(x, y))
            {
                Debug.LogError($"({x}, {y}) this position is not in bounds of the board.");
                return default;
            }

            return _data[x, y];
        }

        public T RemoveItemAt(int x, int y)
        {
            if (!CheckBounds(x, y))
            {
                Debug.LogError($"({x}, {y}) this position is not in bounds of the board.");
                return default;
            }

            var temp = _data[x, y];
            _data[x, y] = default;
            return temp;
        }

        public bool SwapItemsAt(int x1, int y1, int x2, int y2)
        {
            if (!CheckBounds(x1, y1))
            {
                Debug.LogError($"({x1}, {y1}) this position is not in bounds of the board.");
                return false;
            }

            if (!CheckBounds(x2, y2))
            {
                Debug.LogError($"({x2}, {y2}) this position is not in bounds of the board.");
                return false;
            }

            (_data[x1, y1], _data[x2, y2]) = (_data[x2, y2], _data[x1, y1]);

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int y = Dimensions.y - 1; y >= 0; y--)
            {
                builder.Append("[ ");

                for (int x = 0; x < Dimensions.x; x++)
                {
                    builder.Append(IsTileEmpty(x, y) ? "x" : "d");

                    if (x != Dimensions.x - 1)
                    {
                        builder.Append(", ");
                    }
                }

                builder.Append(" ]\n");
            }

            return builder.ToString();
        }
    }
}