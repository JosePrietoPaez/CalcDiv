using Operaciones.Recursos;

namespace Operaciones {
		/// <summary>
		/// Los objetos de esta clase representan reglas de divisibilidad que se basan en restar grupos de cifras.
		/// </summary>
		/// <remarks>
		/// Las reglas de este tipo se pueden aplicar recursivamente.
		/// </remarks>
	public class ReglaRestar : IRegla {
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

		public string ReglaExplicada => string.Format(TextoCalculos.ReglaExplicadaRestar, Divisor, Base, Longitud);

		public long Base => _base;

		public long Divisor => _divisor;

		/// <summary>
		/// Esta propiedad indica la longitud de los bloques de cifras que se deberán restar.
		/// </summary>
		public int Longitud => _longitud;

		public string AplicarRegla(long dividendo) {
			throw new NotImplementedException();
		}
	}
}