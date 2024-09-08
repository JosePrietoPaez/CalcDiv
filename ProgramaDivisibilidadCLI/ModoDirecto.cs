using Operaciones;
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
			OpcionesDirecto flags = (OpcionesDirecto)opciones;
			_estadoSalida = Salida.CORRECTA;
			long @base = flags.Base ?? 10;
			Func<long, long, int, IOpcionesGlobales, (Salida, IRegla)> generadora = SeleccionarFuncionYAjustarFlags(flags);
			return GestionarErrorYUsarDatos(flags, @base, flags.Divisor, flags.Longitud ?? 1, generadora);
		}

		private Salida GestionarErrorYUsarDatos(OpcionesDirecto flags, long @base, long divisor, int longitud
			, Func<long, long, int, IOpcionesGlobales, (Salida, IRegla)> generadora) {
			if (!flags.TipoExtra && (divisor < 2 || @base < 2 || !SonCoprimos(divisor, @base))) {
				Console.Error.WriteLine(ErrorDivisorCoprimo);
				Console.Error.WriteLine(ErrorBase);
				_estadoSalida = Salida.ERROR;
			} else {
				(_estadoSalida, object? elementoCreado) = generadora(divisor, @base, longitud, flags);
				string textoResultado = CalcDivCLI.ObjetoAString(elementoCreado, flags.JSON);
				if (elementoCreado is null) {
					throw new NullReferenceException(ErrorReglaNula);
				}
				CalcDivCLI.EscribirReglaPorConsola(textoResultado, divisor, @base);
				if (elementoCreado is not ReglaCoeficientes) {
					//Por si hiciera falta
				} else {
					if (!SonCoprimos(divisor, @base)) {
						Console.Error.WriteLine(ErrorPrimo);
					}
				}
				if (flags.Dividendo?.Any() ?? false) {
					Console.Error.WriteLine();
					AplicarReglaPorObjeto(elementoCreado, flags);
				}
			}
			return _estadoSalida;
		}

		internal static void AplicarReglaPorObjeto(object? regla, IOpcionesGlobales flags) {
			if (regla is null) return;
			AplicarReglaDivisibilidad((IRegla)regla, flags.DividendoList);
		}

		internal static Func<long, long, int, IOpcionesGlobales, (Salida,IRegla)> SeleccionarFuncionYAjustarFlags(IOpcionesGlobales flags) {
			Func<long, long, int, IOpcionesGlobales, (Salida, IRegla)> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			} else {
				resultado = CrearReglaCoeficientes;
			}
			return resultado;
		}

		internal static (Salida, IRegla) CrearReglaCoeficientes(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			Salida salida;
			IRegla elementoCreado;
			if (!SonCoprimos(divisor,@base)) {
				salida = Salida.ERROR;
				elementoCreado = new ReglaCoeficientes(divisor, @base, coefientes);
			} else {
				ReglaCoeficientes reglaAuxiliar = ReglaDivisibilidadOptima(divisor, coefientes, @base);
				elementoCreado = reglaAuxiliar;
				salida = Salida.CORRECTA;
			}
			return (salida, elementoCreado);
		}

		internal static (Salida, IRegla) CrearReglaExtra(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
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

		private static Salida MostrarAyuda() {
			Console.Out.WriteLine(Ayuda);
			return Salida.CORRECTA;
		}
	}
}
