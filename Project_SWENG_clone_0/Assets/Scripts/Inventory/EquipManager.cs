using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EquipManager : MonoSingleton<EquipManager>
{
    [HideInInspector]
    public List<GameObject> enabledObjects = new List<GameObject>();
    [HideInInspector]
    public CharacterObjectGroups male;
    [HideInInspector]
    public CharacterObjectGroups female;
    [HideInInspector]
    public CharacterObjectListsAllGender allGender;

    public GameObject maleHeadParent;
    public GameObject femaleHeadParent;
    public GameObject hairParent;
    public GameObject maleEyebrowsParent;
    public GameObject femaleEyebrowsParent;
    public GameObject facialHairParent;

    public bool isMale = true;
    
    [Header("Current Equipment")]
    public Item curEquipHelmet;

    public int curEquipHelmetCode;
    public int curEquipHelmetType;
    
    public Item curEquipArmor;

    public Item curEquipWeapon;
    public GameObject weaponModel;
    public Transform weaponSlot;

    public Item curEquipShield;
    public GameObject shieldModel;
    public Transform shieldSlot;
    
    public Material mat;

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
        BuildLists();
        if (enabledObjects.Count != 0)
        {
            foreach (GameObject g in enabledObjects)
            {
                g.SetActive(false);
            }
        }
        enabledObjects.Clear();
        SetupPlayer(0);
    }
    
    


    private void BuildLists()
    {
        //build out male lists
        BuildList(male.headAllElements, "Male_Head_All_Elements");
        BuildList(male.headNoElements, "Male_Head_No_Elements");
        BuildList(male.eyebrow, "Male_01_Eyebrows");
        BuildList(male.facialHair, "Male_02_FacialHair");
        BuildList(male.torso, "Male_03_Torso");
        BuildList(male.arm_Upper_Right, "Male_04_Arm_Upper_Right");
        BuildList(male.arm_Upper_Left, "Male_05_Arm_Upper_Left");
        BuildList(male.arm_Lower_Right, "Male_06_Arm_Lower_Right");
        BuildList(male.arm_Lower_Left, "Male_07_Arm_Lower_Left");
        BuildList(male.hand_Right, "Male_08_Hand_Right");
        BuildList(male.hand_Left, "Male_09_Hand_Left");
        BuildList(male.hips, "Male_10_Hips");
        BuildList(male.leg_Right, "Male_11_Leg_Right");
        BuildList(male.leg_Left, "Male_12_Leg_Left");

        //build out female lists
        BuildList(female.headAllElements, "Female_Head_All_Elements");
        BuildList(female.headNoElements, "Female_Head_No_Elements");
        BuildList(female.eyebrow, "Female_01_Eyebrows");
        BuildList(female.facialHair, "Female_02_FacialHair");
        BuildList(female.torso, "Female_03_Torso");
        BuildList(female.arm_Upper_Right, "Female_04_Arm_Upper_Right");
        BuildList(female.arm_Upper_Left, "Female_05_Arm_Upper_Left");
        BuildList(female.arm_Lower_Right, "Female_06_Arm_Lower_Right");
        BuildList(female.arm_Lower_Left, "Female_07_Arm_Lower_Left");
        BuildList(female.hand_Right, "Female_08_Hand_Right");
        BuildList(female.hand_Left, "Female_09_Hand_Left");
        BuildList(female.hips, "Female_10_Hips");
        BuildList(female.leg_Right, "Female_11_Leg_Right");
        BuildList(female.leg_Left, "Female_12_Leg_Left");

        // build out all gender lists
        BuildList(allGender.all_Hair, "All_01_Hair");
        BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
        BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
        BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
        BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
        BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
        BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
        BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
        BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
        BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
        BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
        BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
        BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
        BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
        BuildList(allGender.elf_Ear, "Elf_Ear");
    }
    
    void BuildList(List<GameObject> targetList, string characterPart)
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

            if (!mat)
            {
                if (go.GetComponent<SkinnedMeshRenderer>())
                    mat = go.GetComponent<SkinnedMeshRenderer>().material;
            }
        }
    }
    
    void SetupPlayer(int newArmorCode)
    {
        if (isMale)
        {
            allGender.all_Hair[curHairCode].SetActive(true);
            male.headAllElements[newArmorCode].SetActive(true);
            male.eyebrow[newArmorCode].SetActive(true);
            male.facialHair[curBeardCode].SetActive(true);
            male.torso[newArmorCode].SetActive(true);
            male.arm_Upper_Right[newArmorCode].SetActive(true);
            male.arm_Upper_Left[newArmorCode].SetActive(true);
            male.arm_Lower_Right[newArmorCode].SetActive(true);
            male.arm_Lower_Left[newArmorCode].SetActive(true);
            male.hand_Right[newArmorCode].SetActive(true);
            male.hand_Left[newArmorCode].SetActive(true);
            male.hips[newArmorCode].SetActive(true);
            male.leg_Right[newArmorCode].SetActive(true);
            male.leg_Left[newArmorCode].SetActive(true);
        }
        else
        {
            allGender.all_Hair[newArmorCode].SetActive(true);
            female.headAllElements[newArmorCode].SetActive(true);
            female.eyebrow[newArmorCode].SetActive(true);
            female.torso[newArmorCode].SetActive(true);
            female.arm_Upper_Right[newArmorCode].SetActive(true);
            female.arm_Upper_Left[newArmorCode].SetActive(true);
            female.arm_Lower_Right[newArmorCode].SetActive(true);
            female.arm_Lower_Left[newArmorCode].SetActive(true);
            female.hand_Right[newArmorCode].SetActive(true);
            female.hand_Left[newArmorCode].SetActive(true);
            female.hips[newArmorCode].SetActive(true);
            female.leg_Right[newArmorCode].SetActive(true);
            female.leg_Left[newArmorCode].SetActive(true);
        }
    }
    
    private void SetHelmet(Item item)
    {
        
        int helmetType  = item.id / 100;
        int helmetCode = item.id % 100 -1;
        
        //helmetType 0 : headCovering Base
        //helmetType 1 : headCovering No FacialHair
        //helmetType 2 : headCovering No Hair
        //helmetType 3 : no head

        curEquipHelmetCode = helmetCode;
        curEquipHelmetType = helmetType;
        if (isMale)
        {
            switch (helmetType)
            {
                case 0:
                    allGender.all_Hair[helmetCode].SetActive(true);
                    break;
                case 1:
                    facialHairParent.SetActive(false);
                    allGender.headCoverings_No_FacialHair[helmetCode].SetActive(true);
                    break;
                case 2:
                    hairParent.SetActive(false);
                    allGender.headCoverings_No_Hair[helmetCode].SetActive(true);
                    break;
                case 3:
                    maleHeadParent.SetActive(false);
                    maleEyebrowsParent.SetActive(false);
                    facialHairParent.SetActive(false);
                    male.headNoElements[helmetCode].SetActive(true);
                    break;
            
            }
        }
        else
        {
            switch (helmetType)
            {
                case 0:
                    allGender.all_Hair[helmetCode].SetActive(true);
                    break;
                case 1:
                    allGender.headCoverings_No_FacialHair[helmetCode].SetActive(true);
                    break;
                case 2:
                    hairParent.SetActive(false);
                    allGender.headCoverings_No_Hair[helmetCode].SetActive(true);
                    break;
                case 3:
                    femaleHeadParent.SetActive(false);
                    femaleEyebrowsParent.SetActive(false);
                    female.headNoElements[helmetCode].SetActive(true);
                    break;
            
            }
        }
        
    }

    public void ResetHelmet()
    {
        if (isMale)
        {
            switch (curEquipHelmetType)
            {
                case 0:
                    allGender.all_Hair[curEquipHelmetType].SetActive(false);
                    break;
                case 1:
                    facialHairParent.SetActive(true);
                    allGender.headCoverings_No_FacialHair[curEquipHelmetCode].SetActive(false);
                    break;
                case 2:
                    hairParent.SetActive(true);
                    allGender.headCoverings_No_Hair[curEquipHelmetCode].SetActive(false);
                    break;
                case 3:
                    maleHeadParent.SetActive(true);
                    maleEyebrowsParent.SetActive(true);
                    facialHairParent.SetActive(true);
                    male.headNoElements[curEquipHelmetCode].SetActive(false);
                    break;
            
            }
        }
        else
        {
            switch (curEquipHelmetType)
            {
                case 0:
                    allGender.all_Hair[curEquipHelmetType].SetActive(false);
                    break;
                case 1:
                    allGender.headCoverings_No_FacialHair[curEquipHelmetCode].SetActive(false);
                    break;
                case 2:
                    hairParent.SetActive(true);
                    allGender.headCoverings_No_Hair[curEquipHelmetCode].SetActive(false);
                    break;
                case 3:
                    femaleHeadParent.SetActive(true);
                    femaleEyebrowsParent.SetActive(true);
                    female.headNoElements[curEquipHelmetCode].SetActive(false);
                    break;
            
            }
        }
    }

    public void EquipHelmet(Item item)
    {
        ResetHelmet();
        SetHelmet(item);
    }

    private void ResetArmor(Item item)
    {
        int curArmorCode = item.id;
        if (isMale)
        {
            male.torso[curArmorCode].SetActive(false);
            male.arm_Upper_Right[curArmorCode].SetActive(false);
            male.arm_Upper_Left[curArmorCode].SetActive(false);
            male.arm_Lower_Right[curArmorCode].SetActive(false);
            male.arm_Lower_Left[curArmorCode].SetActive(false);
            male.hand_Right[curArmorCode].SetActive(false);
            male.hand_Left[curArmorCode].SetActive(false);
            male.hips[curArmorCode].SetActive(false);
            male.leg_Right[curArmorCode].SetActive(false);
            male.leg_Left[curArmorCode].SetActive(false);
        }
        else
        {
            female.torso[curArmorCode].SetActive(false);
            female.arm_Upper_Right[curArmorCode].SetActive(false);
            female.arm_Upper_Left[curArmorCode].SetActive(false);
            female.arm_Lower_Right[curArmorCode].SetActive(false);
            female.arm_Lower_Left[curArmorCode].SetActive(false);
            female.hand_Right[curArmorCode].SetActive(false);
            female.hand_Left[curArmorCode].SetActive(false);
            female.hips[curArmorCode].SetActive(false);
            female.leg_Right[curArmorCode].SetActive(false);
            female.leg_Left[curArmorCode].SetActive(false);
        }
    }
    
    private void SetArmor(Item item)
    {

        int newArmorCode = item.id;
        if (isMale)
        {
            male.torso[newArmorCode].SetActive(true);
            male.arm_Upper_Right[newArmorCode].SetActive(true);
            male.arm_Upper_Left[newArmorCode].SetActive(true);
            male.arm_Lower_Right[newArmorCode].SetActive(true);
            male.arm_Lower_Left[newArmorCode].SetActive(true);
            male.hand_Right[newArmorCode].SetActive(true);
            male.hand_Left[newArmorCode].SetActive(true);
            male.hips[newArmorCode].SetActive(true);
            male.leg_Right[newArmorCode].SetActive(true);
            male.leg_Left[newArmorCode].SetActive(true);
        }
        else
        {
            female.torso[newArmorCode].SetActive(true);
            female.arm_Upper_Right[newArmorCode].SetActive(true);
            female.arm_Upper_Left[newArmorCode].SetActive(true);
            female.arm_Lower_Right[newArmorCode].SetActive(true);
            female.arm_Lower_Left[newArmorCode].SetActive(true);
            female.hand_Right[newArmorCode].SetActive(true);
            female.hand_Left[newArmorCode].SetActive(true);
            female.hips[newArmorCode].SetActive(true);
            female.leg_Right[newArmorCode].SetActive(true);
            female.leg_Left[newArmorCode].SetActive(true);
        }
    }
    public void EquipArmor(Item item)
    {
        ResetArmor(item);
        SetArmor(item);
    }
    
    // Equip Weapon
    public void EquipWeapon(Item newWeapon)
    {
        if(curEquipWeapon != newWeapon) UnEquipWeapon();

        weaponModel = Instantiate(newWeapon.itemObject, weaponSlot);
        curEquipWeapon = newWeapon;
    }

    public void UnEquipWeapon()
    {
        if(weaponModel != null)
            Destroy(weaponModel);
        curEquipWeapon = null;
    }

    public void EquipShield(Item newShield)
    {
        if (curEquipShield != newShield) UnEquipShield();

        shieldModel = Instantiate(newShield.itemObject, shieldSlot);
        curEquipShield = newShield;
    }

    public void UnEquipShield()
    {
        if(shieldModel != null) 
            Destroy(shieldModel);
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
