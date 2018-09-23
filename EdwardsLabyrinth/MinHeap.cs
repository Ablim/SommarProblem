using System;
using System.Collections.Generic;
using System.Text;

namespace EdwardsLabyrinth
{
    public class MinHeap<T, P>
        where T : IComparable<T>
        where P : IComparable<P>
    {
        private (T obj, P prio)[] _heap;

        /// <summary>
        /// A minimum heap with the smallest object at root level. 
        /// </summary>
        public MinHeap()
        {
            _heap = new (T obj, P prio)[10];
            Count = 0;
        }

        /// <summary>
        /// Number of objects in the collection.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Inspect root object. No modification. 
        /// </summary>
        public (T obj, P prio) Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return _heap[0];
        }

        /// <summary>
        /// Take the root object and delete it from the collection.
        /// </summary>
        public (T obj, P prio) Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var root = _heap[0];
            DeleteRoot();
            return root;
        }

        /// <summary>
        /// Add an object to the collection.
        /// </summary>
        public void Push((T obj, P prio) item)
        {
            if (Count == _heap.Length)
                Resize();

            _heap[Count] = item;
            SiftUp(Count);
            Count++;
        }

        /// <summary>
        /// Delete the root object.
        /// </summary>
        public void DeleteRoot()
        {
            if (Count == 0)
                return;

            _heap[0] = _heap[Count - 1];
            SiftDown(0);
            Count--;
        }

        /// <summary>
        /// Test if an object exists in the collection.
        /// </summary>
        public bool Contains(T obj)
        {
            for (int i = 0; i < Count; i++)
            {
                if (obj.Equals(_heap[i].obj))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Update the prio for an object.
        /// </summary>
        public void Update((T obj, P prio) item)
        {
            if (Count == 0)
                return;

            var index = 0;

            for (int i = 0; i < Count; i++)
            {
                if (item.obj.Equals(_heap[i].obj))
                {
                    index = i;
                    break;
                }
            }

            var oldPrio = _heap[index].prio;
            _heap[index].prio = item.prio;

            if (item.prio.CompareTo(oldPrio) < 0)
                SiftUp(index);
            else
                SiftDown(index);
        }

        private void SiftUp(int index)
        {
            //For parent at position n, children are at 2n+1 and 2n+2
            var childIndex = index;
            var parentIndex = 0;

            while (true)
            {
                if (childIndex == 0)
                    break;

                if (childIndex % 2 == 0)
                    parentIndex = (childIndex - 2) / 2;
                else
                    parentIndex = (childIndex - 1) / 2;

                if (_heap[parentIndex].prio.CompareTo(_heap[childIndex].prio) <= 0)
                    break;

                var parent = _heap[parentIndex];
                _heap[parentIndex] = _heap[childIndex];
                _heap[childIndex] = parent;
                childIndex = parentIndex;
            }
        }

        private void SiftDown(int index)
        {
            //For parent at position n, children are at 2n+1 and 2n+2
            var parentIndex = index;
            var childIndex1 = 0;
            var childIndex2 = 0;

            while (true)
            {
                childIndex1 = (2 * parentIndex) + 1;
                childIndex2 = (2 * parentIndex) + 2;

                if (childIndex2 < Count)
                {
                    var parent = _heap[parentIndex];
                    var child1 = _heap[childIndex1];
                    var child2 = _heap[childIndex2];

                    if (child1.prio.CompareTo(child2.prio) < 0) //child1 is smallest
                    {
                        if (parent.prio.CompareTo(child1.prio) > 0)
                        {
                            _heap[parentIndex] = _heap[childIndex1];
                            _heap[childIndex1] = parent;
                            parentIndex = childIndex1;
                            continue;
                        }
                    }
                    else //child2 is smallest
                    {
                        if (parent.prio.CompareTo(child2.prio) > 0)
                        {
                            _heap[parentIndex] = _heap[childIndex2];
                            _heap[childIndex2] = parent;
                            parentIndex = childIndex2;
                            continue;
                        }
                    }
                }
                else if (childIndex1 < Count)
                {
                    var parent = _heap[parentIndex];
                    var child1 = _heap[childIndex1];

                    if (parent.prio.CompareTo(child1.prio) > 0)
                    {
                        _heap[parentIndex] = _heap[childIndex1];
                        _heap[childIndex1] = parent;
                        parentIndex = childIndex1;
                        continue;
                    }
                }

                break;
            }
        }

        private void Resize()
        {
            var newHeap = new (T obj, P prio)[_heap.Length * 2];

            for (int i = 0; i < _heap.Length; i++)
            {
                newHeap[i] = _heap[i];
            }

            _heap = newHeap;
        }
    }
}
