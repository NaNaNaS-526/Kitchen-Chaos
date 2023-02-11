using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private ScriptableKitchenObject kitchenObject;

    [SerializeField] private Transform counterTopPoint;

    public void Interact()
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObject.prefab, counterTopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;
        Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetScriptableKitchenObject());
    }
}