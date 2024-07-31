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
	public class Regla([Range(0, long.MaxValue)] long divisor, [Range(2, long.MaxValue)] long @base, [Range(0,int.MaxValue)] int coeficientes, string nombre = "") : IEquatable<Regla?> {

		/// <summary>
		/// Crea una regla con los coeficientes ya obtenidos, no se comprueba que sean correctos
		/// </summary>
		/// <param name="divisor">Divisor de la regla</param>
		/// <param name="base">Base de la representación de los dividendos</param>
		/// <param name="coeficientes">Lista de coeficientes</param>
		/// <param name="nombre"></param>
		public Regla(long divisor, long @base, IEnumerable<long> coeficientes, string nombre = "") : this(divisor, @base, coeficientes.Count(), nombre) {
			if (!coeficientes.Any()) throw new ArgumentException(TextoCalculos.ReglaVaciaError, nameof(coeficientes));
			_coeficientes = new(coeficientes);
		}

		private List<long>? _coeficientes = null;

		private string _reglaExtra = string.Empty;
		private int _longitud = coeficientes;

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

		/// <summary>
		/// Esta propiedad indica la longitud de la regla, será 0 siempre que no se pueda calcular
		/// </summary>
		/// <remarks>
		/// Al cambiar la longitud, se tendrá que volver a acceder a <see cref="Coeficientes"/> para actualizarlo
		/// </remarks>
		[JsonIgnore]
		public int Longitud { 
			get {
				if (!Calculos.SonCoprimos(Divisor, Base))
					return 0;
				return _longitud;
			}
			set => _longitud = value; }
		/// <summary>
		/// Esta propiedad permite obtener y cambiar el nombre de la regla
		/// </summary>
		[JsonPropertyName("name")]
		public string Nombre { get; set; } = nombre;

		[JsonPropertyName("coefficients")]
		public List<long> Coeficientes { 
			get {
				if (Calculos.SonCoprimos(Divisor, Base)) {
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
			List<long> regla = Calculos.ReglaDivisibilidadOptima(Divisor, Longitud, Base).Coeficientes;
			Longitud = regla.Count;
			return regla;
		}

		public override string ToString() {
			return string.Join(", ", Coeficientes);
		}

		public string ToStringCompleto() {
			if (Nombre.Equals(string.Empty)) return ToString();
			StringBuilder sb = new();
			for (int i = 0; i < Longitud; i++) {
				sb.Append(Nombre).Append(Calculos.NumASubindice(i)).Append(" = ").Append(Coeficientes[i]);
				if (i + 1 != Longitud) {
					sb.Append(", ");
				}
			}
			return sb.ToString();
		}

		public override bool Equals(object? obj) {
			return Equals(obj as Regla);
		}

		public bool Equals(Regla? other) {
			return other is not null &&
				   EqualityComparer<List<long>>.Default.Equals(Coeficientes, other.Coeficientes);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Coeficientes);
		}

		public static bool operator ==(Regla? left, Regla? right) {
			return EqualityComparer<Regla>.Default.Equals(left, right);
		}

		public static bool operator !=(Regla? left, Regla? right) {
			return !(left == right);
		}
	}
}
