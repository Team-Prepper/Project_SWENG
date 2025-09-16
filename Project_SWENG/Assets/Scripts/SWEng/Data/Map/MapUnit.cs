using UnityEngine;
using System.Collections;

namespace SWEng.Data
{
    [SelectionBase]
    [RequireComponent(typeof(GlowHighlight))]
    public class MapUnit : MonoBehaviour
    {

        [SerializeField] private GlowHighlight highlight;
        [SerializeField] private GameObject cloud;

        [SerializeField] private int _originCost;
        public int Cost => _originCost;

        public TileDataScript.TileType tileType;

        public GridCoord2D Pos => Coord2DManager.Instance.
            Convertor.ConvertFromVector3(transform.position);

        [SerializeField] private GameObject _entity;
        
        public bool IsCloud { get; private set; } = true;

        private void Start()
        {
            highlight = GetComponent<GlowHighlight>();

            IsCloud = true;
            cloud.SetActive(true);

        }

        public void OnMouseToggle(bool isOn)
        {
            if (tileType == TileDataScript.TileType.obstacle) return;
            if (highlight) highlight.OnMouseToggleGlow(isOn);
        }

        public void SetSprite(Sprite spr, Vector3 localScale, Vector3 eulerAngle, bool isActive)
        {
            highlight.SetSprite(spr, localScale, eulerAngle, isActive);
        }

        public void SetCost(int cost)
        {
            _originCost = cost;
        }

        public void CloudActiveFalse()
        {
            foreach (GridCoord2D pos in
                MapUnitManager.Instance.GetNeighboursFor(Pos, 3))
            {
                MapUnit mapUnit = MapUnitManager.Instance.GetMapUnitAt(pos);

                if (mapUnit == null) continue;
                if (!mapUnit.IsCloud) continue;
                StartCoroutine(mapUnit.ActiveFalseCloud());

            }
        }

        private IEnumerator ActiveFalseCloud()
        {
            for (int i = 0; i < 10; i++)
            {
                cloud.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
            cloud.SetActive(false);
            IsCloud = false;
            yield break;
        }

        /*
        public void Interaction(ICharacterLogic actor)
        {
            if (tileType == TileDataScript.TileType.village)
            {
                //UIManager.Instance.OpenGUI<GUIShop>("Shop").SetCC(actor, this);
                return;
            }
            if (_cc != null)
            {
                //UIManager.Instance.OpenGUI<GUI_ShowCharacterInfor>("CharacterInfor").SetInfor(_cc, actor);
                return;
            }

            if (_ic != null)
            {
                _ic.Interaction(actor);
                return;
            }

            if (Random.Range(0, 1f) < 0.2f)
            {
                GameObject item = GameManager.Instance.Master.InstantiateItem(transform.position);
                item.GetComponent<IItemController>().SetInitial("Item_Heal");

                CameraManager.Instance.CameraSetting(item.transform, "Character");
                actor.ActionEnd(2f);
                return;
            }

            actor.ActionEnd(0);

        }
        */
    }
}