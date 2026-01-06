using TMPro;
using UnityEngine;
using YG;
using YG.Utils.LB;

namespace Southbyte.LeaderboardSystem
{
    public class LeaderboardRankView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rankText;
        
        
        private void OnDestroy()
        {
            YG2.onGetLeaderboard -= Refresh;
        }
        
        private void Awake()
        {
            YG2.onGetLeaderboard += Refresh;
            YG2.GetLeaderboard(LeaderboardManager.LeaderboardTechName);
        }
        
        
        private void Refresh(LBData lbData)
        {
            if(lbData.currentPlayer == null)
                return;
            
            _rankText.text = lbData.currentPlayer.rank.ToString();
        }
    }
}