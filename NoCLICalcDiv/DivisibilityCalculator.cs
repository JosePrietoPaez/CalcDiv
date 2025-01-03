using Operaciones;
using ModosEjecucion;
using static Divisibility.Resources.APIText;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Divisibility {
	public static class DivisibilityCalculator {

		/// <summary>
		/// Calculates divisibility rules based on the options.
		/// </summary>
		/// <remarks>
		/// Throws <see cref="NotImplementedException"/> if the options are not recognized or are known not to return a rule.
		/// </remarks>
		/// <param name="options"></param>
		/// <returns>
		/// <see cref="EstadoEjecucion"/> with the state of the execution and <see cref="IEnumerable{T}"/> with the calculated rules.
		/// </returns>
		/// <exception cref="NotImplementedException"></exception>
		public static (EstadoEjecucion, IEnumerable<IRegla>) CalculateRule(IOpciones options) {
			return options switch {
				OpcionesDirecto flags => new ModoDirecto().CalcularRegla(flags),
				OpcionesVarias flags => new ModoVarias().CalcularRegla(flags),
				_ => throw new NotImplementedException(InvalidOptionType),
			};
		}

		/// <summary>
		/// Calculates a single divisibility rule based on the divisor and base.
		/// </summary>
		/// <remarks>
		/// Optionally, the length of the rule can be given for coefficient rules, if set.
		/// </remarks>
		/// <param name="divisor"></param>
		/// <param name="base"></param>
		/// <param name="length"></param>
		/// <param name="coefficientRule"></param>
		/// <returns>
		/// <see cref="EstadoEjecucion"/> with the state of the execution and <see cref="IRegla"/> with the calculated rule.
		/// </returns>
		public static (EstadoEjecucion, IRegla) CalculateSingleRule(long divisor, long @base, bool coefficientRule = false, int length = 1) {
			OpcionesDirecto flags = new() {
				Base = @base,
				Divisor = divisor,
				Longitud = length,
				Dividendo = null,
				JSON = false,
				ReglasVariadas = !coefficientRule
			};
			var result = new ModoDirecto().CalcularRegla(flags);
			return (result.Item1, result.Item2.First());
		}

		/// <summary>
		/// Calculates multiple divisibility rules based on the divisors and bases.
		/// </summary>
		/// <remarks>
		/// Optionally, the length of the rules can be given for coefficient rules, if set.
		/// </remarks>
		/// <param name="divisors"></param>
		/// <param name="bases"></param>
		/// <param name="coefficientRule"></param>
		/// <param name="length"></param>
		/// <returns>
		/// <see cref="EstadoEjecucion"/> with the state of the execution and <see cref="IEnumerable{T}"/> with the calculated rules.
		/// </returns>
		public static (EstadoEjecucion, IEnumerable<IRegla>) CalculateMultipleRules(IEnumerable<long> divisors, IEnumerable<long> bases, bool coefficientRule = false, int length = 1) {
			OpcionesVarias flags = new() {
				Longitud = length,
				Dividendo = null,
				VariasReglas = [ string.Join(',', divisors), string.Join(',', bases) ],
				JSON = false,
				ReglasVariadas = !coefficientRule
			};
			return new ModoVarias().CalcularRegla(flags);
		}
	}
}
