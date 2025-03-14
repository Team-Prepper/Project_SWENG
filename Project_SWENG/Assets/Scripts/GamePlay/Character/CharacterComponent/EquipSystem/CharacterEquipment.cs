public class CharacterEquipment : ICharacterComponent {

    ICharacterController _cc;

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void AddItem(string itemCode) { 
        
    }

    public void UseItem(string itemCode) { 
        
    }

}