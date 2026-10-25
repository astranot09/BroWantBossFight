using UnityEngine;
using TMPro;
using DG.Tweening;

public class PopUpDamage : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float targetY = 1f;
    [SerializeField] private float popUpDuration = 0.5f;
    [Header("UI")]
    [SerializeField] private TMP_Text damageText;

    public void SetUpPopUpDamage(int x)
    {
        damageText.text = x.ToString();
        AnimationPopUp();
    }


    private void AnimationPopUp()
    {
        Sequence s = DOTween.Sequence();

        s.Append(transform.DOMoveY((transform.position.y + targetY), popUpDuration))
            .Join(damageText.DOFade(0f, popUpDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            })
            );
    }
}
