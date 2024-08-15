using Operaciones.Recursos;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad que se basan en reducir las cifras del dividendo.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo solo pueden usarse una vez.
	/// </remarks>
	public class ReglaCifras : IRegla {

		private readonly long _divisor, _base;
		private readonly int _cifras;

		public ReglaCifras(long divisor, long @base, int cifras) {
			ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 1);
			ArgumentOutOfRangeException.ThrowIfLessThan(@base, 2);
			ArgumentOutOfRangeException.ThrowIfLessThan(cifras, 1);
			_divisor = divisor;
			_base = @base;
			_cifras = cifras;
		}

		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada => string.Format(TextoCalculos.CalculosExtendidaMensajeCifrasPrincipio, _divisor, _base)
			+ Environment.NewLine
			+ string.Format(TextoCalculos.ReglaExplicadaCifras, Divisor, Cifras, Base);

		[JsonPropertyName("base")]
		public long Base => _base;

		[JsonPropertyName("divisor")]
		public long Divisor => _divisor;

		[JsonPropertyName("digits-used")]
		public int Cifras => _cifras;

		[JsonPropertyName("type")]
		public CasosDivisibilidad Tipo => CasosDivisibilidad.DIGITS;

		public string AplicarRegla(long dividendo) {
			throw new NotImplementedException();
		}
		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
