using System;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlaceHere;
        [SerializeField] private Transform counterTopPoint;
        private KitchenObject _kitchenObject;

        public virtual void Interact(Player player)
        {
            //Debug.Log("BaseCounter Interact");
        }

        public virtual void InteractAlternate(Player player)
        {
            //Debug.Log("BaseCounter InteractAlternate");
        }

        public Transform GetKitchenObjectFollowTransform()
        {
            return counterTopPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            _kitchenObject = kitchenObject;
            if (kitchenObject)
            {
                OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
            }
        }

        public KitchenObject GetKitchenObject()
        {
            return _kitchenObject;
        }

        public void ClearKitchenObject()
        {
            _kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return _kitchenObject;
        }
    }
}