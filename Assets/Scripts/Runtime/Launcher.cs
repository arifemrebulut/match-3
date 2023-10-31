using UnityEngine;

namespace Agave
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardDimensions;
        
        [Header("Services")]
        [SerializeField] private DropPool pool;
        [SerializeField] private DropBoard board;
        [SerializeField] private SwapController swapController;
        
        private ServiceLocator _locator;

        private void Awake()
        {
            RegisterServices();
            
            InitializePool();
            InitializeBoard();
        }

        private void InitializePool()
        {
            var poolCapacity = (boardDimensions.x * boardDimensions.y) * 2;
        
            pool.InitializePool(poolCapacity);
        }

        private void InitializeBoard()
        {
            board.InitializeBoard(boardDimensions);
        }

        private void RegisterServices()
        {
            ServiceLocator.Register(pool);
            ServiceLocator.Register(board);
            ServiceLocator.Register(swapController);
        }
    }
}