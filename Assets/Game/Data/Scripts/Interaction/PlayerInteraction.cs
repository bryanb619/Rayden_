using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    private const float MAX_INTERACTION_DISTANCE = 1.0f;

    [SerializeField] private CanvasManager _canvasManager;

    private Transform _cameraTransform;
    private Interactive _currentInteractive;
    private bool _playerHasRequirements;
    private List<Interactive> _inventory;


    [SerializeField] private Transform raycastLeft, raycastRight;

    [SerializeField] private UnityEvent PickSound;

    //[SerializeField]
    //private AudioSource Pick_Up;

    private void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;

        _currentInteractive = null;
        _playerHasRequirements = false;
        _inventory = new List<Interactive>();

    }

    private void Update()
    {
        LookForInteractive();
        CheckForPlayerInteraction();
    }

    private void LookForInteractive()
    {
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward,
                            out RaycastHit hitInfo, MAX_INTERACTION_DISTANCE))
        {
            //Debug.DrawLine();
            Interactive interactive = hitInfo.collider.GetComponent<Interactive>();
            
            if (interactive == null || !interactive.IsActive())
                ClearCurrentInteractive();
            else if (interactive != _currentInteractive)
                SetCurrentInteractive(interactive);
        }

        else if (Physics.Raycast(raycastLeft.position, raycastLeft.forward,
                            out RaycastHit hitInfoL, MAX_INTERACTION_DISTANCE))
        {
            Interactive interactive = hitInfoL.collider.GetComponent<Interactive>();

            if (interactive == null || !interactive.IsActive())
                ClearCurrentInteractive();
            else if (interactive != _currentInteractive)
                SetCurrentInteractive(interactive);
        }

        else if (Physics.Raycast(raycastRight.position, raycastRight.forward,
                            out RaycastHit hitInfoR, MAX_INTERACTION_DISTANCE))
        {
            Interactive interactive = hitInfoR.collider.GetComponent<Interactive>();

            if (interactive == null || !interactive.IsActive())
                ClearCurrentInteractive();
            else if (interactive != _currentInteractive)
                SetCurrentInteractive(interactive);
        }
        else
            ClearCurrentInteractive();
    }

    private void ClearCurrentInteractive()
    {
        _currentInteractive = null;
        _canvasManager.HideInteractionPanel();
    }

    private void SetCurrentInteractive(Interactive interactive)
    {
        _currentInteractive = interactive;

        if (PlayerHasInteractionRequirements())
            _canvasManager.ShowInteractionPanel(interactive.GetCurrentInteractionText());
        else
            _canvasManager.ShowInteractionPanel(interactive.GetRequirementText());
    }

    private bool PlayerHasInteractionRequirements()
    {
        _playerHasRequirements = false;

        Interactive[] requirements = _currentInteractive.GetRequirements();

        if (requirements != null)
            for (int i = 0; i < requirements.Length; ++i)
                if (!IsInInventory(requirements[i]))
                    return false;

        _playerHasRequirements = true;
        return true;
    }

    private void CheckForPlayerInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && _currentInteractive != null && _playerHasRequirements)
        {
            if (_currentInteractive.GetInteractiveType() == Interactive.InteractiveType.PICKABLE)
                PickCurrentInteractive();

            if(_currentInteractive.GetInteractiveType() == Interactive.InteractiveType.CYBERFRAGMENT)
            {

                AddArchive();
            }

            else
                InteractWithCurrentInteractive();
        }
    }



    private void PickCurrentInteractive()
    {
        _currentInteractive.Interact();

        //int[] intArray;
        //AddToInventory(_currentInteractive);
        AddArchive();
    }

    private void AddArchive()
    {
        CyberFragments cyber = GetComponent<CyberFragments>();
        _currentInteractive.Interact();
        cyber.ActivateInUI(); 
    }

    private void AddToInventory(Interactive item)
    {
        _inventory.Add(item);
        _canvasManager.SetInventoryIcon(_inventory.Count - 1, item.GetIcon());
        //Pick_Up.Play();
        PickSound.Invoke();
    }

    private void RemoveFromInventory(Interactive item)
    {
        _inventory.Remove(item);
        _canvasManager.ClearInventoryIcons();

        for (int i = 0; i < _inventory.Count; ++i)
            _canvasManager.SetInventoryIcon(i, _inventory[i].GetIcon());
    }


    private void AddToArhives(Interactive item)
    {
        //_inventory.Add(item);
        //_canvasManager.SetInventoryIcon(_inventory.Count - 1, item.GetIcon());
        //Pick_Up.Play();
        PickSound.Invoke();
    }

    private void RemoveFromInventory2(Interactive item)
    {
        _inventory.Remove(item);
        _canvasManager.ClearInventoryIcons();

        RemoveFromInventory(_inventory[5]);
    }

    private bool IsInInventory(Interactive item)
    {
        return _inventory.Contains(item);
    }

    private void InteractWithCurrentInteractive()
    {
        Interactive[] requirements = _currentInteractive.GetRequirements();

        if (requirements != null)
        {
            for (int i = 0; i < requirements.Length; ++i)
            {
                requirements[i].gameObject.SetActive(true);
                RemoveFromInventory(requirements[i]);
            }
        }

        _currentInteractive.Interact();
    }
}