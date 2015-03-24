using Enki.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Test.Enki.Common {
	[TestClass]
	public class SingleThreadTest {
		private string firstThreadName = "FirstThreadName";

		[TestInitialize]
		public void Setup() {
		}

		[TestCleanup]
		public void TearDown() {
			while (SingleThread.IsAlive(firstThreadName)) ;
		}

		[TestMethod]
		public void TestUniquenessOfThread() {
			SingleThread.Create(firstThreadName, delegate {
				Thread.Sleep(1000);
				return;
			});


			SingleThread.Create(firstThreadName, delegate {
				Thread.Sleep(1000);
				return;
			});

			Assert.AreEqual(1, SingleThread.ThreadsCount());
		}

		[TestMethod]
		public void TestForceStop() {
			var threadName = "Eternal";
			SingleThread.Create(threadName, delegate {
				try {
					while (true) ;
				} catch (Exception e) {
					Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
				}
				return;
			});

			Assert.AreEqual(1, SingleThread.ThreadsCount());
			SingleThread.ForceStop(threadName);
			Thread.Sleep(100);
			Assert.AreEqual(0, SingleThread.ThreadsCount());
		}

		[TestMethod]
		public void TestIsAlive() {
			var countSize = 1000;
			SingleThread.Create(firstThreadName, delegate {
				try {
					var count = 0;
					while (count < countSize) {
						count++;
					}
					Console.WriteLine("Internal code finished.");
				} catch (Exception e) {
					Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
				}
				return;
			});

			Assert.AreEqual(1, SingleThread.ThreadsCount());
			Assert.IsTrue(SingleThread.IsAlive(firstThreadName));

			while (SingleThread.IsAlive(firstThreadName)) ;

			Assert.IsFalse(SingleThread.IsAlive(firstThreadName));
		}

		[TestMethod]
		public void TestThreadFinalize() {
			var countSize = 1000;
			SingleThread.Create(firstThreadName, delegate {
				try {
					var count = 0;
					while (count < countSize) {
						count++;
					}
					Console.WriteLine("Internal code finished.");
				} catch (Exception e) {
					Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
				}
				return;
			});

			Assert.AreEqual(1, SingleThread.ThreadsCount());
			while (SingleThread.IsAlive(firstThreadName)) ;
			Thread.Sleep(500);
			Assert.IsFalse(SingleThread.IsAlive(firstThreadName));
			Assert.IsNull(SingleThread.Get(firstThreadName));			
		}
	}
}
