using System;
using System.Collections.Generic;
using System.Text;

namespace EdwardsLabyrinth
{
    public class PriorityQueueWithHeap<K, V>
        where K : IComparable<K>
        where V : IComparable<V>
    {
        private MinHeap<K, V> _heap;

        public PriorityQueueWithHeap()
        {
            _heap = new MinHeap<K, V>();
        }

        public int Count { get { return _heap.Count; } }

        public void Add(K key, V value)
        {
            if (_heap.Contains(key))
                _heap.Update((key, value));
            else
                _heap.Push((key, value));
        }

        public (K key, V value) GetMin()
        {
            return _heap.Pop();
        }
    }
}
