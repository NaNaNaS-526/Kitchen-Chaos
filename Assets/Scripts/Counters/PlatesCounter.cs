using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class PlatesCounter : BaseCounter
    {
        public event EventHandler OnPlateSpawned;
        public event EventHandler OnPlateRemoved;
        [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
        private float _spawnPlateTimer;
        private const float SpawnPlateTimerMax = 3.0f;
        private int _platesSpawnedAmount;
        private const int PlatesSpawnedAmountMax = 4;

        private void Update()
        {
            _spawnPlateTimer += Time.deltaTime;
            if (_spawnPlateTimer > SpawnPlateTimerMax)
            {
                _spawnPlateTimer = 0.0f;
                if (_platesSpawnedAmount < PlatesSpawnedAmountMax)
                {
                    _platesSpawnedAmount++;
                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                if (_platesSpawnedAmount > 0)
                {
                    _platesSpawnedAmount--;
                    KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                    OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}