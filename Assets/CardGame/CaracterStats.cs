using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CaracterStats : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;

    //UI요소
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    //새로 추가 되는 변수
    public int maxMana = 10;
    public int currentMana;
    public Slider manaBar;
    public TextMeshProUGUI manaText;


    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
        UpdateUI();
    }

   public void TakeDamage(int damage)  //매개변수 데미지 
    {
        currentHealth -= damage;
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
    }

    public void UseMana(int amount)
    {
        currentMana -= amount;
        if (currentMana < 0)
        {
            currentMana = 0;
        }
        UpdateUI();
    }


    public void GainMana(int amount)
    {
        currentMana += amount;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        if(healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }

        if(healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        if(manaBar != null)
        {
            manaBar.value = (float)currentMana / maxMana;
        }    

        if(manaText != null)
        {
            manaText.text = $" {currentMana} / {maxMana}";
        }
    }
}
