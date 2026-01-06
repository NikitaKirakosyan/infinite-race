using System;
using System.Collections.Generic;
using Southbyte.InventorySystem;
using Southbyte.UIModules;
using UnityEngine;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private TabToggle<GameObject> _clothTabToggle;
        [SerializeField] private TabToggle<GameObject> _weaponTabToggle;
        [SerializeField] private TabToggle<GameObject> _medicineTabToggle;
        [SerializeField] private TabToggle<GameObject> _ammoTabToggle;
        [SerializeField] private InventoryCell _inventoryCellPrefab;
        
        [Inject] private InventoryManager _inventoryManager;
        [Inject] private IInstantiator _instantiator;
        
        private readonly Dictionary<InventoryItemType, List<InventoryCell>> _cellsByType = new ();
        
        
        private void Awake()
        {
            _clothTabToggle.Toggle.onValueChanged.AddListener(OnTabToggleValueChanged);
            _weaponTabToggle.Toggle.onValueChanged.AddListener(OnTabToggleValueChanged);
            _medicineTabToggle.Toggle.onValueChanged.AddListener(OnTabToggleValueChanged);
            _ammoTabToggle.Toggle.onValueChanged.AddListener(OnTabToggleValueChanged);
        }
        
        private void OnEnable()
        {
            _inventoryManager.OnPlayerInventoryChanged += RefreshCells;
            
            _clothTabToggle.Toggle.gameObject.SetActive(false);
            _weaponTabToggle.Toggle.gameObject.SetActive(false);
            _medicineTabToggle.Toggle.gameObject.SetActive(false);
            _ammoTabToggle.Toggle.gameObject.SetActive(false);
            
            _clothTabToggle.Tab.SetActive(false);
            _weaponTabToggle.Tab.SetActive(false);
            _medicineTabToggle.Tab.SetActive(false);
            _ammoTabToggle.Tab.SetActive(false);
            
            SetupCells(inventoryItem => SetupTabTogglesOnEnable(inventoryItem.itemType));
            AutoEnableRelativeToggles();
        }
        
        private void OnDisable()
        {
            _inventoryManager.OnPlayerInventoryChanged -= RefreshCells;
        }
        
        
        private void SetupTabTogglesOnEnable(InventoryItemType inventoryItemType)
        {
            switch(inventoryItemType)
            {
                case InventoryItemType.Cloth:
                    _clothTabToggle.Toggle.gameObject.SetActive(true);
                    return;
                
                case InventoryItemType.Weapon:
                    _weaponTabToggle.Toggle.gameObject.SetActive(true);
                    return;
                
                case InventoryItemType.Medicine:
                    _medicineTabToggle.Toggle.gameObject.SetActive(true);
                    return;
                
                case InventoryItemType.Ammo:
                    _ammoTabToggle.Toggle.gameObject.SetActive(true);
                    return;
            }
        }
        
        private void SetupCells(Action<InventoryItem> preProcess = null)
        {
            var indexes = new Dictionary<InventoryItemType, int>();
            var inventoryItems = _inventoryManager.GetAllPlayerInventoryStoredItems();
            foreach(var inventoryItem in inventoryItems)
            {
                preProcess?.Invoke(inventoryItem);
                
                var type = inventoryItem.itemType;
                indexes.TryAdd(type, 0);
                _cellsByType.TryAdd(type, new List<InventoryCell>());
                
                if(indexes[type] >= _cellsByType[type].Count)
                {
                    TryGetContentParentByInventoryItemType(inventoryItem.itemType, out var contentParent);
                    var inventoryCell = _instantiator.InstantiatePrefabForComponent<InventoryCell>(_inventoryCellPrefab,  contentParent.transform);
                    _cellsByType[type].Add(inventoryCell);
                }
                
                _cellsByType[type][indexes[type]].Setup(inventoryItem);
                indexes[type]++;
            }
            
            foreach(InventoryItemType inventoryItemType in Enum.GetValues(typeof(InventoryItemType)))
            {
                if(indexes.TryGetValue(inventoryItemType, out var index) && index < _cellsByType[inventoryItemType].Count - 1)
                {
                    _cellsByType[inventoryItemType][index].Setup(null);
                    indexes[inventoryItemType]++;
                }
            }
        }
        
        private void AutoEnableRelativeToggles()
        {
            if(_clothTabToggle.Toggle.gameObject.activeSelf)
                _clothTabToggle.IsOn = true;
            else if(_weaponTabToggle.Toggle.gameObject.activeSelf)
                _weaponTabToggle.IsOn = true;
            else if(_medicineTabToggle.Toggle.gameObject.activeSelf)
                _medicineTabToggle.IsOn = true;
            else if(_ammoTabToggle.Toggle.gameObject.activeSelf)
                _ammoTabToggle.IsOn = true;
        }
        
        private void RefreshTabs()
        {
            _clothTabToggle.Tab.SetActive(_clothTabToggle.IsOn);
            _weaponTabToggle.Tab.SetActive(_weaponTabToggle.IsOn);
            _medicineTabToggle.Tab.SetActive(_medicineTabToggle.IsOn);
            _ammoTabToggle.Tab.SetActive(_ammoTabToggle.IsOn);
        }
        
        private void RefreshCells()
        {
            SetupCells();
        }
        
        private bool TryGetContentParentByInventoryItemType(InventoryItemType inventoryItemType, out GameObject contentParent)
        {
            contentParent = null;
            
            switch(inventoryItemType)
            {
                case InventoryItemType.Cloth:
                    contentParent = _clothTabToggle.Tab;
                    return true;
                
                case InventoryItemType.Weapon:
                    contentParent = _weaponTabToggle.Tab;
                    return true;
                
                case InventoryItemType.Medicine:
                    contentParent = _medicineTabToggle.Tab;
                    return true;
                
                case InventoryItemType.Ammo:
                    contentParent = _ammoTabToggle.Tab;
                    return true;
            }
            
            return false;
        }
        
        private void OnTabToggleValueChanged(bool isOn)
        {
            if(isOn)
                RefreshTabs();
        }
    }
}