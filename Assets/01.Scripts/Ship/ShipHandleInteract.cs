using UnityEngine;

public class ShipHandleInteract : Interactble
{
    public override void Interact()
    {
        GameManager.Instance.isShip = true;
        PlayerManager.Instance.PlayerInputOnOff(false);
        ShipManager.Instance.ShipInputOnOff(true);
        CameraManager.Instance.ChangeCamera();
    }
}
