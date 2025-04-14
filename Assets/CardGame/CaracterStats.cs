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


    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void TakeDamage(int damage)  //매개변수 데미지 
    {
        currentHealth -= damage;
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
    }
}
