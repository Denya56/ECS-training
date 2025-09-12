using ESC_training.Entities;
using System.Linq;
using static ESC_training.Config;

namespace ESC_training.Core
{
    internal class ComponentArray<T> : IComponentArray
    {
        private T[] _componentArray = new T[MAX_ENTITIES];
        private Dictionary<int, int> _entityToIndex;
        private Dictionary<int, int> _indexToEntity;
        private int _arrayIndex;

        public void InsertData(Entity entity, T component)
        {
            if (_entityToIndex.ContainsKey(entity.Id))
            {
                throw new InvalidOperationException(
                    $"Component of type {typeof(T)} already exists on Entity {entity.Id}.");
            }

            // get the next free spot in the packed array (the "end")
            int newIndex = _arrayIndex;
            // map entity → index
            _entityToIndex[entity.Id] = newIndex;
            // map index → entity 
            _indexToEntity[newIndex] = entity.Id;
            // store the component in the packed array
            _componentArray[newIndex] = component;
            ++_arrayIndex;
        }

        public void RemoveData(Entity entity)
        {
            // check of entity is in the map i.e. component exists for this entity
            if(!_entityToIndex.ContainsKey(entity.Id))
            {
                throw new KeyNotFoundException(
                    $"Attempting to remove non-existent component of type {typeof(T)} from Entity {entity.Id}.");
            }

            // copy component at the end of the array into deleted element's place to keep the array dense
            // get index of component to remove in packed array
            int removedEntityComponentIndex = _entityToIndex[entity.Id];
            // get index of last component in packed array
            int lastComponentIndex = _arrayIndex - 1;
            // move last component into the place of removed one in packed array
            _componentArray[removedEntityComponentIndex] = _componentArray[lastComponentIndex];

            // update maps; remap last entity's index to the index of the removed one; remap removed entity's index to the index of the last one
            // get the entity of the component currently at the end (just moved to the spot of the deleted one) of the packed array
            int lastComponentEntity = _indexToEntity[lastComponentIndex];
            // update the mapping of the entity to array index; since the last component is now in the removed (moved to the end) component's spot
            _entityToIndex[lastComponentEntity] = removedEntityComponentIndex;
            // update the mapping of the array index to entity for the removed (for now moved to the end) component's spot
            _indexToEntity[removedEntityComponentIndex] = lastComponentEntity;            

            // clear maps of removed entity
            _entityToIndex.Remove(entity.Id);
            // removed entity is the last one since we just moved it
            _indexToEntity.Remove(lastComponentIndex);

            // decrement to match the packed array new size
            --_arrayIndex;
        }
        public T GetData(Entity entity)
        {
            if (!_entityToIndex.ContainsKey(entity.Id))
            {
                throw new KeyNotFoundException(
                    $"Attempting to remove non-existent component of type {typeof(T)} from Entity {entity.Id}.");
            }
            return _componentArray[_entityToIndex[entity.Id]];
        }

        public void EntityDestroyed(Entity entity)
        {
            if (_entityToIndex.ContainsKey(entity.Id))
            {
                RemoveData(entity);
            }
        }
    }
}
