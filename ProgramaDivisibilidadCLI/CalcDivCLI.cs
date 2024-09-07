using CommandLine;
using System.Text;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;
using CommandLine.Text;
using System.Text.Json;
using Operaciones;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static class CalcDivCLI {

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
			Salida _estadoSalida = Salida.CORRECTA;
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("es", false);
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo("es", false);
			SentenceBuilder.Factory = () => new LocalizableSentenceBuilder();
			using (var parser = new Parser(with => { with.HelpWriter = null; })) {
				var resultado = parser.ParseArguments(args, typeof(OpcionesDirecto), typeof(OpcionesVarias), typeof(OpcionesDialogo), typeof(OpcionesManual)); //Parsea los argumentos
				resultado
				.WithParsed(options => {
					//Console.Error.WriteLine(options.Dump());
					_estadoSalida = SeleccionarModo((IOpciones)options);
				})
				.WithNotParsed(errors => {
					_estadoSalida = Salida.ENTRADA_MALFORMADA;
					//Console.Error.WriteLine(resultado);
					MostrarAyuda(resultado, errors);
				});
			}
			return (int)_estadoSalida;
		}

		private static void MostrarAyuda<T>(ParserResult<T> resultado, IEnumerable<Error> errores) {
			var textoAyuda = HelpText.AutoBuild(resultado, ayuda => {
				ayuda.AdditionalNewLineAfterOption = true;
				ayuda.Heading = $"CalcDiv {typeof(CalcDivCLI).Assembly.GetName().Version}";
				ayuda.Copyright = string.Empty;
				ayuda.AddEnumValuesToHelpText = true;
				ayuda.AddDashesToOption = true;
				ayuda.AddNewLineBetweenHelpSections = true;
				return HelpText.DefaultParsingErrorsHandler(resultado, ayuda);
			}, ejemplo => ejemplo
			, verbsIndex: true);
			Console.Error.WriteLine(textoAyuda);
		}

		private static Salida SeleccionarModo(IOpciones obj) {
			IModoEjecucion modo = obj switch {
				OpcionesDialogo => new ModoDialogo(),
				OpcionesDirecto => new ModoDirecto(),
				OpcionesVarias => new ModoVarias(),
				OpcionesManual => new ModoManual(),
				_ => throw new Exception(ErrorTipoVerbo),
			};
			return modo.Ejecutar(obj);
		}

		/// <summary>
		/// Obtiene el string de una regla de coeficientes a partir de su base, divisor y coeficientes.
		/// </summary>
		/// <param name="divisor"></param>
		/// <param name="base"></param>
		/// <param name="longitud"></param>
		/// <param name="flags"></param>
		/// <returns>
		/// String apropiado para la regla según los datos proporcionados.
		/// </returns>
		internal static string ObtenerReglas(this IOpcionesGlobales flags, long divisor, long @base, int longitud) {
			string resultado;
			if (flags.TipoExtra) {
				resultado = ReglaDivisibilidadExtendida(divisor, @base).Item2.ReglaExplicada;
			} else {
				ReglaCoeficientes serie = ReglaDivisibilidadOptima(divisor, longitud, @base);
				resultado = ObjetoAString(serie);
			}
			return resultado;
		}

		private static readonly JsonSerializerOptions opcionesJson = new() {
			WriteIndented = true,
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper) },
			PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower
		};

		/// <summary>
		/// Devuelve un string para la consola, depende del tipo del objeto
		/// </summary>
		/// <returns>
		/// String apropiado según el tipo del objeto y el estado del programa
		/// </returns>
		internal static string ObjetoAString(object? obj, bool json = false) {
			if (obj == null) return ObjetoNuloMensaje;
			if (json) return JsonSerializer.Serialize(obj, opcionesJson);
			string resultadoObjeto = obj switch {
				// Para una regla de reglasObj de coeficientes obtenidas de una regla, usa recursión
				IEnumerable<object?> or IEnumerable<object> => EnumerableStringSeparadoLinea((IEnumerable<object>)obj),//Se juntan los casos para que sean separados por la recursión
				_ => obj.ToString() ?? ObjetoNuloMensaje,
			};
			return resultadoObjeto;
		}

		internal static string EnumerableStringSeparadoLinea<T>(IEnumerable<T> enumerable) {
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
		internal static void EscribirReglaPorConsola(string reglaCoeficientes, long divisor, long @base) {
			Console.Error.WriteLine(MensajeParametrosDirecto, divisor, @base);
			Console.Out.Write(reglaCoeficientes);
			Console.Error.WriteLine();
		}
	}

	/// <summary>
	/// Interfaz para los modos de ejecución de la aplicación.
	/// </summary>
	/// <remarks>
	/// Contiene un método para redirigir el main.
	/// </remarks>
	internal interface IModoEjecucion {
		Salida Ejecutar(IOpciones opciones);
	}

	internal class ModoManual : IModoEjecucion {
		public Salida Ejecutar(IOpciones opciones) {
			Console.Out.WriteLine(Ayuda);
			return Salida.CORRECTA;
		}
	}

}
