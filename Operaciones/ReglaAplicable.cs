using Operaciones.Recursos;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	public abstract class ReglaAplicable : IRegla {
		[JsonPropertyName("rule-explained")]
		public abstract string ReglaExplicada { get; }

		[JsonPropertyName("base")]
		public abstract long Base { get; }

		[JsonPropertyName("divisor")]
		public abstract long Divisor { get; }

		[JsonPropertyName("type")]
		public abstract CasosDivisibilidad Tipo { get; }

		public abstract string AplicarRegla(long dividendo);

		/// <summary>
		/// Esta propiedad indica una aproxima una estimación del punto donde se estima que el dividendo 
		/// es demasiado grande para que se pueda detener el procedimiento de la regla.
		/// </summary>
		/// <remarks>
		/// Puede cambiar con el tiempo.
		/// </remarks>
		protected long LimiteTrivialidadEstimado => 2 * Divisor * Base;

		protected void InsertarMensajeBase(StringBuilder sb, long dividendo) {
			if (Base <= Calculos.BASE_64_STRING.Length) {
				sb.AppendFormat(TextoCalculos.MensajeAlfabetoNumericoExito, Base)
					.AppendLine()
					//No habrá problemas ya que si la base es demasiado grande para la conversión, no pasará por el if
					.AppendLine(Calculos.BASE_64_STRING[0..(int)Base]);
			} else {
				sb.AppendLine(TextoCalculos.MensajeAlfabetoNumericoExceso);
			}
			sb.AppendFormat(TextoCalculos.MensajeAplicarNumero, Base, LongAStringCondicional(dividendo)).AppendLine();
			sb.AppendLine();
		}

		protected void InsertarMensajeFin(StringBuilder sb, long dividendo, long equivalente) {
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

		protected string LongAStringCondicional(long numero) {
			string resultado;
			if (Base <= Calculos.BASE_64_STRING.Length) {
				resultado = Calculos.LongToStringFast(numero, Base);
			} else {
				resultado = Calculos.LongToStringNoAlphabet(numero, Base);
			}
			return resultado;
		}
	}
}
