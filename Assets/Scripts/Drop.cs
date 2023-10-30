using UnityEngine;

namespace Agave
{
    public class Drop : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

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

        public void SetPosition(int x, int y)
        {
            transform.position = new Vector2(x, y);
        }
    }
}