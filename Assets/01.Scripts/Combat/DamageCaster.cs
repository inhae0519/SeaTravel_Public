using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 3f)]
    private float _casterRadius = 1f;
    [SerializeField]
    [Range(0, 1f)]
    private float _casterInterPolation = 0.5f;
    [SerializeField]
    [Range(0, 3f)] 
    private float _castingRange = 1f;

    public LayerMask targetLayer;
    private Entity _owner;

    [SerializeField] private AudioClip attack;

    public void InitCaster(Entity agent)
    {
        _owner = agent;
    }

    public bool CastDamage()
    {
        Vector3 startPos = GetStartPos();
        bool isHit = Physics.SphereCast(startPos, _casterRadius, transform.forward, out RaycastHit hit,
            _castingRange, targetLayer);

        if (isHit)
        {
            Debug.Log($"타격 : {hit.collider.name}");
            if (hit.collider.TryGetComponent<Health>(out Health health))
            {
                if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    SoundManager.Instance.PlayEffect(attack);
                }
                int damage = _owner.damage; //주인의 데미지
                float knockBackPower = 3f; //나중에 스탯으로부터 가져와야 해
                health.ApplyDamage(damage);
            }
        }

        return isHit;
    }

    private Vector3 GetStartPos()
    {
        return transform.position + transform.forward * -_casterInterPolation * 2;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetStartPos(), _casterRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetStartPos() + transform.forward * _castingRange, _casterRadius);
        Gizmos.color = Color.white;
    }
}
