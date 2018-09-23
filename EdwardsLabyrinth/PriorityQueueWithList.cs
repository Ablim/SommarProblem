using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdwardsLabyrinth
{
    public class PriorityQueueWithList<K, V>
        where V : IComparable<V>
        where K : IComparable<K>
    {
        private List<(K key, V value)> _queue;

        public int Count { get { return _queue.Count; } }

        public PriorityQueueWithList()
        {
            _queue = new List<(K, V)>();
        }

        public void Add(K key, V value)
        {
            var existing = _queue.Where(x => x.key.Equals(key));

            if (!existing.Any())
            {
                _queue.Add((key, value));
            }
            else
            {
                var item = existing.First();
                item.value = value;
            }

            _queue.Sort((x, y) => x.value.CompareTo(y.value));
        }

        public (K key, V value) GetMin()
        {
            if (_queue.Any())
            {
                var first = _queue.First();
                _queue.Remove(first);
                return first;
            }
            else
            {
                return default((K, V));
            }
        }
    }
}
