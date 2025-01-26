using Operaciones.Recursos;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad para divisor uno.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo no necesitan usarse, pero existen para que haya consistencia.
	/// </remarks>
	public class ReglaUno : IRegla {
		private readonly long _base;

		internal ReglaUno(long @base) {
			_base = @base;
		}

		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada => TextoCalculos.ReglaExplicadaUno;

		[JsonPropertyName("base")]
		public long Base => _base;

		[JsonPropertyName("divisor")]
		public long Divisor => 1;

		[JsonPropertyName("type")]
		public CasosDivisibilidad Tipo => CasosDivisibilidad.DIVISOR_ONE;

		[JsonPropertyName("error")]
		public string Error => TextoCalculos.MensajeErrorNinguno;

		public string AplicarRegla(BigInteger dividendo) => TextoCalculos.ReglaExplicadaUno;
		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
