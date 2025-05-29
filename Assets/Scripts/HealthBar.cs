using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamagable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure the player has the 'Player' tag assigned.");
            return;
        }
        playerDamagable = player.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {

        healthSlider.value = CalculateSliderPercentage(playerDamagable.Health, playerDamagable.MaxHealth);
        healthBarText.text = "HP " + playerDamagable.Health + " / " + playerDamagable.MaxHealth;
    }

    void OnEnable()
    {
        playerDamagable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamagable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}
