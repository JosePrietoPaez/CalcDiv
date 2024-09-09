using Operaciones.Recursos;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad que se basan en sumar grupos de cifras.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo se pueden aplicar recursivamente.
	/// </remarks>
	public class ReglaSumar : ReglaAplicable {
		private readonly long _divisor;
		private readonly long _base;
		private readonly int _longitud;

		internal ReglaSumar(long divisor, long @base, int longitud) {
			ArgumentOutOfRangeException.ThrowIfNegative(longitud, nameof(longitud));
			ArgumentOutOfRangeException.ThrowIfLessThan(@base, 2, nameof(@base));
			ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 2, nameof(divisor));
			_longitud = longitud;
			_divisor = divisor;
			_base = @base;
		}

		[JsonPropertyName("rule-explained")]
		public override string ReglaExplicada {
			get {
				string potencia;
				try {
					BigInteger resultado = Calculos.PotenciaEnteraGrande(Base, Longitud) - 1;
					potencia = resultado.ToString(); //Puede dar overflow si el exponente es grande
				}
				catch {
					potencia = TextoCalculos.CalculosExtendidaMensajeExceso;
				}
				return string.Format(TextoCalculos.CalculosExtendidaSumarPrincipio, Divisor, Base, Longitud, potencia)
					+ Environment.NewLine
					+ string.Format(TextoCalculos.ReglaExplicadaSumar, Divisor, Base, Longitud);
			}
		}

		[JsonPropertyName("base")]
		public override long Base => _base;

		[JsonPropertyName("divisor")]
		public override long Divisor => _divisor;

		/// <summary>
		/// Esta propiedad indica la longitud de los bloques de cifras que se deberán sumar.
		/// </summary>
		[JsonPropertyName("block-length")]
		public int Longitud => _longitud;

		[JsonPropertyName("type")]
		public override CasosDivisibilidad Tipo => CasosDivisibilidad.ADD_BLOCKS;

		protected override BigInteger ObtenerNuevoDividendo(BigInteger dividendo, StringBuilder sb) {
			byte bloquesDividendo = (byte)(Calculos.Cifras(dividendo, Base) / Longitud + (Calculos.Cifras(dividendo, Base) % Longitud == 0 ? 0 : 1));
			BigInteger[] bloques = new BigInteger[bloquesDividendo];
			BigInteger suma = 0;
			sb.AppendFormat(TextoCalculos.MensajeAplicarRestaInicio, Divisor, Base, dividendo, Longitud).AppendLine();
			for (byte i = 0; i < bloquesDividendo; i++) {
				bloques[i] = Calculos.IntervaloCifras(dividendo, Base, i * Longitud, (i + 1) * Longitud);
				suma += bloques[i];
			}
			sb.AppendFormat(TextoCalculos.MensajeAplicarSumaBloques, Longitud
				, string.Join(", ", bloques.Select(LongAStringCondicional).Reverse())).AppendLine();
			sb.AppendFormat(TextoCalculos.MensajeAplicarSumaSuma, suma).AppendLine();
			return suma;
		}

		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
