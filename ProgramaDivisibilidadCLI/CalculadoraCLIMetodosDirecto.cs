using Listas;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;
using System.Text;

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
			Func<long[],int> funcionEjecutada = SeleccionarFuncionYAjustarFlags();
			if (flags.VariasReglas?.Any() ?? false) {
				flags.DatosRegla = [1,1,1];
				EjectutarVarias(funcionEjecutada, flags.ListaDivisores, flags.ListaBases, flags.ListaCoeficientes);
			} else {
				if (flags.DatosRegla.Count == 2) flags.Directo = flags.Directo!.Append(1);
				_salida = funcionEjecutada([.. flags.DatosRegla]);
			}
		}

		private static Func<long[], int> SeleccionarFuncionYAjustarFlags() {
			Func<long[], int> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = MostrarReglaExtra;
				flags.ListaCoeficientes = [-1];
			} else {
				resultado = MostrarReglaCoeficientes;
			}
			return resultado;
		}

		private static int EjectutarVarias(Func<long[], int> funcionEjecutada, long[] divisores, long[] bases, int[] coeficientes) {
			int valorEjecucion = SALIDA_CORRECTA;
			bool hayFallo = false, hayExito = false;
			for (int i = 0; i < divisores.Length; i++) {
				List<long> datos = [ // Para no hacer un set de una nueva lista cada iteración
					divisores[i],
					bases[0],
					coeficientes[0]
				];
				for (int j = 0; j < bases.Length; j++) {
					datos[1] = bases[j];
					for (int k = 0; k < coeficientes.Length; k++) { // Bucle para todas las posibilidades
						datos[2] = coeficientes[k];
						flags.DatosRegla = new(datos);
						valorEjecucion = funcionEjecutada([divisores[i], bases[j], coeficientes[k]]); // La divisibilidad se maneja en el método
						if (!hayExito && valorEjecucion == SALIDA_CORRECTA) {
							hayExito = true;
						} else if (!hayFallo && valorEjecucion == SALIDA_ERROR) {
							hayFallo = true;
						}
					}
				}
			}
			if (!hayExito) {
				_escritorError.WriteLine(VariasMensajeErrorTotal);
				valorEjecucion = SALIDA_VARIAS_ERROR_TOTAL;
			} else if (hayFallo) {
				_escritorError.WriteLine(VariasMensajeError);
				valorEjecucion = SALIDA_VARIAS_ERROR;
			}
			return valorEjecucion;
		}

		private static int MostrarReglaExtra(long[] datos) {
			long divisor = datos[0], @base = datos[1];
			int valorEjecucion;
			(bool exitoExtendido, string mensajeRegla, int informacion) reglas;
			reglas = ReglaDivisibilidadExtendida(divisor, @base);
			_escritorSalida.WriteLine(reglas.mensajeRegla);
			_escritorSalida.WriteLine();
			if (!reglas.exitoExtendido) {
				long mcd = Mcd(divisor, @base);
				if (mcd != 1) {
					_escritorError.WriteLine(ErrorPrimo);
				}
				_escritorError.WriteLine(string.Format(MensajeParametrosDirecto, divisor/mcd, @base, 1));
				CalcularReglaCoeficientes(divisor, @base);
				valorEjecucion = SALIDA_FRACASO_EXPANDIDA;
			} else {
				valorEjecucion = SALIDA_CORRECTA;
			}
			return valorEjecucion;
		}

		private static int MostrarReglaCoeficientes(long[] datos) {
			long divisor = datos[0], @base = datos[1];
			int coeficientes = (int)datos[2], valorEjecucion;
			long mcd = Mcd(divisor, @base);
			if (mcd == 1) { //Si j y i son coprimos
				_escritorError.WriteLine(string.Format(MensajeParametrosDirecto, divisor, @base, coeficientes));
				CalcularReglaCoeficientes(divisor, @base, coeficientes);
				valorEjecucion = SALIDA_CORRECTA;
			} else { //Si el i y la j no son coprimos
				_escritorError.WriteLine(string.Format(MensajeParametrosDirecto, divisor, @base, coeficientes));
				_escritorError.WriteLine(ErrorPrimo);
				valorEjecucion = SALIDA_ERROR;
			}
			return valorEjecucion;
		}

		/// <summary>
		/// Calcula las reglas de coeficientes especificadas en flags con los argumentos
		/// </summary>
		/// <remarks>
		/// Escribe por consola lo que sea necesario
		/// </remarks>
		private static void CalcularReglaCoeficientes(long divisor, long @base, int coeficientes = 1) {
			string salidaConsola = ObtenerReglas(divisor, @base, coeficientes);
			_escritorError.WriteLine(MensajeFinDirecto);
			_escritorSalida.WriteLine(salidaConsola);
		}

		private static string ObtenerReglas(long divisor, long @base, int coeficientes) {
			string resultado;
			if (flags.Todos) { //Si se piden las 2^coeficientes reglas
				ListSerie<ListSerie<long>> series = new();
				ReglasDivisibilidad(series, divisor, coeficientes, @base);
				if (flags.Nombre != "") {
					foreach (var serie in series) {
						serie.Nombre = flags.Nombre ?? "";
					}
				}
				resultado = SerieRectangularString(series);
			} else {
				ListSerie<long> serie = new(flags.Nombre ?? "");
				ReglaDivisibilidadOptima(serie, divisor, coeficientes, @base);
				resultado = StringSerieConFlags(serie);
			}
			return resultado;
		}

		private static string SerieRectangularString(ListSerie<ListSerie<long>> serie) {
			if (flags.JSON) {
				return Serializar(serie);
			}
			StringBuilder stringBuilder = new();
			for (int i = 0; i < serie.Longitud - 1; i++) {
				stringBuilder.AppendLine(StringSerieConFlags(serie[i]));
			}
			stringBuilder.Append(StringSerieConFlags(serie[serie.Longitud - 1]));
			return stringBuilder.ToString();
		}
	}
}
