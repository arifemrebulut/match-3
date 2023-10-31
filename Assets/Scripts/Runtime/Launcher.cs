using UnityEngine;

namespace Agave
{
    public class Launcher : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private DropPool pool;
        [SerializeField] private DropBoard board;
        [SerializeField] private SwapController swapController;
        
        private ServiceLocator _locator;

        private void Awake()
        {
            RegisterServices();
            
            board.InitializeBoard();
        }

        private void RegisterServices()
        {
            ServiceLocator.Register(pool);
            ServiceLocator.Register(board);
            ServiceLocator.Register(swapController);
        }
    }
}