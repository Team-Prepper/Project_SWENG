using UnityEngine;
using System.Collections.Generic;
using CameraSystem;
using BKTools.Gaming.GridMap2D;
using EasyH;
using EasyH.Unity;

namespace SWEng
{

    public class EntityManager : Singleton<EntityManager>
    {

        private IDictionary<GridCoord2D, IEntity> _entityDict
            = new Dictionary<GridCoord2D, IEntity>();

        public void Interaction(ICharacter actor, GridCoord2D targetPos)
        {
            MapUnit map = MapUnitManager.
                Instance.GetMapUnitAt(targetPos);

            if (map.tileType == TileDataScript.TileType.village)
            {
                ShopInteractionBase interaction =
                    ResourceManager.Instance.ResourceConnector.
                        Import<ShopInteractionBase>(
                            "Event/Event_OpenShop");

                interaction.SetData(map);
                interaction.Interaction(actor);
                return;
            }

            if (_entityDict.ContainsKey(targetPos) &&
                _entityDict[targetPos] != null)
            {
                _entityDict[targetPos].GetInteraction()?.Interaction(actor);
                return;
            }

            if (Random.Range(0, 1f) < 0.2f)
            {
                GameObject item = GameManager.Instance.Master.
                    InstantiateItem(MapUnitManager.Instance.
                        Convertor.ConvertToVector3(targetPos));

                item.GetComponent<IItemController>().
                    SetInitial("Item_Heal");

                CameraManager.Instance.CameraSetting(
                    item.transform, "Character");
                actor.ActionEnd(2f);

                return;
            }

            actor.ActionEnd(0);
            return;
        }

        public void SetEntityAt(GridCoord2D pos, IEntity entity)
        {
            if (!_entityDict.ContainsKey(pos))
            {
                _entityDict.Add(pos, entity);
                return;
            }
            _entityDict[pos] = entity;

        }

        public IEntity GetEntityAt(GridCoord2D pos)
        {
            if (!_entityDict.ContainsKey(pos)) return null;
            return _entityDict[pos];
        }

        public ISearchResult<GridCoord2D> GetPathGroup(GridCoord2D startPos, int point)
        {

            AStarSearch<GridCoord2D> s
                = new AStarSearch<GridCoord2D>();

            s.SetEdgeCostCalcer((from, to) =>
                GetEdgeCost(from, to, point));

            return s.Search(startPos, point,
                (currentNode) => false, GetNeighbours);

        }

        public IList<GridCoord2D> GetPathGroupTo(
            GridCoord2D startPos, GridCoord2D endPos, int point)
        {

            AStarSearch<GridCoord2D> s
                = new AStarSearch<GridCoord2D>();

            s.SetEdgeCostCalcer((from, to) =>
                GetEdgeCost(from, to, point));

            return s.Search(startPos, point,
                (currentNode) => currentNode.Equals(endPos),
                GetNeighbours).GetPathToState(endPos);
        }

        private ISet<GridCoord2D> GetNeighbours(
            GridCoord2D currentNode)
        {

            return MapUnitManager.Instance.GetNeighboursFor(currentNode);
        }

        private int GetEdgeCost(
            GridCoord2D from, GridCoord2D to, int maxValue)
        {
            if (GetEntityAt(to) != null) return maxValue + 1;

            if (MapUnitManager.Instance.
                GetMapUnitAt(to).tileType
                == TileDataScript.TileType.village)
                return maxValue + 1;

            return MapUnitManager.Instance.GetMapUnitAt(to).Cost;
        }

    }
}
