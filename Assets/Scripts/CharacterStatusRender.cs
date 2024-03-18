using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusRender : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] Image hpBar;
    [SerializeField] TMP_Text hpText;
    [SerializeField] int maxHp;
    [SerializeField] int hp;

    [Header("Stamina")]
    [SerializeField] Slider staminaBar;
    [SerializeField] float maxStamina;
    [SerializeField] float stamina;

    private void OnEnable()
    {
        Manager.Data.OnHpChanged += SetHp;
    }

    private void LateUpdate()
    {
        SetStamina();
    }

    [ContextMenu("SetHp")]
    public void SetHp()
    {
        maxHp = Manager.Data.MaxHp;
        hp = Manager.Data.Hp;
        SetHpBar();
    }

    private void SetHpBar()
    {
        float scale = (float)hp / maxHp;
        hpBar.transform.localScale = new Vector2(scale, scale);

        hpText.text = hp.ToString();
    }

    public void SetStamina()
    {
        maxStamina = Manager.Data.MaxStamina;
        stamina = Manager.Data.Stamina;
        SetStaminaBar();
    }

    private void SetStaminaBar()
    {
        staminaBar.value = stamina / maxStamina;
    }

    private void OnDisable()
    {
        Manager.Data.OnHpChanged -= SetHp;
    }
}
