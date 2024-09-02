using CommandLine;
using CommandLine.Text;
using ProgramaDivisibilidad.Recursos;
using System.Numerics;

namespace ProgramaDivisibilidad {

	internal interface IOpciones {
		[Usage(ApplicationAlias = "CalcDiv.exe")]
		public static IEnumerable<Example> Examples {
			get {
				return [
					new(TextoResource.EjemploReglaUno, new OpcionesDirecto {Base = 12, Divisor = 7, Longitud = 2})
				]; 
			}
		}
	}

	[Verb("dialog", false, HelpText = "HelpVerbDialog", ResourceType = typeof(TextoResource))]
	internal class OpcionesDialogo : IOpcionesGlobales {

		[Option("base", MetaValue = "LONG"
			, HelpText = "HelpBaseDialogo"
			, ResourceType = typeof(TextoResource))]
		public long? Base { get; set; }

		[Option("divisor", MetaValue = "LONG"
			, HelpText = "HelpDivisorDialogo"
			, ResourceType = typeof(TextoResource))]
		public long? Divisor { get; set; }

		[Option("length", MetaValue = "INT"
			, HelpText = "HelpLongitudDialogo"
			, ResourceType = typeof(TextoResource))]
		public int? Longitud { get; set; }

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
		public IEnumerable<string> Dividendo { get; set; } = [];

		public List<BigInteger> DividendoList => Dividendo.Select(BigInteger.Parse).ToList();

		/// <summary>
		/// Esta propiedad devuelve si todos los flags, excepto los de valores de diálogo están inactivos.
		/// </summary>
		public bool FlagsInactivos => !(DialogoSencillo || JSON || TipoExtra || Todos);
	}

	[Verb("single", true, HelpText = "HelpVerbSingle", ResourceType = typeof(TextoResource))]
	internal class OpcionesDirecto : IOpcionesGlobales {

		/// <summary>
		/// Devuelve el divisor pasado por parámetro.
		/// </summary>
		[Value(0, MetaName = "divisor", MetaValue = "LONG"
			, Required = true
			, HelpText = "HelpDivisor"
			, ResourceType = typeof(TextoResource))]
		public long Divisor { get; set; }

		/// <summary>
		/// Devuelve la base pasada por parámetro.
		/// </summary>
		[Value(1, MetaName = "base", MetaValue = "LONG"
			, Default = 10
			, HelpText = "HelpBase"
			, ResourceType = typeof(TextoResource))]
		public long Base { get; set; } = 10;

		/// <summary>
		/// Devuelve la longitud pasada por parámetro.
		/// </summary>
		[Value(2, MetaName = "length", MetaValue = "INT"
			, Default = 1
			, HelpText = "HelpLongitud"
			, ResourceType = typeof(TextoResource))]
		public int Longitud { get; set; } = 1;

		public bool TipoExtra { get; set; }
		public bool Todos { get; set; }
		public bool JSON { get; set; }
		public IEnumerable<string> Dividendo { get; set; } = [];

		public List<BigInteger> DividendoList => Dividendo.Select(BigInteger.Parse).ToList();
	}

	[Verb("multiple", false, HelpText = "HelpTextMultiple", ResourceType = typeof(TextoResource))]
	internal class OpcionesVarias : IOpcionesGlobales {

		/// <summary>
		/// Esta propiedad obtiene la información de las reglas para calcularlas.
		/// </summary>
		[Value(0, MetaName = "divisor-base", MetaValue = "[LONG] [LONG]"
			, Min = 1
			, Max = 2
			, HelpText = "HelpVarias"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<string> VariasReglas { get; set; } = [""];

		private const char SEPARADOR = ',';
		private long[]? _listaDivisores = null
			, _listaBases = null;

		/// <summary>
		/// Devuelve la lista de divisores de las reglas pasada a multiple
		/// </summary>
		public long[] Divisores { 
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
		/// Devuelve la lista de bases pasada a multiple
		/// </summary>
		public long[] Bases {
			get {
				if (_listaBases == null) {
					if (VariasReglas.Count() == 1) {
						_listaBases = [10];
						return [10];
					} else {
						string[] lista = VariasReglas!.ElementAt(1).Split(SEPARADOR);
						long[] result = ParsearStringsLong(lista, TextoResource.ErrorBase);
						_listaBases = [.. result];
						return result;
					}
				} else {
					return [.. _listaBases];
				}
			}
		}

		/// <summary>
		/// Devuelve la longitud de las reglas pasada a multiple
		/// </summary>
		[Value(1, MetaName = "length", MetaValue = "INT"
			, HelpText = "HelpLongitud"
			, Default = 1
			, ResourceType = typeof(TextoResource))]
		public int Longitud { get; set; } = 1;

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
		public IEnumerable<string> Dividendo { get; set; } = [];

		public List<BigInteger> DividendoList => Dividendo.Select(BigInteger.Parse).ToList();
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

		[Option('d', longName: "dividend", MetaValue = "[LONG]"
			, Separator = ','
			, HelpText = "HelpDividendo"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<string> Dividendo { get; set; }

		public List<BigInteger> DividendoList => Dividendo.Select(BigInteger.Parse).ToList();

	}
}
