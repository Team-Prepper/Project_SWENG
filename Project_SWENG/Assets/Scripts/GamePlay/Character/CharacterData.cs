using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Custom/CharacterData", order = 2)]
public class CharacterData : ScriptableObject {

    [System.Serializable]
    public class StatusElement {
        public int HP;
        public int Atk;
        public int Dfs;

    }

    public string CharacterName;

    public SkillData DefaultSkill;

    public Sprite Image;

    public Character CharacterPrefab;

    public StatusElement[] StatusElements;

    public bool IsHumanType = true;

}
