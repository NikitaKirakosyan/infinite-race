namespace Southbyte.StoreSystem
{
    public class AdBlockerStoreItemView : StoreItemViewBase
    {
        protected override void ProcessSuccessPurchase()
        {
            Destroy(gameObject);
        }
    }
}