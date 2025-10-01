using UnityEngine;
using SWEng;
using EasyH.Unity.UI;

public class PlayerActionSelectorSpawner : ActionSelectorSpawner
{
    [SerializeField] private string _playerActionSelectUIKey = "PlayerActionSelect";

    private ICharacterController _actionSelector;
    
    public override ICharacterController Spawn()
    {
        _actionSelector ??=
            UIManager.Instance.OpenGUI<GUIPlayerActionSelect>(
                _playerActionSelectUIKey);

        return _actionSelector;

    }
}