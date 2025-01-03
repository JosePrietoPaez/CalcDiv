using Operaciones;
using System.Collections.Generic;
using System.IO;

namespace ModosEjecucion {

	/// <summary>
	/// Interfaz para los modos de ejecución de la aplicación.
	/// </summary>
	/// <remarks>
	/// Contiene un método para redirigir el main.
	/// </remarks>
	public interface IModoEjecucion {

		/// <summary>
		/// Ejecuta la aplicación en el modo correspondiente.
		/// </summary>
		/// <param name="salida"></param>
		/// <param name="error"></param>
		/// <param name="opciones"></param>
		/// <returns>
		/// <see cref="Salida"/> que indica si ha conseguido calcular la regla.
		/// </returns>
		public Salida Ejecutar(TextWriter salida, TextWriter error, IOpciones opciones);

		/// <summary>
		/// Calcula la regla de divisibilidad.
		/// </summary>
		/// <remarks>
		/// Las opciones deben pertenecer a un tipo calculable.
		/// </remarks>
		/// <param name="opciones"></param>
		/// <returns>
		/// <see cref="IRegla"/> con la regla calculada y <see cref="Salida"/> con el estado de la ejecución.
		/// </returns>
		public (EstadoEjecucion, IEnumerable<IRegla>) CalcularRegla(IOpciones opciones);
	}
}
