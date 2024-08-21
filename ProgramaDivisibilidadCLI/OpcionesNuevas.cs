using CommandLine;
using ProgramaDivisibilidad.Recursos;

namespace ProgramaDivisibilidad {

	internal interface IOpciones {}

	[Verb("dialog", true, HelpText = "HelpVerbDialog", ResourceType = typeof(TextoResource))]
	internal class OpcionesDialogo : IOpcionesGlobales {

		[Option("base", MetaValue = "LONG"
			, HelpText = "HelpBaseDialogo"
			, ResourceType = typeof(TextoResource))]
		public long? BaseDialogo { get; set; }

		[Option("divisor", MetaValue = "LONG"
			, HelpText = "HelpDivisorDialogo"
			, ResourceType = typeof(TextoResource))]
		public long? DivisorDialogo { get; set; }

		[Option("length", MetaValue = "INT"
			, HelpText = "HelpLongitudDialogo"
			, ResourceType = typeof(TextoResource))]
		public int? LongitudDialogo { get; set; }

		[Option("no-loop"
			, HelpText = "HelpAnularBucle"
			, ResourceType = typeof(TextoResource))]
		public bool AnularBucle { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -s está activa.
		/// </summary>
		[Option('s', longName: "simple-dialog"
			, HelpText = "HelpSaltar"
			, ResourceType = typeof(TextoResource))]
		public bool DialogoSencillo { get; set; }

		public bool TipoExtra { get; set; }
		public bool Todos { get ; set ; }
		public bool JSON { get; set; }
		public IEnumerable<long>? Dividendo { get; set; }

		/// <summary>
		/// Esta propiedad devuelve si todos los flags, excepto los de valores de diálogo están inactivos.
		/// </summary>
		public bool FlagsInactivos => !(DialogoSencillo || JSON || TipoExtra || Todos);
	}

	[Verb("single", false, HelpText = "HelpVerbSingle", ResourceType = typeof(TextoResource))]
	internal class OpcionesDirecto : IOpcionesGlobales {

