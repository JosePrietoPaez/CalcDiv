using CommandLine;
using System.Text;
using Listas;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;
using System.Diagnostics.CodeAnalysis;
using CommandLine.Text;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static partial class CalculadoraDivisibilidadCLI {
		
		//Inicialización de variables privadas
		private const int SALIDA_CORRECTA = 0, SALIDA_ERROR = 1, SALIDA_ENTRADA_MALFORMADA = 2, SALIDA_VOLUNTARIA = 3, SALIDA_FRACASO_EXPANDIDA = 4
			, SALIDA_VARIAS_ERROR = 5, SALIDA_VARIAS_ERROR_TOTAL = 6;
		private const string SALIDA = "/";
		private static bool _salir = false; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int _salida = SALIDA_CORRECTA; //Salida del programa
		[NotNull] //Para que no me den los warnings
		private static Flags? flags = null;
		private static TextWriter _escritorSalida = Console.Out, _escritorError = Console.Error;
		private static TextReader _lectorEntrada = Console.In;

		/// <summary>
		/// Este método calcula la regla de divisibilidad, obteniendo los datos desde la consola listas desde los argumentos.
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
				GestionarFlags();
			}
			return _salida;
		}

		private static void GestionarFlags() {
			if (flags is null) {
				_salida = SALIDA_ENTRADA_MALFORMADA;
			} else {
				if (flags.Nombre == "-") { // El valor por defecto es - para que se vea en la pantalla de ayuda
					flags.Nombre = string.Empty;
				}
				if (flags.Ayuda) {
					_escritorError.Write(Ayuda);
					_salida = SALIDA_CORRECTA;
				} else if (flags.AyudaCorta) {
					_escritorError.WriteLine(AyudaCorta);
					_salida = SALIDA_CORRECTA;
				} else { 
					if (flags.ActivarDirecto) { //Si se ha activado el modo directo
						IntentarDirecto();
					} else {
						IniciarAplicacion();
					}
				}
			}
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
			_escritorSalida.WriteLine(string.Format(MensajeInicioDialogo,SALIDA));
			bool sinFlags = flags.FlagsInactivos;
			static bool esS(char letra) => letra == 's' || letra == 'S';
			flags.DatosRegla = [0,0,0];
			try {
				do {
					//Si no tiene flags las pedirá durante la ejecución
					if (sinFlags) {
						//Le una tecla de entrada listas lanza excepción si es la _salida
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
						_escritorSalida.Write(ReglaDivisibilidadExtendida(divisor, @base).Item2);

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
						
						CalcularReglaCoeficientes(divisor, @base, coeficientes);

					}
					_escritorError.WriteLine();

					_salir = !ObtenerDeUsuario(MensajeDialogoRepetir, esS);
					_salida = _salir? SALIDA_VOLUNTARIA : SALIDA_CORRECTA;

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
			char entrada = (char)_lectorEntrada.Read();
			if (entrada == SALIDA[0]) throw new Exception(MensajeSalidaVoluntaria);
			_escritorError.WriteLine();
			return comparador(entrada);
		}

		private static void ObtenerDeUsuario(out long dato, long minimo, string mensajeError, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			string? linea = _lectorEntrada.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo) {
				_escritorError.WriteLine(Environment.NewLine + mensajeError);
				_escritorError.Write(mensajePregunta);
				linea = _lectorEntrada.ReadLine();
				LanzarExcepcionSiSalida(linea);
			}
			_escritorError.WriteLine();
		}

		private static void ObtenerDeUsuario(out int dato, long minimo, string mensajeError, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			string? linea = _lectorEntrada.ReadLine();
			while (!int.TryParse(linea, out dato) || dato < minimo) {
				_escritorError.WriteLine(Environment.NewLine + mensajeError);
				_escritorError.Write(mensajePregunta);
				linea = _lectorEntrada.ReadLine();
				LanzarExcepcionSiSalida(linea);
			}
			_escritorError.WriteLine();
		}

		private static void ObtenerDeUsuarioCoprimo(out long dato, long minimo, long coprimo, string mensajeError, string mensajePregunta) {
			_escritorError.Write(mensajePregunta);
			string? linea = _lectorEntrada.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo || Mcd(dato,coprimo) > 1) {
				_escritorError.WriteLine(Environment.NewLine + mensajeError);
				_escritorError.Write(mensajePregunta);
				linea = _lectorEntrada.ReadLine();
				LanzarExcepcionSiSalida(linea);
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
		/// Devuelve el ToString adecuado de <c>serie</c>.
		/// </summary>
		/// <param name="serie"></param>
		/// <returns>
		/// String que representa a <c>serie</c> dependiendo de los flags.
		/// </returns>
		private static string StringSerieConFlags(ListSerie<long> serie) {
			if (flags.JSON) {
				return Serializar(serie);
			}
			if (serie.Nombre != string.Empty && (flags.VariasReglas?.Any() ?? false)) {
				serie.Nombre += '₍' + NumASubindice(flags.Divisor) + '₋' + NumASubindice(flags.Base) + '₎';
			}
			return serie.Nombre != string.Empty ? serie.ToStringCompleto() : serie.ToString()??"";
		}

		/// <summary>
		/// Convierte una lista de <see cref="Listas"/> a un string JSON
		/// </summary>
		/// <param name="listas">listas que serializar a JSON</param>
		/// <param name="indentacion">número de tabulaciones en cada línea</param>
		/// <param name="escribirDatos">indica si se deben escribir los datos de la lista</param>
		/// <returns>
		/// JSON que representa la lista
		/// </returns>
		private static string Serializar(object listas, int indentacion = 0, bool escribirDatos = true) {
			string tabulaciones = Tabulaciones(indentacion)
				, tabulacionesMas = tabulaciones + '\t';
			StringBuilder listasJson = new(tabulaciones + '{' + Environment.NewLine);
			listasJson.Append(ObjetoAJSON(listas, indentacion + 1, escribirDatos));
			listasJson.Append(Environment.NewLine + tabulaciones + '}');
			return listasJson.ToString();
		}

		private static string ObjetoAJSON(object objeto, int indentacion = 0, bool escribirDatos = true) {
			StringBuilder objetoJSON = new();
			string tabulacion = Tabulaciones(indentacion)
				, tabulacionesMas = tabulacion + '\t';
			if (escribirDatos) {
				InsertarPropiedadesLista(objetoJSON,tabulacion);
			}
			if (objeto is ListSerie<long> lista) {
				objetoJSON.Append(tabulacion + "\"coefficients\" : [" + Environment.NewLine);
				for (int i = 0; i < lista.Longitud; i++) {
					if (i == lista.Longitud - 1) {
						objetoJSON.Append(tabulacionesMas + lista.UltimoElemento);
					} else {
						objetoJSON.Append(tabulacionesMas + lista[i] + ',' + Environment.NewLine);
					}
				}
				objetoJSON.Append(Environment.NewLine + tabulacion + ']');
			} else if (objeto is ListSerie<ListSerie<long>> listas) {
				objetoJSON.Append(tabulacion + "\"rules\" : [" + Environment.NewLine);
				for (int i = 0; i < listas.Longitud; i++) {
					if (i == listas.Longitud - 1) {
						objetoJSON.Append(Serializar(listas.UltimoElemento, indentacion + 1, false));
					} else {
						objetoJSON.Append(Serializar(listas[i], indentacion + 1, false) + ',');
					}
				}
				objetoJSON.Append(Environment.NewLine + tabulacion + ']');
			}
			return objetoJSON.ToString();
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
