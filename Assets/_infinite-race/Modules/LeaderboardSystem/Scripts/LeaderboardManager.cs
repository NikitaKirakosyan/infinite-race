using Southbyte.DIConfiguration;
using UnityEngine;
using YG;

namespace Southbyte.LeaderboardSystem
{
    [RegularInitialization]
    public class LeaderboardManager
    {
        public const string LeaderboardTechName = "ScorePointsLeaderboard";
        
        private float _lastLeaderboardUpdateTime;
        
        
        public LeaderboardManager()
        {
            
        }
        
        ~LeaderboardManager()
        {
            
        }
        
        
        private void SetLeaderboardAndSave()
        {
            if(Time.realtimeSinceStartup - _lastLeaderboardUpdateTime < 10f)
                return;
            
            _lastLeaderboardUpdateTime = Time.realtimeSinceStartup;
            YG2.SetLeaderboard(LeaderboardTechName, 1);
            YG2.GetLeaderboard(LeaderboardTechName);
            YG2.SaveProgress();
        }
    }
}