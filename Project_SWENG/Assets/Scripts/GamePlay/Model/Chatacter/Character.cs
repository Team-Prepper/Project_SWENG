using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterSystem {

    public class Character : MonoBehaviour,
    IMoveable, IDicePoint {
        public enum Action { 
            Dice, Move, Attack
        }

        protected ICharacterController _cc;

        [SerializeField] protected IHealthUI _healthUI;

        [SerializeField] protected int _dicePoint;
        [SerializeField] private float _movementDuration = 0.5f;

        [SerializeField] protected GameObject equipCam;
        [SerializeField] protected Animator anim;

        public Stat stat;

        public void TakeDamage(int amount)
        {
            if (!stat.IsAlive()) return;

            stat.Damaged(amount);
            _healthUI.UpdateGUI(stat.HP);

        }

        public void Move(Queue<Vector3> path)
        {
            StartCoroutine(_RotationCoroutine(path, 0.1f));
        }

        private IEnumerator _RotationCoroutine(Queue<Vector3> path, float rotationDuration)
        {
            Transform trParent = transform.parent;

            if (anim)
                anim.SetBool("IsWalk", true);

            foreach (Vector3 targetPos in path)
            {

                UsePoint(2);

                Vector3 startPosition = trParent.position;
                Vector3 direction = targetPos - startPosition;

                Quaternion startRotation = trParent.rotation;
                Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

                float timeElapsed;

                // È¸Àü

                if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
                {
                    timeElapsed = 0;

                    while (timeElapsed < rotationDuration)
                    {
                        timeElapsed += Time.deltaTime;
                        trParent.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / rotationDuration);

                        yield return null;
                    }

                    trParent.rotation = endRotation;

                }

                timeElapsed = 0;

                while (timeElapsed < _movementDuration)
                {
                    timeElapsed += Time.deltaTime;
                    trParent.position = Vector3.Lerp(startPosition, targetPos, timeElapsed / _movementDuration);

                    yield return null;
                }
                trParent.position = targetPos;

                _cc.MoveTo(HexCoordinate.ConvertFromVector3(startPosition), HexCoordinate.ConvertFromVector3(targetPos));
            }

            anim.SetBool("IsWalk", false);

            StartCoroutine(_ActionEnd(.4f));

        }

        public void UsePoint(int usingAmount)
        {
            if (_dicePoint < usingAmount)
            {
                return;
            }
            _dicePoint -= usingAmount;
        }

        public int GetPoint()
        {
            return _dicePoint;
        }

        public virtual void SetPoint(int setValue)
        {
            _dicePoint = setValue;
            _cc.ActionEnd();
        }

        public virtual string GetName() {
            return string.Empty;
        }
        
        public virtual int GetAttackValue()
        {
            return stat.GetAttackValue();
        }

        public virtual int GetTeamIdx() {
            return 1;
        }

        public virtual void DoAction() { 
            
        }

        public virtual void DoAttact() {
            new BasicAttack(_cc, transform.position, 10);
        }

        public virtual void DoMove() { 
            
        }

        public virtual void Initial(ICharacterController cc) {

            if (anim == null) anim = GetComponent<Animator>();
            _cc = cc;
        }

        public virtual void AttackAct(float time)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(_ActionEnd(time));
        }

        IEnumerator _ActionEnd(float spendTime) {
            yield return new WaitForSeconds(spendTime);
            _cc.ActionEnd();
        }

        public virtual void DamageAct()
        {
            anim.SetTrigger("Hit");

        }

        public virtual void DieAct()
        {
            anim.SetTrigger("Die");

        }

        public virtual void SetPlay() {
            SetPoint(10);
        }

        public virtual IList<Action> GetCanDoAction() {
            return new List<Action>();
        }

    }
}
