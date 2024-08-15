using Operaciones.Recursos;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad para divisor cero.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo no se pueden aplicar.
	/// </remarks>
	internal class ReglaCero : IRegla {
		private readonly long divisor;
		private readonly long @base;

		public ReglaCero(long divisor, long @base) {
			if (divisor != 0) {
				throw new ArgumentException(TextoCalculos.ReglaConstructorRestriccionCero, nameof(divisor));
			}
			this.divisor = divisor;
			this.@base = @base;
		}

		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada => TextoCalculos.ReglaExplicadaCero;

		[JsonPropertyName("base")]
		public long Base => @base;

		[JsonPropertyName("divisor")]
		public long Divisor => divisor;

		[JsonPropertyName("type")]
		public CasosDivisibilidad Tipo => CasosDivisibilidad.DIVISOR_CERO;

		public string AplicarRegla(long dividendo) => TextoCalculos.ReglaExplicadaCero;
		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
