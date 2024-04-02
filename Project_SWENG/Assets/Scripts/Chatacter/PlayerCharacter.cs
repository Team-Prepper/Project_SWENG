using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {

    public class PlayerCharacter : NetworkCharacterController,
        IMoveable, IDicePoint  {

        [SerializeField] int _usePointAtAttack = 3;
        [SerializeField] float _movementDuration = 1f;
        [SerializeField] GameObject playerLight;

        [Header("Ref")]
        [SerializeField]
        private Animator _animator;

        public bool canUseSkill = true;

        [SerializeField] ParticleSystem LevelUpEffect;

        public void SetHealthUI(IHealthUI ui) {
            _healthUI = ui;
            _healthUI.UpdateGUI(stat.HP);
        }

        public void Move(Queue<Vector3> path)
        {
            StartCoroutine(_RotationCoroutine(path, 0.5f));
        }

        private IEnumerator _RotationCoroutine(Queue<Vector3> path, float rotationDuration)
        {
            Quaternion startRotation = transform.rotation;

            foreach (Vector3 targetPos in path)
            {
                Vector3 direction = targetPos - transform.position;
                Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

                float timeElapsed = 0;

                if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
                {

                    while (timeElapsed < rotationDuration)
                    {
                        timeElapsed += Time.deltaTime;
                        float lerpStep = timeElapsed / rotationDuration; // 0-1
                        transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                        yield return null;
                    }
                    transform.rotation = endRotation;
                }
                _photonView.RPC("SetPlayerOnHex", RpcTarget.All, 0, transform.position);
                HexGrid.Instance.GetTileAt(HexCoordinate.ConvertFromVector3(transform.position)).Entity = null;

                Vector3 startPosition = transform.position;

                HexCoordinate newHexPos = HexCoordinate.ConvertFromVector3(targetPos);
                //NetworkCloudManager.Instance.CloudActiveFalse(newHexPos);
                Hex goalHex = HexGrid.Instance.GetTileAt(newHexPos);
                goalHex.CloudActiveFalse();
                UsePoint(goalHex.Cost);


                timeElapsed = 0;

                while (timeElapsed < _movementDuration)
                {
                    if (_animator)
                        _animator.SetBool("IsWalk", true);

                    timeElapsed += Time.deltaTime;
                    float lerpStep = timeElapsed / _movementDuration;
                    transform.position = Vector3.Lerp(startPosition, targetPos, lerpStep);
                    yield return null;
                }
                transform.position = targetPos;

                Debug.Log("Selecting the next position!");

            }
            Debug.Log("Movement finished!");
            CamMovement.Instance.IsPlayerMove = false;
            _photonView.RPC("SetPlayerOnHex", RpcTarget.All, 1, transform.position);

            if (_animator)
                _animator.SetBool("IsWalk", false);
        }

        [PunRPC]
        public void SetPlayerOnHex(int type, Vector3 position)
        {
            if (type == 1)
            {
                HexGrid.Instance.GetTileAt(position).Entity = gameObject;
            }
            else
            {
                HexGrid.Instance.GetTileAt(position).Entity = null;
            }
        }

        string IMoveable.GetName()
        {
            return "Player";
        }

        [SerializeField] int _dicePoint;

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

        public void SetPoint(int setValue)
        {
            _dicePoint = setValue;
        }

        private void Awake()
        {
            stat.HP.FillMax();
        }

        protected override void Start()
        {
            base.Start();
            if(!PhotonNetwork.IsMasterClient)
            {
                playerLight.SetActive(false);
            }
        }

        public int Recover(int val)
        {
            if (!stat.IsAlive()) return 0;

            stat.Recover(val);

            //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
            return stat.HP.Value;
        }

        public bool CanAttack()
        {
            return GetPoint() >= _usePointAtAttack;
        }

        protected override void AttackAct(bool isSkill)
        {
            if (isSkill)
            {
                _photonView.RPC("AttackVfx", RpcTarget.All, null);
                return;
            }
            UsePoint(_usePointAtAttack);
        }
        
        public override int GetAttackValue()
        {
            return stat.GetAttackValue() +
                (InventoryManager.Instance.Weapon ? InventoryManager.Instance.Weapon.value : 0);
        }

        [PunRPC]
        public void AttackVfx()
        {
            EffectManager.Instance.SetTarget(gameObject);
            if (InventoryManager.Instance.Weapon)
            {
                int weaponID = InventoryManager.Instance.Weapon.id;
                EffectManager.Instance.ShowImpactVfxHandler(weaponID);
            }
        }

        protected override void DamageAct()
        {
            //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
        }

        public override void DieAct()
        {
            //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
            HexGrid.Instance.GetTileAt(this.transform.position).Entity = null;
            if(PhotonNetwork.IsMasterClient)
            {
                if(GameManager.Instance.remainLife > 0)
                {
                    GameManager.Instance.remainLife -= 1;
                    GameManager.Instance.HealthCountHandler();
                    photonView.RPC("RespawnPlayer", RpcTarget.All, null); 
                }
                else
                {
                    GameManager.Instance.GameEnd(false);
                }
            }
        }

        [PunRPC]
        public void RespawnPlayer()
        {
            Debug.Log("REVIVE");
            gameObject.transform.position = GameManager.Instance.respawnPos.position;
            HexGrid.Instance.GetTileAt(GameManager.Instance.respawnPos.position).Entity = gameObject;
            stat.HP.FillMax();
            Recover(100);
        }

        public void EquipItemHandler(Item item)
        {
            // itemType 0 : helmet
            // itemType 1 : Armor
            // itemType 2 : shield
            
            switch (item.type)
            {
                case Item.ItemType.Helmet:
                    photonView.RPC("EquipItem", RpcTarget.All, 0, item.value); 
                    break;
                case Item.ItemType.Armor:
                    photonView.RPC("EquipItem", RpcTarget.All, 1, item.value); 
                    break;
                case Item.ItemType.Shield:
                    photonView.RPC("EquipItem", RpcTarget.All, 2, item.value); 
                    break;
            }

            //EventEquip?.Invoke(this, null);
        }

        [PunRPC]
        public void EquipItem(int itemType, int value)
        {
            if (!stat.IsAlive()) return;

            switch (itemType)
            {
                case 0:
                    stat.HP.AddMaxValue(value);
                    stat.SetDef(value, true);
                    stat.SetAttackPower(value, true);
                    break;
                case 1:
                    stat.HP.AddMaxValue(value);
                    break;
                case 2:
                    stat.SetDef(value, true);
                    break;
            }
        }
        
        public void UnEquipItemHandler(Item item)
        {
            // itemType 0 : helmet
            // itemType 1 : Armor
            // itemType 2 : shield
            
            switch (item.type)
            {
                case Item.ItemType.Helmet:
                    photonView.RPC("UnequipItem", RpcTarget.All, 0, item.value); 
                    break;
                case Item.ItemType.Armor:
                    photonView.RPC("UnequipItem", RpcTarget.All, 1, item.value); 
                    break;
                case Item.ItemType.Shield:
                    stat.SetDef(item.value, false);
                    break;
            }
            //EventEquip?.Invoke(this, null);
        }
        
        [PunRPC]
        public void UnequipItem(int itemType, int value)
        {
            if (!stat.IsAlive()) return;

            switch (itemType)
            {
                case 0:
                    stat.HP.SubMaxValue(value);
                    stat.SetDef(value, false);
                    stat.SetAttackPower(value, false);
                    break;
                case 1:
                    stat.HP.SubMaxValue(value);
                    break;
                case 2:
                    stat.SetDef(value, false);
                    break;
            }
        }

        public void GetExp(int val)
        {
            if (stat.GetExp(val))
            {
                LevelUpEffect.Play();
                //EventEquip?.Invoke(this, null);
            }
            
        }
    }
}