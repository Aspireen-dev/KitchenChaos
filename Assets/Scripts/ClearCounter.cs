using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private Transform _counterTopPoint;

    public void Interact()
    {
        Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab, _counterTopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;

        Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
    }
}