		/// <summary>
		/// Esta propiedad indica los argumentos pasados a -d, si no se ha indicado estará vacía.
		/// </summary>
		[Value(0, MetaName = "arguments", MetaValue = "LONG LONG (INT)"
			, Min = 2, Max = 3
			, HelpText = "HelpDirecto"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<long>? Directo { get; set; }

		/// <summary>
		/// Propiedad auxiliar a <see cref="Directo"/>, permite gestionar los elementos como una lista.
		/// </summary>
		/// <remarks>
		/// Para guardar los cambios se debe usar el setter.
		/// </remarks>
		public List<long> DatosRegla {
			get {
				return Directo!.ToList();
			}
			set {
				Directo = value.AsEnumerable();
			}
		}

		/// <summary>
		/// Devuelve el primer elemento de <see cref="DatosRegla"/> o <c>-1</c> si es nulo
		/// </summary>
		public long DivisorDirecto {
			get {
				if (DatosRegla is null)
					return -1;
				else
					return DatosRegla.ElementAt(0);
			}
		}

		/// <summary>
		/// Devuelve el segundo elemento de <see cref="DatosRegla"/> o <c>-1</c> si es nulo
		/// </summary>
		public long BaseDirecto {
			get {
				if (DatosRegla is null)
					return -1;
				else
					return DatosRegla.ElementAt(1);
			}
		}

		/// <summary>
		/// Devuelve el tercer elemento de <see cref="DatosRegla"/> o <c>-1</c> si es nulo
		/// </summary>
		public int LongitudDirecta {
			get {
				if (DatosRegla is null || DatosRegla.Count() < 3)
					return -1;
				else
					return (int)DatosRegla.ElementAt(2);
			}
		}

		public bool TipoExtra { get; set; }
		public bool Todos { get; set; }
		public bool JSON { get; set; }
		public IEnumerable<long>? Dividendo { get; set; }
	}

	[Verb("multiple", false, HelpText = "HelpTextMultiple", ResourceType = typeof(TextoResource))]
	internal class OpcionesVarias : IOpcionesGlobales {

		/// <summary>
		/// Esta propiedad indica si la opción -m está activa y cuales.
		/// </summary>
		[Value(0, HelpText = "HelpVarias"
			, Min = 2
			, Max = 3
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<string>? VariasReglas { get; set; }

		private const char SEPARADOR = ',';

		private long[]? _listaDivisores = null
			, _listaBases = null;
		private int? _longitudVarias = null;

		/// <summary>
		/// Devuelve la lista de divisores pasada por <see cref="VariasReglas"/>.
		/// </summary>
		/// <remarks>
		/// Se inicializa una única vez.
		/// <para>
		/// Siempre se devuelve una nueva copia para evitar la modificación indirecta.
		/// </para>
		/// </remarks>
		public long[] ListaDivisores {
			get {
				if (_listaDivisores == null) {
					string[] lista = VariasReglas!.First().Split(SEPARADOR);
					long[] result = ParsearStringsLong(lista, TextoResource.ErrorBase);
					_listaDivisores = [.. result];
					return result;
				} else {
					return [.. _listaDivisores];
				}
			}
		}

		/// <summary>
		/// Devuelve la lista de bases pasada por <see cref="VariasReglas"/>.
		/// </summary>
		/// <remarks>
		/// Se inicializa una única vez.
		/// <para>
		/// Siempre se devuelve una nueva copia para evitar la modificación indirecta.
		/// </para>
		/// </remarks>
		public long[] ListaBases {
			get {
				if (_listaBases == null) {
					string[] lista = VariasReglas!.ElementAt(1).Split(SEPARADOR);
					long[] result = ParsearStringsLong(lista, TextoResource.ErrorBase);
					_listaBases = [.. result];
					return result;
				} else {
					return [.. _listaBases];
				}
			}
		}

		/// <summary>
		/// Esta propiedad devuelve el número de coeficientes deseado para las reglas con -m
		/// </summary>
		public int LongitudesVarias {
			get {
				if (_longitudVarias is null) {
					if (VariasReglas!.Count() == 2) { // Es opcional, así que se pone el valor por defecto
						_longitudVarias = 1;
						VariasReglas = VariasReglas!.Append("1");
					} else {
						if (int.TryParse(VariasReglas!.ElementAt(2), out int numeroParseado)) {
							_longitudVarias = numeroParseado;
						} else {
							throw new FormatException(TextoResource.ErrorCoeficientes);
						}
					}
				}
				return _longitudVarias ?? 1;
			}
			set => _longitudVarias = value;
		}

		private static long[] ParsearStringsLong(string[] numeros, string mensajeError = "") {
			long[] result = [];
			foreach (string s in numeros) {
				if (long.TryParse(s, out long numeroParseado)) {
					if (!result.Contains(numeroParseado)) {
						result = [.. result, numeroParseado];
					}
				} else {
					throw new FormatException(mensajeError);
				}
			}
			return result;
		}

		public bool TipoExtra { get; set; }
		public bool Todos { get; set; }
		public bool JSON { get; set; }
		public IEnumerable<long>? Dividendo { get; set; }
	}

	internal interface IOpcionesGlobales : IOpciones {

		/// <summary>
		/// Esta propiedad indica si la opción -x está activa.
		/// </summary>
		[Option('x', longName: "extra-rule-types"
			, HelpText = "HelpExtendido"
			, ResourceType = typeof(TextoResource)
			, SetName = "extra")]
		public bool TipoExtra { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -a está activa.
		/// </summary>
		[Option('a', longName: "all-rules"
			, HelpText = "HelpTodos"
			, ResourceType = typeof(TextoResource)
			, SetName = "coef")]
		public bool Todos { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -j está activa.
		/// </summary>

		[Option('j', longName: "json"
			, HelpText = "HelpJson"
			, ResourceType = typeof(TextoResource))]
		public bool JSON { get; set; }

		[Option('d', longName:"dividend", MetaValue = "[LONG]"
			, Separator = ','
			, HelpText = "HelpDividendo"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<long>? Dividendo { get; set; }

	}
}
