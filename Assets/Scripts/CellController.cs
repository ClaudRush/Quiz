using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public class CellController : MonoBehaviour
    {
        public static CellController Instance { get; private set; }

        [SerializeField] private bool _manyRandomGroups;

        [SerializeField] private GroupInteractiveItems[] GroupsInteractiveObjects;
        [SerializeField] private GameObject PrefabCell;
        [SerializeField] private Cell[] _cells;
        public Cell[] Cells => _cells;

        private GroupInteractiveItems _selectGroup;
        public GroupInteractiveItems SelectGroup { get; set; }

        private List<InteractiveItem> _listItems;
        private int _firstCellIndex = 3, _lastCellIndex = 6;

        public event Action OnChangeCellData;

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
            _selectGroup = RandomGroup();
            LoadLevelData();
            ReloadCells();
        }

        public void ReloadCells()
        {
            var shuffleList = CreateShuffledList();

            LoadLevelData();
            InitCells(shuffleList, _firstCellIndex, _lastCellIndex);
            if (!GameplayController.Instance.GameOver)
                GameplayController.Instance.SetTask();
        }

        public void InitCells(List<InteractiveItem> shuffleList, int firstIndex, int lastIndex)
        {
            var FIRST_ITEM = 0;

            for (int i = firstIndex; i < lastIndex; i++)
            {
                _cells[i].gameObject.SetActive(true);
                _cells[i].Init(shuffleList[FIRST_ITEM].NameItem, shuffleList[FIRST_ITEM].Sprite);
                var cellSprite = _cells[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
                cellSprite.sprite = _cells[i].Sprite;
                shuffleList.RemoveAt(FIRST_ITEM);
            }

            if (_manyRandomGroups)
            {
                _selectGroup = RandomGroup();
            }
            OnChangeCellData?.Invoke();
        }

        public void LoadLevelData()
        {
            var level = GameplayController.Instance.Level;
            switch (level)
            {
                case 1:
                    _firstCellIndex = 3;
                    _lastCellIndex = 6;
                    break;
                case 2:
                    _firstCellIndex = 0;
                    break;
                case 3:
                    _lastCellIndex = 9;
                    break;
                default:
                    foreach (var item in Cells)
                    {
                        item.gameObject.SetActive(false);
                    }
                    _firstCellIndex = 3;
                    _lastCellIndex = 6;
                    GameplayController.Instance.EndGame();
                    break;
            }
        }

        public GroupInteractiveItems RandomGroup()
        {
            var randomGroup = Random.Range(0, GroupsInteractiveObjects.Length);
            return GroupsInteractiveObjects[randomGroup];
        }

        public List<InteractiveItem> CreateShuffledList()
        {
            _listItems = new List<InteractiveItem>();
            var items = _selectGroup.InteractiveItems;
            ListExtension.CreateList(_listItems, items);
            var shuffleList = _listItems;
            ListExtension.Shuffle(shuffleList);

            return shuffleList;
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

        public static void CreateList<T>(List<T> list, T[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                list.Add(items[i]);
            }
        }
    }
}

