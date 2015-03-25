using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Hosting;

namespace Enki.Common {

	public static class SingleThread {
		private static List<Tuple<string, Thread, DateTime>> _nameThreadStartDate = new List<Tuple<string, Thread, DateTime>>();

		/// <summary>
		/// Efetua a criação de uma Thread única.
		/// </summary>
		/// <param name="functionName">Nome único da Thread para que ela não seja criada mais de uma vez.</param>
		/// <param name="work">Função a ser executada pela Thread.</param>
		/// <returns>Thread criada</returns>
		public static Thread Create(string functionName, ThreadStart work) {
			Thread ret = null;
			lock (_nameThreadStartDate) {
				if (Exists(functionName)) return null;
				ret = CreateThread(functionName, work);
				ret.Name = functionName;
				_nameThreadStartDate.Add(new Tuple<string, Thread, DateTime>(functionName, ret, DateTime.Now));
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
			try {
				if (!Exists(functionName)) return null;
				var tuple = _nameThreadStartDate.Find(n => n.Item1 == functionName);
				return tuple.Item2;
			} catch {
				return null;
			}
		}

		/// <summary>
		/// Recupera a lista de todas as Threads em fila.
		/// </summary>
		/// <returns>Lista encontrada.</returns>
		public static List<string> GetNames() {
			return new List<string>(_nameThreadStartDate.ConvertAll(m => m.Item1));
		}

		/// <summary>
		/// Efetua a contagem de Threads em andamento no sistema.
		/// </summary>
		/// <returns>Quantidade de threads registradas</returns>
		public static int ThreadsCount() {
			return _nameThreadStartDate.Count;
		}

		/// <summary>
		/// Solicita o encerramento de uma thread, interrompendo o processo pela metade.
		/// Utiliza Thread.Abort();
		/// </summary>
		/// <param name="functionName">Nome da thread única na fila.</param>
		public static void ForceStop(string functionName) {
			lock (_nameThreadStartDate) {
				if (!Exists(functionName)) return;
				var thisThread = Get(functionName);
				RemoveItem(functionName);
				thisThread.Abort();
			}
		}

		/// <summary>
		/// Solicita o encerramento de uma thread, interrompendo o processo.
		/// Utiliza Thread.Interrupt()
		/// </summary>
		/// <param name="functionName">Nome da thread única na fila.</param>
		public static void Stop(string functionName) {
			lock (_nameThreadStartDate) {
				if (!Exists(functionName)) return;
				var thisThread = Get(functionName);
				RemoveItem(functionName);
				thisThread.Interrupt();
			}
		}

		/// <summary>
		/// Retorna a situação atual da thread
		/// </summary>
		/// <param name="functionName">Nome da thread a ser verificada.</param>
		/// <returns>True se está ativa e false se não está</returns>
		public static bool IsAlive(string functionName) {
			try {
				if (!Exists(functionName)) return false;
				return Get(functionName).IsAlive;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Recupera a data e hora em que foi iniciada a Thread
		/// </summary>
		/// <param name="functionName">Nome da thread a ser verificada.</param>
		/// <returns>Data ou null se não encontrar</returns>
		public static DateTime? GetStartDate(string functionName) {
			try {
				if (!Exists(functionName)) return null;
				var tuple = _nameThreadStartDate.Find(m => m.Item1 == functionName);
				return tuple.Item3;
			} catch {
				return null;
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
					RemoveItem(functionName);
				}
				return;
			});
			ret.IsBackground = false;
			return ret;
		}

		/// <summary>
		/// Verifica se um item existe na listagem.
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		private static bool Exists(string functionName) {
			return _nameThreadStartDate.Exists(n => n.Item1 == functionName);
		}

		/// <summary>
		/// Remove o item com o nome informado da listagem
		/// </summary>
		/// <param name="functionName"></param>
		private static void RemoveItem(string functionName) {
			_nameThreadStartDate.RemoveAll(n => n.Item1 == functionName);
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