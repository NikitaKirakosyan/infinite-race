namespace Southbyte.LocalizationSystem
{
    public readonly struct LocPair
    {
        public readonly string key;
        public readonly string substitution;
        
        public bool IsValid => !key.IsNullOrEmptyOrWhiteSpace() && !substitution.IsNullOrEmptyOrWhiteSpace();
        
        
        public LocPair(string key, string substitution)
        {
            this.key = key;
            this.substitution = substitution;
        }
        
        public LocPair(string key, int substitution)
        {
            this.key = key;
            this.substitution = substitution.ToString();
        }
        
        public LocPair(string key, float substitution)
        {
            this.key = key;
            this.substitution = substitution.ToString();
        }
    }
}