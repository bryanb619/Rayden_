using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EquipmentManager>();
            }
            return _instance;
        }
    }

    private static EquipmentManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    #endregion Singleton

    public Equipment[] defaultWear;

    private Equipment[] currentEquipment;
    private SkinnedMeshRenderer[] currentMeshes;

    public SkinnedMeshRenderer targetMesh;

    // Callback for when an item is equipped
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

    public event OnEquipmentChanged onEquipmentChanged;

    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipAllDefault();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    public Equipment GetEquipment(EquipmentSlot slot)
    {
        return currentEquipment[(int)slot];
    }

    // Equip a new item
    public void Equip(Equipment newItem)
    {
        Equipment oldItem = null;

        // Find out what slot the item fits in
        // and put it there.
        int slotIndex = (int)newItem.equipSlot;

        // If there was already an item in the slot
        // make sure to put it back in the inventory
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];

            inventory.Add(oldItem);
        }

        // An item has been equipped so we trigger the callback
        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke(newItem, oldItem);

        currentEquipment[slotIndex] = newItem;
        Debug.Log(newItem.name + " equipped!");

        if (newItem.prefab)
        {
            AttachToMesh(newItem.prefab, slotIndex);
        }
        //equippedItems [itemIndex] = newMesh.gameObject;
    }

    private void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }

            // Equipment has been removed so we trigger the callback
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
    }

    private void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipAllDefault();
    }

    private void EquipAllDefault()
    {
        foreach (Equipment e in defaultWear)
        {
            Equip(e);
        }
    }

    private void AttachToMesh(SkinnedMeshRenderer mesh, int slotIndex)
    {
        if (currentMeshes[slotIndex] != null)
        {
            Destroy(currentMeshes[slotIndex].gameObject);
        }

        SkinnedMeshRenderer newMesh = Instantiate(mesh) as SkinnedMeshRenderer;
        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }
}