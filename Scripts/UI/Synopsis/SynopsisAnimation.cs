using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynopsisAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _center;
    [SerializeField] private float _radius = 100f;
    
    private TextMeshProUGUI _synopsisText;
    private Color _color;
    private float _elapsedTime;

    private void Start()
    {
        _synopsisText = GetComponent<TextMeshProUGUI>();
        _color = _synopsisText.color;
    }

    private void Update()
    {
        Fade();
    }

   private void Fade()
    {
        _elapsedTime += Time.deltaTime;

        float distance = Vector3.Distance(_center.position, transform.position);

        float alpha = Mathf.Clamp01(1 - distance / _radius);

        _color.a = alpha;
        _synopsisText.color = _color;
    }
}