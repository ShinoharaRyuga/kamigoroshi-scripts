using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderView : MonoBehaviour
{
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _seSlider;
    [SerializeField] Slider _voiceSlider;

    private void Start()
    {
        var amgr = AudioManager.Instance;

        if (!amgr) return;

        foreach(var slider in new Slider[3] {_bgmSlider, _seSlider, _voiceSlider})
        {
            slider.maxValue = amgr.VolumeScale;
            slider.wholeNumbers = true;
        }

        _bgmSlider.value = amgr.BGMVolumeScale;
        _seSlider.value = amgr.SEVolumeScale;
        _voiceSlider.value = amgr.VoiceVolumeScale;

        amgr.SetBgmVolume(_bgmSlider.value);
        amgr.SetSeVolume(_seSlider.value);
        amgr.SetVoiceVolume(_voiceSlider.value);

        _bgmSlider.onValueChanged.AddListener(amgr.SetBgmVolume);
        _seSlider.onValueChanged.AddListener(amgr.SetSeVolume);
        _voiceSlider.onValueChanged.AddListener(amgr.SetVoiceVolume);
    }
}
