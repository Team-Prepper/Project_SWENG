using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;

    private PlayerHealth playerHealth;

    private Image healthImageF;
    private Image healthImageB;
    private Image damagedHealthImage;
    private float damagedHealthFadeTimer;
    private int damagedHealthPreviousHealthAmount;

    private Image lowHealthFlashingImage;
    private float lowHealthAlphaChange;

    private void Awake()
    {
        healthImageF = transform.Find("HealthBar").Find("HealthFront").GetComponent<Image>();
        healthImageB = transform.Find("HealthBar").Find("HealthBack").GetComponent<Image>();
        
        damagedHealthImage = transform.Find("HealthBar").Find("Damaged").GetComponent<Image>();
        lowHealthFlashingImage = transform.Find("HealthBar").Find("Flashing").GetComponent<Image>();
        lowHealthAlphaChange = +4f;
        lowHealthFlashingImage.gameObject.SetActive(false);

        healthImageF.fillAmount = .3f;
        healthImageB.fillAmount = .3f;
    }


    private void Update()
    {
        // Is the damaged health image visible?
        if (damagedHealthImage.color.a > 0)
        {
            // Cound down fade timer
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer < 0)
            {
                // Fade timer over, lower alpha
                Color newColor = damagedHealthImage.color;
                newColor.a -= Time.deltaTime * 3f;
                damagedHealthImage.color = newColor;
            }
        }

        if (lowHealthFlashingImage.gameObject.activeSelf)
        {
            // Flashing health image
            Color lowHealthColor = lowHealthFlashingImage.color;
            lowHealthColor.a += lowHealthAlphaChange * Time.deltaTime;
            if (lowHealthColor.a > 1f)
            {
                lowHealthAlphaChange *= -1f;
                lowHealthColor.a = 1f;
            }
            if (lowHealthColor.a < 0f)
            {
                lowHealthAlphaChange *= -1f;
                lowHealthColor.a = 0f;
            }
            lowHealthFlashingImage.color = lowHealthColor;
        }
    }

    public void SetPlayer(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;

        damagedHealthPreviousHealthAmount = playerHealth.GetHealth();

        playerHealth.OnHealthShieldChanged += PlayerHealth_OnHealthShieldChanged;
    }

    private void PlayerHealth_OnHealthShieldChanged(object sender, System.EventArgs e)
    {
        // Health changed, reset fade timer
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        if (damagedHealthImage.color.a <= 0)
        {
            // Damaged health bar not visible, set size
            damagedHealthImage.fillAmount = (float)damagedHealthPreviousHealthAmount / PlayerHealth.HEALTH_MAX;
        }
        // Make damaged health bar visible
        Color damagedColorFullAlpha = damagedHealthImage.color;
        damagedColorFullAlpha.a = 1f;
        damagedHealthImage.color = damagedColorFullAlpha;

        // Set the previous health amount to the current health amount
        damagedHealthPreviousHealthAmount = playerHealth.GetHealth();

        lowHealthFlashingImage.gameObject.SetActive(playerHealth.GetHealth() <= 30);
    }

    
}
