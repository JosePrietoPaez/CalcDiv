using CommandLine;
using System;
using System.Collections;

namespace ProgramaDivisibilidad {
	public class Flags {

		private readonly bool extendido,
			inverso,
			todos;

		private readonly IEnumerable<long> directo;

		private readonly string? nombre;

		private readonly bool json;

		public Flags(bool extendido, bool inverso, bool todos, IEnumerable<long> directo, string? nombre, bool json) {
			this.extendido = extendido;
			this.inverso = inverso;
			this.todos = todos;
			this.directo = directo;
			this.nombre = nombre;
			this.json = json;
		}

		[Option('x',longName: "extended-rules",HelpText ="May return other types of rules, also outputs explanations on how to apply them, ignores all other flags, except -d")]
		public bool Extendido { get { return extendido; } }

		[Option('r',longName: "reverse-order",HelpText ="Reverses the order of coefficient rule output")]
		public bool Inverso { get { return inverso; } }

		[Option('a',longName: "output-all-rules", HelpText ="Outputs all coefficient rules with elements whose absolute value is less than the divisor")]
		public bool Todos { get { return todos; } }

		[Option('d',longName: "direct-output", Min = 2, Max = 3, HelpText ="Reads the divisor, base, and optionally the number of coefficients as arguments, not as input. Must be ")]
		public IEnumerable<long> Directo { get { return directo; } }

		[Option('n', longName: "named-rule",HelpText ="Gives the coefficient rule a name, which is included in its output")]
		public string? Nombre { get { return nombre; } }

		[Option('j', longName: "json", HelpText ="Outputs coefficient rule as a JSON object, changes made by other flags are not ignored")]
		public bool JSON { get { return json; } }

	}
}
