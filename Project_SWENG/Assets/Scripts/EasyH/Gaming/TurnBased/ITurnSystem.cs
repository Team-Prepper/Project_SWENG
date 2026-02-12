using UnityEngine;
using System;

namespace EasyH.Gaming.TurnBased
{

    public interface ITurnSystem : IObservable<int>
    {
        public void SetGameProceedCondition(Func<bool> condition);
        public void SetStartCondition(Func<bool> condition);
        
        public int TurnSpend { get; }
        public int ActiveTeamIdx { get; }

        public void StartGame();
        public void AddTeamMember(IMemberState m);
        public void RemoveTeamMember(IMemberState m);
        public int GetTeamMemberCnt(int idx);
        public void TurnEnd();
    }
    
}