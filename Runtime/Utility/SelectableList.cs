using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.Utility
{
    [Serializable]
    public class SelectableList<T> : IList<T>
    {
        [SerializeField] private int selectedIndex;
        [SerializeField] private List<T> list;
        public bool AllowDeselect = true;

        private bool _shouldSendSelectEvents = true;

        public T SelectedItem
        {
            get => list.Count > 0 ? list[selectedIndex] : default;
        }

        public int SelectedIndex
        {
            get => list.Count > 0 ? selectedIndex : -1;
        }

        public delegate void SelectedItemHandler(T selectedItem, int index);

        public event SelectedItemHandler OnItemSelected;
        public event SelectedItemHandler OnItemDeselected;

        public SelectableList()
        {
            list = new List<T>();
        }

        public SelectableList(SelectableList<T> selectableList)
        {
            list = selectableList.list;
        }

        public void SelectIndex(int index)
        {
            if (!list.IsIndexValid(index))
                throw new IndexOutOfRangeException();

            _shouldSendSelectEvents = _shouldSendSelectEvents || selectedIndex != index;
            
            // If the index hasn't changed, don't do anything.
            if (!_shouldSendSelectEvents)
                return;

            var prevSelectedItem = SelectedItem;
            var prevSelectedIndex = selectedIndex;
            
            selectedIndex = index;
            OnItemDeselected?.Invoke(prevSelectedItem, prevSelectedIndex);
            OnItemSelected?.Invoke(SelectedItem, selectedIndex);
            
            _shouldSendSelectEvents = false;
        }

        #region IList<T> Implementation
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
        #endregion
    }
}