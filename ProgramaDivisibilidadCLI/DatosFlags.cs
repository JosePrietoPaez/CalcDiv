using CommandLine;
using ProgramaDivisibilidadCLI;
using ProgramaDivisibilidadCLI.Recursos;
using System;
using System.Collections;

namespace ProgramaDivisibilidad {
	public class Flags {

		[Option('x',longName: "extended-rules"
			,HelpText ="HelpExtendido"
			,ResourceType =typeof(TextoResource))]
		public bool Extendido { get; set; }

		[Option('a',longName: "output-all-rules"
			, HelpText = "HelpTodos"
			, ResourceType = typeof(TextoResource))]
		public bool Todos { get; set; }

		[Option('H', longName: "long-help"
			, HelpText = "HelpAyuda"
			, ResourceType = typeof(TextoResource))]
		public bool Ayuda { get; set; }

		[Option('h', longName: "short-help"
			, HelpText = "HelpAyudaCorta"
			, ResourceType = typeof(TextoResource))]
		public bool AyudaCorta { get; set; }

		[Option('d',longName: "direct-output", Min = 2, Max = 3
			, HelpText = "HelpDirecto"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<long> Directo { get; set; }

		[Option('n', longName: "named-rule", Default = "-"
			, HelpText = "HelpNombre"
			, ResourceType = typeof(TextoResource))]
		public string Nombre { get; set; }

		[Option('j', longName: "json"
			, HelpText = "HelpJson"
			, ResourceType = typeof(TextoResource))]
		public bool JSON { get; set; }

		[Option('s', longName: "skip-additional-input"
			, HelpText = "HelpSaltar"
			, ResourceType = typeof(TextoResource))]
		public bool SaltarPreguntas { get; set; }

	}
}
