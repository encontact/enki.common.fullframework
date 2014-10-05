using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enki.Common {
	public class ParallelTask<T> where T : class {
		private Action<T> _taskWork { get; set; }
		private ThreadSafeQueue<T> _queue { get; set; }
		private List<Thread> _threads { get; set; }

		public ParallelTask(IEnumerable<T> list, Action<T> taskWork, int simultaneousTasks) {
			_taskWork = taskWork;

			_queue = new ThreadSafeQueue<T>();
			foreach (var item in list) _queue.QueueOrDequeue(item);
			_threads = new List<Thread>();
			for (var i = 0; i < simultaneousTasks; i++) {
				var t = new Thread(Work);
				t.IsBackground = true;
				_threads.Add(t);
			}
		}

		private void Work() {
			T workItem = null;
			while ((workItem = _queue.QueueOrDequeue(null)) != null) {
				_taskWork(workItem);
			}
		}

		public void Start() {
			foreach (var t in _threads) {
				t.Start();
			}
		}

		public void Wait() {
			foreach (var t in _threads) {
				t.Join();
			}
		}

		public void Complete() {
			Start();
			Wait();
		}

		public static ParallelTask<T> CreateTask(IEnumerable<T> list, Action<T> taskWork, int simultaneousTasks = 5) {
			return new ParallelTask<T>(list, taskWork, simultaneousTasks);
		}
	}
}
