using Listas;
using Operaciones;
using ProgramaDivisibilidadCLI.Recursos;

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
			if (flags.Directo.Count() == 2) flags.Directo = flags.Directo.Append(1);
			(bool exitoExtendido, string mensajeRegla, int informacion) reglas = (false, "", -1);
			if (flags.TipoExtra) {
				reglas = Calculos.ReglaDivisibilidadExtendida(flags.Divisor, flags.Base);
				Console.WriteLine(reglas.mensajeRegla);
				if (!reglas.exitoExtendido) {
					salida = SALIDA_FRACASO_EXPANDIDA;
				}
			}
			long mcd = Calculos.Mcd(flags.Divisor, flags.Base);
			if (!reglas.exitoExtendido && mcd == 1) { //Si falla al obtener reglas alternativas listas no se ha buscado, y base y divisor son coprimos
				Console.Error.WriteLine(string.Format(TextoResource.MensajeParametrosDirecto, flags.Divisor, flags.Base, flags.Coeficientes));
				CalcularReglaCoeficientes(flags.Divisor, flags.Base, flags.Coeficientes);
			} else if (mcd != 1) { //Si el divisor y la base no son coprimos
				Console.Error.WriteLine(string.Format(TextoResource.MensajeParametrosDirecto, flags.Divisor, flags.Base, flags.Coeficientes));
				ReferirAExtraYCalcularRegla(flags.Divisor, flags.Base, flags.Coeficientes);
			} else { //Si se ha obtenido una regla alternativa
				salida = SALIDA_CORRECTA;
			}
		}

		private static void ReferirAExtraYCalcularRegla(long divisor, long @base, int coeficientes) {
			string comando = $"-xd {divisor} {@base}";
			CasosDivisibilidad caso = Calculos.CasoEspecialRegla(divisor, @base).caso; // Para no tener que comprobarlos por separado
			if (flags.TipoExtra)
				switch (caso) {
					case CasosDivisibilidad.USAR_NORMAL:
						long mcd = Calculos.Mcd(@base, divisor);
						Console.Error.WriteLine(string.Format(TextoResource.ErrorPrimo, mcd, divisor / mcd));
						CalcularReglaCoeficientes(divisor / mcd, @base, coeficientes);
						break;
					case CasosDivisibilidad.SUMAR_BLOQUES or CasosDivisibilidad.RESTAR_BLOQUES: // Seguramente no se llegue nunca pero tendria que demostrarlo
						CalcularReglaCoeficientes(divisor, @base, coeficientes);
						break;
				} else
				switch (caso) {
					case CasosDivisibilidad.MIRAR_CIFRAS:
						Console.Error.WriteLine(TextoResource.DirectoReferirExtendidoPotencias + comando);
						break;
					case CasosDivisibilidad.USAR_NORMAL:
						long mcd = Calculos.Mcd(@base, divisor);
						Console.Error.WriteLine(TextoResource.DirectoReferirExtendidoValido);
						Console.Error.WriteLine(string.Format(TextoResource.ErrorPrimo, mcd, divisor / mcd));
						CalcularReglaCoeficientes(divisor / mcd, @base, coeficientes);
						break;
					case CasosDivisibilidad.SUMAR_BLOQUES or CasosDivisibilidad.RESTAR_BLOQUES: // Seguramente no se llegue nunca pero tendria que demostrarlo
						Console.Error.WriteLine(TextoResource.DirectoReferirExtendidoUsable + comando);
						CalcularReglaCoeficientes(divisor, @base, coeficientes);
						break;
					default:
						Console.Error.WriteLine(TextoResource.DirectoReferirExtendidoErrorInesperado + comando);
						break;
				}
		}

		/// <summary>
		/// Calcula las reglas de coeficientes especificadas en flags con los argumentos
		/// </summary>
		/// <remarks>
		/// Escribe por consola lo que sea necesario
		/// </remarks>
		private static void CalcularReglaCoeficientes(long divisor, long @base, int coeficientes) {
			string salidaConsola = ObtenerReglas(divisor, @base, coeficientes);
			Console.Error.WriteLine(TextoResource.MensajeFinDirecto);
			Console.Write(salidaConsola);
			salida = SALIDA_CORRECTA;
			Console.Error.WriteLine();
		}

		private static string ObtenerReglas(long divisor, long @base, int coeficientes) {
			string resultado;
			if (flags.Todos) { //Si se piden las 2^coeficientes reglas
				ListSerie<ListSerie<long>> series = new();
				Calculos.ReglasDivisibilidad(series, divisor, coeficientes, @base);
				if (flags.Nombre != "") {
					foreach (var serie in series) {
						serie.Nombre = flags.Nombre;
					}
				}
				resultado = SerieRectangularString(series);
			} else {
				ListSerie<long> serie = new(flags.Nombre);
				Calculos.ReglaDivisibilidadOptima(serie, divisor, coeficientes, @base);
				resultado = StringSerieConFlags(serie);
			}
			return resultado;
		}
	}
}
