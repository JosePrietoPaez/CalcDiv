using Operaciones.Recursos;
using System.Numerics;
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

		internal ReglaRestar(long divisor, long @base, int longitud) {
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
					BigInteger resultado = Calculos.PotenciaEnteraGrande(Base, Longitud) + 1;
					potencia = resultado.ToString(); //Puede dar overflow si el exponente es grande
				}
				catch {
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
		public override CasosDivisibilidad Tipo => CasosDivisibilidad.SUBTRACT_BLOCKS;

		[JsonPropertyName("error")]
		public override string Error => TextoCalculos.MensajeErrorNinguno;

		public override string ToString() {
			return ReglaExplicada;
		}

		protected override BigInteger ObtenerNuevoDividendo(BigInteger dividendo, StringBuilder sb) {
			long bloquesDividendo = Calculos.Cifras(dividendo, Base) / Longitud + (Calculos.Cifras(dividendo, Base) % Longitud == 0 ? 0 : 1);
			BigInteger[] impares = new BigInteger[(bloquesDividendo / 2) + (bloquesDividendo & 1)]
				, pares = new BigInteger[bloquesDividendo / 2];
			sb.AppendFormat(TextoCalculos.MensajeAplicarRestaInicio, Divisor, Base, dividendo, Longitud).AppendLine();
			for (byte i = 0; i < bloquesDividendo / 2 * 2; i++) {
				BigInteger bloque = Calculos.IntervaloCifras(dividendo, Base, (byte)(i * Longitud), (byte)((i + 1) * Longitud));
				if ((i & 1) == 0) { // Si es par
					impares[i >> 1] = bloque;
				} else {
					pares[i >> 1] = bloque;
				}
			}
			BigInteger sumaImpares = 0, sumaPares = 0;
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