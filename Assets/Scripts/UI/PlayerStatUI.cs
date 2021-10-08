using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStatUI : MonoBehaviour
{
    [SerializeField] PlayerStats _playerStats;


    [SerializeField] Slider _energyBar;
    [SerializeField] Image _healthImage;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _starsText;


    private void Start()
    {
        _playerStats.OnEnergyChange += UpdateEnergy;
        _playerStats.OnHealthChange += UpdateHealth;

        GameManager.Instance.OnStarCollected += StarsUpdate;

    }
    private void UpdateEnergy(float amount)
    {
        _energyBar.value = amount;
    }

    private void UpdateHealth(float amount)
    {
        _healthText.text = amount.ToString();
        _healthImage.fillAmount = amount / _playerStats.GetMaxHealth();
    }

    private void StarsUpdate(int amount)
    {
        _starsText.text = "Stars: " + amount.ToString();
    }
}
