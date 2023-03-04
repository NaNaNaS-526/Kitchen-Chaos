using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList = new();
    public string recipeName;
}