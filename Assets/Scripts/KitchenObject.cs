using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private ScriptableKitchenObject scriptableKitchenObject;

    public ScriptableKitchenObject GetScriptableKitchenObject()
    {
        return scriptableKitchenObject;
    }
}