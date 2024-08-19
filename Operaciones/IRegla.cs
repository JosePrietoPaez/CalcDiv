using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad.
	/// </summary>
	/// <remarks>
	/// Esta interfaz contiene propiedades y métodos para gestionar su tipo y propiedades.
	/// <para>
	/// Tiene un método fábrica para crear subclases a partir del tipo: <see cref="GenerarReglaPorTipo(CasosDivisibilidad, long, long)"/>
	/// </para>
	/// </remarks>
	public interface IRegla {

		/// <summary>
		/// Esta propiedad tiene la explicación de como usar la regla para averiguar si un dividendo es divisible entre el divisor.
		/// </summary>
		[JsonPropertyName("rule-explained")]
		public string ReglaExplicada { get; }

		/// <summary>
		/// Esta propiedad permite obtener la base de la regla.
		/// </summary>
		[JsonPropertyName("base")]
		public long Base { get; }

		/// <summary>
		/// Esta propiedad permite obtener el divisor de la regla.
		/// </summary>
		[JsonPropertyName("divisor")]
		public long Divisor { get; }

		/// <summary>
		/// Esta propiedad indica el tipo de regla que contiene el tipo.
		/// </summary>
		[JsonPropertyName("type")]
		public CasosDivisibilidad Tipo { get; }

		/// <summary>
		/// Aplica la regla a un dividendo y determina si es divisible entre él.
		/// </summary>
		/// <param name="dividendo">El número al que se le aplicará la regla</param>
		/// <returns>
		/// <see cref="string"/> con la explicación del procedimiento y resultado.
		/// </returns>
		public string AplicarRegla(long dividendo);

		/// <summary>
		/// Genera una regla dados un caso y su información, la regla se habrá inicializado.
		/// </summary>
		/// <remarks>
		/// Se salta el flujo de control que asegura que las reglas sean correctas.
		/// </remarks>
		/// <param name="caso">Caso del tipo de regla</param>
		/// <param name="base">Base en la que se aplicará la regla</param>
		/// <param name="divisor">Divisor de la regla</param>
		/// <param name="informacion">Un dato que se necesitará para generar la regla</param>
		/// <returns>
		/// <see cref="IRegla"/> del tipo correspondiente.
		/// </returns>
		internal static IRegla GenerarReglaPorTipo(CasosDivisibilidad caso, long divisor, long @base, int informacion) {
			return caso switch {
				CasosDivisibilidad.DIVISOR_ZERO => new ReglaCero(divisor, @base),
				CasosDivisibilidad.DIVISOR_ONE => new ReglaUno(divisor, @base),
				CasosDivisibilidad.COEFFICIENTS => new ReglaCoeficientes(divisor, @base, 1),
				CasosDivisibilidad.DIGITS => new ReglaCifras(divisor, @base, informacion),
				CasosDivisibilidad.ADD_BLOCKS => new ReglaSumar(divisor, @base, informacion),
				CasosDivisibilidad.SUBSTRACT_BLOCKS => new ReglaRestar(divisor, @base, informacion),
				_ => throw new NotImplementedException()
			};
		}

		public static IRegla GenerarReglaPorTipo(long @base, long divisor) {
			var (caso, informacion) = Calculos.CasoEspecialRegla(@base, divisor);
			return GenerarReglaPorTipo(caso, @base, divisor, informacion);
		}

		public string AplicarVariosDividendos(IEnumerable<long> dividendos) {
			StringBuilder stringBuilder = new();
			foreach (long dividendo in dividendos) {
				stringBuilder.AppendLine(AplicarRegla(dividendo));
			}
			return stringBuilder.ToString();
		}
	}
}
