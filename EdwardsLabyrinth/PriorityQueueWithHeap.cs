using System;
using System.Collections.Generic;
using System.Text;

namespace EdwardsLabyrinth
{
    public class PriorityQueueWithHeap
    {

    }

    public class MinHeap<T>
        where T : IComparable<T>
    {
        private T[] _heap;

        public MinHeap()
        {
            _heap = new T[10];
            Count = 0;
        }

        public int Count { get; private set; }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return _heap[0];
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var root = _heap[0];
            DeleteRoot();
            return root;
        }

        public void Push(T item)
        {
            if (Count == _heap.Length)
                Resize();

            _heap[Count] = item;
            SiftUp(Count);
            Count++;
        }

        public void DeleteRoot()
        {
            if (Count == 0)
                return;

            _heap[0] = _heap[Count - 1];
            SiftDown(0);
            Count--;
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

                if (_heap[parentIndex].CompareTo(_heap[childIndex]) <= 0)
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
                    
                    if (child1.CompareTo(child2) < 0) //child1 is smallest
                    {
                        if (parent.CompareTo(child1) > 0)
                        {
                            _heap[parentIndex] = _heap[childIndex1];
                            _heap[childIndex1] = parent;
                            parentIndex = childIndex1;
                            continue;
                        }
                    }
                    else //child2 is smallest
                    {
                        if (parent.CompareTo(child2) > 0)
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

                    if (parent.CompareTo(child1) > 0)
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
            var newHeap = new T[_heap.Length * 2];

            for (int i = 0; i < _heap.Length; i++)
            {
                newHeap[i] = _heap[i];
            }

            _heap = newHeap;
        }
    }
}
