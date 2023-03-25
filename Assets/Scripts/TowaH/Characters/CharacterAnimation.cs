using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const float _rotateSpeed = 5f;
    private const float _radius = 0.15f;
    private float _angle = 0f;
    private Vector2 _offset;
    private Vector2 _center;

    private void Awake() {
        _center = transform.position;
    }
    
    void Update()
    {
        _angle += _rotateSpeed * Time.deltaTime;
        _offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * _radius;
        transform.position = _center + _offset;
    }
}
