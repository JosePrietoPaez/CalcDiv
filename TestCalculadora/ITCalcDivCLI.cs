using Operaciones;
using Operaciones.Recursos;
using ProgramaDivisibilidad;
using ProgramaDivisibilidad.Recursos;
using ModosEjecucion.Recursos;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Diagnostics;

namespace TestCalculadoraIT; 
/// <summary>
/// Esta clase contendrá pruebas de integración en la que usaremos la salida por consola para determinar si funciona correctamente
/// </summary>
/// <remarks>
/// No son unitarias, ya que se prueba la salida del método main, que involucra varias clases
/// </remarks>
[TestFixture]
internal class ITCalcDivCLI {

	private TextWriter _salidaOriginal
		,_escritorSalida
		,_errorOriginal
		,_escritorError;
	private TextReader _entradaOriginal
		,_lectorEntrada;
	private string[] _args = [];

	private readonly CultureInfo _culturaActual = CultureInfo.CurrentCulture;

	[SetUp]
	public void CambiarEntradaYSalida() {
		_salidaOriginal = Console.Out;
		_entradaOriginal = Console.In;
		_errorOriginal = Console.Error;
		_escritorSalida = new StringWriter();
		_escritorError = new StringWriter();
		Console.SetOut(_escritorSalida);
		Console.SetError(_escritorError);
	}

	[Test(Description = "Aunque no haya usado NUnit demasiado quiero dejar un test con Assert.Charlie(), además si falla sabré que pasa algo")]
	public void Charlie() {
		Assert.Charlie();
	}

	[Test(Description = "Al llamar a la calculadora en modo directo con base y divisor coprimos devuelve la regla por consola")]
	public void Calculadora_Directo_ArgumentosCorrectos_SalidaCeroYUnaRegla() {
		_args = ["single","7","10","--length","4"];
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(7, 4, 10);

		int salida = CalcDivCLI.Main(_args);

		string[] lineasResultado = LineasDeWriter(_escritorSalida);
		Assert.Multiple(() => {
			Assert.That(lineasResultado[0], Is.EqualTo(regla.ToString()));
			Assert.That(lineasResultado, Has.Length.EqualTo(1));
			Assert.That(salida, Is.Zero);
			Assert.That(_escritorError.ToString(), Is.Not.Empty);
		});
	}

	[Test(Description = "Al llamar a la calculadora en modo directo con -j con base y divisor coprimos devuelve la regla por consola en JSON")]
	public void Calculadora_DirectoJSON_ArgumentosCorrectos_SalidaCeroJSONCorrectoYUnaRegla() {
		_args = ["single", "17", "10","--length", "3", "-j"];
		long @base = 10, divisor = 17;
		int coeficientes = 3;
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, coeficientes, @base);
		JsonNode? nodo = null;

		int salida = CalcDivCLI.Main(_args);

