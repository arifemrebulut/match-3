using UnityEngine;

namespace Agave
{
    [CreateAssetMenu(menuName = "Match3/DropType", fileName = "NewDropType")]
    public class DropTypeSO : ScriptableObject
    {
        public Sprite image;
    }
}