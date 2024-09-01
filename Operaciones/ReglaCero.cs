using Operaciones.Recursos;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad para divisor cero.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo no se pueden aplicar.
	/// </remarks>
	public class ReglaCero : IRegla {
		private readonly long @base;

		internal ReglaCero(long @base) {
			this.@base = @base;
		}

		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada => TextoCalculos.ReglaExplicadaCero;

		[JsonPropertyName("base")]
		public long Base => @base;

		[JsonPropertyName("divisor")]
		public long Divisor => 0;

		[JsonPropertyName("type")]
		public CasosDivisibilidad Tipo => CasosDivisibilidad.DIVISOR_ZERO;

		public string AplicarRegla(BigInteger dividendo) => TextoCalculos.ReglaExplicadaCero;
		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
