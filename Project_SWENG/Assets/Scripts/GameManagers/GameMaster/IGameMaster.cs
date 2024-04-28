using CharacterSystem;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UISystem;
using UnityEngine;

public class Team {
    Dictionary<ICharacterController, bool> _members;

    public Team() {
        _members = new Dictionary<ICharacterController, bool>();
    }

    public void AddMember(ICharacterController c) {
        _members.Add(c, false);
    }

    public void MemberTurnEnd(ICharacterController c)
    {
        if (!_members.ContainsKey(c)) return;
        _members[c] = true;
    }

    public void RemoveMember(ICharacterController c) {
        _members.Remove(c);
    }

    public void StartTurn() {

        Dictionary<ICharacterController, bool> members = new Dictionary<ICharacterController, bool>();
        List<ICharacterController> list = new List<ICharacterController>();

        foreach (ICharacterController m in _members.Keys)
        {
            members.Add(m, false);
            list.Add(m);
        }

        _members = members;

        foreach (ICharacterController m in list)
        {
            m.SetPlay();
        }

    }

    public bool CanNextTurn()
    {
        foreach (bool b in _members.Values)
        {
            if(!b) return false;
        }
        return true;
    }
}

public interface IGameMaster
{
    public enum Phase {
        Ready,
        Play,
        End
    }

    public GameObject InstantiateCharacter(GameObject prefab, Vector3 position, Quaternion rotation);

    public void AddTeamMember(ICharacterController c, int teamIdx);
    public void RemoveTeamMember(ICharacterController c, int teamIdx);
    public void TurnEnd(ICharacterController c);

    public void GameEnd(bool victory);

}
