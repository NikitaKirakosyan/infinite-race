using Southbyte.EquipmentSystem;
using UnityEngine;

namespace Southbyte.InventorySystem
{
    public class EquipmentInventoryCell : InventoryCell
    {
        [SerializeField] private EquipmentItemType _equipmentItemType;
        
        public EquipmentItemType EquipmentItemType => _equipmentItemType;
    }
}