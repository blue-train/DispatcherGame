﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailroadSwitch : MonoBehaviour
{
    public RailroadSegment startSegment;
    public RailroadSegment endSegment1;
    public RailroadSegment endSegment2;
    
    public RailroadSegment ActiveRailroadSegment { get; set; }

    private void Awake()
    {
        ActiveRailroadSegment = endSegment1;
    }

    private void Start()
    {
        if (ActiveRailroadSegment == endSegment1)
        {
            endSegment2.Hide();
        }
        else
        {
            endSegment1.Hide();
        }
    }

    public bool HasThisStartSegment(RailroadSegment segment)
    {
        return startSegment == segment;
    }

    public void SwitchActiveRailroadSegment()
    {
        ActiveRailroadSegment = ActiveRailroadSegment == endSegment1 ? endSegment2 : endSegment1;

        if (ActiveRailroadSegment == endSegment1)
        {
            endSegment1.Show();
            endSegment2.Hide();
        }
        else
        {
            endSegment1.Hide();
            endSegment2.Show();
        }
    }
}
