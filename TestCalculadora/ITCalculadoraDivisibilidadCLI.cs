using Listas;
using NUnit.Framework.Legacy;
using Operaciones;
using ProgramaDivisibilidad;
using System.Globalization;
using System.Text.Json.Nodes;

namespace TestCalculadora {
	/// <summary>
	/// Esta clase contendrá pruebas de integración en la que usaremos la salida por consola para determinar si funciona correctamente
	/// </summary>
	/// <remarks>
	/// No son unitarias, ya que se prueba la salida del método main, que involucra varias clases
	/// </remarks>
	[TestFixture]
	internal class ITCalculadoraDivisibilidadCLI {

		private TextWriter _salidaOriginal
			,_escritorSalida
			,_errorOriginal
			,_escritorError;
		private TextReader _entradaOriginal
			,_lectorEntrada;
		private string[] _args = [];

		private CultureInfo _culturaActual = CultureInfo.CurrentCulture;

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

		[Test(Description = "Al llamar con -h se escribirá el texto de ayuda de los recursos")]
		public void Calculadora_AyudaCorta_EscribeLaAyuda() {
			_args = ["-h"];
			string[] textoEsperado = ProgramaDivisibilidad.Recursos.TextoResource.AyudaCorta.Split(Environment.NewLine);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineasResultado = LineasDeWriter(_escritorSalida);

			Assert.Multiple(() => {
				Assert.That(lineasResultado, Is.EqualTo(textoEsperado));
				Assert.That(salida, Is.Zero);
			});
		}

		[Test(Description = "Al llamar con -H se escribirá el texto de ayuda de los recursos")]
		public void Calculadora_Ayuda_CambioCultura_EscribeLaAyudaEnNuevoIdioma() {
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
			Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
			_args = ["-H"];
			string[] textoEsperado = ProgramaDivisibilidad.Recursos.TextoResource.Ayuda.Split(Environment.NewLine);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineasResultado = LineasDeWriter(_escritorSalida);

			Assert.Multiple(() => {
				Assert.That(lineasResultado, Is.EqualTo(textoEsperado));
				Assert.That(salida, Is.Zero);
				Assert.That(lineasResultado[0], Contains.Substring("Help"));
			});
			Thread.CurrentThread.CurrentUICulture = _culturaActual;
			Thread.CurrentThread.CurrentCulture = _culturaActual;
		}

		[Test(Description = "Al llamar a la calculadora en modo directo con base y divisor coprimos devuelve la regla por consola")]
		public void Calculadora_Directo_ArgumentosCorrectos_SalidaCeroYUnaRegla() {
			_args = ["-d","7","10","4"];
			ListSerie<long> regla = new();
			Calculos.ReglaDivisibilidadOptima(regla, 7, 4, 10);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
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
			_args = ["-jd", "17", "10", "3"];
			long @base = 10, divisor = 17;
			int coeficientes = 3;
			ListSerie<long> regla = new();
			Calculos.ReglaDivisibilidadOptima(regla, divisor, coeficientes, @base);
			JsonNode? nodo = null;

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineasResultado = LineasDeWriter(_escritorSalida);

			Assert.Multiple(() => {
				Assert.DoesNotThrow(() => nodo = JsonValue.Parse(_escritorSalida.ToString()!));
				Assert.That(nodo, Is.Not.Null);
				Assert.That((string?)nodo["name"], Is.Not.Null.And.Empty);
				Assert.That((long?)nodo["base"], Is.EqualTo(@base));
				Assert.That((long?)nodo["divisor"], Is.EqualTo(divisor));
				Assert.That((JsonArray?)nodo["coefficients"], Is.Not.Null.And.Property("Count").EqualTo(coeficientes));
				Assert.That(nodo["rules"], Is.Null);
				JsonArray array = (JsonArray)nodo["coefficients"]!;
				for (int i = 0; i < regla.Longitud; i++) {
					Assert.That((long)array[i], Is.EqualTo(regla[i]));
				}
			});
		}

		[Test(Description = "Al llamar a la calculadora en modo directo con -j y -a con base y divisor coprimos devuelve todas las reglas por consola en JSON")]
		public void Calculadora_DirectoTodosJSONNombre_ArgumentosCorrectos_SalidaCeroJSONCorrectoYTodasLasReglas() {
			string nombre = "Nombre";
			_args = ["-ajd", "17", "5", "3", "-n", nombre];
			long @base = 5, divisor = 17;
			int coeficientes = 3;
			ListSerie<ListSerie<long>> reglas = new();
			Calculos.ReglasDivisibilidad(reglas, divisor, coeficientes, @base);
			JsonNode? nodo = null;

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineasResultado = LineasDeWriter(_escritorSalida);

			Assert.Multiple(() => {
				Assert.DoesNotThrow(() => nodo = JsonValue.Parse(_escritorSalida.ToString()!));
				Assert.That(nodo, Is.Not.Null);
				Assert.That((string?)nodo["name"], Is.Not.Null.And.EqualTo(nombre));
				Assert.That((long?)nodo["base"], Is.EqualTo(@base));
				Assert.That((long?)nodo["divisor"], Is.EqualTo(divisor));
				Assert.That(nodo["coefficients"], Is.Null);
				Assert.That((JsonArray?)nodo["rules"], Is.Not.Null.And.Property("Count").EqualTo(Calculos.PotenciaEntera(2,coeficientes)));
				JsonArray array = (JsonArray)nodo["rules"]!;
				for (int i = 0; i < reglas.Longitud; i++) {
					for (int j = 0; j < reglas[0].Longitud; j++) {
						Assert.That((long)array[i]["coefficients"][j], Is.EqualTo(reglas[i][j]));
					}
				}
			});
		}

		[Test(Description = "Al llamar a la calculadora en modo directo con base y divisor coprimos devuelve la regla por consola")]
		public void Calculadora_Directo_ArgumentosNoCoprimos_SalidaUnoYSinRegla() {
			_args = ["-d", "7", "14"]; //Mcd = 7

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineasResultado = LineasDeWriter(_escritorSalida)
				, lineasError = LineasDeWriter(_escritorError);

			Assert.Multiple(() => {
				Assert.That(lineasResultado[0], Is.Empty);
				Assert.That(lineasResultado, Has.Length.EqualTo(1));
				Assert.That(salida, Is.EqualTo(1));
				Assert.That(lineasError, Has.Length.GreaterThan(1)); //Uno para el mensaje de parámetros
			});
		}

		[Test(Description = "Al llamar a la calculadora en modo directo inverso con base y divisor coprimos devuelve la regla por consola en el orden inverso")]
		public void Calculadora_DirectoTodosNombre_ArgumentosCorrectos_SalidaCeroYUnaRegla() {
			_args = ["-ad", "5", "13", "5","-n","Nombre"];
			ListSerie<ListSerie<long>> regla = new("Nombre");
			Calculos.ReglasDivisibilidad(regla, 5, 5, 13); //Genera 2^cantidad reglas
			string[] lineasReglas = ReglasToArray(regla);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);

			string[] lineasResultado = LineasDeWriter(_escritorSalida);
			Assert.Multiple(() => {
				Assert.That(lineasResultado[0..32], Is.EqualTo(lineasReglas)); //La segunda porque primero se escriben los argumentos 
				Assert.That(lineasResultado, Has.Length.EqualTo(lineasReglas.Length));
				Assert.That(salida, Is.Zero);
			});
		}

		private string[] ReglasToArray(ListSerie<ListSerie<long>> serie) {
			return serie.Select(regla => regla.ToStringCompleto()).ToArray();
		}

		[TearDown]
		public void RestaurarEntradaYSalida() {
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
}
