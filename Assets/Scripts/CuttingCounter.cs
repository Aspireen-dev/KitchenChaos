using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _cutKitchenObjectSO;

    public override void Interact(Player player)
    {
        // If player has no kitchen object, and counter has a kitchen object, give it to the player
        if (HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        // If player has a kitchen object, and counter has no kitchen object, give it to the counter
        else
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        // If counter has a kitchen object, cut it
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(_cutKitchenObjectSO, this);
        }
    }
}
