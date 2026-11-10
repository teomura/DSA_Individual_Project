using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontierDispatch.Core
{
    // Assignment Phase 3. Implement as a binary min-heap.

    /// <summary>
    /// A minimum-priority queue implemented as a binary min-heap.
    ///
    /// Items with lower integer priority values should be returned before
    /// items with higher priority values.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    /// </summary>
    public sealed class MinPriorityQueue<T>
    {
        // The list stores the physical representation of the binary heap.
        // Your implementation is responsible for preserving the min-heap invariant.
        private readonly List<Entry> _heap = new List<Entry>();

        /// <summary>
        /// Number of items currently stored in the queue.
        /// </summary>
        public int Count => _heap.Count;

        /// <summary>
        /// Adds an item with the supplied priority.
        /// Lower priority values are removed before higher priority values.
        /// </summary>
        public void Enqueue(T item, int priority)
        {
            // TODO: insert the new entry and restore the min-heap invariant.
            _heap.Add(new Entry(item, priority));
            int currentIndex = _heap.Count - 1;
            SiftUp(currentIndex);
        }

        /// <summary>
        /// Returns the minimum-priority item without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Peek()
        {
            // TODO: return the current minimum without changing the queue.
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("The dispatch queue is empty.");
            }
            return _heap[0].Item;
        }

        /// <summary>
        /// Removes and returns the minimum-priority item.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Dequeue()
        {
            // TODO: remove the minimum item and restore the min-heap invariant.
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("The dispatch queue is empty.");
            }
            Entry root = _heap[0];
            Swap(_heap.Count - 1, 0);
            _heap.RemoveAt(_heap.Count - 1);
            SiftDown(0);

            return root.Item;
        }

        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = ParentIndex(index);
                if (_heap[parentIndex].Priority > _heap[index].Priority)
                {
                    Swap(parentIndex, index);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private void SiftDown(int index)
        {
            while (index < _heap.Count)
            {
                int leftIndex = LeftChildIndex(index);
                int rightIndex = RightChildIndex(index);
                if (leftIndex < _heap.Count && rightIndex < _heap.Count)
                {
                    int minPriorityIndex = 0;
                    if (_heap[leftIndex].Priority > _heap[rightIndex].Priority)
                    {
                        minPriorityIndex = rightIndex;
                    }
                    else
                    {
                        minPriorityIndex = leftIndex;
                    }
                    if (_heap[index].Priority > _heap[minPriorityIndex].Priority)
                    {
                        Swap(index, minPriorityIndex);
                        index = minPriorityIndex;
                    }
                }
                else if (leftIndex < _heap.Count)
                {
                    if (_heap[index].Priority < _heap[leftIndex].Priority)
                    {
                        Swap(index, leftIndex);
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Stores one item together with the priority used by the heap.
        /// </summary>
        private readonly struct Entry
        {
            public Entry(T item, int priority)
            {
                Item = item;
                Priority = priority;
            }

            public T Item { get; }
            public int Priority { get; }
        }

        private static int ParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private static int LeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        private static int RightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        private void Swap(int a, int b)
        {
            Entry temp = _heap[a];
            _heap[a] = _heap[b];
            _heap[b] = temp;
        }
    }
}