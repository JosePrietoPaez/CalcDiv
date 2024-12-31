﻿using CommandLine;
using System.Text;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;
using static ModosEjecucion.Recursos.TextoEjecucion;
using CommandLine.Text;
using System.Text.Json;
using Operaciones;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Numerics;
using ModosEjecucion;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static class CalcDivCLI {

		private static bool puesto = false;

		/// <summary>
		/// Este método calcula la regla de divisibilidad, obteniendo los datos desde la consola reglasObj desde los argumentos.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>
		/// Salida con el código apropiado.
		/// </returns>
		public static int Main(string[] args) {
			EstadoEjecucion _estadoSalida = EstadoEjecucion.CORRECTA;
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("es", false);
			//Thread.CurrentThread.CurrentUICulture = new CultureInfo("es", false);
			SentenceBuilder.Factory = () => new LocalizableSentenceBuilder();
			using (var parser = new Parser(with => { with.HelpWriter = null; })) {
				var resultado = parser.ParseArguments(args, typeof(OpcionesDirecto), typeof(OpcionesVarias), typeof(OpcionesDialogo), typeof(OpcionesManual)); //Parsea los argumentos
				resultado
				.WithParsed(options => {
					//Console.Error.WriteLine(options.Dump());
					if (!puesto) {
						Salida.opcionesJson.Converters.Add(new BigIntegerConverter());
						puesto = true;
					}
					_estadoSalida = SeleccionarModo((IOpciones)options);
				})
				.WithNotParsed(errors => {
					_estadoSalida = EstadoEjecucion.ENTRADA_MALFORMADA;
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

		private static EstadoEjecucion SeleccionarModo(IOpciones obj) {
			IModoEjecucion modo = obj switch {
				OpcionesDialogo => new ModoDialogo(),
				OpcionesDirecto => new ModoDirecto(),
				OpcionesVarias => new ModoVarias(),
				OpcionesManual => new ModoManual(),
				_ => throw new Exception(ErrorTipoVerbo),
			};
			return modo.Ejecutar(obj);
		}
	}

	internal class ModoManual : IModoEjecucion {
		public EstadoEjecucion Ejecutar(IOpciones opciones) {
			Console.Out.WriteLine(Ayuda);
			return EstadoEjecucion.CORRECTA;
		}
	}

	/// <summary>
	/// Genera JSON para <see cref="BigInteger"/>, haciendo que se vea su valor y no sus propiedades.
	/// </summary>
	/// <remarks>
	/// Gracias a Dios por https://stackoverflow.com/questions/64788895/serialising-biginteger-using-system-text-json
	/// </remarks>
	public class BigIntegerConverter : JsonConverter<BigInteger> {

		/// <inheritdoc/>
		public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			if (reader.TokenType != JsonTokenType.Number)
				throw new JsonException(string.Format("Found token {0} but expected token {1}", reader.TokenType, JsonTokenType.Number));
			using var doc = JsonDocument.ParseValue(ref reader);
			return BigInteger.Parse(doc.RootElement.GetRawText(), NumberFormatInfo.InvariantInfo);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options) =>
			writer.WriteRawValue(value.ToString(NumberFormatInfo.InvariantInfo), false);
	}

}
