using Operaciones.Recursos;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad.
	/// </summary>
	/// <remarks>
	/// Esta clase contiene propiedades y métodos para gestionar su tipo y propiedades.
	/// </remarks>
	public class Regla([Range(0, long.MaxValue)] long divisor, [Range(2, long.MaxValue)] long @base, [Range(0,int.MaxValue)] int coeficientes, string nombre = "") {

		/// <summary>
		/// Crea una regla con los coeficientes ya obtenidos, no se comprueba que sean correctos
		/// </summary>
		/// <param name="divisor">Divisor de la regla</param>
		/// <param name="base">Base de la representación de los dividendos</param>
		/// <param name="coeficientes">Lista de coeficientes</param>
		/// <param name="nombre"></param>
		public Regla(long divisor, long @base, List<long> coeficientes, string nombre = "") : this(divisor, @base, coeficientes.Count, nombre) {
			if (coeficientes.Count == 0) throw new ArgumentException(TextoCalculos.ReglaVaciaError, nameof(coeficientes));
			_coeficientes = new(coeficientes);
		}

		private List<long>? _coeficientes = null;

		private string _reglaExtra = string.Empty;

		/// <summary>
		/// Esta propiedad permite obtener el divisor y recalcular la regla al cambiarlo
		/// </summary>
		[JsonPropertyName("divisor")]
		[Range(0, long.MaxValue)]
		public long Divisor => divisor;
		/// <summary>
		/// Esta propiedad permite obtener la base y recalcular la regla al cambiarlo
		/// </summary>
		[JsonPropertyName("base")]
		[Range(2, long.MaxValue)]
		public long Base => @base;

		public int Longitud { get; set; } = coeficientes;

		/// <summary>
		/// Esta propiedad permite obtener y cambiar el nombre de la regla
		/// </summary>
		[JsonPropertyName("name")]
		public string Nombre { get; set; } = nombre;

		[JsonPropertyName("coefficients")]
		public List<long> Coeficientes { 
			get {
				if (Calculos.Mcd(Base, Divisor) == 1) {
					if (_coeficientes is null || _coeficientes.Count < Longitud) {
						if (Divisor < 1) throw new InvalidOperationException();
						_coeficientes = CalcularRegla();
					}
					return _coeficientes[..Longitud];
				}
				return [];
			}
		}

		[JsonPropertyName("explained-rule")]
		public string ReglaExplicada {
			get {
				if (_reglaExtra.Equals(string.Empty)) {
					_reglaExtra = Calculos.ReglaDivisibilidadExtendida(Divisor, Base).Item2;
				}
				return _reglaExtra;
			}
		}

		private List<long> CalcularRegla() {
			return Calculos.ReglaDivisibilidadOptima(Divisor, Longitud, Base);
		}

		public override bool Equals(object? obj) {
			return obj is Regla regla &&
				   Divisor == regla.Divisor &&
				   Base == regla.Base &&
				   Longitud == regla.Longitud &&
				   Nombre == regla.Nombre;
		}

		public override int GetHashCode() {
			return HashCode.Combine(Divisor, Base, Longitud, Nombre);
		}

		public override string ToString() {
			return string.Join(", ", Coeficientes);
		}

		public string ToStringCompleto() {
			StringBuilder sb = new();
			for (int i = 0; i < Longitud; i++) {
				sb.Append(Nombre).Append(Calculos.NumASubindice(i)).Append('=').Append(Coeficientes[i]);
				if (i + 1 != Longitud) {
					sb.Append(", ");
				}
			}
			return sb.ToString();
		}
	}
}
