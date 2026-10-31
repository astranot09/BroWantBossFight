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
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private float transitionDuration = 0.5f;

    private float originalLens;
    private float targetLensSize;
    private float startLensSize;
    private float transitionTimer;
    private bool isTransitioning;

    private void Start()
    {
        originalLens = cam.Lens.OrthographicSize;
    }

    public void AddTarget(Transform target)
    {
        cam.Target.TrackingTarget = target;
    }

    private void Update()
    {
        if (!isTransitioning) return;

        transitionTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(transitionTimer / transitionDuration);

        // Smoothly interpolate between starting size and target size
        LensSettings lens = cam.Lens;
        lens.OrthographicSize = Mathf.Lerp(startLensSize, targetLensSize, progress);
        cam.Lens = lens;
        confiner.InvalidateBoundingShapeCache();

        if (progress >= 1f)
        {
            isTransitioning = false;
        }
    }


    public void ChangeCamValue(float value)
    {
        if (cam == null) return;

        startLensSize = cam.Lens.OrthographicSize;
        targetLensSize = value;
        transitionTimer = 0f;
        isTransitioning = true;
    }
    public void ResetCamValue()
    {
        ChangeCamValue(originalLens);
        confiner.InvalidateBoundingShapeCache();
    }
}
