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
		/// <param name="caso">Caso de </param>
		/// <param name="base"></param>
		/// <param name="divisor"></param>
		/// <param name="informacion"></param>
		/// <returns></returns>
		public static IRegla GenerarReglaPorTipo(CasosDivisibilidad caso, long @base, long divisor, int informacion) {
			return caso switch {
				CasosDivisibilidad.CERO => new ReglaCero(divisor, @base),
				CasosDivisibilidad.UNO => new ReglaUno(divisor, @base),
				CasosDivisibilidad.USAR_COEFICIENTES => new ReglaCoeficientes(divisor, @base, 1),
				CasosDivisibilidad.MIRAR_CIFRAS => new ReglaCifras(divisor, @base, informacion),
				CasosDivisibilidad.SUMAR_BLOQUES => new ReglaSumar(divisor, @base, informacion),
				CasosDivisibilidad.RESTAR_BLOQUES => new ReglaRestar(divisor, @base, informacion),
				_ => throw new NotImplementedException()
			};
		}

		public static IRegla GenerarReglaPorTipo(long @base, long divisor) {
			var (caso, informacion) = Calculos.CasoEspecialRegla(@base, divisor);
			return GenerarReglaPorTipo(caso, @base, divisor, informacion);
		}
	}
}
