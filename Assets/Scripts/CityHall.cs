using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityHall : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;

    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private Slider slider;

    void Awake()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (health <= 0)
        {
            Debug.Log("GAME OVER");
            Destroy(this.gameObject);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void ReceiveDamage(float damagePoints)
    {
        Debug.Log(damagePoints + " damage received");
        health -= damagePoints;
    }
}
