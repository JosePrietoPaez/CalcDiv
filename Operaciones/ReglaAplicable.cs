using Operaciones.Recursos;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Clase abstracta que representa reglas que se pueden aplicar, generalmente de forma recursiva.
	/// </summary>
	/// <remarks>
	/// Ofrece métodos para estandarizar la forma de escribir las explicaciones.
	/// </remarks>
	public abstract class ReglaAplicable : IRegla {
		[JsonPropertyName("rule-explained")]
		public abstract string ReglaExplicada { get; }

		[JsonPropertyName("base")]
		public abstract long Base { get; }

		[JsonPropertyName("divisor")]
		public abstract long Divisor { get; }

		[JsonPropertyName("type")]
		public abstract CasosDivisibilidad Tipo { get; }

		public virtual string AplicarRegla(BigInteger dividendo) {
			StringBuilder sb = new();
			BigInteger dividendoMenor = BigInteger.Abs(dividendo),
				dividendoActual;
			int iteracionesRestantes = int.MaxValue; // Se cambiará si el actual es mayor que el menor y hará una cantidad limitada
			bool saltarBucle = false;
			InsertarMensajeBase(sb, dividendo);
			do {
				if (iteracionesRestantes < int.MaxValue) { // Iteraciones después de la primera
					sb.AppendFormat(TextoCalculos.MensajeAplicarRepeticion, dividendoMenor).AppendLine();
				}
				dividendoActual = BigInteger.Abs(ObtenerNuevoDividendo(dividendoMenor, sb));
				sb.AppendLine();
				if (iteracionesRestantes == 0) { // No debería haber dos mil millones de iteraciones, forzando a que se haya encontrado un dividendo mayor
					saltarBucle = true;
				}
				if (dividendoActual < dividendoMenor) {
					dividendoMenor = dividendoActual;
				} else if (dividendoActual == dividendoMenor) {
					sb.AppendFormat(TextoCalculos.MensajeAplicarDividendoIgual, dividendoActual).AppendLine();
					break;
				} else if (iteracionesRestantes > 2) {
					iteracionesRestantes = 2;
					sb.AppendFormat(TextoCalculos.MensajeAplicarMinimoEncontrado, dividendoActual, dividendoMenor).AppendLine();
				}
				iteracionesRestantes--;
				sb.AppendLine();
			} while (dividendoMenor > LimiteTrivialidadEstimado
			& !saltarBucle); // Mientras sea demasiado grande o no tengamos un mínimo que nos oblique a parar
			InsertarMensajeFin(sb, dividendo, dividendoMenor);
			return sb.ToString();
		}

		/// <summary>
		/// Esta propiedad indica una aproxima una estimación del punto donde se estima que el dividendo 
		/// es demasiado grande para que se pueda detener el procedimiento de la regla.
		/// </summary>
		/// <remarks>
		/// Puede cambiar con el tiempo.
		/// </remarks>
		protected long LimiteTrivialidadEstimado => 2 * Divisor * Base;

		protected void InsertarMensajeBase(StringBuilder sb, BigInteger dividendo) {
			if (Base != 10) {
				if (Base <= Calculos.BASE_64_STRING.Length) {
					sb.AppendFormat(TextoCalculos.MensajeAlfabetoNumericoExito, Base)
						.AppendLine()
						//No habrá problemas ya que si la base es demasiado grande para la conversión, no pasará por el if
						.AppendLine(Calculos.BASE_64_STRING[0..(int)Base]);
				} else {
					sb.AppendLine(TextoCalculos.MensajeAlfabetoNumericoExceso);
				}
				sb.AppendFormat(TextoCalculos.MensajeAplicarNumero, Base, LongAStringCondicional(dividendo), dividendo).AppendLine();
				sb.AppendLine();
			} 
		}

		protected void InsertarMensajeFin(StringBuilder sb, BigInteger dividendo, BigInteger equivalente) {
			if (equivalente <= LimiteTrivialidadEstimado) {
				sb.AppendFormat(TextoCalculos.MensajeAplicarFinPorTamaño, equivalente).AppendLine();
			} else {
				sb.AppendFormat(TextoCalculos.MensajeAplicarFinDemasiadoGrande, LongAStringCondicional(equivalente), Divisor).AppendLine();
			}
			if (equivalente % Divisor == 0) {
				sb.AppendFormat(TextoCalculos.MensajeAplicarFin, equivalente, Divisor, dividendo).AppendLine();
			} else {
				sb.AppendFormat(TextoCalculos.MensajeAplicarFinNoDivisible, equivalente, Divisor, dividendo).AppendLine();
			}
		}

		protected string LongAStringCondicional(BigInteger numero) {
			string resultado;
			if (Base <= Calculos.BASE_64_STRING.Length) {
				resultado = Calculos.LongToStringFast(numero, Base);
			} else {
				resultado = Calculos.LongToStringNoAlphabet(numero, Base);
			}
			return resultado;
		}

		/// <summary>
		/// Calcula un dividendo equivalente a <c>dividendo</c>,
		/// </summary>
		/// <param name="dividendo">Dividendo que se quiere reducir</param>
		/// <param name="sb">StringBuilder para guardar los mensajes</param>
		/// <returns>
		/// <see cref="BigInteger"/> igual de divisible entre <see cref="Divisor"/> que <c>dividendo</c>.
		/// </returns>
		protected abstract BigInteger ObtenerNuevoDividendo(BigInteger dividendo, StringBuilder sb);
	}
}
