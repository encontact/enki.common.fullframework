using System;
using System.Threading;

namespace Enki.Common
{
    /// <summary>
    /// Classe responsável por criar formas de facilitar o uso de chamadas assincronas no sistema.
    /// </summary>
    [Obsolete("Utilizar nova estrutura de Threads: ParallelTask, ThreadSafeQueue, SingleThread")]
    public static class Threading
    {
        /// <summary>
        /// Executa uma chamada dinâmica de forma assincrora.
        /// Exemplo de uso: RunAsynchronously( () => { Console.WriteLine("Inclua seus comandos"); }, () => { Console.WriteLine("Comandos terminados."); } );
        /// Exemplo sem callBack: RunAsynchronously( () => { Console.WriteLine("Inclua seus comandos"); }, null );
        /// </summary>
        /// <param name="method">Método a ser executado de forma assincrona.</param>
        /// <param name="callback">Método de Callback(Executado após termino da execução assincrona).</param>
        /// <exception cref="ThreadAbortException">Todas as exceções deste tipo são ignoradas e não executam callBack</exception>
        /// <exception cref="Exception">Todas as exceções deste tipo são ignoradas e não executam callBack</exception>
        public static void RunAsynchronously(Action method, Action callback)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    method();
                }
                catch (ThreadAbortException) { /* Exceção ignorada */ }
                catch (Exception) { /* Exceção ignorada */ }

                // Efetua a chamada da função de callback se não for nula.
                // Somente é executado se não houver exceção.
                if (callback != null) callback();
            });
        }
    }
}