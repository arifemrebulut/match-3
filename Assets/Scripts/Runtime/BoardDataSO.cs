using UnityEngine;

namespace Agave
{
    [CreateAssetMenu(menuName = "Match3/BoardData", fileName = "NewBoardData")]
    public class BoardDataSO : ScriptableObject
    {
        public Vector2Int dimensions;

        [Space]
        public bool[] spawnerColumnIndexes;
    }
}