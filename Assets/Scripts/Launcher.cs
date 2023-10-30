using UnityEngine;

namespace Agave
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardDimensions;
    
        [Space]
        [SerializeField] private DropPool objectPool;
        [SerializeField] private DropBoard board;

        private ServiceLocator _locator;

        void Start()
        {
            var poolCapacity = (boardDimensions.x * boardDimensions.y) * 2;
        
            objectPool.InitializePool(poolCapacity);
            board.InitializeBoard(boardDimensions);
            
            InitializeServices();
        }

        void InitializeServices()
        {
            ServiceLocator.Register(objectPool);
            ServiceLocator.Register(board);
        }
    }
}