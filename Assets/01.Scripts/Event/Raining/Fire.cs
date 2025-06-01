using System;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject _fogEffect;
    [SerializeField] private int damage;
    [SerializeField] private float _tickTime;
    [SerializeField] private AudioClip AudioClip;
    private float _defaultTickTime;

    private void Awake()
    {
        _defaultTickTime = _tickTime;
        _tickTime = 0;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("FireShip"))
        {
            _tickTime -= Time.deltaTime;
            if (_tickTime < 0)
            {
                Health health = other.transform.root.GetComponent<Health>();
                health.ApplyDamage(damage);
                _tickTime = _defaultTickTime;
            }
        }
    }

    public void Extinction()
    {
        Vector3 pos = transform.position;
        pos.y += 1;
        Instantiate(_fogEffect, pos, Quaternion.identity);
        Destroy(gameObject);
    }
}
