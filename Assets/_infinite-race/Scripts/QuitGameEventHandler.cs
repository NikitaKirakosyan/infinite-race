using UnityEngine;
using YG;

namespace Southbyte
{
    public class QuitGameEventHandler : MonoBehaviour
    {
        public void SaveOnQuit()
        {
            YG2.SaveProgress();
        }
    }
}