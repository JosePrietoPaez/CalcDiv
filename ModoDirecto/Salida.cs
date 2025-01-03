using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using System.Text;
using static ModosEjecucion.Recursos.TextoEjecucion;
using System.Collections.Generic;
using System;
using System.IO;

namespace ModosEjecucion {
	public enum EstadoEjecucion {
		CORRECTA = 0,
		ERROR = 1,
		ENTRADA_MALFORMADA = 2,
		VOLUNTARIA = 3,
		FRACASO_EXPANDIDA = 4,
		VARIAS_ERROR = 5,
		VARIAS_ERROR_TOTAL = 6
	}

	public class Salida(EstadoEjecucion estado = EstadoEjecucion.CORRECTA) {
		public EstadoEjecucion Estado { get; set; } = estado;

		public List<(TextWriter, string, bool)> Mensajes { get; set; } = [];

		public void EscribirMensajes() {
			foreach ((TextWriter writer, string mensaje, bool nuevaLinea) in Mensajes) {
				if (mensaje.Equals(Environment.NewLine)) {
					writer.WriteLine();
				} else if (mensaje.Length > 0) {
					if (nuevaLinea) {
						writer.WriteLine(mensaje);
					} else {
						writer.Write(mensaje);
					}
				}
			}
		}

		public static readonly JsonSerializerOptions opcionesJson = new() {
			WriteIndented = true,
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper) },
			PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower
		};

		/// <summary>
		/// Devuelve un string para la consola, depende del tipo del objeto
		/// </summary>
		/// <returns>
		/// String apropiado según el tipo del objeto y el estado del programa
		/// </returns>
		public static string ObjetoAString(object? obj, bool json = false) {
			if (obj == null) return ObjetoNuloMensaje;
			if (json) return JsonSerializer.Serialize(obj, opcionesJson);
			string resultadoObjeto = obj switch {
				// Para una regla de reglasObj de coeficientes obtenidas de una regla, usa recursión
				IEnumerable<object?> or IEnumerable<object> => EnumerableStringSeparadoLinea((IEnumerable<object>)obj),//Se juntan los casos para que sean separados por la recursión
				_ => obj.ToString() ?? ObjetoNuloMensaje,
			};
			return resultadoObjeto;
		}

		/// <summary>
		/// Devuelve un <see langword="string"></see> con los elementos de un <see cref="IEnumerable{T}"/> separados por una nueva línea.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable"></param>
		/// <returns>
		/// <see langword="string"/> con los elementos de <paramref name="enumerable"/> separados por una nueva línea.
		/// </returns>
		public static string EnumerableStringSeparadoLinea<T>(IEnumerable<T> enumerable) {
			StringBuilder stringBuilder = new();
			foreach (T item in enumerable) {
				stringBuilder.AppendLine(ObjetoAString(item));
			}
			stringBuilder.Remove(stringBuilder.Length - Environment.NewLine.Length, Environment.NewLine.Length); // Podría cambiar el foreach para no hacer esto pero seria mas complicado
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Escribe la regla de coeficientes por el <see cref="TextWriter"/> proporcionado
		/// </summary>
		public void EscribirReglaPorConsola(string reglaCoeficientes, long divisor, long @base, TextWriter salida, TextWriter error) {
			Mensajes.Add((error, string.Format(MensajeParametrosDirecto, divisor, @base), true));
			Mensajes.Add((salida, reglaCoeficientes, false));
			Mensajes.Add((error, Environment.NewLine, true));
		}

	}
}
