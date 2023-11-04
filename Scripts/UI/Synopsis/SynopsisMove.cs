using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynopsisMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 currentPosition = _rectTransform.anchoredPosition;
        currentPosition.x += _moveSpeed * Time.deltaTime;
        _rectTransform.anchoredPosition = currentPosition;
    }
}