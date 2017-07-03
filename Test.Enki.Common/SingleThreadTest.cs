using Enki.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Test.Enki.Common
{
    [TestClass]
    public class SingleThreadTest
    {
        private string firstThreadName = "FirstThreadName";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void TearDown()
        {
            while (SingleThread.IsAlive(firstThreadName)) ;
        }

        [TestMethod]
        public void TestUniquenessOfThread()
        {
            SingleThread.Create(firstThreadName, delegate
            {
                Thread.Sleep(1000);
                return;
            });

            SingleThread.Create(firstThreadName, delegate
            {
                Thread.Sleep(1000);
                return;
            });

            Assert.AreEqual(1, SingleThread.ThreadsCount());
        }

        [TestMethod]
        public void TestForceStop()
        {
            SingleThread.Create(firstThreadName, delegate
            {
                try
                {
                    while (true) ;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
                }
                return;
            });

            Assert.AreEqual(1, SingleThread.ThreadsCount());
            SingleThread.ForceStop(firstThreadName);
            Thread.Sleep(100);
            Assert.AreEqual(0, SingleThread.ThreadsCount());
        }

        [TestMethod]
        public void TestIsAlive()
        {
            SingleThread.Create(firstThreadName, delegate
            {
                try
                {
                    Thread.Sleep(1500);
                    Console.WriteLine("Internal code finished.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
                }
                return;
            });

            Assert.IsTrue(SingleThread.IsAlive(firstThreadName));

            while (SingleThread.IsAlive(firstThreadName)) ;

            Assert.IsFalse(SingleThread.IsAlive(firstThreadName));
        }

        [TestMethod]
        public void TestThreadFinalize()
        {
            SingleThread.Create(firstThreadName, delegate
            {
                try
                {
                    Thread.Sleep(1500);
                    Console.WriteLine("Internal code finished.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
                }
                return;
            });

            while (SingleThread.IsAlive(firstThreadName)) ;
            Thread.Sleep(500);
            Assert.IsFalse(SingleThread.IsAlive(firstThreadName));
            Assert.IsNull(SingleThread.Get(firstThreadName));
        }

        [TestMethod]
        public void TestThreadGetThreadStartDate()
        {
            SingleThread.Create(firstThreadName, delegate
            {
                try
                {
                    while (true) ;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Teste de SingleThread TestStop parou na Exception.", e);
                }
                return;
            });

            Thread.Sleep(2000);
            Assert.IsNotNull(SingleThread.GetStartDate(firstThreadName));
            Assert.IsTrue(SingleThread.GetStartDate(firstThreadName).Value.CompareTo(DateTime.Now.AddSeconds(-5)) > 0);
            SingleThread.ForceStop(firstThreadName);
        }

        [TestMethod]
        public void TestThreadOverhead()
        {
            var threadName = "NameThread";
            for (var i = 0; i < 1000; i++)
            {
                SingleThread.Create(string.Concat(threadName, i), delegate
                {
                    Thread.Sleep(5000);
                    return;
                });
            }
            for (var i = 1000; i > 0; i--)
            {
                SingleThread.Create(string.Concat(threadName, i), delegate
                {
                    Thread.Sleep(5000);
                    return;
                });
            }
            Assert.IsTrue(SingleThread.GetNames().Count > 0);
        }
    }
}