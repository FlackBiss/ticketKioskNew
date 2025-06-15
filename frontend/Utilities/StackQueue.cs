using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Utilities
{
    public class StackQueue<T>
    {
        private readonly LinkedList<T> _linkedList = new();

        public void Push(T obj)
        {
            _linkedList.AddFirst(obj);
        }

        public void Enqueue(T obj)
        {
            _linkedList.AddFirst(obj);
        }

        public T Pop()
        {
            var obj = _linkedList.First.Value;
            _linkedList.RemoveFirst();
            return obj;
        }

        public T Dequeue()
        {
            var obj = _linkedList.Last.Value;
            _linkedList.RemoveLast();
            return obj;
        }

        public T PeekStack()
        {
            return _linkedList.First.Value;
        }

        public T PeekQueue()
        {
            return _linkedList.Last.Value;
        }

        public int Count => _linkedList.Count;
    }
}
