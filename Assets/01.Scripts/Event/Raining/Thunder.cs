using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject fire;
    [SerializeField] private AudioClip AudioClip;

    private void OnEnable()
    {
        SoundManager.Instance.PlayEffect(AudioClip);
        bool isHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit,100, ground);
        
        if (isHit)
        {
            GameObject fireObj = Instantiate(fire, ShipManager.Instance.ShipTrm);
            fireObj.transform.position = hit.point;
        }
    }
}
