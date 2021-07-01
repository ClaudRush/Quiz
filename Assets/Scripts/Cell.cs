using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour
{
    public bool Answer;
    public string Name = " ";
    public Sprite Sprite;

    public event Action OnAnswer;

    public void Init(string name, Sprite sprite)
    {
        Name = name;
        Sprite = sprite;
    }

    public void InitaAnswer(bool answer)
    {
        Answer = answer;
    }

    private void OnEnable()
    {
        CellController.Instance.OnChangeCellData += CellInfo;

        if (!GameplayController.Instance.ActiveCells.Contains(this))
        {
            GameplayController.Instance.ActiveCells.Add(this);
        }
    }

    private void OnDisable()
    {
        CellController.Instance.OnChangeCellData -= CellInfo;

        if (GameplayController.Instance.ActiveCells.Contains(this))
        {
            GameplayController.Instance.ActiveCells.Remove(this);
        }
    }

    public void CellInfo()
    {
        Debug.Log("Cell name: " + Name);
    }

    private void OnMouseDown()
    {
        if (!GameplayController.Instance.GameOver)
        {
            if (!GameplayController.Instance.TapReload)
            {
                if (Answer)
                {
                    GameplayController.Instance.StarEffect(gameObject.transform.localPosition);
                    GameplayController.Instance.PreviousAnswer = Name;
                    GameplayController.Instance.Level++;

                    Debug.Log("Correct answer!");

                    AnimationItem(Answer);

                    StartCoroutine(RightAnswer());
                    StartCoroutine(GameplayController.Instance.ReloadTap(2f));
                }
                else
                {
                    Debug.Log("Wrong answer...");
                    AnimationItem(Answer);
                    StartCoroutine(GameplayController.Instance.ReloadTap(1f));
                }
            }
        }
    }

    public IEnumerator RightAnswer()
    {
        yield return new WaitForSeconds(2f);
        OnAnswer?.Invoke();
    }

    public void AnimationItem(bool answer)
    {
        var cellSprite = transform.GetChild(0);
        if (answer)
        {
            Vector3 OriginalScale = cellSprite.localScale;
            var sequence = DOTween.Sequence();
            sequence.Append(cellSprite.DOScale(new Vector3(0, 0), 0));
            sequence.Join(cellSprite.DOScale(new Vector3(OriginalScale.x + 0.5f, OriginalScale.y + 0.5f), 0.2f).SetEase(Ease.InOutBounce))
                .Append(cellSprite.DOScale(new Vector3(OriginalScale.x - 0.3f, OriginalScale.y - 0.3f), 0.15f).SetEase(Ease.InOutBounce))
                 .Append(cellSprite.DOScale(OriginalScale, 0.1f).SetEase(Ease.InOutBounce));
        }
        else
        {
            cellSprite.DOShakePosition(1, new Vector3(0.3f, 0, 0)).SetEase(Ease.InBounce);
        }
    }
}
