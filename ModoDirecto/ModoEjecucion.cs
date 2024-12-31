namespace ModosEjecucion {

	/// <summary>
	/// Interfaz para los modos de ejecución de la aplicación.
	/// </summary>
	/// <remarks>
	/// Contiene un método para redirigir el main.
	/// </remarks>
	public interface IModoEjecucion {
		EstadoEjecucion Ejecutar(IOpciones opciones);
	}
}
