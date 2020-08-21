﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public float Speed { get; set; } // Задается в WagonGenerator

    private const float distanceDiff = 0.04f;
    private float distance;
    private WagonType wagonType;
    private RailroadSegment startingSegment;
    private RailroadSegment currentSegment;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (CheckpointsManager.Instance.Checkpoints.ContainsKey(collision.gameObject))
        //{
        //    var checkpoint = CheckpointsManager.Instance.Checkpoints[collision.gameObject];
            
        //    if (checkpoint.WType != wagonType)
        //    {
        //        ProgressManager.Instance.WrongWagons++;
        //    }
        //}

        if (collision.CompareTag("Finish"))
        {
            var checkpoint = collision.GetComponentInParent<Checkpoint>();

            if (checkpoint.WType != wagonType)
            {
                ProgressManager.Instance.WrongWagons++;
            }
        }

        if (collision.CompareTag("WagonDeleter"))
        {
            Stop();
        }
    }

    private void Move()
    {
        if (Mathf.Abs(distance - currentSegment.Length) < distanceDiff)
        {
            currentSegment = currentSegment.GetNextRailroadSegment();
            distance = 0;

            if (currentSegment == null)
            {
                currentSegment = startingSegment;
            }
        }

        //transform.position = currentSegment.GetPointAtDistance(distance);
        var currentPos = transform.position;
        Debug.Log($"my prev pos is {currentPos}");
        rb.MovePosition(currentSegment.GetPointAtDistance(distance));
        Debug.Log($"my last pos is {transform.position}");

        var rot = currentSegment.GetRotationAtDistance(distance);
        //transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y + 90, rot.eulerAngles.x + 90);
        rb.MoveRotation(Quaternion.Euler(0, rot.eulerAngles.y + 90, rot.eulerAngles.x + 90));

        distance += Time.deltaTime * Speed;
    }

    private void Stop()
    {
        Destroy(gameObject);
    }

    public void SetType(WagonType type, Sprite sprite)
    {
        wagonType = type;
        sr.sprite = sprite;
    }

    public void SetStartingSegment(RailroadSegment segment)
    {
        currentSegment = startingSegment = segment;
    }
}

public enum WagonType
{
    Red, Green, Blue, Purple
}
