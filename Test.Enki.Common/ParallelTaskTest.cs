using Enki.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Test.Enki.Common {
	[TestClass]
	public class ParallelTaskTest {
		[TestMethod]
		public void TestProcessAllTasks() {
			var SumList = new List<Tuple<int, int, int>>() {
				new Tuple<int, int, int>(1,2,3),
				new Tuple<int, int, int>(5,2,7),
				new Tuple<int, int, int>(6,5,11),
				new Tuple<int, int, int>(2,2,4),
				new Tuple<int, int, int>(1,1,2),
				new Tuple<int, int, int>(5,5,10),
				new Tuple<int, int, int>(10,12,22),
				new Tuple<int, int, int>(5,4,9),
				new Tuple<int, int, int>(3,2,5),
				new Tuple<int, int, int>(12,12,24)
			};
			var ItensPerTask = 5;

			var task = ParallelTask<Tuple<int, int, int>>.CreateTask(SumList, item => {
				try {
					Assert.AreEqual(item.Item3, (item.Item1 + item.Item2));
				} catch {
					Assert.Fail("Erro ao processar soma de item.");
				}
			}, ItensPerTask);
			task.Run();
		}

		[TestMethod]
		public void TestProcessAllTasksWithOnComplete() {
			List<bool> resultItens = new List<bool>();
			var SumList = new List<Tuple<int, int, int>>() {
				new Tuple<int, int, int>(1,2,3),
				new Tuple<int, int, int>(5,2,7),
				new Tuple<int, int, int>(6,5,11),
				new Tuple<int, int, int>(2,2,4),
				new Tuple<int, int, int>(1,1,2),
				new Tuple<int, int, int>(5,5,10),
				new Tuple<int, int, int>(10,12,22),
				new Tuple<int, int, int>(5,4,9),
				new Tuple<int, int, int>(3,2,5),
				new Tuple<int, int, int>(12,12,24)
			};
			var ItensPerTask = 2;

			var task = ParallelTask<Tuple<int, int, int>>.CreateTask(
				SumList, 
				item => {
					// Lógica a ser processada para cada item da lista.
					try {
						Assert.AreEqual(item.Item3, (item.Item1 + item.Item2));
						resultItens.Add(true);
					} catch {
						Assert.Fail("Erro ao processar soma de item.");
						resultItens.Add(false);
					}
				}, 
				ItensPerTask, 
				() => {
					// Valida se todos os 10 itens foram contabilizados com sucesso no teste
					Assert.AreEqual(10, resultItens.Where(x => x == true).Count());
				}
			);
			task.Run();
		}

		[TestMethod]
		public void TestSimpleTaskOrder() {
			List<bool> resultItens = new List<bool>();
			var SumList = new List<Tuple<int, int, int>>() {
				new Tuple<int, int, int>(1,2,3),
				new Tuple<int, int, int>(5,2,7),
				new Tuple<int, int, int>(6,5,11),
				new Tuple<int, int, int>(2,2,4),
				new Tuple<int, int, int>(1,1,2),
				new Tuple<int, int, int>(5,5,10),
				new Tuple<int, int, int>(10,12,22),
				new Tuple<int, int, int>(5,4,9),
				new Tuple<int, int, int>(3,2,5),
				new Tuple<int, int, int>(12,12,24)
			};
			var ItensPerTask = 1;

			var ResultStringList = new List<string>() {
				"1+2=3 -> Iniciado",
				"1+2=3 -> Concluido",
				"5+2=7 -> Iniciado",
				"5+2=7 -> Concluido",
				"6+5=11 -> Iniciado",
				"6+5=11 -> Concluido",
				"2+2=4 -> Iniciado",
				"2+2=4 -> Concluido",
				"1+1=2 -> Iniciado",
				"1+1=2 -> Concluido",
				"5+5=10 -> Iniciado",
				"5+5=10 -> Concluido",
				"10+12=22 -> Iniciado",
				"10+12=22 -> Concluido",
				"5+4=9 -> Iniciado",
				"5+4=9 -> Concluido",
				"3+2=5 -> Iniciado",
				"3+2=5 -> Concluido",
				"12+12=24 -> Iniciado",
				"12+12=24 -> Concluido"
			};
			var ResultToCompareList = new List<string>();

			var task = ParallelTask<Tuple<int, int, int>>.CreateTask(
				SumList,
				item => {
					ResultToCompareList.Add(item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> Iniciado");
					Assert.AreEqual(item.Item3, (item.Item1 + item.Item2));
					ResultToCompareList.Add(item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> Concluido");
				},
				ItensPerTask,
				() => {
					for (int i = 0; i < ResultStringList.Count; i++) {
						Assert.AreEqual(ResultStringList[i], ResultToCompareList[i]);
					}
				}
			);
			task.Run();
		}

		[TestMethod]
		public void TestDoubleTasksOrder() {
			List<bool> resultItens = new List<bool>();
			var SumList = new List<Tuple<int, int, int>>() {
				new Tuple<int, int, int>(1,2,3),
				new Tuple<int, int, int>(5,2,7),
				new Tuple<int, int, int>(6,5,11),
				new Tuple<int, int, int>(2,2,4),
				new Tuple<int, int, int>(1,1,2),
				new Tuple<int, int, int>(5,5,10),
				new Tuple<int, int, int>(10,12,22),
				new Tuple<int, int, int>(5,4,9),
				new Tuple<int, int, int>(3,2,5),
				new Tuple<int, int, int>(12,12,24)
			};
			var threadQuantity = 2;

			var ResultStringList = new List<string>() {
				"1+2=3 -> Iniciado",
				"5+5=10 -> Iniciado",
				"1+2=3 -> Concluido",
				"5+2=7 -> Iniciado",
				"5+5=10 -> Concluido",
				"10+12=22 -> Iniciado",
				"5+2=7 -> Concluido",
				"6+5=11 -> Iniciado",
				"10+12=22 -> Concluido",
				"5+4=9 -> Iniciado",
				"6+5=11 -> Concluido",
				"2+2=4 -> Iniciado",
				"5+4=9 -> Concluido",
				"3+2=5 -> Iniciado",
				"2+2=4 -> Concluido",
				"1+1=2 -> Iniciado",
				"3+2=5 -> Concluido",
				"12+12=24 -> Iniciado",
				"1+1=2 -> Concluido",
				"12+12=24 -> Concluido"
			};

			var ResultToCompareList = new List<string>();

			var task = ParallelTask<Tuple<int, int, int>>.CreateTask(
				SumList,
				item => {
					ResultToCompareList.Add(item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> Iniciado");
					for (int i = 0; i < 1000; i++) {
						var x = item.Item1 + item.Item2;
						if (x == item.Item3) {
							Console.WriteLine("Calculo " + item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> OK");
						}
					}
					ResultToCompareList.Add(item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> Concluido");
				},
				threadQuantity,
				() => {
					CollectionAssert.AllItemsAreUnique(ResultToCompareList);
					for (int i = 0; i < ResultStringList.Count; i++) {
						var toCompare = ResultToCompareList[i];
						CollectionAssert.Contains(ResultStringList, toCompare);
					}
				}
			);
			task.Run();
		}

		[TestMethod]
		public void TestCancelTask() {
			List<bool> resultItens = new List<bool>();
			var SumList = new List<Tuple<int, int, int>>() {
				new Tuple<int, int, int>(1,2,3),
				new Tuple<int, int, int>(5,2,7),
				new Tuple<int, int, int>(6,5,11),
				new Tuple<int, int, int>(2,2,4),
				new Tuple<int, int, int>(1,1,2),
				new Tuple<int, int, int>(5,5,10),
				new Tuple<int, int, int>(10,12,22),
				new Tuple<int, int, int>(5,4,9),
				new Tuple<int, int, int>(3,2,5),
				new Tuple<int, int, int>(12,12,24)
			};
			var ItensPerTask = 1;

			var ResultStringList = new List<string>() {
				"1+2=3 -> Iniciado",
				"1+2=3 -> Concluido",
				"5+2=7 -> Iniciado",
				"5+2=7 -> Concluido",
				"6+5=11 -> Iniciado",
				"6+5=11 -> Concluido",
				"2+2=4 -> Iniciado",
				"2+2=4 -> Concluido",
				"1+1=2 -> Iniciado",
				"1+1=2 -> Concluido",
				"5+5=10 -> Iniciado",
				"5+5=10 -> Concluido",
				"10+12=22 -> Iniciado",
				"10+12=22 -> Concluido",
				"5+4=9 -> Iniciado",
				"5+4=9 -> Concluido",
				"3+2=5 -> Iniciado",
				"3+2=5 -> Concluido",
				"12+12=24 -> Iniciado",
				"12+12=24 -> Concluido"
			};
			var ResultToCompareList = new List<string>();

			var task = ParallelTask<Tuple<int, int, int>>.CreateTask(
				SumList,
				item => {
					ResultToCompareList.Add(item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> Iniciado");
					for (int i = 0; i < 100000; i++) {
						var x = item.Item1 + item.Item2;
						if (x == item.Item3) {
							Console.WriteLine("Calculo " + item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> OK");
						}
					}
					ResultToCompareList.Add(item.Item1 + "+" + item.Item2 + "=" + item.Item3 + " -> Concluido");
				},
				ItensPerTask,
				() => {
					for (int i = 0; i < ResultStringList.Count; i++) {
						Assert.AreEqual(ResultStringList[i], ResultToCompareList[i]);
					}
				}
			);
			task.Start();
			task.Cancel();
		}

	}
}
