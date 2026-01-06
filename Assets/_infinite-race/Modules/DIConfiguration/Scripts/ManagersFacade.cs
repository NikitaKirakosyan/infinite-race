using UnityEngine;
using Zenject;

namespace Southbyte.DIConfiguration
{
    // From what I understand, there has to be a facade monobehaviour if we want interfaces like IDisposable or ITickable to work in a sub-container
    // https://github.com/Mathijs-Bakker/Extenject/blob/master/Documentation/SubContainers.md#creating-sub-containers-on-gameobjects-by-using-game-object-context
    public class ManagersFacade : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ManagersFacade> { }
    }
}
