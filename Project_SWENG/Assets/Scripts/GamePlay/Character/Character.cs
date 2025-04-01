using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [SerializeField] private float _dieTime = 2f;
    [SerializeField] private Animator _anim;
    [SerializeField] private CircularHPBar _hpUI;

    private ICharacterController _cc;

    private CharacterMove _moveComp;
    private CharacterAttack _attackComp;

    private Action _removeAction;

    public void Die()
    {
        Invoke(nameof(DieEvent), _dieTime);
    }

    public void AddRemoveAction(Action action) {
        _removeAction += action;
    }

    private void DieEvent() {

        GameObject item = GameManager.Instance.GameMaster.InstantiateItem(transform.position);
        item.GetComponent<IItemController>().SetInitial("Item_Heal");
        
        _removeAction?.Invoke();

    }

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;

        transform.SetParent(_cc.transform);
        transform.localPosition = Vector3.zero;

        _moveComp = gameObject.GetComponent<CharacterMove>();
        _attackComp = gameObject.GetComponent<CharacterAttack>();

        if (_moveComp == null)
            _moveComp = gameObject.AddComponent<CharacterMove>();
        if (_attackComp == null)
            _attackComp = gameObject.AddComponent<CharacterAttack>();

        _moveComp.SetCC(_cc);
        _attackComp.SetCC(_cc);

        _cc.Status.Subscribe(_hpUI);
    }

    public void SetHPViewActive(bool visible) {
        _hpUI.gameObject.SetActive(visible);
    }

    public void Move(Queue<Vector3> path,
        Action<HexCoordinate, HexCoordinate> moveAction) {
        _moveComp.Move(path, moveAction);
    }

    public void Interaction(HexCoordinate targetPos)
    {
        HexGrid.Instance.GetMapUnitAt(targetPos).Interaction(_cc);
        _cc.DicePoint.UsePoint(1);
    }

    public void EquipItem(string targetItem)
    {
        Debug.LogFormat("Equip: {0}", targetItem);
    }

    public void ChangeAnimClip(string key, AnimationClip clip) {
        
        AnimatorOverrideController overrideController =
            new AnimatorOverrideController(_anim.runtimeAnimatorController);
        
        Debug.Log(key);
        overrideController[key] = clip; // 기존 Idle 애니메이션을 다른 클립으로 변경
        _anim.runtimeAnimatorController = overrideController;

    }

    public void PlayAnim(string triggerType, string triggerValue) {
        switch (triggerType)
        {
            case "SetBoolTrue":
                _anim.SetBool(triggerValue, true);
                return;
            case "SetBoolFalse":
                _anim.SetBool(triggerValue, false);
                return;
            case "SetTrigger":
            default:
                _anim.SetTrigger(triggerValue);
                return;
        }
    }

    public IList<IActionSelector.Action> GetActionList()
    {
        List<IActionSelector.Action> list = new List<IActionSelector.Action>();

        if (_cc.DicePoint.GetPoint() > 0)
            list.Add(IActionSelector.Action.Interaction);

        _moveComp.TryAddAction(list);
        _attackComp.TryAddAction(list);

        return list;

    }

}