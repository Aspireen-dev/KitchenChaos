using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "ScriptableObjects/KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
