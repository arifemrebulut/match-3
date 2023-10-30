using UnityEngine;

namespace Agave
{
    public class Drop : MonoBehaviour
    {
        public bool IsMoving { get; set; }
        
        private SpriteRenderer _spriteRenderer;
        private SwapController _swapController;
        
        private DropTypeSO _type;
        
        public DropTypeSO Type
        {
            get => _type;
            set
            {
                _type = value;

                if (_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                }
                
                _spriteRenderer.sprite = _type.image;
            }
        }

        private void Start()
        {
            _swapController = ServiceLocator.Get<SwapController>();
        }

        public void SetPosition(int x, int y)
        {
            transform.position = new Vector2(x, y);
        }
        
        private void OnMouseDown()
        {
            _swapController.SelectFirst(this);
        }

        private void OnMouseUp()
        {
            _swapController.SelectFirst(null);
        }

        private void OnMouseEnter()
        {
            _swapController.SelectSecond(this);
        }
    }
}