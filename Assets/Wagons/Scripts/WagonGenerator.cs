﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonGenerator : MonoBehaviour
{
    public static WagonGenerator Instance { get; private set; }

    public Wagon wagonPrefab;
    public float wagonGeneratorInterval;
    public float wagonSpeed;
    public int totalWagonsCount;

    public event System.Action<WagonType> WagonPrepared;
    public event System.Action WagonInstantiated;

    private int passedWagonsCount;
    private float time = 1.0f;
    private WagonType prevType;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("GenerateWagon", 0f, wagonGeneratorInterval);
    }

    private void GenerateWagon()
    {
        if (passedWagonsCount == totalWagonsCount)
        {
            return;
        }

        var typesCount = System.Enum.GetNames(typeof(WagonType)).Length;
        int typeIndex;
        bool ok;
        do
        {
            typeIndex = Random.Range(0, typesCount); // warning! max int is EXCLUSIVE!
            ok = CheckpointsManager.Instance.UsedWagonTypes.Contains((WagonType)typeIndex) && ((WagonType)typeIndex != prevType);
        } while (!ok);

        WagonPrepared?.Invoke((WagonType)typeIndex);
        prevType = (WagonType)typeIndex;
        StartCoroutine(InstantiateWagon(typeIndex));
    }

    private IEnumerator InstantiateWagon(int typeIndex)
    {
        yield return new WaitForSeconds(time);

        var segment = RailroadManager.Instance.GetFirstRailroadSegment();
        var wagon = Instantiate(wagonPrefab, segment.GetPoint(0), Quaternion.identity, transform);
        wagon.startSegment = segment;
        wagon.Speed = wagonSpeed;
        wagon.SetWagonType(typeIndex);
        passedWagonsCount++;
        WagonInstantiated?.Invoke();
        yield break;
    }
}
