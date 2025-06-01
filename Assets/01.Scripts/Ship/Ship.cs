using System;
using UnityEngine;

public class Ship : Entity
{
    public ShipCam ShipCamCompo { get; private set; }
    [SerializeField] private ShipInputReader shipInput;
    public ShipInputReader ShipInput => shipInput;

    protected override void Awake()
    {
        RigidbodyCompo = GetComponent<Rigidbody>();
        HealthCompo = GetComponent<Health>();
        ShipCamCompo = GetComponentInChildren<ShipCam>();
    }

    public void SetDamage()
    {
        UIManager.Instance.SetShipHealth(HealthCompo.ReturnCurrentHealth());
    }
}
