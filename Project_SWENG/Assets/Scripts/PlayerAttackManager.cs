using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackManager : MonoBehaviour
{
    Animator animator;

    //ATTACK VARIABLES
    [SerializeField] string attackLastPerformed;
    [SerializeField] bool lightAttackInput;
    [SerializeField] bool heavyAttackInput;
    [SerializeField] bool chargeAttackInput;

    [Header("Weapon")]
    public ParticleSystem weaponFX;
    public ParticleSystem weaponChargeFX;

    string oh_Light_Attack_01 = "OH_Light_Attack_01";
    string oh_Light_Attack_02 = "OH_Light_Attack_02";
    string oh_Heavy_Attack_01 = "OH_Heavy_Attack_01";
    string oh_Heavy_Attack_02 = "OH_Heavy_Attack_02";

    string th_Light_Attack_01 = "TH_Light_Attack_01";
    string th_Light_Attack_02 = "TH_Light_Attack_02";
    string th_Heavy_Attack_01 = "TH_Heavy_Attack_01";
    string th_Heavy_Attack_02 = "TH_Heavy_Attack_02";

    string oh_Charge_Attack_01 = "OH_Charge_Attack_01_Wind_Up";
    string oh_Charge_Attack_02 = "OH_Charge_Attack_02_Wind_Up";
    string th_Charge_Attack_01 = "TH_Charge_Attack_01_Wind_Up";
    string th_Charge_Attack_02 = "TH_Charge_Attack_02_Wind_Up";

    [SerializeField] bool isTwoHandingWeapon;
    [SerializeField] bool isPerformingAction;

    [SerializeField] bool isSprinting;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        HandleLightAttackCombo();
        HandleHeavyAttackCombo();
        HandleChargeAttackCombo();
       
        HandleTwoHand();
        HandleLightAttack();
        HandleHeavyAttack();
        HandleChargeAttack();

    }

    private void HandleTwoHand()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            isTwoHandingWeapon = !isTwoHandingWeapon;
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        }
    }

    private void HandleLightAttack()
    {
        if (isPerformingAction)
            return;

        if (PlayerInputBase.Instance.inputML)
        {
            lightAttackInput = true;
            weaponFX.Stop();
            weaponFX.Play();
        }

        if (lightAttackInput)
        {
            lightAttackInput = false;

            if (isTwoHandingWeapon)
            {
                if (isSprinting)
                {
                    PlayActionAnimation("TH_Running_Attack_01", true);
                    return;
                }

                PlayActionAnimation("TH_Light_Attack_01", true);
                attackLastPerformed = th_Light_Attack_01;
            }
            else
            {
                if (isSprinting)
                {
                    PlayActionAnimation("OH_Running_Attack_01", true);
                    return;
                }

                PlayActionAnimation("OH_Light_Attack_01", true);
                attackLastPerformed = oh_Light_Attack_01;
            }
        }
    }

    private void HandleLightAttackCombo()
    {
        if (isPerformingAction)
        {
            if (attackLastPerformed == oh_Light_Attack_01)
            {
                if (PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("OH_Light_Attack_02", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = oh_Light_Attack_02;
                    return;
                }
            }
            else if (attackLastPerformed == oh_Light_Attack_02)
            {
                if (PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("OH_Light_Attack_01", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = oh_Light_Attack_01;
                }
            }
            else if (attackLastPerformed == th_Light_Attack_01)
            {
                if (PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("TH_Light_Attack_02", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = th_Light_Attack_02;
                }
            }
            else if (attackLastPerformed == th_Light_Attack_02)
            {
                if (PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("TH_Light_Attack_01", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = th_Light_Attack_01;
                }
            }
        }
    }

    private void HandleHeavyAttack()
    {
        if (isPerformingAction)
            return;

        if (PlayerInputBase.Instance.inputShift && PlayerInputBase.Instance.inputML)
        {
            heavyAttackInput = true;
        }

        if (heavyAttackInput)
        {

            heavyAttackInput = false;

            if (isTwoHandingWeapon)
            {
                if (isSprinting)
                {
                    PlayActionAnimation("TH_Jumping_Attack_01", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    return;
                }

                PlayActionAnimation("TH_Heavy_Attack_01", true);
                weaponFX.Stop();
                weaponFX.Play();
                attackLastPerformed = th_Heavy_Attack_01;
            }
            else
            {
                if (isSprinting)
                {
                    PlayActionAnimation("OH_Jumping_Attack_01", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    return;
                }

                PlayActionAnimation("OH_Heavy_Attack_01", true);
                weaponFX.Stop();
                weaponFX.Play();
                attackLastPerformed = oh_Heavy_Attack_01;
            }
        }
    }

    private void HandleHeavyAttackCombo()
    {
        if (isPerformingAction)
        {
            if (attackLastPerformed == oh_Heavy_Attack_01)
            {
                if (PlayerInputBase.Instance.inputShift && PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("OH_Heavy_Attack_02", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = oh_Heavy_Attack_02;
                }
            }
            else if (attackLastPerformed == oh_Heavy_Attack_02)
            {
                if (PlayerInputBase.Instance.inputShift && PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("OH_Heavy_Attack_01", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = oh_Heavy_Attack_01;
                }
            }
            else if (attackLastPerformed == th_Heavy_Attack_01)
            {
                if (PlayerInputBase.Instance.inputShift && PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("TH_Heavy_Attack_02", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = th_Heavy_Attack_02;
                }
            }
            else if (attackLastPerformed == th_Heavy_Attack_02)
            {
                if (PlayerInputBase.Instance.inputShift && PlayerInputBase.Instance.inputML)
                {
                    PlayActionAnimation("TH_Heavy_Attack_01", true);
                    weaponFX.Stop();
                    weaponFX.Play();
                    attackLastPerformed = th_Heavy_Attack_01;
                }
            }
        }
    }

    private void HandleChargeAttack()
    {
        if (PlayerInputBase.Instance.inputC)
        {
            chargeAttackInput = true;
        }

        if (isPerformingAction)
            return;

        if (chargeAttackInput)
        {
            chargeAttackInput = false;

            if (isTwoHandingWeapon)
            {
                PlayActionAnimation("TH_Charge_Attack_01_Wind_Up", true);
                weaponChargeFX.Stop();
                weaponChargeFX.Play();
                attackLastPerformed = th_Charge_Attack_01;
            }
            else
            {
                PlayActionAnimation("OH_Charge_Attack_01_Wind_Up", true);
                weaponChargeFX.Stop();
                weaponChargeFX.Play();
                attackLastPerformed = oh_Charge_Attack_01;
            }
        }
    }

    private void HandleChargeAttackCombo()
    {
        if (isPerformingAction)
        {
            if (PlayerInputBase.Instance.inputC)
            {
                chargeAttackInput = true;
            }

            if (chargeAttackInput)
            {
                chargeAttackInput = false;

                if (attackLastPerformed == oh_Charge_Attack_01)
                {
                    if (PlayerInputBase.Instance.inputC)
                    {
                        PlayActionAnimation("OH_Charge_Attack_02_Wind_Up", true);
                        weaponChargeFX.Stop();
                        weaponChargeFX.Play();
                        attackLastPerformed = oh_Charge_Attack_02;
                    }
                }
                else if (attackLastPerformed == oh_Charge_Attack_02)
                {
                    if (PlayerInputBase.Instance.inputC)
                    {
                        PlayActionAnimation("OH_Charge_Attack_01_Wind_Up", true);
                        weaponChargeFX.Stop();
                        weaponChargeFX.Play();
                        attackLastPerformed = oh_Charge_Attack_01;
                    }
                }
                else if (attackLastPerformed == th_Charge_Attack_01)
                {
                    if (PlayerInputBase.Instance.inputC)
                    {
                        PlayActionAnimation("TH_Charge_Attack_02_Wind_Up", true);
                        weaponChargeFX.Stop();
                        weaponChargeFX.Play();
                        attackLastPerformed = th_Charge_Attack_02;
                    }
                }
                else if (attackLastPerformed == th_Charge_Attack_02)
                {
                    if (PlayerInputBase.Instance.inputC)
                    {
                        PlayActionAnimation("TH_Charge_Attack_01_Wind_Up", true);
                        weaponChargeFX.Stop();
                        weaponChargeFX.Play();
                        attackLastPerformed = th_Charge_Attack_01;
                    }
                }
            }
        }
    }

    private void PlayActionAnimation(string animation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.CrossFade(animation, 0.2f);
    }
}
