using Operaciones.Recursos;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad que se basan en sumar grupos de cifras.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo se pueden aplicar recursivamente.
	/// </remarks>
	internal class ReglaSumar : IRegla {
		private readonly long _divisor;
		private readonly long _base;
		private readonly int _longitud;

		public ReglaSumar(long divisor, long @base, int longitud) {
			ArgumentOutOfRangeException.ThrowIfNegative(longitud, nameof(longitud));
			ArgumentOutOfRangeException.ThrowIfLessThan(@base, 2, nameof(@base));
			ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 2, nameof(divisor));
			_longitud = longitud;
			_divisor = divisor;
			_base = @base;
		}

		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada {
			get {
				string potencia;
				try {
					long resultado = Calculos.PotenciaEntera(Base, Longitud) - 1;
					potencia = resultado.ToString(); //Puede dar overflow si el exponente es grande
				}
				catch (OverflowException) {
					potencia = TextoCalculos.CalculosExtendidaMensajeExceso;
				}
				return string.Format(TextoCalculos.CalculosExtendidaSumarPrincipio, Divisor, Base, Longitud, potencia)
					+ Environment.NewLine
					+ string.Format(TextoCalculos.ReglaExplicadaSumar, Divisor, Base, Longitud);
			}
		}

		[JsonPropertyName("base")]
		public long Base => _base;

		[JsonPropertyName("divisor")]
		public long Divisor => _divisor;

		/// <summary>
		/// Esta propiedad indica la longitud de los bloques de cifras que se deberán sumar.
		/// </summary>
		[JsonPropertyName("block-length")]
		public int Longitud => _longitud;

		[JsonPropertyName("type")]
		public CasosDivisibilidad Tipo => CasosDivisibilidad.ADD_BLOCKS;

		public string AplicarRegla(long dividendo) {
			throw new NotImplementedException();
		}
		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
