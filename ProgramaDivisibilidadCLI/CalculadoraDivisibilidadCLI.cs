﻿using CommandLine;
using System.Text;
using Listas;
using Operaciones;
using System.Diagnostics.CodeAnalysis;
using CommandLine.Text;
using ProgramaDivisibilidadCLI.Recursos;
using ReadText.LocalizedDemo;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static partial class CalculadoraDivisibilidadCLI {
		
		//Inicialización de variables privadas
		private const int SALIDA_CORRECTA = 0, SALIDA_ERROR = 1, SALIDA_ENTRADA_MALFORMADA = 2, SALIDA_VOLUNTARIA = 3, SALIDA_FRACASO_EXPANDIDA = 4;
		private const string SALIDA = "/";
		private static bool salir = false; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int salida = SALIDA_CORRECTA; //Salida del programa
		[NotNull] //Para que no me den los warnings
		private static Flags? flags = null;

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
					//Console.Error.WriteLine(options.Dump());
				})
				.WithNotParsed(errors => { salida = SALIDA_ENTRADA_MALFORMADA;
					//Console.Error.WriteLine(resultado);
					MostrarAyuda(resultado, errors);
				});
			if (salida != SALIDA_ENTRADA_MALFORMADA) {
				GestionarFlags();
			}
			return salida;
		}

		private static void GestionarFlags() {
			if (flags is null) {
				salida = SALIDA_ENTRADA_MALFORMADA;
			} else {
				if (flags.Nombre == "-") { // El valor por defecto es - para que se vea en la pantalla de ayuda
					flags.Nombre = string.Empty;
				}
				if (flags.Ayuda) {
					Console.Error.Write(TextoResource.Ayuda);
					salida = SALIDA_CORRECTA;
				} else if (flags.AyudaCorta) {
					Console.Error.WriteLine(TextoResource.AyudaCorta);
					salida = SALIDA_CORRECTA;
				} else { 
					if (flags.Directo.Count() > 1) { //Si se ha activado el modo directo
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
			Console.Error.WriteLine(textoAyuda);
		}

		/// <summary>
		/// Lógica de la aplicación en modo diálogo.
		/// </summary>
		/// <remarks>
		/// Véase la documentación de 
		/// <see cref="Main(string[])"/>
		/// para ver el significado de la salida.
		/// </remarks>
		/// <returns>
		/// Código de la salida de la aplicación.
		/// </returns>
		private static void IniciarAplicacion() { //Si no se proporcionan los argumentos de forma directa, se establece un diálogo con el usuario para obtenerlos
			Console.WriteLine(string.Format(TextoResource.MensajeInicioDialogo,SALIDA));
			bool sinFlags = SinFlags();
			long @base, divisor;
			string nombre = "";
			static bool esS(char letra) => letra == 's' || letra == 'S';
			try {
				do {
					//Si no tiene flags las pedirá durante la ejecución
					if (sinFlags) {
						//Le una tecla de entrada listas lanza excepción si es la salida
						flags.TipoExtra = !ObtenerDeUsuario(TextoResource.MensajeDialogoExtendido, esS);

						if (!flags.TipoExtra) {
							flags.JSON = ObtenerDeUsuario(TextoResource.MensajeDialogoJson, esS);
						}
						
					}

					if (flags.TipoExtra) {

						//Pregunta el dato en bucle hasta que sea correcto a lanza excepción si es el mensaje de salida
						ObtenerDeUsuario(out divisor, 0
							, TextoResource.ErrorDivisor
							, TextoResource.MensajeDialogoDivisor);

						ObtenerDeUsuario(out @base, 2
							, TextoResource.ErrorBase
							, TextoResource.MensajeDialogoBase);

						//Intenta buscar una regla alternativa
						Console.Write(Calculos.ReglaDivisibilidadExtendida(divisor,@base).Item2);
						Console.Error.WriteLine();

					} else {

						ObtenerDeUsuario(out divisor, 0
							, TextoResource.ErrorDivisor
							, TextoResource.MensajeDialogoDivisor);

						ObtenerDeUsuario(out @base, 2
							, TextoResource.ErrorBase
							, TextoResource.MensajeDialogoBase);

						ObtenerDeUsuario(out int coeficientes, 1
							, TextoResource.ErrorCoeficientes
							, TextoResource.MensajeDialogoCoeficientes);

						if (sinFlags) {
							flags.Todos = ObtenerDeUsuario(TextoResource.MensajeDialogoTodas, esS);

							ObtenerDeUsuario(out nombre, TextoResource.MensajeDialogoRegla);
						}

						ReferirAExtraYCalcularRegla(divisor,@base,coeficientes);
						Console.Error.WriteLine();

					}

					salir = !ObtenerDeUsuario(TextoResource.MensajeDialogoRepetir, esS);

				} while (!salir);
			} catch {
				salir = true;
				Console.WriteLine(Environment.NewLine + TextoResource.MensajeDialogoInterrumpido);
			}			
			salida = salir? SALIDA_VOLUNTARIA : SALIDA_CORRECTA;
		}

		private static bool SinFlags() {
			return !(flags.Todos || flags.DialogoSencillo || flags.JSON || flags.TipoExtra || flags.Nombre != string.Empty || flags.Directo.Any());
		}

		private static bool ObtenerDeUsuario(string mensaje, Func<char,bool> comparador) {
			Console.Error.Write(mensaje);
			char entrada = Console.ReadKey().KeyChar;
			if (entrada == SALIDA[0]) throw new Exception(TextoResource.MensajeSalidaVoluntaria);
			Console.Error.WriteLine();
			return comparador(entrada);
		}

		private static void ObtenerDeUsuario(out long dato, long minimo, string mensajeError, string mensajePregunta) {
			Console.Error.Write(mensajePregunta);
			string? linea = Console.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo) {
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.ReadLine();
				LanzarExcepcionSiSalida(linea);
			}
			Console.Error.WriteLine();
		}

		private static void ObtenerDeUsuario(out int dato, long minimo, string mensajeError, string mensajePregunta) {
			Console.Error.Write(mensajePregunta);
			string? linea = Console.ReadLine();
			while (!int.TryParse(linea, out dato) || dato < minimo) {
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.ReadLine();
				LanzarExcepcionSiSalida(linea);
			}
			Console.Error.WriteLine();
		}

		private static void ObtenerDeUsuarioCoprimo(out long dato, long minimo, long coprimo, string mensajeError, string mensajePregunta) {
			Console.Error.Write(mensajePregunta);
			string? linea = Console.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo || Calculos.Mcd(dato,coprimo) > 1) {
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.ReadLine();
				LanzarExcepcionSiSalida(linea);
			}
			Console.Error.WriteLine();
		}

		private static void ObtenerDeUsuario(out string dato, string mensajePregunta) {
			Console.Error.Write(mensajePregunta);
			dato = Console.ReadLine() ?? "";
			LanzarExcepcionSiSalida(dato);
			Console.Error.WriteLine();
		}

		private static void LanzarExcepcionSiSalida(string? linea) {
			if (linea == SALIDA) throw new Exception(TextoResource.MensajeSalidaVoluntaria);
		}

		private static string SerieRectangularString(ListSerie<ListSerie<long>> serie) {
			if (flags.JSON) {
				return Serializar(serie);
			}
			StringBuilder stringBuilder = new();
			for (int i = 0; i < serie.Longitud - 1; i++) {
				stringBuilder.AppendLine(StringSerieConFlags(serie[i]));
			}
			stringBuilder.Append(StringSerieConFlags(serie[serie.Longitud - 1]));
			return stringBuilder.ToString();
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
			return serie.Nombre != string.Empty ? serie.ToStringCompleto() : serie.ToString()??"";
		}

		/// <summary>
		/// Convierte una lista de <see cref="Listas"/> a un string JSON
		/// </summary>
		/// <param name="listas"></param>
		/// <param name="indentacion"></param>
		/// <returns>
		/// JSON que representa la lista
		/// </returns>
		private static string Serializar(object listas, int indentacion = 0) {
			string tabulaciones = Tabulaciones(indentacion)
				, tabulacionesMas = tabulaciones + '\t';
			StringBuilder listasJson = new(tabulaciones + '{' + Environment.NewLine);
			InsertarPropiedadesLista(listasJson,tabulacionesMas);
			listasJson.Append(ObjetoAJSON(listas, indentacion + 1));
			listasJson.Append(tabulacionesMas + ']' + Environment.NewLine + tabulaciones + '}');
			return listasJson.ToString();
		}

		private static string ObjetoAJSON(object objeto, int indentacion = 0) {
			StringBuilder objetoJSON = new();
			string tabulacion = Tabulaciones(indentacion)
				, tabulacionesMas = tabulacion + '\t';
			if (objeto is ListSerie<long> lista) {
				objetoJSON.Append(tabulacionesMas + "\"coefficients\" : " + Environment.NewLine + tabulacionesMas + '[' + Environment.NewLine);
				for (int i = 0; i < lista.Longitud; i++) {
					if (i == lista.Longitud - 1) {
						objetoJSON.Append(tabulacionesMas + lista.UltimoElemento + Environment.NewLine);
					} else {
						objetoJSON.Append(tabulacionesMas + lista[i] + ',' + Environment.NewLine);
					}
				}
			} else if (objeto is ListSerie<ListSerie<long>> listas) {
				for (int i = 0; i < listas.Longitud; i++) {
					objetoJSON.Append(tabulacionesMas + "\"rules\" : " + Environment.NewLine + tabulacionesMas + '[' + Environment.NewLine);
					if (i == listas.Longitud - 1) {
						objetoJSON.Append(Serializar(listas.UltimoElemento, indentacion + 1) + Environment.NewLine);
					} else {
						objetoJSON.Append(Serializar(listas[i], indentacion + 1) + ',' + Environment.NewLine);
					}
				}
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
