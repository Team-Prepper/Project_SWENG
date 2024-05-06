using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;

public interface IEnemyPlayer {
    public void AddSelector(BasicEnemyActionSelector selector);
    public void RemoveSelector(BasicEnemyActionSelector selector);
    public void ActionAdd(BasicEnemyActionSelector selector, IList<Character.Action> actions);
    public void ActionSelect();
}

public class EnemyPlayer : MonoBehaviour, IEnemyPlayer {

    IDictionary<BasicEnemyActionSelector, IList<Character.Action>> _data;
    BasicEnemyActionSelector _nowSelector;

    public EnemyPlayer() {
        _data = new Dictionary<BasicEnemyActionSelector, IList<Character.Action>>();
    }

    public void AddSelector(BasicEnemyActionSelector selector) {
        _data.Add(selector, null);
    }
    public void RemoveSelector(BasicEnemyActionSelector selector)
    {
        _data.Remove(selector);

    }

    public void ActionAdd(BasicEnemyActionSelector selector, IList<Character.Action> actions) {

        _data[selector] = actions;

        if (_nowSelector == null || _nowSelector.Equals(selector))
        {
            _nowSelector = selector;
            ActionSelect();
        }

    }
    public void ActionSelect() {

        IList<Character.Action> actions = _data[_nowSelector];

        while (actions.Count == 0) {
            _nowSelector.DoAction(Character.Action.Dice);
            _data[_nowSelector] = actions;

            _nowSelector = _SelectNewSelector();

            if (_nowSelector == null)
                return;

            actions = _data[_nowSelector];

        }

        _nowSelector.CamSetting();
        StartCoroutine(_ActionSelect(actions));
    }

    IEnumerator _ActionSelect(IList<Character.Action> actions) {

        yield return new WaitForSeconds(1f);

        if (actions.Contains(Character.Action.Attack))
        {
            _nowSelector.DoAction(Character.Action.Attack);
        }
        else if (actions.Contains(Character.Action.Move))
        {
            _nowSelector.DoAction(Character.Action.Move);
        }
        else
        {
            _nowSelector.DoAction(Character.Action.Dice);
        }

        _data[_nowSelector] = actions;
    }

    BasicEnemyActionSelector _SelectNewSelector()
    {
        foreach (KeyValuePair<BasicEnemyActionSelector, IList<Character.Action>> item in _data)
        {
            if (item.Value.Count == 0) continue;
            return item.Key;
        }

        return null;

    }

}