using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Hosting;

namespace Enki.Common {

	public static class SingleThread {
		private static Dictionary<string, Thread> _threads = new Dictionary<string, Thread>();

		/// <summary>
		/// Efetua a criação de uma Thread única.
		/// </summary>
		/// <param name="functionName">Nome único da Thread para que ela não seja criada mais de uma vez.</param>
		/// <param name="work">Função a ser executada pela Thread.</param>
		/// <returns>Thread criada</returns>
		public static Thread Create(string functionName, ThreadStart work) {
			Thread ret = null;
			lock (_threads) {
				if (_threads.ContainsKey(functionName)) return null;
				ret = CreateThread(functionName, work);
				ret.Name = functionName;
				_threads.Add(functionName, ret);
			}
			if (ret != null) ret.Start();
			return ret;
		}

		/// <summary>
		/// Recupera uma determinada thread da fila.
		/// </summary>
		/// <param name="functionName">Nome da thread a ser recuperada.</param>
		/// <returns></returns>
		public static Thread Get(string functionName) {
			if (!_threads.ContainsKey(functionName)) return null;
			return _threads[functionName];
		}

		/// <summary>
		/// Recupera a lista de todas as Threads em fila.
		/// </summary>
		/// <returns>Lista encontrada.</returns>
		public static List<string> GetNames() {
			return new List<string>(_threads.Keys);
		}

		/// <summary>
		/// Efetua a contagem de Threads em andamento no sistema.
		/// </summary>
		/// <returns>Quantidade de threads registradas</returns>
		public static int ThreadsCount() {
			return _threads.Count;
		}

		/// <summary>
		/// Solicita o encerramento de uma thread, interrompendo o processo pela metade.
		/// Utiliza Thread.Abort();
		/// </summary>
		/// <param name="functionName">Nome da thread única na fila.</param>
		public static void ForceStop(string functionName) {
			lock (_threads) {
				if (!_threads.ContainsKey(functionName)) return;
				var thisThread = _threads[functionName];
				_threads.Remove(functionName);
				thisThread.Abort();
			}
		}

		/// <summary>
		/// Solicita o encerramento de uma thread, interrompendo o processo.
		/// Utiliza Thread.Interrupt()
		/// </summary>
		/// <param name="functionName">Nome da thread única na fila.</param>
		public static void Stop(string functionName) {
			lock (_threads) {
				if (!_threads.ContainsKey(functionName)) return;
				var thisThread = _threads[functionName];
				_threads.Remove(functionName);
				thisThread.Interrupt();
			}
		}

		/// <summary>
		/// Retorna a situação ataul da thread
		/// </summary>
		/// <param name="functionName">Nome da thread a ser verificada.</param>
		/// <returns>True se está ativa e false se não está</returns>
		public static bool IsAlive(string functionName) {
			try {
				if (!_threads.ContainsKey(functionName)) return false;
				return _threads[functionName].IsAlive;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Método interno para criação da Thread
		/// </summary>
		/// <param name="functionName">Nome único da thread</param>
		/// <param name="work">Lógica a ser processada</param>
		/// <returns>Thread criada.</returns>
		private static Thread CreateThread(string functionName, ThreadStart work) {
			var ret = new Thread((ThreadStart)delegate {
				var job = new JobHost();
				try {
					job.DoWork(() => work());
				} finally {
					job.Stop(true);
					_threads.Remove(functionName);
				}
				return;
			});
			ret.IsBackground = false;
			return ret;
		}
	}

	public class JobHost : IRegisteredObject {
		private readonly object _lock = new object();
		private bool _shuttingDown;

		public JobHost() {
			HostingEnvironment.RegisterObject(this);
		}

		public void Stop(bool immediate) {
			lock (_lock) {
				_shuttingDown = true;
			}
			HostingEnvironment.UnregisterObject(this);
		}

		public void DoWork(Action work) {
			lock (_lock) {
				if (_shuttingDown) {
					return;
				}
				work();
			}
		}
	}
}