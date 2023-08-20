using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleTon<CameraManager>
{
    [SerializeField]
    private CinemachineVirtualCamera _vCam = null;
    private CinemachineBasicMultiChannelPerlin _noise = null;

    private Sequence _seq = null;

    private void Awake()
    {
        if (_vCam == null)
        {
            Debug.LogError("VCam ¾øÀ½.");
            return;
        }
        _noise = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ResetCamera()
    {
        _seq?.Kill();
    }

    public void ShakeCamera(float frequency, float amplitude, float animationTime, Ease easeType)
    {
        _seq?.Kill();
        _noise.m_AmplitudeGain = amplitude;
        _noise.m_FrequencyGain = frequency;
        _seq = DOTween.Sequence();
        _seq.Append(DOTween.To(() => amplitude,
            x => _noise.m_AmplitudeGain = x, 0f, animationTime)).SetEase(easeType);
        _seq.Join(DOTween.To(() => frequency,
            x => _noise.m_FrequencyGain = x, 0f, animationTime)).SetEase(easeType);
    }
}
