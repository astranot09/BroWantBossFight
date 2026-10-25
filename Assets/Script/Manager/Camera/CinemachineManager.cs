using Unity.Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    [SerializeField] private CinemachineCamera cam;

    private float originalLens;

    private void Start()
    {
        originalLens = cam.Lens.OrthographicSize;
    }

    public void AddTarget(Transform target)
    {
        cam.Target.TrackingTarget = target;
    }

    public void ChangeCamValue(float value)
    {
        if (cam == null) return;

        LensSettings lens = cam.Lens;
        lens.OrthographicSize = value;
        cam.Lens = lens;
    }
    public void ResetCamValue()
    {
        ChangeCamValue(originalLens);
    }
}
