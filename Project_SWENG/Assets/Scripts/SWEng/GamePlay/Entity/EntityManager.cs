using EasyH;
using SWEng.Data;
using UnityEngine;
using System.Collections.Generic;
using CameraSystem;

namespace SWEng.GamePlay
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
                    AssetOpener.Import<ShopInteractionBase>(
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

        public IPathGroup GetPathGroup(GridCoord2D startPos, int point)
        {
            IDictionary<GridCoord2D, GridCoord2D?> visitedNodes =
                new Dictionary<GridCoord2D, GridCoord2D?>();
            IDictionary<GridCoord2D, int> costSoFar =
                new Dictionary<GridCoord2D, int>();

            Queue<GridCoord2D> nodesToVisitQueue = new Queue<GridCoord2D>();

            nodesToVisitQueue.Enqueue(startPos);
            costSoFar.Add(startPos, 0);
            visitedNodes.Add(startPos, null);

            while (nodesToVisitQueue.Count > 0)
            {
                GridCoord2D currentNode = nodesToVisitQueue.Dequeue();

                foreach (GridCoord2D pos in
                    MapUnitManager.Instance.GetNeighboursFor(currentNode))
                {
                    if (GetEntityAt(pos) != null)
                        continue;
                        
                    if (MapUnitManager.Instance.GetMapUnitAt(pos).tileType
                        == TileDataScript.TileType.village) continue;

                    int nodeCost = MapUnitManager.Instance.
                        GetMapUnitAt(pos).Cost;
                    int currentCost = costSoFar[currentNode];
                    int newCost = currentCost + nodeCost;

                    if (newCost > point) continue;

                    if (!visitedNodes.ContainsKey(pos))
                    {
                        visitedNodes[pos] = currentNode;
                        costSoFar[pos] = newCost;
                        nodesToVisitQueue.Enqueue(pos);

                    }
                    else if (costSoFar[pos] > newCost)
                    {
                        costSoFar[pos] = newCost;
                        visitedNodes[pos] = currentNode;

                    }
                }
            }

            return new BFSPathGroup(visitedNodes);
        }

        public IPathGroup GetPathGroupTo
            (GridCoord2D startPos, GridCoord2D endPos, int point)
        {
            IDictionary<GridCoord2D, GridCoord2D?> visitedNodes
                = new Dictionary<GridCoord2D, GridCoord2D?>();
            IDictionary<GridCoord2D, int> costSoFar
                = new Dictionary<GridCoord2D, int>();

            Queue<GridCoord2D> nodesToVisitQueue = new Queue<GridCoord2D>();

            nodesToVisitQueue.Enqueue(startPos);
            costSoFar.Add(startPos, 0);
            visitedNodes.Add(startPos, null);

            while (nodesToVisitQueue.Count > 0)
            {
                GridCoord2D currentNode = nodesToVisitQueue.Dequeue();
                foreach (GridCoord2D pos in
                    MapUnitManager.Instance.GetNeighboursFor(currentNode))
                {
                    if (pos.Equals(endPos))
                    {
                        visitedNodes[pos] = currentNode;
                        return new BFSPathGroup(visitedNodes);
                    }

                    if (GetEntityAt(pos) != null)
                        continue;
                        
                    if (MapUnitManager.Instance.GetMapUnitAt(pos).tileType
                        == TileDataScript.TileType.village) continue;

                    int newCost = costSoFar[currentNode] + MapUnitManager.Instance.
                        GetMapUnitAt(pos).Cost;

                    if (newCost > point) continue;

                    if (!visitedNodes.ContainsKey(pos))
                    {
                        visitedNodes[pos] = currentNode;
                        costSoFar[pos] = newCost;
                        nodesToVisitQueue.Enqueue(pos);

                    }
                    else if (costSoFar[pos] > newCost)
                    {
                        costSoFar[pos] = newCost;
                        visitedNodes[pos] = currentNode;

                    }
                }
            }
            return new BFSPathGroup(new Dictionary<GridCoord2D, GridCoord2D?>());
        }

    }
}
