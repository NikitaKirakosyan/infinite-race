using Southbyte.ScreensSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.InventorySystem
{
    public class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private static Image DragImage;
        
        [SerializeField] private Image _itemImage;
        
        [Inject] private InventoryManager _inventoryManager;
        
        public InventoryItem Item { get; private set; }
        
        
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if(DragImage == null)
            {
                DragImage = Instantiate(_itemImage, transform.root, true);
                DragImage.raycastTarget = false;
            }
            
            DragImage.sprite = _itemImage.sprite;
            DragImage.gameObject.SetActive(true);
        }
        
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            DragImage.transform.position = Input.mousePosition;
        }
        
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            DragImage.gameObject.SetActive(false);
            
            var pointer = eventData.pointerEnter;
            if(pointer == null)
                return;
            
            if(pointer.TryGetComponent(out InventoryCell anotherCell))
            {
                var storedItem = Item;
                var receivedItem = anotherCell.Item;
                
                anotherCell.Setup(storedItem);
                Setup(receivedItem);
                
                if(anotherCell is EquipmentInventoryCell)
                {
                    if(receivedItem != null)
                        _inventoryManager.PutInPlayerInventory(receivedItem.id);
                    
                    if(storedItem != null)
                        _inventoryManager.PullFromPlayerInventory(storedItem.id);
                }
                
                return;
            }
            
            var itemId = Item.id;
            Setup(null);
            _inventoryManager.PutInPlayerInventory(itemId);
        }
        
        
        public void Setup(InventoryItem item)
        {
            Item = item;
            
            if(Item == null)
            {
                _itemImage.color = Color.clear;
            }
            else
            {
                _itemImage.color = Color.white;
                _itemImage.sprite = Item.iconSprite;
            }
        }
    }
}