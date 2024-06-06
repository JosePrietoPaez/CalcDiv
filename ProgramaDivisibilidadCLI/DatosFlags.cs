using CommandLine;
using System;
using System.Collections;

namespace ProgramaDivisibilidad {
	public class Flags {

		private readonly bool extendido,
			todos,
			ayuda;

		private readonly IEnumerable<long> directo;

		private readonly string nombre;

		private readonly bool json;

		public Flags(bool extendido, bool todos,bool ayuda, IEnumerable<long> directo, string nombre, bool json) {
			this.extendido = extendido;
			this.todos = todos;
			this.ayuda = ayuda;
			this.directo = directo;
			this.nombre = nombre;
			this.json = json;
		}

		[Option('x',longName: "extended-rules",HelpText ="May return other types of rules, also outputs explanations on how to apply them, ignores all other flags, except -d")]
		public bool Extendido { get { return extendido; } }

		[Option('a',longName: "output-all-rules", HelpText ="Outputs all coefficient rules with elements whose absolute value is less than the divisor")]
		public bool Todos { get { return todos; } }

		[Option('H', longName:"long-help", HelpText ="Outputs a longer help document explaining this application more in-depth")]
		public bool Ayuda { get { return ayuda; } }

		[Option('d',longName: "direct-output", Min = 2, Max = 3, HelpText ="Reads the divisor, base, and optionally the number of coefficients as arguments, not as input. Must be two or three valid long integers")]
		public IEnumerable<long> Directo { get { return directo; } }

		[Option('n', longName: "named-rule", Default ="",HelpText ="Gives the coefficient rule a name, which is included in its output")]
		public string Nombre { get { return nombre; } }

		[Option('j', longName: "json", HelpText ="Outputs coefficient rule as a JSON object, changes made by other flags are not ignored")]
		public bool JSON { get { return json; } }

	}
}
