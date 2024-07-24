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
		private static bool _salir; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int _salida; //Salida del programa
		[NotNull] //Para que no me den los warnings, no debería ser null durante las partes relevantes del código
		private static Flags? flags = null;
		private static TextWriter _escritorSalida = Console.Out, _escritorError = Console.Error;
		private static TextReader _lectorEntrada = Console.In;

		/// <summary>
		/// Este método calcula la regla de divisibilidad, obteniendo los datos desde la consola reglas desde los argumentos.
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
						//Le una tecla de entrada reglas lanza excepción si es la _salida
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
			while (!long.TryParse(linea, out dato) || dato < minimo || Mcd(dato,coprimo) > 1) {
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
		private static string ObjetoAString(object? obj, bool escribirDatos = true) {
			if (obj == null) return ObjetoNuloMensaje;
			if (flags.JSON) return Serializar(obj,escribirDatos: escribirDatos);
			string resultadoObjeto = obj switch {
				// Para una regla de coeficientes única
				ListSerie<long> regla => regla.Nombre != string.Empty ? regla.ToStringCompleto() : regla.ToString() ?? "",
				// Para una regla de reglas de coeficientes obtenidas de una regla, usa recursión
				ListSerie<ListSerie<long>> or List<string> or List<object> => EnumerableStringSeparadoLinea((IEnumerable<object>)obj),//Se juntan los casos para que sean separados por la recursión
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
		/// Convierte una regla de <see cref="Listas"/> a un string JSON
		/// </summary>
		/// <param name="listas">reglas que serializar a JSON</param>
		/// <param name="indentacion">número de tabulaciones en cada línea</param>
		/// <param name="escribirDatos">indica si se deben escribir los datos de la regla</param>
		/// <returns>
		/// JSON que representa la regla
		/// </returns>
		private static string Serializar(object listas, int indentacion = 0, bool escribirDatos = true) {
			string tabulaciones = Tabulaciones(indentacion);
			bool encapsular = listas is not List<object>;
			StringBuilder listasJson = new();
			if (encapsular) listasJson.Append(tabulaciones + '{' + Environment.NewLine);
			listasJson.Append(ObjetoAJSON(listas, indentacion + (encapsular ? 1 : 0), escribirDatos));
			if (encapsular) listasJson.Append(Environment.NewLine + tabulaciones + '}');
			return listasJson.ToString();
		}

		private static string ObjetoAJSON(object objeto, int indentacion = 0, bool escribirDatos = true) {
			StringBuilder objetoJSON = new();
			string tabulacion = Tabulaciones(indentacion)
				, tabulacionesMas = tabulacion + '\t';
			if (escribirDatos) {
				InsertarPropiedadesLista(objetoJSON,tabulacion);
			}
			if (objeto is ListSerie<long> regla) { // Para una regla
				objetoJSON.Append(tabulacion + "\"coefficients\" : [" + Environment.NewLine);
				if (!regla.Vacia) {
					AppendListaCasoFinalDistinto(objetoJSON, regla, tabulacionesMas + regla.UltimoElemento, (i) => tabulacionesMas + regla[i] + ',' + Environment.NewLine);
				}
				objetoJSON.Append(Environment.NewLine + tabulacion + ']');
			} else if (objeto is ListSerie<ListSerie<long>> reglas) { // Para conjuntos de reglas derivadas
				objetoJSON.Append(tabulacion + "\"rules\" : [" + Environment.NewLine);
				if (!reglas.Vacia) {
					AppendListaCasoFinalDistinto(objetoJSON, reglas, Serializar(reglas.UltimoElemento, indentacion + 1, false), (i) => Serializar(reglas[i], indentacion + 1, false) + ',');
				}
				objetoJSON.Append(Environment.NewLine + tabulacion + ']');
			} else if (objeto is List<object> lista) {
				objetoJSON.Append(tabulacion + "[" + Environment.NewLine);
				if (lista.Count > 0) {
					AppendListaCasoFinalDistinto(objetoJSON, lista, Serializar(lista[^1], indentacion + 1, true), (i) => Serializar(lista[i], indentacion + 1, true) + ',');
				}
				objetoJSON.Append(Environment.NewLine + tabulacion + ']');
			}
			return objetoJSON.ToString();
		}

		private static void AppendListaCasoFinalDistinto<T>(StringBuilder builder, IEnumerable<T> lista, string casoFinal, Func<int,string> generadorNormal) {
			IEnumerator<T> enumerator = lista.GetEnumerator();
			int longitud = lista.Count();
			for (int i = 0; enumerator.MoveNext(); i++) {
				if (i == longitud - 1) {
					builder.Append(casoFinal);
				} else {
					builder.Append(generadorNormal(i));
				}
			}
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
			object? reglas = null;
			if (flags.Todos) { //Si se piden las 2^coeficientes reglas
				ListSerie<ListSerie<long>> series = new();
				ReglasDivisibilidad(series, divisor, coeficientes, @base);
				if (flags.Nombre != "") {
					foreach (var serie in series) {
						serie.Nombre = flags.Nombre ?? "";
					}
				}
				reglas = series;
			} else {
				ListSerie<long> serie = new(flags.Nombre ?? "");
				ReglaDivisibilidadOptima(serie, divisor, coeficientes, @base);
				reglas = serie;
			}
			resultado = ObjetoAString(reglas);
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
