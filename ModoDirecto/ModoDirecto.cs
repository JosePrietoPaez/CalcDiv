using Operaciones;
using System.Numerics;
using static Operaciones.Calculos;
using static ModosEjecucion.Recursos.TextoEjecucion;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ModosEjecucion {
	public class ModoDirecto : IModoEjecucion {

		private readonly Output _estadoSalida = new();

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// <see cref="Output"/> que indica si ha conseguido calcular la regla.
		/// </returns>
		public Output Ejecutar(TextWriter salida, TextWriter error, IOpciones opciones) {
			OpcionesDirecto flags = (OpcionesDirecto)opciones;
			Func<long, long, int, IOpcionesGlobales, (ExitState, IRegla)> generadora = SeleccionarFuncionYAjustarFlags(flags);
			return GestionarErrorYUsarDatos(flags, flags.Base, flags.Divisor, flags.Longitud ?? 1, generadora, salida, error);
		}

		private Output GestionarErrorYUsarDatos(OpcionesDirecto flags, long @base, long divisor, int longitud
			, Func<long, long, int, IOpcionesGlobales, (ExitState, IRegla)> generadora
			, TextWriter salida, TextWriter error) {
			if (flags.ReglasCoeficientes && (divisor < 2 || @base < 2 || !SonCoprimos(divisor, @base))) {
				_estadoSalida.Mensajes.Add((error, ErrorDivisorCoprimo, true));
				_estadoSalida.Mensajes.Add((error, ErrorBase, true));
				_estadoSalida.Estado = ExitState.ERROR;
			} else {
				(_estadoSalida.Estado, object? elementoCreado) = generadora(divisor, @base, longitud, flags);
				string textoResultado = Output.ObjetoAString(elementoCreado, flags.JSON);
				if (elementoCreado is null) {
					throw new NullReferenceException(ErrorReglaNula);
				}
				_estadoSalida.EscribirReglaPorConsola(textoResultado, divisor, @base, salida, error);
				if (elementoCreado is not ReglaCoeficientes) {
					//Por si hiciera falta
				} else {
					if (!SonCoprimos(divisor, @base)) {
						_estadoSalida.Mensajes.Add((error, ErrorPrimo, true));
					}
				}
				if (flags.Dividendo?.Any() ?? false) {
					_estadoSalida.Mensajes.Add((salida, Environment.NewLine, true));
					AplicarReglaPorObjeto(elementoCreado, flags, _estadoSalida, salida);
				}
			}
			return _estadoSalida;
		}

		internal static void AplicarReglaPorObjeto(object? regla, IOpcionesGlobales flags, Output salida, TextWriter text) {
			if (regla is null) return;
			AplicarReglaDivisibilidad((IRegla)regla, flags.DividendoList, salida, text);
		}

		internal static Func<long, long, int, IOpcionesGlobales, (ExitState,IRegla)> SeleccionarFuncionYAjustarFlags(IOpcionesGlobales flags) {
			Func<long, long, int, IOpcionesGlobales, (ExitState, IRegla)> resultado;
			if (flags.ReglasCoeficientes) {
				resultado = CrearReglaCoeficientes;
			} else { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			}
			return resultado;
		}

		internal static (ExitState, IRegla) CrearReglaCoeficientes(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			ExitState salida;
			IRegla elementoCreado;
			if (SonCoprimos(divisor, @base)) {
				ReglaCoeficientes reglaAuxiliar = ReglaDivisibilidadOptima(divisor, coefientes, @base);
				elementoCreado = reglaAuxiliar;
				salida = ExitState.NO_ERROR;
			} else {
				salida = ExitState.ERROR;
				elementoCreado = new ReglaCoeficientes(divisor, @base, coefientes);
			}
			return (salida, elementoCreado);
		}

		internal static (ExitState, IRegla) CrearReglaExtra(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			var resultado = ReglaDivisibilidadExtendida(divisor, @base);
			ExitState salida;
			if (resultado.Item1) {
				salida = ExitState.NO_ERROR;
			} else {
				salida = ExitState.VARIED_RULE_ERROR;
			}
			return (salida, resultado.Item2);
		}

		private static void AplicarReglaDivisibilidad(IRegla regla, IEnumerable<BigInteger> dividendos, Output salida, TextWriter text) {
			foreach (BigInteger dividendo in dividendos) {
				salida.Mensajes.Add((text, regla.AplicarRegla(dividendo), true));
			}
		}

		public (ExitState, IEnumerable<IRegla>) CalcularRegla(IOpciones opciones) {
			OpcionesDirecto flags = (OpcionesDirecto)opciones;
			Func<long, long, int, IOpcionesGlobales, (ExitState, IRegla)> generadora = SeleccionarFuncionYAjustarFlags(flags);
			var resultado = generadora(flags.Divisor, flags.Base, flags.Longitud ?? 1, flags);
			return (resultado.Item1, [resultado.Item2]);
		}
	}
}
