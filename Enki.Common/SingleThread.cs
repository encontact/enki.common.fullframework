using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Hosting;

namespace Enki.Common {
	public static class SingleThread {

		private static Dictionary<string, Thread> _threads = new Dictionary<string, Thread>();

		public static Thread Create(string functionName, ThreadStart work) {
			var ret =  ModifyThread(functionName, work, ThreadAction.Add);
			if (ret != null) ret.Start();
			return ret;
		}

		private static Thread ModifyThread(string functionName, ThreadStart work, ThreadAction action) {
			lock (_threads) {
				switch (action) {
					case ThreadAction.Add:
						if (_threads.ContainsKey(functionName)) return null;
						var ret = CreateThread(functionName,work);
						_threads.Add(functionName, ret);
						return ret;
					case ThreadAction.Remove:
						if (!_threads.ContainsKey(functionName)) return null;
						_threads.Remove(functionName);
						break;
				}
			}
			return null;
		}

		private static Thread CreateThread(string functionName, ThreadStart work) {
			var ret = new Thread((ThreadStart)delegate {
				try {
					new JobHost().DoWork(() => work());
				} finally {
					ModifyThread(functionName, null, ThreadAction.Remove);
				}
			});
			ret.IsBackground = false;
			return ret;
		}

		private enum ThreadAction{
			Add,
			Remove
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