		string[] lineasResultado = LineasDeWriter(_escritorSalida);
		Assert.Multiple(() => {
			Assert.DoesNotThrow(() => nodo = JsonNode.Parse(_escritorSalida.ToString()));
			Assert.That(nodo, Is.Not.Null);
			Assert.That((long?)nodo["base"], Is.EqualTo(@base));
			Assert.That((long?)nodo["divisor"], Is.EqualTo(divisor));
			Assert.That((JsonArray?)nodo["coefficients"], Is.Not.Null.And.Property("Count").EqualTo(coeficientes));
			Assert.That(nodo["rules"], Is.Null);
			JsonArray array = nodo["coefficients"] as JsonArray;
			for (int i = 0; i < regla.Longitud; i++) {
				Assert.That((long)array[i], Is.EqualTo(regla.Coeficientes[i]));
			}
		});
	}

	[Test(Description = "Al llamar a la calculadora en modo directo con datos no numéricos devuelve una pantalla de error y 2")]
	public void Calculadora_Directo_ArgumentosInvalidos_SalidaDosYError() {
		_args = ["single", "-sd", "5", "3"];

		int salida = CalcDivCLI.Main(_args);

		Assert.Multiple(() => {
			Assert.That(salida, Is.EqualTo(2));
			Assert.That(_escritorError.ToString(), Contains.Substring(TextoResource.SentenceErrorsHeadingText));
		});
	}

	[Test(Description = "Al llamar a la calculadora en modo directo con base y divisor coprimos devuelve la regla por consola")]
	public void Calculadora_DirectoCoeficientes_ArgumentosNoCoprimos_SalidaUnoYSinRegla() {
		_args = ["single", "7", "14"]; //Mcd = 7

		int salida = CalcDivCLI.Main(_args);

		string[] lineasResultado = LineasDeWriter(_escritorSalida)
			, lineasError = LineasDeWriter(_escritorError);
		Assert.Multiple(() => {
			Assert.That(lineasResultado[0], Is.Empty);
			Assert.That(lineasResultado, Has.Length.EqualTo(1));
			Assert.That(salida, Is.EqualTo(1));
			Assert.That(lineasError, Has.Length.GreaterThan(1)); //Uno para el mensaje de parámetros
		});
	}

	[Test(Description = "Al llamar a la calculadora en modo directo con base y divisor coprimos devuelve la regla por consola")]
	public void Calculadora_DirectoExtra_ArgumentosNoCoprimos_SalidaUnoYSinRegla() {
		_args = ["single", "1", "10", "-x"];

		int salida = CalcDivCLI.Main(_args);

		string[] lineasResultado = LineasDeWriter(_escritorSalida)
			, lineasError = LineasDeWriter(_escritorError);
		Assert.Multiple(() => {
			Assert.That(lineasResultado[0], Is.EqualTo(TextoCalculos.ReglaExplicadaUno));
			Assert.That(lineasResultado, Has.Length.EqualTo(1));
			Assert.That(salida, Is.Zero);
			Assert.That(lineasError[0], Is.EqualTo(string.Format(TextoEjecucion.MensajeParametrosDirecto, 1, 10, 1))); //Uno para el mensaje de parámetros
		});
	}

	[Test(Description = "Al llamar con -m y dos strings de longs separados por comas, todos coprimos entre sí, devuelve todas las reglas de divisibilidad")]
	public void Calculadora_VariasReglas_ArgumentosValidos_DevuelveTodasLasReglasYCero() { //Se comprueba la salida en otra prueba
		_args = ["multiple", "3,7,11,101", "10,20", "--length", "3"];
		int longitudEsperada = 9; // Ocho de las reglas y otro más

		int salida = CalcDivCLI.Main(_args);

		string[] lineasResultado = LineasDeWriter(_escritorSalida);
		Assert.Multiple(() => {
			Assert.That(salida, Is.EqualTo(0));
			Assert.That(lineasResultado, Has.Length.EqualTo(longitudEsperada));
		});
	}

	[Test(Description = "Al llamar con -m y dos strings de longs separados por comas, todos coprimos entre sí, devuelve todas las reglas de divisibilidad")]
	public void Calculadora_VariasReglas_ArgumentosParcialmenteCoprimos_DevuelveTodasLasReglasPosiblesYCinco() { //Se comprueba la salida en otra prueba
		_args = ["multiple", "3,7,11,101,20,10", "10,20", "--length", "3"];
		int longitudEsperada = 9;

		int salida = CalcDivCLI.Main(_args);

		string[] lineasResultado = LineasDeWriter(_escritorSalida);
		Assert.Multiple(() => {
			Assert.That(salida, Is.EqualTo(5));
			Assert.That(lineasResultado, Has.Length.EqualTo(longitudEsperada));
		});
	}

	[Test(Description = "Al llamar con -m y dos strings de longs separados por comas, todos coprimos entre sí, devuelve todas las reglas de divisibilidad")]
	public void Calculadora_VariasReglas_ArgumentosNoCoprimos_NoDevuelveReglasYSeis() { //Se comprueba la salida en otra prueba
		_args = ["multiple", "2,4,5,10", "10,20", "--length", "3"];

		int salida = CalcDivCLI.Main(_args);

		Assert.Multiple(() => {
			Assert.That(salida, Is.EqualTo(6));
			Assert.That(_escritorSalida.ToString(), Is.Empty);
		});
	}

	[Test]
	public void Calculadora_VariasJSON_ArgumentosParcialmenteCorrectos_DevuelveLasCorrectasJSONCorrectoYCinco() {
		int[] divisores = [3, 7, 101, 20], bases = [10, 13];
		_args = ["multiple", "-j", "3,7,101,20", "10,13", "--length", "3"];
		JsonArray jsonReglas = [];
		List<ReglaCoeficientes> reglas = [];
		foreach (int i in divisores) {
			foreach (int j in bases) {
				reglas.Add(new(i,j,3));
			}
		}

		int salida = CalcDivCLI.Main(_args);
		Assert.DoesNotThrow(() => jsonReglas = (JsonArray)JsonNode.Parse(_escritorSalida.ToString())!); // Debe devolver una lista de objetos
		Assert.Multiple(() => {
			Assert.That(salida, Is.EqualTo(5));
			Assert.That(jsonReglas, Is.Not.Null);
			for (int indiceReglas = 0; indiceReglas < reglas.Count; indiceReglas++) { // Comprobamos en cada regla que los elementos están en el mismo orden
				Assert.That((long)jsonReglas[indiceReglas]["base"], Is.EqualTo(reglas[indiceReglas].Base));
				Assert.That((long)jsonReglas[indiceReglas]["divisor"], Is.EqualTo(reglas[indiceReglas].Divisor));
				for (int indiceCoeficientes = 0; indiceCoeficientes < reglas[indiceReglas].Longitud; indiceCoeficientes++) { // Comprobamos cada regla
					Assert.That((long)jsonReglas[indiceReglas]["coefficients"][indiceCoeficientes], Is.EqualTo(reglas[indiceReglas].Coeficientes[indiceCoeficientes]));
				}
			}
		});
	}

	[Test]
	public void Calculadora_VariasExtraJSON_ArgumentosCorrectos_DevuelveJSONCorrectoYCero() {
		int[] divisores = [0, 1, 2, 7, 9], bases = [10];
		_args = ["multiple", "-xj", "0,1,2,7,9", "10", "--length", "3"];
		JsonArray jsonReglas = [];
		List<ReglaCoeficientes> reglas = [];
		foreach (int i in divisores) {
			foreach (int j in bases) {
				reglas.Add(new(i, j, 3));
			}
		}

		int salida = CalcDivCLI.Main(_args);
		Assert.DoesNotThrow(() => jsonReglas = (JsonArray)JsonNode.Parse(_escritorSalida.ToString())!); // Debe devolver una lista de objetos
		Assert.Multiple(() => {
			Assert.That(salida, Is.Zero);
			Assert.That(jsonReglas, Is.Not.Null);
			for (int indiceReglas = 0; indiceReglas < reglas.Count; indiceReglas++) { // Comprobamos en cada regla que los elementos están en el mismo orden
				Assert.That((long)jsonReglas[indiceReglas]["base"], Is.EqualTo(reglas[indiceReglas].Base));
				Assert.That((long)jsonReglas[indiceReglas]["divisor"], Is.EqualTo(reglas[indiceReglas].Divisor));
			}
			Assert.That((string)jsonReglas[0]["type"], Is.EqualTo(CasosDivisibilidad.DIVISOR_ZERO.ToString()));
			Assert.That((string)jsonReglas[1]["type"], Is.EqualTo(CasosDivisibilidad.DIVISOR_ONE.ToString()));
			Assert.That((string)jsonReglas[2]["type"], Is.EqualTo(CasosDivisibilidad.DIGITS.ToString()));
			Assert.That((string)jsonReglas[3]["type"], Is.EqualTo(CasosDivisibilidad.SUBSTRACT_BLOCKS.ToString()));
			Assert.That((string)jsonReglas[4]["type"], Is.EqualTo(CasosDivisibilidad.ADD_BLOCKS.ToString()));
		});
	}

	[Test]
	public void Calculadora_DialogoSinBuclesCoeficientesCorrectos_DevuelveReglaYCero() {
		_args = ["dialog", "-s", "--no-loop"];
		int longitud = 5;
		long divisor = 7, @base = 10;
		ReglaCoeficientes reglaEsperada = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		_lectorEntrada =
			new StringReader(@base + _escritorError.NewLine
			 + divisor + _escritorError.NewLine
			 + longitud + _escritorError.NewLine
			 + "-");
		Console.SetIn(_lectorEntrada);

		int salida = CalcDivCLI.Main(_args);

		List<long> coeficientes = _escritorSalida.ToString()!.Split(", ").Select(long.Parse).ToList();
		Assert.Multiple(() => {
			Assert.That(salida, Is.Zero);
			Assert.That(coeficientes, Is.EquivalentTo(reglaEsperada.Coeficientes));
		});
	}

	private static string[] ReglasToArray(List<ReglaCoeficientes> reglas) {
		return reglas.Select(regla => regla.ToString()).ToArray();
	}

	[TearDown]
	public void RestaurarEntradaYSalida() {
		Thread.CurrentThread.CurrentUICulture = _culturaActual;
		Thread.CurrentThread.CurrentCulture = _culturaActual;
		_lectorEntrada?.Close();
		_escritorSalida.Close();
		_escritorError.Close();
		Console.SetOut(_salidaOriginal);
		Console.SetIn(_entradaOriginal);
		Console.SetError(_errorOriginal);
	}

	private static string[] LineasDeWriter(TextWriter writer) {
		return writer.ToString()?.Split(Environment.NewLine) ?? [];
	}
}
