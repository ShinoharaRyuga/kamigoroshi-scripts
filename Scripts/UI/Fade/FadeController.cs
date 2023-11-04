using System;
using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    const float FADE_TIME = 1.5f;

    [SerializeField] FadeImage _image;
    IFade _fade;
    private float _cutoutRange;

    private bool _isFade = false;

    public float CutoutRange
    {
        get { return _cutoutRange; }
        set
        {
            _cutoutRange = Mathf.Clamp01(value);
            _fade.Range = _cutoutRange;
        }
    }

    void Awake()
    {
        Init();
        _fade.Range = _cutoutRange;
    }

    void Init()
    {
        _fade = _image.GetComponent<IFade>();
    }

    void OnValidate()
    {
        Init();
        _fade.Range = _cutoutRange;
    }

    public Coroutine FadeOut(Action onComplete = null)
    {
        if (!_isFade)
        {
            _isFade = true;
            StopAllCoroutines();
            AudioManager.Instance.PlaySound(SoundType.SE, "SE_Loading", 1f);
            return StartCoroutine(FadeoutCoroutine(FADE_TIME, () =>
            {
                onComplete?.Invoke();
                _isFade = false;
            }));
        }

        return null;
    }

    public Coroutine FadeIn(Action onComplete = null)
    {
        if (!_isFade)
        {
            _isFade = true;
            StopAllCoroutines();
            return StartCoroutine(FadeinCoroutine(FADE_TIME, () =>
            {
                onComplete?.Invoke();
                _isFade = false;
            }));
        }

        return null;
    }

    IEnumerator FadeoutCoroutine(float time, Action onComplete)
    {
        float startTime = Time.time;
        float endTime = startTime + time;

        var endFrame = new WaitForEndOfFrame();

        while (Time.time <= endTime)
        {
            float elapsedTime = Time.time - startTime;
            _cutoutRange = Mathf.Clamp01(elapsedTime / time);
            _fade.Range = _cutoutRange;
            yield return endFrame;
        }

        _cutoutRange = 1;
        _fade.Range = _cutoutRange;

        onComplete?.Invoke();
    }

    IEnumerator FadeinCoroutine(float time, Action onComplete)
    {
        float startTime = Time.time;
        float endTime = startTime + time;

        var endFrame = new WaitForEndOfFrame();

        while (Time.time <= endTime)
        {
            float elapsedTime = Time.time - startTime;
            _cutoutRange = 1 - Mathf.Clamp01(elapsedTime / time);
            _fade.Range = _cutoutRange;
            yield return endFrame;
        }

        _cutoutRange = 0;
        _fade.Range = _cutoutRange;

        onComplete?.Invoke();
    }
}