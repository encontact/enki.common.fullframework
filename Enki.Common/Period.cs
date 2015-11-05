using System;

namespace Enki.Common
{
    /// <summary>
    /// Representa um período de tempo com possibilidade de início e fim.
    /// </summary>
    public class Period
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        /// <summary>
        /// Construtor padrão, recebendo início e fim do período.
        /// </summary>
        /// <param name="start">Opcional, data de início do período.</param>
        /// <param name="end">Opcional, data de término do período.</param>
        public Period(DateTime? start, DateTime? end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Verifica se dois períodos coincidem um com o outro.
        /// </summary>
        /// <param name="p">Período a ser comparado.</param>
        /// <returns>True se há coincidência dos períodos e False se não há.</returns>
        public bool Coincide(Period p)
        {
            if ((Start == null || !Start.HasValue) && (End == null || !End.HasValue)) return false;
            if ((p.Start == null || !p.Start.HasValue) && (p.End == null || !p.End.HasValue)) return false;
            if (Start == null || !Start.HasValue)
            {
                if (p.Start == null || !p.Start.HasValue)
                {
                    return p.End.Value <= End.Value;
                }
                if (p.End == null || !p.End.HasValue)
                {
                    return p.Start.Value <= End.Value;
                }
            }
            if (End == null || !End.HasValue)
            {
                if (p.Start == null || !p.Start.HasValue)
                {
                    return p.End.Value >= Start.Value;
                }
                if (p.End == null || !p.End.HasValue)
                {
                    return p.Start.Value >= Start.Value;
                }
            }
            if (p.Start == null || !p.Start.HasValue)
            {
                return p.End.Value >= Start.Value;
            }
            if (p.End == null || !p.End.HasValue)
            {
                return p.Start.Value <= End.Value;
            }
            return p.End.Value >= Start.Value && p.Start.Value <= (End ?? DateTime.Now);
        }

        /// <summary>
        /// Verifica se existe uma data dentro do período
        /// </summary>
        /// <param name="day">Data a ser verificada</param>
        /// <returns>True se a data pertence ao período e False se não pertence.</returns>
        public bool HasDay(DateTime day)
        {
            if (Start == null || !Start.HasValue)
            {
                return day <= End.Value;
            }
            if (End == null || !End.HasValue)
            {
                return day >= Start.Value;
            }
            return day >= Start.Value && day <= End.Value;
        }

        /// <summary>
        /// Efetua a validação do período informado.
        /// </summary>
        /// <exception cref="NoPeriodException">Exceção de período não informado.</exception>
        /// <exception cref="EndDateGreaterThenStartDateException">Exceção de data final maior que data inicial.</exception>
        public void Validate()
        {
            if (Start == null && End == null)
            {
                throw new NoPeriodException("Não houve período informado.");
            }
            if (Start != null && End != null)
            {
                if (Start >= End)
                {
                    throw new StartDateGreaterThenEndDateException("A data inicial não pode ser maior que a data final.");
                }
            }
        }

        #region Exceções da classe

        public class StartDateGreaterThenEndDateException : System.Exception
        {
            public StartDateGreaterThenEndDateException() : base()
            {
            }

            public StartDateGreaterThenEndDateException(string message) : base(message)
            {
            }

            public StartDateGreaterThenEndDateException(string message, System.Exception inner) : base(message, inner)
            {
            }
        }

        public class NoPeriodException : System.Exception
        {
            public NoPeriodException() : base()
            {
            }

            public NoPeriodException(string message) : base(message)
            {
            }

            public NoPeriodException(string message, System.Exception inner) : base(message, inner)
            {
            }
        }

        #endregion Exceções da classe
    }
}