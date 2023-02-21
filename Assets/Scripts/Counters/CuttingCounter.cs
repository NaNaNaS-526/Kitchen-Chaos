using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgressBar
    {
        public event EventHandler<IHasProgressBar.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler OnCut;

        [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
        private int _cuttingProgress;

        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        _cuttingProgress = 0;
                        CuttingRecipeSO cuttingRecipeSO =
                            getCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                        });
                        return;
                    }
                }
            }

            if (HasKitchenObject())
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
            {
                _cuttingProgress++;
                OnCut?.Invoke(this, EventArgs.Empty);
                CuttingRecipeSO cuttingRecipeSO = getCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                {
                    ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
                if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                {
                    KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                }
            }
        }

        private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            CuttingRecipeSO cuttingRecipeSO = getCuttingRecipeSOWithInput(inputKitchenObjectSO);
            return cuttingRecipeSO;
        }

        private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
        {
            CuttingRecipeSO cuttingRecipeSO = getCuttingRecipeSOWithInput(inputKitchenObjectSO);
            if (cuttingRecipeSO)
            {
                return cuttingRecipeSO.output;
            }

            return null;
        }

        private CuttingRecipeSO getCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (var cuttingRecipeSO in cuttingRecipeSOArray)
            {
                if (cuttingRecipeSO.input == inputKitchenObjectSO)
                {
                    return cuttingRecipeSO;
                }
            }

            return null;
        }
    }
}