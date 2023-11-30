using Photon.Pun;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [HideInInspector]
    public CharacterObjectListsAllGender allGender;
    [HideInInspector]
    public CharacterObjectGroups body;

    private PlayerController _playerController;

    [Header("Network")]
    private PhotonView _photonView;

    public GameObject maleHeadParent;
    public GameObject femaleHeadParent;
    public GameObject hairParent;
    public GameObject maleEyebrowsParent;
    public GameObject femaleEyebrowsParent;
    public GameObject facialHairParent;

    [SerializeField] private bool isMale = true;
    
    [Header("Current Equipment")]
    public Item curEquipHelmet;

    private int curEquipHelmetCode = 0;
    private int curEquipHelmetType = 0;
    
    private Item curEquipArmor;

    private Item curEquipWeapon;
    private GameObject weaponModel;
    [SerializeField] private Transform weaponSlot;

    private Item curEquipShield;
    private GameObject shieldModel;
    [SerializeField] private Transform shieldSlot;
    
    private Material mat;

    [Range(0, 21)]
    public int curFaceCode;
    [Range(0, 6)]
    public int curEyebrowCode;
    [Range(0,17)]
    public int curBeardCode = 17;
    [Range(0, 37)]
    public int curHairCode = 33;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _playerController = GetComponent<PlayerController>();
        _BuildLists();
        _SetupPlayer(0);
    }

    private void _BuildLists()
    {
        // build out all gender lists
        _BuildList(allGender.all_Hair, "All_01_Hair");
        _BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
        _BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
        _BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
        _BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
        _BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
        _BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
        _BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
        _BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
        _BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
        _BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
        _BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
        _BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
        _BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
        _BuildList(allGender.elf_Ear, "Elf_Ear");

        if (isMale)
        {
            //build out body lists
            _BuildList(body.headAllElements, "Male_Head_All_Elements");
            _BuildList(body.headNoElements, "Male_Head_No_Elements");
            _BuildList(body.eyebrow, "Male_01_Eyebrows");
            _BuildList(body.facialHair, "Male_02_FacialHair");
            _BuildList(body.torso, "Male_03_Torso");
            _BuildList(body.arm_Upper_Right, "Male_04_Arm_Upper_Right");
            _BuildList(body.arm_Upper_Left, "Male_05_Arm_Upper_Left");
            _BuildList(body.arm_Lower_Right, "Male_06_Arm_Lower_Right");
            _BuildList(body.arm_Lower_Left, "Male_07_Arm_Lower_Left");
            _BuildList(body.hand_Right, "Male_08_Hand_Right");
            _BuildList(body.hand_Left, "Male_09_Hand_Left");
            _BuildList(body.hips, "Male_10_Hips");
            _BuildList(body.leg_Right, "Male_11_Leg_Right");
            _BuildList(body.leg_Left, "Male_12_Leg_Left");

            return;

        }

        //build out body lists
        _BuildList(body.headAllElements, "Female_Head_All_Elements");
        _BuildList(body.headNoElements, "Female_Head_No_Elements");
        _BuildList(body.eyebrow, "Female_01_Eyebrows");
        _BuildList(body.facialHair, "Female_02_FacialHair");
        _BuildList(body.torso, "Female_03_Torso");
        _BuildList(body.arm_Upper_Right, "Female_04_Arm_Upper_Right");
        _BuildList(body.arm_Upper_Left, "Female_05_Arm_Upper_Left");
        _BuildList(body.arm_Lower_Right, "Female_06_Arm_Lower_Right");
        _BuildList(body.arm_Lower_Left, "Female_07_Arm_Lower_Left");
        _BuildList(body.hand_Right, "Female_08_Hand_Right");
        _BuildList(body.hand_Left, "Female_09_Hand_Left");
        _BuildList(body.hips, "Female_10_Hips");
        _BuildList(body.leg_Right, "Female_11_Leg_Right");
        _BuildList(body.leg_Left, "Female_12_Leg_Left");
    }

    private void _SetFace()
    {
        allGender.all_Hair[0].SetActive(true);
        body.headAllElements[0].SetActive(true);
        body.eyebrow[0].SetActive(true);
    }

    private void _BuildList(List<GameObject> targetList, string characterPart)
    {
        Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>();

        Transform targetRoot = null;

        // find character parts parent object in the scene
        foreach (Transform t in rootTransform)
        {
            if (t.gameObject.name == characterPart)
            {
                targetRoot = t;
                break;
            }
        }
        targetList.Clear();

        // cycle through all child objects of the parent object
        for (int i = 0; i < targetRoot.childCount; i++)
        {
            GameObject go = targetRoot.GetChild(i).gameObject;
            go.SetActive(false);
            targetList.Add(go);

            if (mat) continue;

            if (go.GetComponent<SkinnedMeshRenderer>())
                mat = go.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        }
    }
    
    private void _SetupPlayer(int newArmorCode)
    {
        allGender.all_Hair[curHairCode].SetActive(true);

        _SetFace();
        _SetHelmetByBool(true);
        _SetArmorByBool(newArmorCode, true);

        if (isMale)
            body.facialHair[curBeardCode].SetActive(true);
    }

    public Item GetEquipWeaponHasSkill()
    {
        if (curEquipWeapon != null && curEquipWeapon.hasSkill)
            return curEquipWeapon;
        
        return null;
    }

    public Item GetEquipHelmet()
    {
        if (curEquipHelmet)
            return curEquipHelmet;
        return null;
    }
    public Item GetEquipArmor()
    {
        if (curEquipArmor)
            return curEquipArmor;
        return null;
    }
    
    public Item GetEquipShield()
    {
        if (curEquipShield)
            return curEquipShield;
        return null;
    }

    private void _SetHelmet(int itemID)
    {
        //helmetType 0 : headCovering Base
        //helmetType 1 : headCovering No FacialHair
        //helmetType 2 : headCovering No Hair
        //helmetType 3 : no head

        curEquipHelmetCode = itemID % 100 - 1; ;
        curEquipHelmetType = itemID / 100; ;

        _photonView.RPC("_SetHelmetByBool", RpcTarget.All, true);
       
    }

    public void _ResetHelmet()
    {
        _photonView.RPC("_SetHelmetByBool", RpcTarget.All, false);
    }

    [PunRPC]
    private void _SetHelmetByBool(bool tryEquip)
    {
        switch (curEquipHelmetType)
        {
            case 0:
                allGender.all_Hair[curEquipHelmetType].SetActive(tryEquip);
                break;
            case 1:
                facialHairParent.SetActive(!tryEquip);
                allGender.headCoverings_No_FacialHair[curEquipHelmetCode].SetActive(tryEquip);
                break;
            case 2:
                hairParent.SetActive(!tryEquip);
                allGender.headCoverings_No_Hair[curEquipHelmetCode].SetActive(tryEquip);
                break;
            case 3:
                body.headNoElements[curEquipHelmetCode].SetActive(tryEquip);
                hairParent.SetActive(!tryEquip);
                if (isMale)
                {
                    maleHeadParent.SetActive(!tryEquip);
                    maleEyebrowsParent.SetActive(!tryEquip);
                    facialHairParent.SetActive(!tryEquip);
                    break;

                }
                femaleHeadParent.SetActive(!tryEquip);
                femaleEyebrowsParent.SetActive(!tryEquip);
                break;
        }
    }

    public void EquipHelmet(Item item)
    {
        _ResetHelmet();
        if(curEquipHelmet)
            _playerController.UnEquipItemHandler(curEquipHelmet);
        _SetHelmet(item.id);
        curEquipHelmet = item;
        _playerController.EquipItemHandler(item);
    }

    private void _ResetArmor(int curArmorCode)
    {
        _photonView.RPC("_SetArmorByBool", RpcTarget.All, curArmorCode, false);
    }

    private void _SetArmor(int newArmorCode)
    {
        _photonView.RPC("_SetArmorByBool", RpcTarget.All, newArmorCode, true);
    }

    [PunRPC]
    private void _SetArmorByBool(int newArmorCode, bool tryEquip)
    {
        body.torso[newArmorCode].SetActive(tryEquip);
        int setArmorCode = newArmorCode % 18;
        body.arm_Upper_Right[setArmorCode].SetActive(tryEquip);
        body.arm_Upper_Left[setArmorCode].SetActive(tryEquip);
        body.arm_Lower_Right[setArmorCode].SetActive(tryEquip);
        body.arm_Lower_Left[setArmorCode].SetActive(tryEquip);
        body.hand_Right[setArmorCode].SetActive(tryEquip);
        body.hand_Left[setArmorCode].SetActive(tryEquip);
        body.hips[setArmorCode].SetActive(tryEquip);
        body.leg_Right[setArmorCode].SetActive(tryEquip);
        body.leg_Left[setArmorCode].SetActive(tryEquip);
    }

    public void EquipArmor(Item item)
    {
        if(curEquipArmor == null)
        {
            _ResetArmor(0);
        }
        else
        {
            _ResetArmor(curEquipArmor.id);
            _playerController.UnEquipItemHandler(curEquipArmor);
        }
        
        _SetArmor(item.id);
        curEquipArmor = item;
        _playerController.EquipItemHandler(item);
    }
    
    // Equip Weapon
    public void EquipWeapon(Item newWeapon)
    {
        if (curEquipWeapon != newWeapon)
            UnequipWeapon();

        weaponModel = Instantiate(newWeapon.itemObject, weaponSlot);
        curEquipWeapon = newWeapon;
    }

    private void UnequipWeapon()
    {
        if(weaponModel != null)
            Destroy(weaponModel);
        curEquipWeapon = null;
    }

    public void EquipShield(Item newShield)
    {
        if (curEquipShield != newShield)
        {
            UnequipShield();
        }
            
        
        shieldModel = Instantiate(newShield.itemObject, shieldSlot);
        curEquipShield = newShield;
        _playerController.EquipItemHandler(newShield);
    }

    private void UnequipShield()
    {
        if (shieldModel != null)
        {
            Destroy(shieldModel);
            _playerController.UnEquipItemHandler(curEquipShield);
        }
            
        curEquipShield = null;
    }

}

