using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Southbyte
{
    public class MobileInput : MonoBehaviour
    {
        [SerializeField] private EventTrigger _leftButton;
        [SerializeField] private EventTrigger _rightButton;
        [SerializeField] private EventTrigger _brakeButton;
        [SerializeField] private EventTrigger _gasButton;
        
        public bool IsLeftButtonDown { get; private set; }
        public bool IsRightButtonDown { get; private set; }
        public bool IsBrakeButtonDown { get; private set; }
        public bool IsGasButtonDown { get; private set; }
        
        
        private void Awake()
        {
            _leftButton.triggers[0].callback.AddListener(OnLeftButtonDown);
            _leftButton.triggers[1].callback.AddListener(OnLeftButtonUp);
            _rightButton.triggers[0].callback.AddListener(OnRightButtonDown);
            _rightButton.triggers[1].callback.AddListener(OnRightButtonUp);
            _brakeButton.triggers[0].callback.AddListener(OnBrakeButtonDown);
            _brakeButton.triggers[1].callback.AddListener(OnBrakeButtonUp);
            _gasButton.triggers[0].callback.AddListener(OnGasButtonDown);
            _gasButton.triggers[1].callback.AddListener(OnGasButtonUp);
        }
        
        
        private void OnLeftButtonDown(BaseEventData arg0)
        {
            IsLeftButtonDown = true;
        }
        
        private void OnLeftButtonUp(BaseEventData arg0)
        {
            IsLeftButtonDown = false;
        }
        
        private void OnRightButtonDown(BaseEventData arg0)
        {
            IsRightButtonDown = true;
        }
        
        private void OnRightButtonUp(BaseEventData arg0)
        {
            IsRightButtonDown = false;
        }
        
        private void OnBrakeButtonDown(BaseEventData arg0)
        {
            IsBrakeButtonDown = true;
        }
        
        private void OnBrakeButtonUp(BaseEventData arg0)
        {
            IsBrakeButtonDown = false;
        }
        
        private void OnGasButtonDown(BaseEventData arg0)
        {
            IsGasButtonDown = true;
        }
        
        private void OnGasButtonUp(BaseEventData arg0)
        {
            IsGasButtonDown = false;
        }
    }
}