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
		private static void IntentarDirecto() { //Intenta dar las reglas de forma directa, cambia salida para mostrar el error
			salida = SALIDA_CORRECTA;
			if (flags.DatosRegla.Count == 2) flags.Directo = flags.Directo.Append(1);
			if (flags.TipoExtra) {
				MostrarReglaExtra(flags.Divisor, flags.Base);
			} else {
				MostrarReglaCoeficientes(flags.Divisor, flags.Base, flags.Coeficientes);
			}
		}

		private static void MostrarReglaExtra(long divisor, long @base) {
			(bool exitoExtendido, string mensajeRegla, int informacion) reglas;
			reglas = ReglaDivisibilidadExtendida(divisor, @base);
			Console.WriteLine(reglas.mensajeRegla);
			if (!reglas.exitoExtendido) {
				long mcd = Mcd(divisor, @base);
				if (mcd != 1) {
					Console.Error.WriteLine(ErrorPrimo);
				}
				Console.Error.WriteLine(string.Format(MensajeParametrosDirecto, divisor/mcd, @base, 1));
				CalcularReglaCoeficientes(divisor, @base);
				salida = SALIDA_FRACASO_EXPANDIDA;
			} else {
				salida = SALIDA_CORRECTA;
			}
		}

		private static void MostrarReglaCoeficientes(long divisor, long @base, int coeficientes) {
			long mcd = Mcd(flags.Divisor, flags.Base);
			if (mcd == 1) { //Si base y divisor son coprimos
				Console.Error.WriteLine(string.Format(MensajeParametrosDirecto, divisor, @base, coeficientes));
				CalcularReglaCoeficientes(flags.Divisor, flags.Base, flags.Coeficientes);
				salida = SALIDA_CORRECTA;
			} else { //Si el divisor y la base no son coprimos
				Console.Error.WriteLine(string.Format(MensajeParametrosDirecto, divisor, @base, coeficientes));
				Console.Error.WriteLine(ErrorPrimo);
				salida = SALIDA_ERROR;
			}
		}

		/// <summary>
		/// Calcula las reglas de coeficientes especificadas en flags con los argumentos
		/// </summary>
		/// <remarks>
		/// Escribe por consola lo que sea necesario
		/// </remarks>
		private static void CalcularReglaCoeficientes(long divisor, long @base, int coeficientes = 1) {
			string salidaConsola = ObtenerReglas(divisor, @base, coeficientes);
			Console.Error.WriteLine(MensajeFinDirecto);
			Console.Write(salidaConsola);
			Console.Error.WriteLine();
		}

		private static string ObtenerReglas(long divisor, long @base, int coeficientes) {
			string resultado;
			if (flags.Todos) { //Si se piden las 2^coeficientes reglas
				ListSerie<ListSerie<long>> series = new();
				ReglasDivisibilidad(series, divisor, coeficientes, @base);
				if (flags.Nombre != "") {
					foreach (var serie in series) {
						serie.Nombre = flags.Nombre;
					}
				}
				resultado = SerieRectangularString(series);
			} else {
				ListSerie<long> serie = new(flags.Nombre);
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
