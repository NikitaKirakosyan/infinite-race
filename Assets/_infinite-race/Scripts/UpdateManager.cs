using System.Collections.Generic;
using UnityEngine;

namespace Southbyte
{
    public class UpdateManager : MonoBehaviour
    {
        private readonly List<IUpdatable> _updatables = new (100);
        
        
        private void Update()
        {
            foreach(var updatable in _updatables)
            {
                updatable.Tick();
            }
        }
        
        
        public void Register(IUpdatable u)
        {
            _updatables.Add(u);
        }
        
        public void Unregister(IUpdatable u)
        {
            var idx = _updatables.IndexOf(u);
            if (idx < 0)
                return;
            
            var last = _updatables.Count - 1;
            if (idx != last)
                _updatables[idx] = _updatables[last];
            
            _updatables.RemoveAt(last);
        }
    }
}