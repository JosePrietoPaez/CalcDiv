using Operaciones;
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
			Func<long, long, int, IOpcionesGlobales, (Salida, object)> generadora = SeleccionarFuncionYAjustarFlags(flags);
			if (!flags.TipoExtra && (flags.DivisorDirecto < 2 || flags.BaseDirecto < 2 || !SonCoprimos(flags.DivisorDirecto, flags.BaseDirecto))) {
				Console.Error.WriteLine(ErrorDivisorCoprimo);
				Console.Error.WriteLine(ErrorBase);
				_estadoSalida = Salida.ERROR;
			} else {
				if (flags.DatosRegla.Count == 2) flags.Directo = flags.Directo!.Append(1);
				(_estadoSalida, object elementoCreado) = generadora(flags.DivisorDirecto, flags.BaseDirecto, flags.LongitudDirecta, flags);
				string textoResultado = CalcDivCLI.ObjetoAString(elementoCreado, flags.JSON);
				CalcDivCLI.EscribirReglaPorConsola(textoResultado, flags.DivisorDirecto, flags.BaseDirecto, flags.LongitudDirecta);
				if (!SonCoprimos(flags.DivisorDirecto, flags.BaseDirecto)) {
					Console.Error.WriteLine(ErrorPrimo);
				}
				if (flags.Dividendo?.Any() ?? false)
					AplicarReglaDivisibilidad((IRegla)elementoCreado, flags.Dividendo);
			}
			return _estadoSalida;
		}

		internal static Func<long, long, int, IOpcionesGlobales, (Salida, object)> SeleccionarFuncionYAjustarFlags(IOpcionesGlobales flags) {
			Func<long, long, int, IOpcionesGlobales, (Salida, object)> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			} else {
				resultado = CrearReglaCoeficientes;
			}
			return resultado;
		}

		internal static (Salida, object) CrearReglaCoeficientes(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
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

		internal static (Salida, object) CrearReglaExtra(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			var resultado = ReglaDivisibilidadExtendida(divisor, @base);
			Salida salida;
			if (resultado.Item1) {
				salida = Salida.CORRECTA;
			} else {
				salida = Salida.FRACASO_EXPANDIDA;
			}
			return (salida, resultado.Item2);
		}

		private static void AplicarReglaDivisibilidad(IRegla regla, IEnumerable<long> dividendos) {
			foreach (long dividendo in dividendos) {
				Console.Out.WriteLine(regla.AplicarRegla(dividendo));
			}
		}
	}
}
