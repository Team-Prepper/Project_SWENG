using UnityEngine;

public interface IGameMaster
{
    public enum Phase {
        Ready,
        Play,
        End
    }

    public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation);
    public GameObject InstantiateItem(Vector3 position);

    public void AddTeamMember(ICharacterController c, int teamIdx);
    public void RemoveTeamMember(ICharacterController c, int teamIdx);
    public void TurnEnd(ICharacterController c);

    public void GameEnd(bool victory);
    void StartGame();
}
