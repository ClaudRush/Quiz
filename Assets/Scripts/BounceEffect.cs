using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BounceEffect : MonoBehaviour
{
    public Sequence sequence;
    void Start()
    {
        EnterAnimation();
    }

    public void EnterAnimation()
    {
        sequence = DOTween.Sequence();
        Vector3 OriginalScale = new Vector3(1f,1f);
        sequence.Append(transform.DOScale(new Vector3(0, 0), 0));
        sequence.Append(transform.DOScale(new Vector3(OriginalScale.x + 0.5f, OriginalScale.y + 0.5f), 0.2f).SetEase(Ease.InOutBounce))
            .Append(transform.DOScale(new Vector3(OriginalScale.x - 0.3f, OriginalScale.y - 0.3f), 0.15f).SetEase(Ease.InOutBounce))
             .Append(transform.DOScale(OriginalScale, 0.1f).SetEase(Ease.InOutBounce));
    }

}
