using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private Entity _entity;
    
    public ActionData actionData;

    public UnityEvent OnHitEvent;
    public UnityEvent OnDeadEvent;
    
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _entity = GetComponent<Entity>();
    }
    
    public void Initialize(Entity agent)
    {
        _entity = agent;
        actionData = new ActionData();
        _currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        if(_entity.isDead) return;

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        OnHitEvent?.Invoke();

        if (_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
            _entity.isDead = true;
        }
    }
    public void AddHealth(float value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, maxHealth);
        UIManager.Instance.SetPlayerStat(PlayerStatEnum.Health, _currentHealth/maxHealth);
    }

    public float ReturnCurrentHealth()
    {
        return _currentHealth / maxHealth;
    }
}
