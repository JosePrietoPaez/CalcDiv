using CommandLine;
using ProgramaDivisibilidadCLI.Recursos;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Calse usada por el parser para inicializar las pociones
	/// </summary>
	public class Flags {

		/// <summary>
		/// Esta propiedad indica si la opción -x está activa.
		/// </summary>
		[Option('x',longName: "extended-rules"
			,HelpText ="HelpExtendido"
			,ResourceType =typeof(TextoResource))]
		public bool Extendido { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -a está activa.
		/// </summary>
		[Option('a',longName: "output-all-rules"
			, HelpText = "HelpTodos"
			, ResourceType = typeof(TextoResource))]
		public bool Todos { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -H está activa.
		/// </summary>
		[Option('H', longName: "long-help"
			, HelpText = "HelpAyuda"
			, ResourceType = typeof(TextoResource))]
		public bool Ayuda { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -h está activa.
		/// </summary>
		[Option('h', longName: "short-help"
			, HelpText = "HelpAyudaCorta"
			, ResourceType = typeof(TextoResource))]
		public bool AyudaCorta { get; set; }

		/// <summary>
		/// Esta propiedad indica los argumentos pasados a -d, si no se ha indicado estará vacía.
		/// </summary>
		[Option('d',longName: "direct-output", Min = 2, Max = 3
			, HelpText = "HelpDirecto"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<long> Directo { get; set; }

		/// <summary>
		/// Esta propiedad indica el nombre pasado a -n, si no se ha indicado será "-". 
		/// </summary>
		/// <remarks>
		/// Al principio del programa se cambia al string vacío si su valor es "-".
		/// </remarks>
		[Option('n', longName: "named-rule", Default = "-"
			, HelpText = "HelpNombre"
			, ResourceType = typeof(TextoResource))]
		public string Nombre { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -j está activa.
		/// </summary>
		[Option('j', longName: "json"
			, HelpText = "HelpJson"
			, ResourceType = typeof(TextoResource))]
		public bool JSON { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -s está activa.
		/// </summary>
		[Option('s', longName: "skip-additional-input"
			, HelpText = "HelpSaltar"
			, ResourceType = typeof(TextoResource))]
		public bool SaltarPreguntas { get; set; }

	}
}
