using CommandLine;
using System.Text;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;
using System.Diagnostics.CodeAnalysis;
using CommandLine.Text;
using System.Text.Json;
using Operaciones;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static partial class CalculadoraDivisibilidadCLI {
		
		//Inicialización de variables privadas
		private const int SALIDA_CORRECTA = 0, SALIDA_ERROR = 1, SALIDA_ENTRADA_MALFORMADA = 2, SALIDA_VOLUNTARIA = 3, SALIDA_FRACASO_EXPANDIDA = 4
			, SALIDA_VARIAS_ERROR = 5, SALIDA_VARIAS_ERROR_TOTAL = 6;
		private const string SALIDA = "/";
		private static bool _salir; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int _salida; //Salida del programa
		[NotNull] //Para que no me den los warnings, no debería ser null durante las partes relevantes del código
		private static Flags? flags = null;
		private static TextWriter _escritorSalida = Console.Out, _escritorError = Console.Error;
		private static TextReader _lectorEntrada = Console.In;
		private readonly static JsonSerializerOptions opcionesJson = new() { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement) };

		/// <summary>
		/// Este método calcula la regla de divisibilidad, obteniendo los datos desde la consola reglasObj desde los argumentos.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>
		/// <list type="bullet">
		/// <item>0, ejecución correcta</item>
		/// <item>1, código de error general</item>
		/// <item>2, argumentos incorrectos en modo directo</item>
		/// <item>3, el usuario sale voluntariamente durante el diálogo</item>
		/// <item>4, fracaso al encontrar una regla alternativa en modo directo</item>
		/// </list>
		/// </returns>
		public static int Main(string[] args) {
			flags = null;
			_salir = false;
			_salida = SALIDA_CORRECTA;
			_escritorSalida = Console.Out; // Si se ejecutaba y se cambiaba de consola, no se actualizaba
			_escritorError = Console.Error;
			_lectorEntrada = Console.In;
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("es", false);
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo("es", false);
			SentenceBuilder.Factory = () => new LocalizableSentenceBuilder();
			var parser = new Parser(with => with.HelpWriter = null);
			var resultado = parser.ParseArguments<Flags>(args); //Parsea los argumentos
				resultado
				.WithParsed(options => { flags = options;
					//_escritorError.WriteLine(options.Dump());
				})
				.WithNotParsed(errors => { _salida = SALIDA_ENTRADA_MALFORMADA;
					//_escritorError.WriteLine(resultado);
					MostrarAyuda(resultado, errors);
				});
			if (_salida != SALIDA_ENTRADA_MALFORMADA) {
				SeleccionarModo();
			}
			return _salida;
		}

		private static void SeleccionarModo() {
			if (flags is null) {
				_salida = SALIDA_ENTRADA_MALFORMADA;
			} else {
				if (flags.Nombre == "-") { // El valor por defecto es - para que se vea en la pantalla de ayuda
					flags.Nombre = string.Empty;
				}
				Action modoElegido;
				if (flags.Ayuda) {
					modoElegido = EscribirAyudaLarga;
				} else if (flags.AyudaCorta) {
					modoElegido = EscribirAyudaCorta;
				} else { 
					if (flags.ActivarDirecto) { //Si se ha activado el modo directo
						modoElegido = IntentarDirecto;
					} else {
						modoElegido = IniciarAplicacion;
					}
				}
				modoElegido();
			}
		}

		private static void EscribirAyudaLarga() {
			_escritorSalida.Write(Ayuda);
			_salida = SALIDA_CORRECTA;
		}

		private static void EscribirAyudaCorta() {
			_escritorSalida.Write(AyudaCorta);
			_salida = SALIDA_CORRECTA;
		}

		private static void MostrarAyuda<T>(ParserResult<T> resultado, IEnumerable<Error> errores) {
			var textoAyuda = HelpText.AutoBuild(resultado, ayuda => {
				ayuda.AdditionalNewLineAfterOption = true;
				ayuda.Heading = $"CalcDiv {typeof(CalculadoraDivisibilidadCLI).Assembly.GetName().Version}";
				ayuda.Copyright = string.Empty;
				ayuda.AddEnumValuesToHelpText = true;
				return HelpText.DefaultParsingErrorsHandler(resultado, ayuda);
			}, ejemplo => ejemplo);
			_escritorError.WriteLine(textoAyuda);
		}

		/// <summary>
		/// Lógica de la aplicación en modo diálogo.
		/// </summary>
		/// <remarks>
		/// Véase la documentación de 
		/// <see cref="Main(string[])"/>
		/// para ver el significado de la _salida.
		/// </remarks>
		/// <returns>
		/// Código de la _salida de la aplicación.
		/// </returns>
		private static void IniciarAplicacion() { //Si no se proporcionan los argumentos de forma directa, se establece un diálogo con el usuario para obtenerlos
			_escritorError.WriteLine(MensajeInicioDialogo,SALIDA);
			bool sinFlags = flags.FlagsInactivos;
			static bool esS(char letra) => letra == 's' || letra == 'S';
			flags.DatosRegla = [0,0,0];
			try {
				do {
					//Si no tiene flags las pedirá durante la ejecución
					if (sinFlags) {
						//Le una tecla de entrada reglasObj lanza excepción si es la _salida
						flags.TipoExtra = !ObtenerDeUsuario(MensajeDialogoExtendido, esS);

						if (!flags.TipoExtra) {
							flags.JSON = ObtenerDeUsuario(MensajeDialogoJson, esS);
						}

					}

					//Pregunta el dato en bucle hasta que sea correcto a lanza excepción si es el mensaje de _salida

					ObtenerDeUsuario(out long @base, 2
						, ErrorBase
						, MensajeDialogoBase);

					if (flags.TipoExtra) { //Si la regla es de algún tipo extra

						ObtenerDeUsuario(out long divisor, 0
							, ErrorDivisor
							, MensajeDialogoDivisor);

						flags.DatosRegla = [divisor, @base, 1];

						//Intenta buscar una regla alternativa
						_escritorSalida.WriteLine(ReglaDivisibilidadExtendida(divisor, @base).Item2 + Environment.NewLine);

					} else { //Si la regla debe ser de coeficientes

						//Pregunta hasta que sea coprimo con base o sea _salida
						ObtenerDeUsuarioCoprimo(out long divisor, 2
							, @base
							, ErrorDivisorCoprimo
							, MensajeDialogoDivisor);

						ObtenerDeUsuario(out int coeficientes, 1
							, ErrorCoeficientes
							, MensajeDialogoCoeficientes);

						flags.DatosRegla = [divisor, @base, coeficientes];

						if (sinFlags) {
							flags.Todos = ObtenerDeUsuario(MensajeDialogoTodas, esS);

							ObtenerDeUsuario(out string nombre, MensajeDialogoRegla);
							flags.Nombre = nombre;
						}

						string resultado = ObtenerReglas(divisor, @base, coeficientes);
						EscribirReglaPorWriter(resultado + Environment.NewLine, _escritorSalida, _escritorError, divisor, @base, coeficientes);

					}

					_salir = !ObtenerDeUsuario(MensajeDialogoRepetir, esS);
					_salida = SALIDA_CORRECTA;

				} while (!_salir);
			}
			catch (SalidaException) {
				_salida = SALIDA_VOLUNTARIA;
				_escritorError.WriteLine(Environment.NewLine + MensajeDialogoInterrumpido);
			}
			catch (Exception e) {
				_salida = SALIDA_ERROR;
				_escritorError.WriteLine(e.StackTrace);
				_escritorError.WriteLine(DialogoExcepcionInesperada);
			}
		}

		private static bool ObtenerDeUsuario(string mensaje, Func<char,bool> comparador) {
			_escritorError.Write(mensaje);
			char entrada = Console.ReadKey().KeyChar; //Necesario usar la consola
			LanzarExcepcionSiSalida(entrada.ToString());
			_escritorError.WriteLine();
			return comparador(entrada);
		}

		private static void ObtenerDeUsuario(out long dato, long minimo, string mensajeError, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			string? linea = _lectorEntrada.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo) {
				LanzarExcepcionSiSalida(linea);
				_escritorError.WriteLine(Environment.NewLine + mensajeError);
				_escritorError.Write(mensajePregunta);
				linea = _lectorEntrada.ReadLine();
			}
			_escritorError.WriteLine();
		}

		private static void ObtenerDeUsuario(out int dato, long minimo, string mensajeError, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			string? linea = _lectorEntrada.ReadLine();
			while (!int.TryParse(linea, out dato) || dato < minimo) {
				LanzarExcepcionSiSalida(linea);
				_escritorError.WriteLine(Environment.NewLine + mensajeError);
				_escritorError.Write(mensajePregunta);
				linea = _lectorEntrada.ReadLine();
			}
			_escritorError.WriteLine();
		}

		private static void ObtenerDeUsuarioCoprimo(out long dato, long minimo, long coprimo, string mensajeError, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			string? linea = _lectorEntrada.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo || !SonCoprimos(dato, coprimo)) {
				LanzarExcepcionSiSalida(linea);
				_escritorError.WriteLine(Environment.NewLine + mensajeError);
				_escritorError.Write(mensajePregunta);
				linea = _lectorEntrada.ReadLine();
			}
			_escritorError.WriteLine();
		}

		private static void ObtenerDeUsuario(out string dato, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			dato = _lectorEntrada.ReadLine() ?? "";
			LanzarExcepcionSiSalida(dato);
			_escritorError.WriteLine();
		}

		private static void LanzarExcepcionSiSalida(string? linea) {
			if (linea == SALIDA) throw new SalidaException(MensajeSalidaVoluntaria);
		}

		/// <summary>
		/// Devuelve un string para la consola, depende del tipo del objeto
		/// </summary>
		/// <returns>
		/// String apropiado según el tipo del objeto y el estado del programa
		/// </returns>
		private static string ObjetoAString(object? obj) {
			if (obj == null) return ObjetoNuloMensaje;
			if (flags.JSON) return JsonSerializer.Serialize(obj, opcionesJson);
			string resultadoObjeto = obj switch {
				// Para una regla de reglasObj de coeficientes obtenidas de una regla, usa recursión
				IEnumerable<object?> or IEnumerable<object> => EnumerableStringSeparadoLinea((IEnumerable<object>)obj),//Se juntan los casos para que sean separados por la recursión
				Regla regla => regla.ToStringCompleto(),
				_ => obj.ToString() ?? ObjetoNuloMensaje,
			};
			return resultadoObjeto;
		}

		private static string EnumerableStringSeparadoLinea<T>(IEnumerable<T> enumerable) {
			StringBuilder stringBuilder = new();
			foreach (T item in enumerable) {
				stringBuilder.AppendLine(ObjetoAString(item));
			}
			stringBuilder.Remove(stringBuilder.Length - Environment.NewLine.Length, Environment.NewLine.Length); // Podría cambiar el foreach para no hacer esto pero seria mas complicado
			return stringBuilder.ToString();

		}

		/// <summary>
		/// Escribe la regla de coeficientes por el <see cref="TextWriter"/> proporcionado
		/// </summary>
		private static void EscribirReglaPorWriter(string reglaCoeficientes, TextWriter writerSalida, TextWriter writerError, long divisor, long @base, int coeficientes = 1) {
			writerError.WriteLine(MensajeParametrosDirecto, divisor, @base, coeficientes);
			EscribirLineaErrorCondicional(MensajeFinDirecto);
			writerSalida.Write(reglaCoeficientes);
			writerError.WriteLine();
		}

		/// <summary>
		/// Obtiene el string de una regla de coeficientes a partir de su base, divisor y coeficientes.
		/// </summary>
		/// <param name="divisor"></param>
		/// <param name="base"></param>
		/// <param name="coeficientes"></param>
		/// <returns>
		/// String apropiado para la regla según los datos proporcionados.
		/// </returns>
		private static string ObtenerReglas(long divisor, long @base, int coeficientes) {
			string resultado;
			object? reglasObj = null;
			if (flags.Todos) { //Si se piden las 2^coeficientes reglasObj
				List<Regla> reglas = ReglasDivisibilidad(divisor, coeficientes, @base);
				if (flags.Nombre != "") {
					foreach (var serie in reglas) {
						serie.Nombre = flags.Nombre ?? "";
					}
				}
				reglasObj = reglas;
			} else {
				Regla serie = ReglaDivisibilidadOptima(divisor, coeficientes, @base);
				serie.Nombre = flags.Nombre;
				reglasObj = serie;
			}
			resultado = ObjetoAString(reglasObj);
			return resultado;
		}

		private static string Tabulaciones(int cantidad) {
			string tabulaciones = "";
			while (cantidad > 0) {
				tabulaciones += '\t';
				cantidad--;
			}
			return tabulaciones;
		}

		private static void InsertarPropiedadesLista(StringBuilder stringBuilder, string tabulaciones = "") {
			stringBuilder.Append(tabulaciones + "\"divisor\" : " + flags.Divisor + ',' + Environment.NewLine);
			stringBuilder.Append(tabulaciones + "\"base\" : " + flags.Base + ',' + Environment.NewLine);
			stringBuilder.Append(tabulaciones + "\"name\" : " + '\"' + flags.Nombre + '\"' + ',' + Environment.NewLine);
		}
	}
}
