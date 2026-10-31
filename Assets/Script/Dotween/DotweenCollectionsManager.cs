using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;

[System.Serializable] 
public enum AnimationType
{
    ScaleUp,
    ScaleRotate
}

public class DotweenCollectionsManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AnimationType animationType;

    [Header("Scale Setting")]
    [SerializeField] private float scaleTarget = 1.2f;
    private Vector3 originalScale;
    private Vector3 targetScale;

    [Header("Setting Rotate Value")]
    [SerializeField] private float targetRotateValue = 10f;
    private Vector3 originalRotate;

    [Header("Animation Starting Setting")]
    [SerializeField] private RectTransform uiTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float animationMoveDuration = 0.5f;
    [SerializeField] private Ease entryEaseType = Ease.InBack;
    [SerializeField] private bool moveOnStart;

    [Header("Animation Setting")]
    [SerializeField] private float animationHoverDuration = 0.2f;
    [SerializeField] private bool hoverAnimation;

    private Sequence startSequence;
    private Sequence hoverSequence;



    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * scaleTarget;
        originalRotate = transform.localRotation.eulerAngles;

        if (moveOnStart)
        {
            StartCoroutine(PlayMoveInRoutine());
        }

    }
    private IEnumerator PlayMoveInRoutine()
    {
        
        yield return null;

        MoveInAnimation();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverAnimation) return;
        CheckHoverAnimationType(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hoverAnimation) return;
        CheckHoverAnimationType(false);
    }

    private void MoveInAnimation()
    {
        //For UI
        if(uiTransform != null)
        {
            startSequence.Append(uiTransform.DOAnchorPosY(targetTransform.localPosition.y, animationMoveDuration)
            .SetEase(entryEaseType)
            .SetUpdate(true));
        }
        else //For Gameobject (if there any)
        {
            startSequence.Append(transform.DOMove(targetTransform.localPosition, animationMoveDuration)
            .SetEase(entryEaseType)
            .SetUpdate(true));
        }
        startSequence.SetDelay(0.5f)
                     .SetUpdate(UpdateType.Normal, true);
    }


    private void CheckHoverAnimationType(bool on)
    {
        switch (animationType)
        {
            case AnimationType.ScaleUp:

                if (on)
                {
                    hoverSequence.Kill();
                    hoverSequence.Append(transform.DOScale(targetScale, animationHoverDuration))
                        .SetUpdate(true);
                }
                else
                {
                    hoverSequence.Append(transform.DOScale(originalScale, animationHoverDuration))
                        .SetUpdate(true);
                }
                break;
            case AnimationType.ScaleRotate:
                if (on)
                {
                    float randomZ = Random.Range(-targetRotateValue, targetRotateValue);

                    hoverSequence.Kill();
                    hoverSequence.Append(transform.DOScale(targetScale, animationHoverDuration))
                        .Join(transform.DORotate(new Vector3(0, 0, randomZ), animationHoverDuration))
                        .SetUpdate(true);
                }
                else
                {
                    hoverSequence.Append(transform.DOScale(originalScale, animationHoverDuration))
                        .Join(transform.DORotate(originalRotate, animationHoverDuration))
                        .SetUpdate(true);
                }
                break;
        }
    }
}
