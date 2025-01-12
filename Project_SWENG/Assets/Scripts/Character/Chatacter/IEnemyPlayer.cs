using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyPlayer {
    public void AddSelector(BasicEnemyActionSelector selector);
    public void RemoveSelector(BasicEnemyActionSelector selector);
    public void ActionAdd(BasicEnemyActionSelector selector, IList<CharacterStatus.Action> actions);
}

public class EnemyPlayer : MonoBehaviour, IEnemyPlayer {

    IDictionary<BasicEnemyActionSelector, IList<CharacterStatus.Action>> _data;
    BasicEnemyActionSelector _nowSelector;

    public EnemyPlayer() {
        _data = new Dictionary<BasicEnemyActionSelector, IList<CharacterStatus.Action>>();
    }

    public void AddSelector(BasicEnemyActionSelector selector) {
        _data.Add(selector, null);
    }

    public void RemoveSelector(BasicEnemyActionSelector selector)
    {
        _data.Remove(selector);

    }

    public void ActionAdd(BasicEnemyActionSelector selector, IList<CharacterStatus.Action> actions) {

        _data[selector] = actions;

        if (_nowSelector == null || _nowSelector.Equals(selector))
        {
            _nowSelector = selector;
            _ActionSelect();
        }

    }

    void _ActionSelect() {

        if (!_data.TryGetValue(_nowSelector, out IList<CharacterStatus.Action> actions)) return;

        while (actions == null || actions.Count == 0) {
            _nowSelector.DoAction(CharacterStatus.Action.TurnEnd);

            _data[_nowSelector] = null;
            _nowSelector = _SelectNewSelector();

            if (_nowSelector == null)
                return;

            actions = _data[_nowSelector];

        }

        _nowSelector.CamSetting();

        StartCoroutine(_ActionSelect3(() => {

            if (actions.Contains(CharacterStatus.Action.Attack))
            {
                _nowSelector.DoAction(CharacterStatus.Action.Attack);
            }
            else if (actions.Contains(CharacterStatus.Action.Move))
            {
                _nowSelector.DoAction(CharacterStatus.Action.Move);
            }
            else
            {
                _nowSelector.DoAction(CharacterStatus.Action.Dice);
            }

            if (_nowSelector != null && _data.ContainsKey(_nowSelector))
                _data.Remove(_nowSelector);

        }));
    }

    IEnumerator _ActionSelect3(CallbackMethod callback)
    {
        yield return new WaitForSecondsRealtime(1f);

        callback?.Invoke();
    }

    BasicEnemyActionSelector _SelectNewSelector()
    {
        foreach (KeyValuePair<BasicEnemyActionSelector, IList<CharacterStatus.Action>> item in _data)
        {
            if (item.Value == null) continue;
            return item.Key;
        }

        return null;

    }

}