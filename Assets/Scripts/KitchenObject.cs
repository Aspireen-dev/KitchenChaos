using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO() => _kitchenObjectSO;
}
