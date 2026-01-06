using UnityEngine;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public abstract class ScreenTabBase : MonoBehaviour
    {
        [Inject] protected ScreenManager _screenManager;
    }
}