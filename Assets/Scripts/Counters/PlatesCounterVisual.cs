using System;
using System.Collections.Generic;
using UnityEngine;

namespace Counters
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform counterTopPoint;
        [SerializeField] private Transform plateVisualPrefab;
        private List<GameObject> _plateVisualGameObjectList;

        private void Awake()
        {
            _plateVisualGameObjectList = new List<GameObject>();
        }

        private void Start()
        {
            platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
            platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
        }

        private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
        {
            GameObject plateGameObject = _plateVisualGameObjectList[^1];
            _plateVisualGameObjectList.Remove(plateGameObject);
            Destroy(plateGameObject);
        }

        private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
        {
            Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
            float plateOffsetY = 0.1f;
            plateVisualTransform.localPosition =
                new Vector3(0.0f, plateOffsetY * _plateVisualGameObjectList.Count, 0.0f);
            _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
        }
    }
}