using Operaciones.Recursos;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad para divisor uno.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo no necesitan usarse, pero existen para que haya consistencia.
	/// </remarks>
	internal class ReglaUno : IRegla {
		private readonly long divisor;
		private readonly long @base;

		public ReglaUno(long divisor, long @base) {
			if (divisor != 1) {
				throw new ArgumentException(TextoCalculos.ReglaConstructorRestriccionCero, nameof(divisor));
			}
			this.divisor = divisor;
			this.@base = @base;
		}

		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada => TextoCalculos.ReglaExplicadaUno;

		[JsonPropertyName("base")]
		public long Base => @base;

		[JsonPropertyName("divisor")]
		public long Divisor => divisor;

		public string AplicarRegla(long dividendo) => TextoCalculos.ReglaExplicadaUno;
	}
}
