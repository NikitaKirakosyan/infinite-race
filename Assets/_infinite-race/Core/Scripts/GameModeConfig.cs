using NKLogger;
using UnityEngine;

namespace Southbyte
{
    
    [CreateAssetMenu(menuName = EditorMenuNames.RaceSystemRoot + "GameModeConfig")]
    public class GameModeConfig : ScriptableObject
    {
        public GameMode mode;
        
        [Header("Time Attack")]
        public float timeLimit = 60f;
        
        [Header("Scoring")]
        public int scorePerSecond = 1;
        public float scorePerDistance = 1f;

        [Header("Rewards")]
        public float moneyPerDistance = 0.55f;
        public float moneyPerScore = 0.12f;
        public int baseMoneyReward;
        
        
        public static GameModeConfig GetConfig(GameMode mode)
        {
            switch(mode)
            {
                case GameMode.CrashUntil:
                    return Resources.Load<GameModeConfig>("GameModeConfigs/CrashUntilGameMode");
                
                case GameMode.TimeAttack:
                    return Resources.Load<GameModeConfig>("GameModeConfigs/TimeAttackGameMode");
                
                case GameMode.Endless:
                    return Resources.Load<GameModeConfig>("GameModeConfigs/EndlessGameMode");
            }
            
            DebugPro.LogError($"Unexpected game mode {mode}!");
            return null;
        }
    }
}
