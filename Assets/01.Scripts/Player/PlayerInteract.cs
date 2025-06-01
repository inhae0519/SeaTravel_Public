using System;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float distance;

    private Player _player;
    private RaycastHit _hit;
    private Interactble _currentInteract;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        _player.PlayerInput.InteractEvent += HandleInteract;
    }

    private void OnDisable()
    {
        _player.PlayerInput.InteractEvent -= HandleInteract;
        promptText.text = null;
    }
    
    private void HandleInteract()
    {
        if(_hit.collider == null)
            return;
        
        _currentInteract.Interact();
    }


    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, distance, interactableLayer))
        {
            if (_hit.collider.TryGetComponent(out _currentInteract))
            {
                promptText.text = _currentInteract.prompt;
            }
        }
        else
        {
            promptText.text = null;
        }
    }
}
