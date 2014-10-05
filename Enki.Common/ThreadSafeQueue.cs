using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enki.Common {
	public class ThreadSafeQueue<T> where T : class {
		private Queue<T> _queue = new Queue<T>();

		public T QueueOrDequeue(T item = null) {
			lock (_queue) {
				if (item != null) {
					_queue.Enqueue(item);
					return null;
				} else {
					if (_queue.Count == 0) return null;
					return _queue.Dequeue();
				}
			}
		}
	}
}
