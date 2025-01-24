using System.Collections.Generic;

public class Team {

    IDictionary<ICharacterController, bool> _members;

    public Team()
    {
        _members = new Dictionary<ICharacterController, bool>();
    }

    public void AddMember(ICharacterController c)
    {
        _members.Add(c, false);
    }

    public void MemberTurnEnd(ICharacterController c)
    {
        if (!_members.ContainsKey(c)) return;
        _members[c] = true;
    }

    public void RemoveMember(ICharacterController c)
    {
        _members.Remove(c);
    }

    public void StartTurn()
    {

        IDictionary<ICharacterController, bool> members = new Dictionary<ICharacterController, bool>();
        IList<ICharacterController> list = new List<ICharacterController>();

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
            if (!b) return false;
        }
        return true;
    }
}