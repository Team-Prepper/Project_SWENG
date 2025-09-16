using System;

namespace SWEng.GamePlay
{
    public interface ICharacterStat : ICharacterComponent
    {
        public string Name { get; }
        public string CharacterCode { get; }
        
        public int Level { get; set; }

        public int Atk { get; }
        public int Dfs { get; }
        public string Skill { get; }
        
        public void SetCharacterCode(string characterCode);
        
        public void AddAtk(int amount);
        public void AddDfs(int amount);
    }

}