using Operaciones.Recursos;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad que se basan en restar grupos de cifras.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo se pueden aplicar recursivamente.
	/// </remarks>
	public class ReglaRestar : ReglaAplicable {
		private readonly long _divisor, _base;
		private readonly int _longitud;

		public ReglaRestar(long divisor, long @base, int longitud) {
			ArgumentOutOfRangeException.ThrowIfNegative(longitud, nameof(longitud));
			ArgumentOutOfRangeException.ThrowIfLessThan(@base, 2, nameof(@base));
			ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 2, nameof(divisor));
			_divisor = divisor;
			_base = @base;
			_longitud = longitud;
		}

		[JsonPropertyName("rule-explained")]
		public override string ReglaExplicada {
			get {
				string potencia;
				try {
					long resultado = Calculos.PotenciaEntera(Base, Longitud) + 1;
					potencia = resultado.ToString(); //Puede dar overflow si el exponente es grande
				}
				catch (OverflowException) {
					potencia = TextoCalculos.CalculosExtendidaMensajeExceso;
				}
				return string.Format(TextoCalculos.CalculosExtendidaRestarPrincipio, Divisor, Base, Longitud, potencia)
					+ Environment.NewLine
					+ string.Format(TextoCalculos.ReglaExplicadaRestar, Divisor, Base, Longitud);
			}
		}

		[JsonPropertyName("base")]
		public override long Base => _base;

		[JsonPropertyName("divisor")]
		public override long Divisor => _divisor;

		/// <summary>
		/// Esta propiedad indica la longitud de los bloques de cifras que se deberán restar.
		/// </summary>
		[JsonPropertyName("block-length")]
		public int Longitud => _longitud;

		[JsonPropertyName("type")]
		public override CasosDivisibilidad Tipo => CasosDivisibilidad.SUBSTRACT_BLOCKS;

		public override string ToString() {
			return ReglaExplicada;
		}

		protected override long ObtenerNuevoDividendo(long dividendo, StringBuilder sb) {
			byte bloquesDividendo = (byte)(Calculos.Cifras(dividendo, Base) / Longitud + (Calculos.Cifras(dividendo, Base) % Longitud == 0 ? 0 : 1));
			long[] impares = new long[(bloquesDividendo / 2) + (bloquesDividendo & 1)]
				, pares = new long[bloquesDividendo / 2];
			sb.AppendFormat(TextoCalculos.MensajeAplicarRestaInicio, Divisor, Base, dividendo, Longitud).AppendLine();
			for (byte i = 0; i < bloquesDividendo / 2 * 2; i++) {
				long bloque = Calculos.IntervaloCifras(dividendo, Base, (byte)(i * Longitud), (byte)((i + 1) * Longitud));
				if ((i & 1) == 0) { // Si es par
					impares[i >> 1] = bloque;
				} else {
					pares[i >> 1] = bloque;
				}
			}
			long sumaImpares = 0, sumaPares = 0;
			if (impares.Length > pares.Length) {
				impares[^1] = Calculos.IntervaloCifras(dividendo, Base, (byte)((bloquesDividendo - 1) * Longitud), (byte)(bloquesDividendo * Longitud));
				sumaImpares += impares[^1];
			}
			sb.AppendFormat(TextoCalculos.MensajeAplicarRestaBloques, Longitud
				, string.Join(", ", impares.Select(LongAStringCondicional).Reverse())
				, string.Join(", ", pares.Select(LongAStringCondicional).Reverse())).AppendLine();
			
			for (byte i = 0; i < bloquesDividendo / 2; i++) {
				sumaImpares += impares[i];
				sumaPares += pares[i];
			}
			sb.AppendFormat(TextoCalculos.MensajeAplicarRestaSuma, sumaImpares, sumaPares).AppendLine();
			sb.AppendFormat(TextoCalculos.MensajeAplicarRestaResta, sumaImpares - sumaPares).AppendLine();
			return sumaPares - sumaImpares;
		}
	}
}