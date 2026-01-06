using System;
using System.Collections;
using UnityEngine;

namespace Southbyte
{
    public class RandomSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] _targets;
        [SerializeField] private float _delay = 0.1f;
        
        private int _index;
        
        
        private void OnEnable()
        {
            StartCoroutine(SwitchRoutine());
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        
        
        private IEnumerator SwitchRoutine()
        {
            while(true)
            {
                _targets[_index].SetActive(false);
                
                _index++;
                if(_index >= _targets.Length)
                    _index = 0;
                
                _targets[_index].SetActive(true);
                
                yield return new WaitForSeconds(_delay);
            }
        }
    }
}