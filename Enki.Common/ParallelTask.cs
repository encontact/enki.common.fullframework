using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enki.Common {
	/// <summary>
	/// Cria Threads paralelas para execução de uma lista de processos de forma sincrona.
	/// </summary>
	/// <typeparam name="T">Tipo do objeto da lista.</typeparam>
	public class ParallelTask<T> where T : class {
		private ParallelOptions _options { get; set; }
		private CancellationTokenSource _cancelation { get; set; } 
		private ParallelLoopResult _parallelTasks { get; set; }
		private IEnumerable<T> _itemList { get; set; }
		private Action<T> _taskAction { get; set; }
		private Action _onCompleteAction { get; set; }

		public ParallelTask(IEnumerable<T> list, Action<T> taskAction, int simultaneousTasks, Action onCompleteAction = null) {
			_cancelation = new CancellationTokenSource();
			_options = new System.Threading.Tasks.ParallelOptions();
			_options.MaxDegreeOfParallelism = simultaneousTasks; // -1 is for unlimited. 1 is for sequential.
			_options.CancellationToken = _cancelation.Token;
			_itemList = list;
			_taskAction = taskAction;
			_onCompleteAction = onCompleteAction;
		}

		public void Start() {
			try {
				_parallelTasks = System.Threading.Tasks.Parallel.ForEach(_itemList, _options, item => {
					_taskAction(item);
					_options.CancellationToken.ThrowIfCancellationRequested();
				});
			} catch (OperationCanceledException) {
				// VER O QUE FAZER SE FOR CANCELADO.
			}
		}

		public void Wait() {
			while(!_parallelTasks.IsCompleted) {
				Thread.Sleep(500);
			}
			if (_onCompleteAction != null) _onCompleteAction();
		}

		public void Cancel() {
			_cancelation.Cancel(true);
		}

		[Obsolete("Alterar chamada para o método Run(), este método será descontinuado.")]
		public void Complete() {
			Run();
		}

		public void Run() {
			Start();
			Wait();
		}

		public static ParallelTask<T> CreateTask(IEnumerable<T> list, Action<T> taskWork, int simultaneousTasks = 5, Action onCompleteAction = null) {
			return new ParallelTask<T>(list, taskWork, simultaneousTasks, onCompleteAction);
		}
	}
}
