using System.Collections.Generic;
using UnityEngine;

namespace Agave
{
    public class DropPool : ObjectPool<Drop>, IService
    {
        [SerializeField] private List<DropTypeSO> dropsTypes;

        public Drop GetRandomDrop()
        {
            var drop = base.GetPooledObject();
        
            return RandomizeDropType(drop);
        }

        public void ReturnDropToPool(Drop dropToReturn)
        {
            dropToReturn.transform.localScale = Vector3.one;
        
            base.ReturnObjectToPool(dropToReturn);
        }

        public DropTypeSO GetNextType(Drop dropToChangeType)
        {
            var typeIndex = dropsTypes.IndexOf(dropToChangeType.Type);
            return dropsTypes[(typeIndex + 1) % dropsTypes.Count];
        }

        private Drop RandomizeDropType(Drop dropToRandomize)
        {
            var randomDropType = dropsTypes[Random.Range(0, dropsTypes.Count)];
            dropToRandomize.Type = randomDropType;

            return dropToRandomize;
        }
    }
}