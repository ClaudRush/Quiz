using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public class CellController : MonoBehaviour
    {
        public static CellController Instance { get; private set; }

        public GroupInteractiveObjects[] GroupsInteractiveObjects;
        public GroupInteractiveObjects SelectGroup;

        [SerializeField] private readonly GameObject PrefabCell;
        [SerializeField] private Cell[] _cells;
        public Cell[] Cells => _cells;

        private List<InteractiveObject> _listItems;
        public List<InteractiveObject> ListItems => _listItems;

        public int FirstCellIndex = 3, LastCellIndex = 6;

        public event Action OnChangeCellData;
        public event Action EndGame;
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Another instance of GameplayController already exists");
            }
            Instance = this;
        }
        private void Start()
        {
            SelectGroup = RandomGroup();
            LoadLevelData();
            ReloadCells();
        }
        public void ReloadCells()
        {
            _listItems = new List<InteractiveObject>();
           
            var items = SelectGroup.InteractiveObjects;

            CreateList(_listItems, items);

            var shuffleList = _listItems;
            ListExtension.Shuffle(shuffleList);

            LoadLevelData();
            InitCells(shuffleList, FirstCellIndex, LastCellIndex);

            if (!GameplayController.Instance.GameOver)
            {
                GameplayController.Instance.SetTask();
            }
        }
        public void InitCells(List<InteractiveObject> shuffleList, int firstIndex, int lastIndex)
        {
            var FIRST_ITEM = 0;

            for (int i = firstIndex; i < lastIndex; i++)
            {
                _cells[i].gameObject.SetActive(true);
                _cells[i].Init(shuffleList[FIRST_ITEM].NameObject, shuffleList[FIRST_ITEM].Sprite);
                var cellSprite = _cells[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
                cellSprite.sprite = _cells[i].Sprite;
                shuffleList.RemoveAt(FIRST_ITEM);
            }
            SelectGroup = RandomGroup();//Можно убрать, тогда будет одна коллекция на ссесию 
            OnChangeCellData?.Invoke();
        }
        public void LoadLevelData()
        {
            var level = GameplayController.Instance.Level;
            switch (level)
            {
                case 1:
                    FirstCellIndex = 3;
                    LastCellIndex = 6;
                    break;
                case 2:
                    FirstCellIndex = 0;
                    break;
                case 3:
                    LastCellIndex = 9;
                    break;
                default:
                    foreach (var item in Cells)
                    {
                        item.gameObject.SetActive(false);
                    }
                    FirstCellIndex = 3;
                    LastCellIndex = 6;
                    GameplayController.Instance.EndGame();
                    break;
            }
        }
        public GroupInteractiveObjects RandomGroup()
        {
            var randomGroup = Random.Range(0, GroupsInteractiveObjects.Length);
            return GroupsInteractiveObjects[randomGroup];
        }
        public IEnumerator TheEndGame()
        {
            yield return new WaitForSeconds(3f);
            GameplayController.Instance.EndGame();
        }
        public void CreateList<T>(List<T> list, T[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                list.Add(items[i]);
            }
        }
    }

}
namespace System
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var rnd = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

