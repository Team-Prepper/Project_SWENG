using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Character;

public class GUI_PlayerHealth : MonoBehaviour
{
    PlayerController _target;

    [SerializeField] Image _healthFront;
    [SerializeField] Image _healthBack;
    [SerializeField] Image _afterimage;
    [SerializeField] TextMeshProUGUI _hpText;
    
    public float lerpSpeed = 100f;

    float _curValue;

    private void Awake()
    {
        PlayerController.EventChangeHp += GUI_ChangeHealth;
        PlayerController.EventEquip += GUI_SetMaxHealth;
    }

    public void SetPlayerHealth(GameObject player)
    {
        _target = player.GetComponent<PlayerController>();
        SetHealth();
        _afterimage.fillAmount = 1;
        _curValue = _target.stat.HP.Value;
    }

    void GUI_ChangeHealth(object sender, IntEventArgs e)
    {
        SetHealth();
        StartCoroutine(LerpValue(e.Value));
    }
    
    void GUI_SetMaxHealth(object sender, EventArgs e)
    {
        SetHealth();
    }

    private IEnumerator LerpValue(float endValue)
    {
        float elapsedTime = 0f;
        float startValue = _curValue;
        while (elapsedTime < 1f)
        {
            _curValue = Mathf.Lerp(startValue, endValue, elapsedTime);
            _afterimage.fillAmount = _target.stat.HP.ConvertToRate();
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }

    void SetHealth()
    {
        _healthFront.fillAmount = _target.stat.HP.ConvertToRate();
        _healthBack.fillAmount  = _target.stat.HP.ConvertToRate();
        _hpText.text = _target.stat.HP.Value.ToString() + " / " + _target.stat.HP.MaxValue;
    }
}
