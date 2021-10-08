using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] float _health = 100f;
    [SerializeField] float _maxHealth = 100f;

    [SerializeField] float _energy = 100f;
    [SerializeField] float _maxEnergy = 100f;
    public bool hasEnergy = true;
    bool _recharging = false;
    bool _energyInUse = false;

    [SerializeField] ParticleSystem _playerDamagedEffect;
    [SerializeField] AudioClip _playerDamageSound;
    

    public Action<float> OnHealthChange;
    public Action<float> OnEnergyChange;

    public Action OnPlayerDead;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        OnPlayerDead += GameManager.Instance.GameOver;
    }

    public void Damage(float amount)
    {
        _health -= amount;
        _health = Mathf.Clamp(_health, 0f, _maxHealth);

        OnHealthChange?.Invoke(_health);

        if(_health == 0f)
        {
            OnPlayerDead?.Invoke();
        }
        _playerDamagedEffect.Play();

        SFXPlayer.Instance.PlaySoundEffect(transform.position, _playerDamageSound);
    }

    public void Heal(float amount)
    {
        _health += amount;
        _health = Mathf.Clamp(_health, 0f, _maxHealth);

        OnHealthChange?.Invoke(_health);

    }

    public void DrainEnergy(float amount)
    {
        if (_energy - amount <= 0f)
        {
            hasEnergy = false;
        }
        
        _energy -= amount;
        _energy = Mathf.Clamp(_energy, 0f, _maxEnergy);

        OnEnergyChange?.Invoke(_energy);
    }

    public void RechargeEnergy(float amount)
    {
        _energy += amount;
        _energy = Mathf.Clamp(_energy, 0f, _maxEnergy);

        if(_energy > 0.5f)
        {
            hasEnergy = true;
        }

        OnEnergyChange?.Invoke(_energy);
    }




    public float GetMaxHealth()
    {
        return _maxHealth;
    }
}
