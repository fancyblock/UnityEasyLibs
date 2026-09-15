using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    public class EasyScrollView : MonoBehaviour
    {
        public enum Dir
        {
            vertical,
            horizontal,
        }

        public enum eFixedItem
        {
            none,
            first,
            last,
        }

        public event Action<int, GameObject> ON_ITEM_FILL;

        [SerializeField] protected GameObject m_itemPrefab;
        
#if UNITY_EDITOR
        public GameObject ItemPrefab => m_itemPrefab;
#endif
        
        [SerializeField] protected GameObject m_fixedItemPrefab;
        [SerializeField] protected Vector2 m_itemSize;
        [SerializeField] protected Vector2 m_itemInterval;
        [SerializeField] protected int m_perItemCount = 1;                      // 
        [SerializeField] protected eFixedItem m_fixedItemType = eFixedItem.none;    // 

        [SerializeField] protected ScrollRect m_scrollRect;
        [SerializeField] protected RectTransform m_itemContainer;
        [SerializeField] protected Dir m_direction;

        protected int m_itemCount;
        protected int m_maxVisualCount;

        protected List<GameObject> m_pendingItemList = new List<GameObject>();
        protected float m_currentScrollPosotion;
        protected Dictionary<int, GameObject> m_itemDic = new Dictionary<int, GameObject>();
        protected List<int> m_currentPageIndeies = new List<int>();

        protected GameObject m_fixedItemGo = null;


        public void Load(int count, Action<int, GameObject> setFunc)
        {
            if (ON_ITEM_FILL != null)
            {
                var callList = ON_ITEM_FILL.GetInvocationList();

                foreach (var c in callList)
                {
                    object delObj = c.GetType().GetProperty("Method").GetValue(c, null);
                    string funcName = (string)delObj.GetType().GetProperty("Name").GetValue(delObj, null);
                    ON_ITEM_FILL -= c as Action<int, GameObject>;
                }
            }

            ON_ITEM_FILL += setFunc;

            Load(count);
        }

        public void Load(int count)
        {
            m_itemCount = count;

            int realCount = count / m_perItemCount + (count % m_perItemCount > 0 ? 1 : 0);

            // size the content
            if (m_direction == Dir.vertical)
            {
                m_itemContainer.sizeDelta = new Vector2(m_itemContainer.sizeDelta.x, (m_itemSize.y + m_itemInterval.y) * realCount);
            }
            else if (m_direction == Dir.horizontal)
            {
                m_itemContainer.sizeDelta = new Vector2((m_itemSize.x + m_itemInterval.x) * realCount, m_itemContainer.sizeDelta.y);
            }

            // precreate the items
            if (m_pendingItemList.Count == 0 && m_itemDic.Count == 0)
            {
                m_maxVisualCount = 0;

                if (m_direction == Dir.vertical)
                {
                    float viewHeight = GetComponent<RectTransform>().rect.height;
                    float viewCount = viewHeight / (m_itemSize.y + m_itemInterval.y);

                    m_maxVisualCount = Mathf.CeilToInt(viewCount) + 1;
                    m_maxVisualCount *= m_perItemCount;
                }
                else if (m_direction == Dir.horizontal)
                {
                    float viewWidth = GetComponent<RectTransform>().rect.width;
                    float viewCount = viewWidth / (m_itemSize.x + m_itemInterval.x);

                    m_maxVisualCount = Mathf.CeilToInt(viewCount) + 1;
                    m_maxVisualCount *= m_perItemCount;
                }

                for (int i = 0; i < m_maxVisualCount; i++)
                {
                    GameObject go = Instantiate(m_itemPrefab, m_itemContainer);
                    go.transform.localScale = Vector3.one;

                    recycleItem(go);
                }
            }
            else
            {
                // recycle all the items
                foreach (GameObject go in m_itemDic.Values)
                    recycleItem(go);

                m_itemDic.Clear();
            }

            //precreate the fixed item
            if(m_fixedItemType != eFixedItem.none)
            {
                if(m_fixedItemGo == null)
                {
                    GameObject go = Instantiate(m_fixedItemPrefab, m_itemContainer);
                    go.transform.localScale = Vector3.one;
                    m_fixedItemGo = go;

                    recycleItem(go);
                }
                else
                {
                    recycleItem(m_fixedItemGo);
                }
            }

            if (m_direction == Dir.vertical)
                initVerticalItems();
            else if (m_direction == Dir.horizontal)
                initHorizontalItems();
        }

        public void SnapTo(int index)
        {
            index = Mathf.Clamp(index, 0, m_itemCount - 1);

            if (m_direction == Dir.vertical)
                m_itemContainer.localPosition = new Vector2(m_itemContainer.localPosition.x, Mathf.FloorToInt((float)index / (float)m_perItemCount) * (m_itemSize.y + m_itemInterval.y));
            else if (m_direction == Dir.horizontal)
                m_itemContainer.localPosition = new Vector3(-Mathf.FloorToInt((float)index / (float)m_perItemCount) * (m_itemSize.x + m_itemInterval.x), m_itemContainer.localPosition.y);

            refreshView(true);
        }

        public int CURRENT_SNAP_INDEX
        {
            get
            {
                if (m_direction == Dir.vertical)
                    return Mathf.FloorToInt(m_itemContainer.localPosition.y / (m_itemSize.y + m_itemInterval.y)) * m_perItemCount;
                else if (m_direction == Dir.horizontal)
                    return Mathf.FloorToInt(-m_itemContainer.localPosition.x / (m_itemSize.x + m_itemInterval.x)) * m_perItemCount;

                return 0;
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < m_itemCount; i++)
            {
                if (m_itemDic.ContainsKey(i) && m_itemDic[i] != null)
                    ON_ITEM_FILL(i, m_itemDic[i]);
            }
        }

        public int ITEM_COUNT
        {
            get{ return m_itemCount; }
        }

        public Transform ITEM_CONTAINER
        {
            get { return m_itemContainer; }
        }

        /// <summary>
        /// Get the displayed items (for some hack useage)
        /// </summary>
        public List<GameObject> DISPLAYED_ITEMS
        {
            get
            {
                return m_itemDic.Values.ToList();
            }
        }

        /// <summary>
        /// unstable
        /// </summary>
        public void AddOneItem()
        {
            m_itemCount++;
            refreshView();
        }

        public void Roll2Last()
        {
            StartCoroutine(rolling2last());

            IEnumerator rolling2last()
            {
                yield return null;

                if (m_direction == Dir.vertical)
                    m_scrollRect.verticalNormalizedPosition = 0f;
                else
                    m_scrollRect.horizontalNormalizedPosition = 0f;
            }
        }


        private void Awake()
        {
            m_scrollRect.onValueChanged.AddListener(onScrollChange);
        }

        private void Update()
        {
            refreshView();
        }

        private void onScrollChange(Vector2 arg0)
        {
            refreshView();
        }


        #region private part

        private void initVerticalItems()
        {
            m_currentScrollPosotion = m_itemContainer.localPosition.y;

            int startIndex = Mathf.FloorToInt(m_currentScrollPosotion / (m_itemSize.y + m_itemInterval.y)) * m_perItemCount;
            startIndex = Mathf.Clamp(startIndex, 0, int.MaxValue);

            m_currentPageIndeies.Clear();

            for (int i = 0; i < m_maxVisualCount; i++)
            {
                int index = startIndex + i;

                if (index >= m_itemCount)
                    break;

                GameObject itemGo = allocItem(index);
                itemGo.transform.localPosition = getItemPosition(index);
                m_itemDic.Add(index, itemGo);
                ON_ITEM_FILL(index, itemGo);

                m_currentPageIndeies.Add(index);
            }
        }

        private void initHorizontalItems()
        {
            m_currentScrollPosotion = m_itemContainer.localPosition.x;

            int startIndex = Mathf.FloorToInt(m_currentScrollPosotion / (m_itemSize.x + m_itemInterval.x)) * m_perItemCount;
            startIndex = Mathf.Clamp(startIndex, 0, int.MaxValue);

            m_currentPageIndeies.Clear();

            for (int i = 0; i < m_maxVisualCount; i++)
            {
                int index = startIndex + i;

                if (index >= m_itemCount)
                    break;

                GameObject itemGo = allocItem(index);
                itemGo.transform.localPosition = getItemPosition(index);
                m_itemDic.Add(index, itemGo);
                ON_ITEM_FILL(index, itemGo);

                m_currentPageIndeies.Add(index);
            }
        }

        private Vector2 getItemPosition(int index)
        {
            Vector2 position = Vector2.zero;

            if (m_direction == Dir.vertical)
            {
                int row = index / m_perItemCount;
                int col = index % m_perItemCount;

                float rowOffset = (m_itemContainer.rect.width - (m_itemSize.x * m_perItemCount + m_itemInterval.x * (m_perItemCount - 1))) / 2.0f + m_itemSize.x * 0.5f;

                position.x = rowOffset + col * (m_itemSize.x + m_itemInterval.x);
                position.y = -(m_itemSize.y * 0.5f + m_itemInterval.y + row * (m_itemSize.y + m_itemInterval.y));
            }
            else if (m_direction == Dir.horizontal)
            {
                int row = index % m_perItemCount;
                int col = index / m_perItemCount;

                float colOffset = (m_itemContainer.rect.height - (m_itemSize.y * m_perItemCount + m_itemInterval.y * (m_perItemCount - 1))) / 2.0f + m_itemSize.y * 0.5f;

                position.x = m_itemSize.x * 0.5f + m_itemInterval.x + col * (m_itemSize.x + m_itemInterval.x);
                position.y = -(colOffset + row * (m_itemSize.y + m_itemInterval.y));
            }

            return position;
        }

        private void refreshView(bool forceRefresh = false)
        {
            float scrollPosition = 0;

            if (m_direction == Dir.vertical)
                scrollPosition = m_itemContainer.localPosition.y;
            else if (m_direction == Dir.horizontal)
                scrollPosition = m_itemContainer.localPosition.x;

            if (!forceRefresh && Mathf.Abs(m_currentScrollPosotion - scrollPosition) < float.Epsilon)
                return;

            if (m_direction == Dir.vertical)
                refreshVerticalView(scrollPosition);
            else if (m_direction == Dir.horizontal)
                refreshHorizontalView(scrollPosition);

            m_currentScrollPosotion = scrollPosition;
        }

        private void refreshVerticalView(float scrollPosition)
        {
            int startIndex = Mathf.FloorToInt(scrollPosition / (m_itemSize.y + m_itemInterval.y)) * m_perItemCount;
            startIndex = Mathf.Clamp(startIndex, 0, int.MaxValue);

            refreshViewItems(startIndex);
        }

        private void refreshHorizontalView(float scrollPosition)
        {
            int startIndex = Mathf.FloorToInt(-scrollPosition / (m_itemSize.x + m_itemInterval.x)) * m_perItemCount;
            startIndex = Mathf.Clamp(startIndex, 0, int.MaxValue);

            refreshViewItems(startIndex);
        }

        private void refreshViewItems(int startIndex)
        {
            List<int> pageIndeies = new List<int>();

            for (int i = 0; i < m_maxVisualCount; i++)
            {
                int index = startIndex + i;

                if (index >= m_itemCount)
                    break;

                pageIndeies.Add(index);
            }

            var oldIndexes = new List<int>( m_currentPageIndeies.Except<int>(pageIndeies) );
            foreach (int oldIndex in oldIndexes)
            {
                recycleItem(m_itemDic[oldIndex]);
                m_itemDic.Remove(oldIndex);
            }

            var newIndexes = new List<int>( pageIndeies.Except<int>(m_currentPageIndeies) );
            foreach (int newIndex in newIndexes)
            {
                GameObject itemGo = allocItem(newIndex);
                itemGo.transform.localPosition = getItemPosition(newIndex);
                m_itemDic.Add(newIndex, itemGo);
                ON_ITEM_FILL(newIndex, itemGo);
            }

            m_currentPageIndeies = pageIndeies;
        }

        private GameObject allocItem(int index)
        {
            GameObject go = null;

            if (m_fixedItemType != eFixedItem.none)
            {
                if(m_fixedItemType == eFixedItem.first && index == 0)
                {
                    go = m_fixedItemGo;
                    go.SetActive(true);
                }
                else if(m_fixedItemType == eFixedItem.last && index == (m_itemCount - 1))
                {
                    go = m_fixedItemGo;
                    go.SetActive(true);
                }
            }

            if (go == null)
            {
                go = m_pendingItemList.First<GameObject>();
                m_pendingItemList.Remove(go);
                go.SetActive(true);
            }

            return go;
        }

        private void recycleItem(GameObject item)
        {
            if (m_fixedItemType != eFixedItem.none && item == m_fixedItemGo)
            {
                // item.SetActive(false);
            }
            else
            {
                m_pendingItemList.Add(item);
            }

            item.SetActive(false);
        }

        #endregion
    }
}