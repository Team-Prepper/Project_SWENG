using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonCharacterController :
    MonoBehaviourPun, ICharacterController
{

    [SerializeField] private PhotonStatus _status;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private PhotonView _view;

    public int TeamIdx { get; private set; }

    public HexCoordinate HexPos =>
        HexCoordinate.ConvertFromVector3(transform.position);

    public Character Character { get; private set; }
    public IStatus Status => _status;
    public Inventory Inventory => _inventory;
    public IDicePoint DicePoint { get; private set; }
    public bool IsRollDice { get; set; }

    private IActionSelector _actionSelector;
    private bool _camSync;

    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        _view.RPC(nameof(PunAllInitial), RpcTarget.All,
            characterName, teamIdx, camSync);
    }

    [PunRPC]
    private void PunAllInitial(string characterName, int teamIdx, bool camSync)
    {
        HexGrid.Instance.GetMapUnitAt(transform.position).
            SetCC(gameObject, this);

        Character = Instantiate(CharacterManager.Instance.
            GetCharacterData(characterName).CharacterPrefab);

        Character.SetCC(this);

        Status.SetCC(this);
        Status.CharacterCode = characterName;

        Inventory.SetCC(this);

        DicePoint = new DicePoint();

        TeamIdx = teamIdx;
        _camSync = camSync;

        if (!PhotonNetwork.IsMasterClient) return;

        GameManager.Instance.GameMaster.
            AddTeamMember(this, TeamIdx);

        Character.AddRemoveAction(() =>
        {
            GameManager.Instance.GameMaster.
                RemoveTeamMember(this, TeamIdx);

            PhotonNetwork.Destroy(gameObject);

        });

    }

    public void Remove()
    {
        _view.RPC(nameof(PunMasterRemove), RpcTarget.MasterClient);
    }

    [PunRPC]
    public void PunMasterRemove()
    {
        HexGrid.Instance.GetMapUnitAt(HexPos).ResetEntityState();

        Character.Die();
    }

    public void CamSetting(string key)
    {
        if (!_camSync)
        {
            CameraManager.Instance.CameraSetting(transform, key);
            return;
        }

        _view.RPC(nameof(PunAllCamSetting), RpcTarget.All, key);
    }

    [PunRPC]
    private void PunAllCamSetting(string key)
    {
        CameraManager.Instance.CameraSetting(transform, key);
    }

    public void PlayAnim(string triggerType, string triggerValue)
    {
        _view.RPC(nameof(PunAllPlayAnim), RpcTarget.All,
            triggerType, triggerValue);
    }

    [PunRPC]
    public void PunAllPlayAnim(string type, string value)
    {
        Character.PlayAnim(type, value);
    }

    public void TakeDamage(int amount)
    {
        Status.TakeDamage(amount);
    }

    public void SetPlay()
    {
        _view.RPC(nameof(PunAllSetPlay), RpcTarget.All);
    }

    [PunRPC]
    private void PunAllSetPlay()
    {
        if (_actionSelector == null) return;

        IsRollDice = false;

        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector)
    {
        _actionSelector = actionSelector;
    }

    public void ActionEnd(float time = 0)
    {
        if (_actionSelector == null) return;

        Invoke(nameof(_ActionEnd), time);
    }

    public void _ActionEnd()
    {
        IList<IActionSelector.Action> list
            = Character.GetActionList();

        if (IsRollDice == false)
            list.Add(IActionSelector.Action.Dice);

        _actionSelector.Ready(this, list);
    }

    public void TurnEnd()
    {
        _view.RPC(nameof(PunMasterTurnEnd), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void PunMasterTurnEnd()
    {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        _view.RPC(nameof(PunAllMoveTo), RpcTarget.All,
            before.x, before.z, after.x, after.z);
    }

    [PunRPC]
    private void PunAllMoveTo(int bX, int bZ, int aX, int aZ)
    {
        HexGrid.Instance.GetMapUnitAt
            (new HexCoordinate(bX, bZ)).ResetEntityState();
        HexGrid.Instance.GetMapUnitAt
            (new HexCoordinate(aX, aZ)).SetCC(gameObject, this);
    }

    public void Move(Queue<Vector3> path)
    {
        Character.Move(path, MoveTo);
    }

    public void Interaction(HexCoordinate targetPos)
    {
        Character.Interaction(targetPos);
    }

}