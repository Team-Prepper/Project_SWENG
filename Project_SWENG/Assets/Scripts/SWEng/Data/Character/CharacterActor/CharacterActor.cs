using System;
using UnityEngine;

namespace SWEng.Data {

    public class CharacterActor : MonoBehaviour
    {

        [SerializeField] private float _dieTime = 2f;
        [SerializeField] private Animator _anim;
        [SerializeField] private CircularHPBar _hpUI;

        private Action _removeAction;

        public void Die()
        {
            Invoke(nameof(DieEvent), _dieTime);
        }

        public void AddRemoveAction(Action action)
        {
            _removeAction += action;
        }

        private void DieEvent()
        {
            _removeAction?.Invoke();
        }

        public void SetHPViewActive(bool visible)
        {
            //_hpUI.gameObject.SetActive(visible);
        }

        public void EquipItem(string targetItem)
        {
            Debug.LogFormat("Equip: {0}", targetItem);
        }

        public void ChangeAnimClip(string key, AnimationClip clip)
        {

            AnimatorOverrideController overrideController =
                new AnimatorOverrideController(_anim.runtimeAnimatorController);

            Debug.Log(key);
            overrideController[key] = clip; // 기존 Idle 애니메이션을 다른 클립으로 변경
            _anim.runtimeAnimatorController = overrideController;

        }
        
        public void PlayAnim(string triggerType, string triggerValue)
        {
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

    }
}