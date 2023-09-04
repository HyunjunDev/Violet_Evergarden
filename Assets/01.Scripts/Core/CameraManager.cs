using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleTon<CameraManager>
{
    private Sequence _seq = null;

    public void ResetCamera()
    {
        _seq?.Kill();
    }

    public void ShakeCamera(ShakeCameraDataSO data)
    {
        CinemachineVirtualCamera vCam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _seq?.Kill();
        noise.m_AmplitudeGain = data.amplitude;
        noise.m_FrequencyGain = data.frequency;
        _seq = DOTween.Sequence();
        _seq.Append(DOTween.To(() => data.amplitude,
            x => noise.m_AmplitudeGain = x, 0f, data.shakeTime)).SetEase(data.easeType);
        _seq.Join(DOTween.To(() => data.frequency,
            x => noise.m_FrequencyGain = x, 0f, data.shakeTime)).SetEase(data.easeType);
    }
}
