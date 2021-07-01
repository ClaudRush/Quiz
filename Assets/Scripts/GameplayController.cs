using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance { get; private set; }

    [SerializeField] private Text _taskText;
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private GameObject _fadePanel;
    [SerializeField] private GameObject _starEffect;

    public int Level;
    public bool GameOver;
    public bool TapReload;

    private Cell _cellAnswer;
    private string _previousAnswer = " ";
    private int _randomCell;

    public List<Cell> ActiveCells { get; private set; }
    public string PreviousAnswer { get => _previousAnswer; set => _previousAnswer = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Another instance of GameplayController already exists");
        }
        Instance = this;

        ActiveCells = new List<Cell>();
        Level = 1;
    }
    
    public void SetTask()
    {
        _randomCell = Random.Range(0, ActiveCells.Count);
        if (_cellAnswer != null)
        {
            _cellAnswer.Answer = false;
            _cellAnswer.OnAnswer -= CellController.Instance.ReloadCells;
        }
        InitAnswerCell();
    }

    public void InitAnswerCell()
    {
        _cellAnswer = ActiveCells[_randomCell];
        _cellAnswer.InitaAnswer(true);
        _taskText.text = string.Format("Find {0}", _cellAnswer.Name);
        _cellAnswer.OnAnswer += CellController.Instance.ReloadCells;

        if (_previousAnswer == _cellAnswer.Name && !GameOver && Level != 1)
        {
            CellController.Instance.ReloadCells();
        }
    }

    public void StarEffect (Vector3 position)
    {
        var effect = Instantiate(_starEffect, position, Quaternion.identity);
        effect.transform.localScale = new Vector3(1, 1);
        effect.transform.parent = null;
        Destroy(effect, 2);
    }

    public void EndGame()
    {
        Level = 1;
        GameOver = true;
        FadePanelAnimation(GameOver);
        _taskText.text = " ";
        _restartPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SetTask();
        GameOver = false;
        FadePanelAnimation(GameOver);
        _restartPanel.SetActive(false);
        LoadingBounce();
        TapReload = false;
    }

    public void LoadingBounce()
    {
        var bounceList = FindObjectsOfType<BounceEffect>();
        foreach (var item in bounceList)
        {
            item.EnterAnimation();
        }
    }
    public void FadePanelAnimation(bool gameOver)
    {
        if (gameOver)
        {
            _fadePanel.GetComponent<Image>().DOColor(new Color(255f, 255f, 255f, 1f), 2f);
        }
        else
        {
            _fadePanel.GetComponent<Image>().DOColor(new Color(255f, 255f, 255f, 0f), 2f);
        }
    }

    public IEnumerator ReloadTap(float seconds)
    {
        TapReload = true;
        yield return new WaitForSeconds(seconds);
        TapReload = false;
    }
}