// classe for keeping the lists organized, allows for simple switching from male/female objects
[System.Serializable]
public class CharacterObjectGroups
{
    public List<GameObject> headAllElements;
    public List<GameObject> headNoElements;
    public List<GameObject> eyebrow;
    public List<GameObject> facialHair;
    public List<GameObject> torso;
    public List<GameObject> arm_Upper_Right;
    public List<GameObject> arm_Upper_Left;
    public List<GameObject> arm_Lower_Right;
    public List<GameObject> arm_Lower_Left;
    public List<GameObject> hand_Right;
    public List<GameObject> hand_Left;
    public List<GameObject> hips;
    public List<GameObject> leg_Right;
    public List<GameObject> leg_Left;
}

// classe for keeping the lists organized, allows for organization of the all gender items
[System.Serializable]
public class CharacterObjectListsAllGender
{
    public List<GameObject> headCoverings_Base_Hair;
    public List<GameObject> headCoverings_No_FacialHair;
    public List<GameObject> headCoverings_No_Hair;
    public List<GameObject> all_Hair;
    public List<GameObject> all_Head_Attachment;
    public List<GameObject> chest_Attachment;
    public List<GameObject> back_Attachment;
    public List<GameObject> shoulder_Attachment_Right;
    public List<GameObject> shoulder_Attachment_Left;
    public List<GameObject> elbow_Attachment_Right;
    public List<GameObject> elbow_Attachment_Left;
    public List<GameObject> hips_Attachment;
    public List<GameObject> knee_Attachement_Right;
    public List<GameObject> knee_Attachement_Left;
    public List<GameObject> all_12_Extra;
    public List<GameObject> elf_Ear;
}
