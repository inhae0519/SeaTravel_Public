using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStat : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyDownValue;
    private float _currentEnergy;
    private bool isDrained;
    
    [Header("Hunger")]
    [SerializeField] private float maxHunger;
    [SerializeField] private float hungerDownValue;
    private float _currentHunger;
    
    [Header("Thirst")]
    [SerializeField] private float maxThirst;
    [SerializeField] private float thirstDownValue;
    private float _currentThirst;
    
    [Header("Oxygen")]
    [SerializeField] private float maxOxygen;
    [SerializeField] private float oxygenDownValue;
    [SerializeField] private float oxygenUpValue;
    private float _currentOxygen;

    
    [FormerlySerializedAs("addHealthValue")]
    [Header("Health")]
    [SerializeField] private float upHealthValue;
    [SerializeField] private float maxHealthUpInterval;
    [SerializeField] private float downHealthValue;
    [SerializeField] private float maxHealthDownInterval;
    private float _currentHealthUpInterval;
    private float _currentHealthDownInterval;
    
    private Player _player;
    private Health _health;

    private void Start()
    {
        _player = GetComponent<Player>();
        _health = _player.HealthCompo;
        _currentHunger = maxHunger;
        _currentThirst = maxThirst;
        _currentOxygen = maxOxygen;
        _currentEnergy = maxEnergy;
        _currentHealthUpInterval = maxHealthUpInterval;
    }
    
    private void Update()
    {
        DefaultStatDown();
        OxygenProcess();
        HealthDown();
        HealthUp();
    }
    
    private void DefaultStatDown()
    {
        if (_currentHunger >= 0)
        {
            _currentHunger -= Time.deltaTime * hungerDownValue;
            UIManager.Instance.SetPlayerStat(PlayerStatEnum.Hunger,_currentHunger / maxHunger);
        }

        if (_currentThirst >= 0)
        {
            _currentThirst -= Time.deltaTime * thirstDownValue;
            UIManager.Instance.SetPlayerStat(PlayerStatEnum.Thirst,_currentThirst / maxThirst);
        }
    }

    private void HealthDown()
    {
        if (_currentHunger <= 0 || _currentThirst <= 0 || _currentOxygen <= 0)
        {
            _currentHealthDownInterval -= Time.deltaTime;
            if (_currentHealthDownInterval <= 0)
            {
                _health.ApplyDamage(downHealthValue);
                _currentHealthDownInterval = maxHealthDownInterval;
            }
        }
    }

    private void HealthUp()
    {
                
        if (_currentHunger / maxHunger >= 0.8f && 
            _currentThirst / maxThirst >= 0.8f && 
            _health.ReturnCurrentHealth() < 1f &&
            _currentOxygen > 0)
        {
            _currentHealthUpInterval -= Time.deltaTime;
            if (_currentHealthUpInterval <= 0)
            {
                _health.AddHealth(upHealthValue);
                _currentHealthUpInterval = maxHealthUpInterval;
            }
        }
    }

    private void OxygenProcess()
    {
        if (_player.IsWater && _currentOxygen >= 0)
        {
            _currentOxygen -= Time.deltaTime * oxygenDownValue;
            UIManager.Instance.SetPlayerStat(PlayerStatEnum.Oxygen,_currentOxygen / maxOxygen);
        }
        else if (_currentOxygen >= maxOxygen)
        {
            UIManager.Instance.EnableOxygenBar(false);
        }
        else if(!_player.IsWater)
        {
            _currentOxygen += Time.deltaTime * oxygenUpValue;
            UIManager.Instance.SetPlayerStat(PlayerStatEnum.Oxygen,_currentOxygen / maxOxygen);
        }
    }
    
    public bool EnergyDown()
    {
        if (isDrained)
            return false;
        
        if (_currentEnergy <= 0)
        {
            isDrained = true;
            return false;
        } 
        
        _currentEnergy -= Time.deltaTime * energyDownValue;
        UIManager.Instance.SetPlayerStat(PlayerStatEnum.Energy,_currentEnergy / maxEnergy);
        return true;
    }

    public void EnergyUp(bool isWalk)
    {
        float value;
        if (isWalk)
            value = energyDownValue * 0.5f;
        else
            value = energyDownValue;

        if (_currentEnergy <= maxEnergy)
        {
            _currentEnergy += Time.deltaTime * value;
            if (isDrained && _currentEnergy / maxEnergy >= 0.2)
                isDrained = false;
            UIManager.Instance.SetPlayerStat(PlayerStatEnum.Energy,_currentEnergy / maxEnergy);
        }
    }

    public float ReturnCurrentEnergy() => _currentEnergy / maxEnergy;
    
    public void AddCurrentStat(PlayerStatEnum statType, float value)
    {
        switch (statType)
        {
            case PlayerStatEnum.Health:
                _health.AddHealth(value);
                break;
            case PlayerStatEnum.Hunger:
                _currentHunger = Mathf.Clamp(_currentHunger + value, 0, maxHunger);
                break;
            case PlayerStatEnum.Thirst:
                _currentThirst = Mathf.Clamp(_currentThirst + value, 0, maxThirst);
                break;
        }
    }
}
