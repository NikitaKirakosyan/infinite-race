namespace Southbyte.StoreSystem
{
    public class PickaxeDiamondStoreItemView : StoreItemViewBase
    {
        protected override void ProcessSuccessPurchase()
        {
            Destroy(gameObject);
        }
    }
}