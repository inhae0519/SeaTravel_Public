using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Chest : Interactble
{
    [SerializeField] Transform enemyParents;
    private bool _isOpen;
    [SerializeField] private float openTime;
    [SerializeField] private GameObject particle;
    private Transform _top;

    private void Awake()
    {
        _top = transform.Find("Top");
    }

    public override void Interact()
    {
        if(enemyParents.childCount > 0)
            return;
        
        if(_isOpen)
            return;

        _isOpen = true;
        particle.SetActive(false);
        _top.transform.DOLocalRotate(new Vector3(-120f, 0f, 0f), openTime).OnComplete(()=>GameManager.Instance.GameClear());
    }
}
