using UnityEngine;
using SWEng.Data;
using CameraSystem;


namespace SWEng.GamePlay
{
    public class ItemController : MonoBehaviour, IItemController
    {
        [SerializeField] private string _itemCode;
        [SerializeField] private float _rotateSpeed = 20;
        [SerializeField] private Transform _itemParentTr;

        [SerializeField] private ItemInteractionBase _itemInteraction;

        public virtual void SetInitial(string itemCode)
        {
            _itemCode = itemCode;

            EntityManager.Instance.SetEntityAt(
                Coord2DManager.Instance.Convertor.
                    ConvertFromVector3(transform.position), this);

            ItemData data = ItemDataManager.Instance.GetItemData(itemCode);

            Instantiate(data.Prefab, _itemParentTr);

        }

        public virtual EntityInteractionBase GetInteraction()
        {
            CameraManager.Instance.CameraSetting(transform, "Character");

            _itemInteraction.SetData(_itemCode, Equip);

            return _itemInteraction;
        }

        public virtual void Equip()
        {
            EntityManager.Instance.
                SetEntityAt(MapUnitManager.Instance.Convertor.
                    ConvertFromVector3(transform.position), null);
                    
            Destroy(gameObject);
        }

        private void Update()
        {
            _itemParentTr.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
        }
    }
}