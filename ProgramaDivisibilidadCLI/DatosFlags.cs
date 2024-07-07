using CommandLine;
using ProgramaDivisibilidad.Recursos;

namespace ProgramaDivisibilidad {

	interface IAyudaCortaOpciones {

		[Option('H', longName: "long-help"
			, HelpText = "HelpAyuda"
			, ResourceType = typeof(TextoResource)
			, SetName = "ayuda-larga")]
		public bool Ayuda { get; set; }
	}

	interface IAyudaLargaOpciones {

		[Option('h', longName: "short-help"
			, HelpText = "HelpAyudaCorta"
			, ResourceType = typeof(TextoResource)
			, SetName = "ayuda-corta")]
		public bool AyudaCorta { get; set; }
	}

	/// <summary>
	/// Opciones disponibles solo para reglas de coeficientes
	/// </summary>
	interface ICoeficientesOpciones {

		[Option('n', longName: "name", Default = "-"
			, HelpText = "HelpNombre"
			, ResourceType = typeof(TextoResource)
			, SetName = "coef")]
		public string Nombre { get; set; }

		[Option('a', longName: "all-rules"
			, HelpText = "HelpTodos"
			, ResourceType = typeof(TextoResource)
			, SetName = "coef")]
		public bool Todos { get; set; }

		[Option('j', longName: "json"
			, HelpText = "HelpJson"
			, ResourceType = typeof(TextoResource)
			, SetName = "coef")]
		public bool JSON { get; set; }

	}

	/// <summary>
	/// Opciones disponibles solo para reglas extra
	/// </summary>
	interface IExtraOpciones {

		[Option('x', longName: "extra-rule-types"
			, HelpText = "HelpExtendido"
			, ResourceType = typeof(TextoResource)
			, SetName = "extra")]
		public bool TipoExtra { get; set; }

	}

	/// <summary>
	/// Clase usada por el parser para inicializar las pociones
	/// </summary>
	public class Flags : IAyudaCortaOpciones, IAyudaLargaOpciones, ICoeficientesOpciones, IExtraOpciones {

		/// <summary>
		/// Esta propiedad indica si la opción -x está activa.
		/// </summary>
		public bool TipoExtra { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -a está activa.
		/// </summary>
		public bool Todos { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -H está activa.
		/// </summary>
		public bool Ayuda { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -h está activa.
		/// </summary>
		public bool AyudaCorta { get; set; }

		/// <summary>
		/// Esta propiedad indica los argumentos pasados a -d, si no se ha indicado estará vacía.
		/// </summary>
		[Option('d',longName: "direct-output", Min = 2, Max = 3
			, HelpText = "HelpDirecto"
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<long> Directo { get; set; }

		/// <summary>
		/// Propiedad auxiliar a <see cref="Directo"/>, permite gestionar los elementos como una lista.
		/// </summary>
		/// <remarks>
		/// Para guardar los cambios se debe usar el setter.
		/// </remarks>
		public List<long> DatosRegla {
			get {
				return Directo.ToList();
			}
			set {
				Directo = value.AsEnumerable();
			}
		}

		/// <summary>
		/// Devuelve el primer elemento de <see cref="DatosRegla"/> o <c>-1</c> si es nulo
		/// </summary>
		public long Divisor {
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
		public long Base {
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
		public int Coeficientes {
			get {
				if (DatosRegla is null || DatosRegla.Count() < 3)
					return -1;
				else
					return (int)DatosRegla.ElementAt(2);
			}
		}

		/// <summary>
		/// Esta propiedad indica el nombre pasado a -n, si no se ha indicado será "-". 
		/// </summary>
		/// <remarks>
		/// Al principio del programa se cambia al string vacío si su valor es "-".
		/// </remarks>
		public string Nombre { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -j está activa.
		/// </summary>
		public bool JSON { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -s está activa.
		/// </summary>
		[Option('s', longName: "simple-dialog"
			, HelpText = "HelpSaltar"
			, ResourceType = typeof(TextoResource))]
		public bool DialogoSencillo { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -m está activa y cuales.
		/// </summary>
		[Option('m', longName: "multiple-rules"
			, HelpText = "HelpVarias"
			, Min = 2
			, Max = 3
			, ResourceType = typeof(TextoResource))]
		public IEnumerable<string> VariasReglas { get; set; }

		private const char SEPARADOR = ',';

		private List<long>? _listaDivisores = null
			, _listaBases = null;
		private List<int>? _listaCoeficientes = null;

		/// <summary>
		/// Devuelve la lista de divisores pasada por <see cref="VariasReglas"/>.
		/// </summary>
		/// <remarks>
		/// Se inicializa una única vez.
		/// </remarks>
		public List<long> ListaDivisores { 
			get {
				if (_listaDivisores == null) {
					string[] lista = VariasReglas.First().Split(SEPARADOR);
					List<long> result = ParsearStringsLong(lista, TextoResource.ErrorBase);
					_listaDivisores = new(result);
					return result;
				} else {
					return _listaDivisores;
				}
			}
		}

		/// <summary>
		/// Devuelve la lista de bases pasada por <see cref="VariasReglas"/>.
		/// </summary>
		/// <remarks>
		/// Se inicializa una única vez.
		/// </remarks>
		public List<long> ListaBases {
			get {
				if (_listaBases == null) {
					string[] lista = VariasReglas.ElementAt(1).Split(SEPARADOR);
					List<long> result = ParsearStringsLong(lista, TextoResource.ErrorBase);
					_listaBases = new(result);
					return result;
				} else {
					return _listaBases;
				}
			}
		}

		/// <summary>
		/// Devuelve la lista de coeficientes pasada por <see cref="VariasReglas"/>.
		/// </summary>
		/// <remarks>
		/// Se inicializa una única vez.
		/// </remarks>
		public List<int> ListaCoeficientes {
			get {
				if (_listaCoeficientes == null) {
					string[] lista = VariasReglas.Last().Split(SEPARADOR);
					List<int> result = ParsearStringsInt(lista,TextoResource.ErrorCoeficientes);
					_listaCoeficientes = new(result);
					return result;
				} else {
					return _listaCoeficientes;
				}
			}
		}

		private List<long> ParsearStringsLong(string[] numeros, string mensajeError = "") {
			List<long> result = [];
			long numeroParseado;
			foreach (string s in numeros) {
				if (long.TryParse(s, out numeroParseado)) {
					if (!result.Contains(numeroParseado)) {
						result.Add(numeroParseado);
					}
				} else {
					throw new FormatException(mensajeError);
				}
			}
			return result;
		}

		private List<int> ParsearStringsInt(string[] numeros, string mensajeError = "") {
			return ParsearStringsLong(numeros,mensajeError).Select(numero => (int) numero).ToList();
		}

		/// <summary>
		/// Esta propiedad devuelve si hay algún flag activo.
		/// </summary>
		public bool FlagsInactivos { 
			get {
				return !(DialogoSencillo || JSON || Ayuda || Ayuda || AyudaCorta || TipoExtra || Todos || Nombre.Length != 0 || Directo.Any());
			} 
		}

		/// <summary>
		/// Esta propiedad indica si se debería usar el modo directo según las opciones habilitadas
		/// </summary>
		public bool ActivarDirecto {
			get {
				return Directo.Any() || VariasReglas.Any();
			}
		}

	}
}
