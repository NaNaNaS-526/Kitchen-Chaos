using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class StoveCounter : BaseCounter, IHasProgressBar
    {
        public event EventHandler<IHasProgressBar.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

        public class OnStateChangedEventArgs : EventArgs
        {
            public State state;
        }

        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned,
        }

        [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
        [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
        private State _currentState;
        private float _fryingTimer;
        private float _burningTimer;
        private FryingRecipeSO _fryingRecipeSO;
        private BurningRecipeSO _burningRecipeSO;

        private void Start()
        {
            _currentState = State.Idle;
        }

        private void Update()
        {
            if (!HasKitchenObject()) return;
            switch (_currentState)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _fryingTimer / _fryingRecipeSO.fryingTimerMax
                    });
                    if (_fryingTimer > _fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        _currentState = State.Fried;
                        _burningTimer = 0.0f;
                        _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = _currentState
                        });
                    }

                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = _burningTimer / _burningRecipeSO.burningTimerMax
                    });
                    if (_burningTimer > _burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                        _currentState = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = _currentState
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = 0f
                        });
                    }

                    break;
                case State.Burned:
                    break;
            }
        }


        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        _currentState = State.Frying;
                        _fryingTimer = 0.0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = _currentState
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                        {
                            ProgressNormalized = _fryingTimer / _fryingRecipeSO.fryingTimerMax
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
                    _currentState = State.Idle;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = _currentState
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgressBar.OnProgressChangedEventArgs
                    {
                        ProgressNormalized = 0f
                    });
                }
            }
        }

        private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
            return fryingRecipeSO;
        }

        private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
        {
            FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
            if (fryingRecipeSO)
            {
                return fryingRecipeSO.output;
            }

            return null;
        }

        private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (var fryingRecipeSO in fryingRecipeSOArray)
            {
                if (fryingRecipeSO.input == inputKitchenObjectSO)
                {
                    return fryingRecipeSO;
                }
            }

            return null;
        }

        private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (var burningRecipeSO in burningRecipeSOArray)
            {
                if (burningRecipeSO.input == inputKitchenObjectSO)
                {
                    return burningRecipeSO;
                }
            }

            return null;
        }
    }
}