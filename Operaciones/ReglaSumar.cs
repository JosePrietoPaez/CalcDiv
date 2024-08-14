using Operaciones.Recursos;

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

		public string ReglaExplicada => string.Format(TextoCalculos.ReglaExplicadaSumar, Divisor, Base, Longitud);

		public long Base => _base;

		public long Divisor => _divisor;

		/// <summary>
		/// Esta propiedad indica la longitud de los bloques de cifras que se deberán sumar.
		/// </summary>
		public int Longitud => _longitud;

		public string AplicarRegla(long dividendo) {
			throw new NotImplementedException();
		}
	}
}
