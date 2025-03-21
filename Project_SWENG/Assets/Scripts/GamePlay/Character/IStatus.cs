using EHTool.UtilKit;
using System;

public interface IStatus : IObservable<IStatus> {

    public string Name { get; }
    public string CharacterCode { get; set; }

    public GaugeValue<int> HP { get; }
    public int Level { get; set; }

    public int Atk { get; }
    public int Dfs { get; }
    public SkillData Skill { get; }
    public bool IsAlive { get; }

    public void SetCC(ICharacterController cc);
    
    public void AddAtk(int amount);
    public void AddDfs(int amount);

    public void TakeDamage(int amount);

}