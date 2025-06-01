using UnityEngine;

public class ShipManager : MonoSingleton<ShipManager>
{
    private Transform _shipTrm = null;
    public Transform ShipTrm
    {
        get
        {
            if (_shipTrm == null)
            {
                _shipTrm = GameObject.FindGameObjectWithTag("ShipRoot").transform;
            }
            return _shipTrm;
        }
    }
    
    
    private Ship _ship;
    public Ship Ship
    {
        get
        {
            if (_ship == null)
            {
                _ship = ShipTrm.GetComponent<Ship>();
            }

            return _ship;
        }
    }
    
    public void ShipInputOnOff(bool isTrue)
    {
        Ship.ShipInput.OnOffInput(isTrue);
    }
}
