using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleTon<CameraManager>
{
    private CinemachineVirtualCamera _lastVCam = null;
    public CinemachineVirtualCamera LastVCam => _lastVCam;
    private Sequence _seq = null;

    [SerializeField]
    private List<CinemachineVirtualCamera> _vCams = new List<CinemachineVirtualCamera>();
    private int _camIndex = 0;

    public void ResetCamera()
    {
        _seq?.Kill();
    }

    public void ChangeRoomCamera(PolygonCollider2D cameraAreaCollider)
    {
        CinemachineVirtualCamera vCam = _vCams[_camIndex];
        CinemachineConfiner2D confiner = vCam.GetComponent<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = cameraAreaCollider;
        _camIndex = (_camIndex + 1) % _vCams.Count;
        vCam.gameObject.SetActive(true);
        for(int i = 0; i < _vCams.Count; i++)
        {
            if (_vCams[i] == vCam)
            {
                continue;
            }
            _vCams[i].gameObject.SetActive(false);
        }
    }

    public void BakeCurrentConfiner(Room room)
    {
        CinemachineVirtualCamera vCam = _vCams[0];
        CinemachineConfiner2D confiner = vCam.GetComponent<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = room.cameraAreaCollider;
        confiner.InvalidateCache();
    }

    public void ShakeCamera(ShakeCameraDataSO data)
    {
        if(_lastVCam != null)
        {
            CinemachineBasicMultiChannelPerlin lastNoise = _lastVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            lastNoise.m_AmplitudeGain = 0f;
            lastNoise.m_FrequencyGain = 0f;
        }

        ICinemachineCamera cam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        if(cam == null)
        {
            return;
        }
        CinemachineVirtualCamera vCam = cam.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _lastVCam = vCam;

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
