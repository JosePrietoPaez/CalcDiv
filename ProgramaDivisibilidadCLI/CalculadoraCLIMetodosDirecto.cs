using Operaciones;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;

namespace ProgramaDivisibilidad {
	public static partial class CalculadoraDivisibilidadCLI {

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// booleano que indica si ha conseguido calcular la regla.
		/// </returns>
		private static void IntentarDirecto() { //Intenta dar las reglas de forma directa, cambia _salida para mostrar el error
			_salida = SALIDA_CORRECTA;
			Func<long, long, int, (int, object)> generadora = SeleccionarFuncionYAjustarFlags();
			Func<object,string> consumidora = SeleccionarConsumidora();
			if (flags.VariasReglas?.Any() ?? false) {
				flags.DatosRegla = [1,1,1];
				_salida = EjectutarVarias(generadora, consumidora, flags.ListaDivisores, flags.ListaBases, flags.CoeficientesVarias);
			} else {
				if (flags.DivisorDirecto < 2 || flags.BaseDirecto < 2 || !SonCoprimos(flags.DivisorDirecto, flags.BaseDirecto)) {
					_escritorError.WriteLine(ErrorDivisorCoprimo);
					_escritorError.WriteLine(ErrorBase);
					_salida = SALIDA_ERROR;
				} else {
					if (flags.DatosRegla.Count == 2) flags.Directo = flags.Directo!.Append(1);
					(_salida, object elementoCreado) = generadora(flags.DivisorDirecto, flags.BaseDirecto, flags.LongitudDirecta);
					string textoResultado = ObjetoAString(elementoCreado);
					EscribirReglaPorWriter(textoResultado, _escritorSalida, _escritorError, flags.DivisorDirecto, flags.BaseDirecto, flags.LongitudDirecta);
					if (!SonCoprimos(flags.DivisorDirecto, flags.BaseDirecto)) {
						_escritorError.WriteLine(ErrorPrimo);
					}
					if (flags.Dividendo?.Any() ?? false)
						AplicarReglaDivisibilidad((IRegla)elementoCreado, flags.Dividendo);
				}
			}
		}

		private static int EjectutarVarias(Func<long, long, int, (int,object)> funcionGeneradora,
			Func<object, string> funcionConsumidora, long[] divisores, long[] bases, int coeficiente) {
			int valorEjecucion = SALIDA_CORRECTA;
			bool hayFallo = false, hayExito = false;
			List<object> reglas = new(divisores.Length * bases.Length); // Contendrá las listas o listas de listas
			foreach (long divisor in divisores) {
				foreach (long @base in bases) {
					flags.DatosRegla = [divisor, @base, coeficiente];
					(valorEjecucion, object nuevoElemento) = funcionGeneradora(divisor, @base, coeficiente); // La divisibilidad se maneja en el método
					reglas.Add(nuevoElemento);
					if (!hayExito && valorEjecucion == SALIDA_CORRECTA) {
						hayExito = true;
					} else if (!hayFallo && valorEjecucion != SALIDA_CORRECTA) {
						hayFallo = true;
					}
				}
			}
			if (!hayExito) { // Si no hay reglas, no se escriben
				_escritorError.WriteLine(VariasMensajeErrorTotal);
				valorEjecucion = SALIDA_VARIAS_ERROR_TOTAL;
			} else {
				if (hayFallo) {
					_escritorError.WriteLine(VariasMensajeError);
					valorEjecucion = SALIDA_VARIAS_ERROR;
				}
			}
			return valorEjecucion;
		}

		private static Func<object, string> SeleccionarConsumidora() {
			return (o) => "Sin implementar";
		}

		private static Func<long, long, int, (int, object)> SeleccionarFuncionYAjustarFlags() {
			Func<long, long, int, (int, object)> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			} else {
				resultado = CrearReglaCoeficientes;
			}
			return resultado;
		}

		private static (int, object) CrearReglaCoeficientes(long divisor, long @base, int coefientes) {
			int salida;
			object elementoCreado = new ReglaCoeficientes(divisor, @base, coefientes);
			if (!SonCoprimos(divisor,@base)) {
				salida = SALIDA_ERROR;
			} else if (flags.Todos) {
				List<ReglaCoeficientes> listaAuxiliar = ReglasDivisibilidad(divisor, coefientes, @base);
				elementoCreado = listaAuxiliar;
				salida = SALIDA_CORRECTA;
			} else {
				ReglaCoeficientes reglaAuxiliar = ReglaDivisibilidadOptima(divisor, coefientes, @base);
				elementoCreado = reglaAuxiliar;
				salida = SALIDA_CORRECTA;
			}
			return (salida, elementoCreado);
		}

		private static (int, object) CrearReglaExtra(long divisor, long @base, int coefientes) {
			var resultado = ReglaDivisibilidadExtendida(divisor, @base);
			int salida;
			if (resultado.Item1) {
				salida = SALIDA_CORRECTA;
			} else {
				salida = SALIDA_FRACASO_EXPANDIDA;
			}
			return (salida, resultado.Item2);
		}
	}
}
