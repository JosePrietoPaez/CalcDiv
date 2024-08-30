using Operaciones;
using ProgramaDivisibilidad.Recursos;
using System.Numerics;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;

namespace ProgramaDivisibilidad {
	class ModoDirecto : IModoEjecucion {

		private Salida _estadoSalida;

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// booleano que indica si ha conseguido calcular la regla.
		/// </returns>
		public Salida Ejecutar(IOpciones opciones) { //Intenta dar las reglas de forma directa, cambia _salida para mostrar el error
			OpcionesDirecto flags = (OpcionesDirecto) opciones;
			_estadoSalida = Salida.CORRECTA;
			Func<long, long, int, IOpcionesGlobales, (Salida, object?)> generadora = SeleccionarFuncionYAjustarFlags(flags);
			if (!flags.TipoExtra && (flags.Divisor < 2 || flags.Base < 2 || !SonCoprimos(flags.Divisor, flags.Base))) {
				Console.Error.WriteLine(ErrorDivisorCoprimo);
				Console.Error.WriteLine(ErrorBase);
				_estadoSalida = Salida.ERROR;
			} else {
				(_estadoSalida, object? elementoCreado) = generadora(flags.Divisor, flags.Base, flags.Longitud, flags);
				string textoResultado = CalcDivCLI.ObjetoAString(elementoCreado, flags.JSON);
				if (elementoCreado is null) {
					throw new NullReferenceException(ErrorReglaNula);
				}
				CalcDivCLI.EscribirReglaPorConsola(textoResultado, flags.Divisor, flags.Base, flags.Longitud);
				if (elementoCreado is not ReglaCoeficientes) {
					//Por si hiciera falta
				} else {
					if (!SonCoprimos(flags.Divisor, flags.Base)) {
						Console.Error.WriteLine(ErrorPrimo);
					}
				}
				if (flags.Dividendo?.Any() ?? false) {
					Console.Error.WriteLine();
					AplicarReglaDivisibilidad((IRegla)elementoCreado, flags.DividendoList);
				}
			}
			return _estadoSalida;
		}

		internal static Func<long, long, int, IOpcionesGlobales, (Salida, object?)> SeleccionarFuncionYAjustarFlags(IOpcionesGlobales flags) {
			Func<long, long, int, IOpcionesGlobales, (Salida, object?)> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			} else {
				resultado = CrearReglaCoeficientes;
			}
			return resultado;
		}

		internal static (Salida, object?) CrearReglaCoeficientes(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			Salida salida;
			object elementoCreado;
			if (!SonCoprimos(divisor,@base)) {
				salida = Salida.ERROR;
				elementoCreado = new ReglaCoeficientes(divisor, @base, coefientes);
			} else if (flags.Todos) {
				List<ReglaCoeficientes> listaAuxiliar = ReglasDivisibilidad(divisor, coefientes, @base);
				elementoCreado = listaAuxiliar;
				salida = Salida.CORRECTA;
			} else {
				ReglaCoeficientes reglaAuxiliar = ReglaDivisibilidadOptima(divisor, coefientes, @base);
				elementoCreado = reglaAuxiliar;
				salida = Salida.CORRECTA;
			}
			return (salida, elementoCreado);
		}

		internal static (Salida, object?) CrearReglaExtra(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			var resultado = ReglaDivisibilidadExtendida(divisor, @base);
			Salida salida;
			if (resultado.Item1) {
				salida = Salida.CORRECTA;
			} else {
				salida = Salida.FRACASO_EXPANDIDA;
			}
			return (salida, resultado.Item2);
		}

		private static void AplicarReglaDivisibilidad(IRegla regla, IEnumerable<BigInteger> dividendos) {
			foreach (BigInteger dividendo in dividendos) {
				Console.Out.WriteLine(regla.AplicarRegla(dividendo));
			}
		}
	}
}
