using Listas;
using Operaciones;
using ProgramaDivisibilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TestCalculadora {
	/// <summary>
	/// Esta clase contendrá pruebas de integración en la que usaremos la salida por consola para determinar si funciona correctamente
	/// </summary>
	/// <remarks>
	/// No son unitarias, ya que se prueba la salida del método main, que involucra varias clases
	/// </remarks>
	[TestFixture]
	internal class ITCalculadoraDivisibilidadCLI {

		private TextWriter _salidaOriginal,
			_lectorSalida;
		private TextReader _entradaOriginal,
			_lectorEntrada;
		private string[] _args = [];

		[SetUp]
		public void CambiarEntradaYSalida() {
			_salidaOriginal = Console.Out;
			_entradaOriginal = Console.In;
			_lectorSalida = new StringWriter();
			Console.SetOut(_lectorSalida);
		}

		[Test(Description = "Al llamar a la calculadora en modo directo con base y divisor coprimos devuelve la regla por consola")]
		public void Calculadora_Directo_ArgumentosCorrectos_SalidaCeroYUnaRegla() {
			_args = ["-d","7","10","4"];
			ListSerie<long> regla = new();
			Calculos.ReglaDivisibilidadOptima(regla, 7, 4, 10);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineasResultado = LineasDeWriter(_lectorSalida);

			Assert.Multiple(() => {
				Assert.That(lineasResultado[0], Is.EqualTo(regla.ToString()));
				Assert.That(lineasResultado, Has.Length.EqualTo(1));
				Assert.That(salida, Is.Zero);
			});
		}

		[Test(Description = "Al llamar a calculadora con -H se escribe Ayuda.txt")]
		public void Calculadora_Ayuda_EscribeElArchivo() {
			_args = ["-H"];

			int salida = CalculadoraDivisibilidadCLI.Main(_args);

			Assert.Multiple(() => {
				Assert.That(_lectorSalida.ToString()?[0..^Environment.NewLine.Length], // Hay un \r\n de más
					Is.EqualTo(File.ReadAllText("Ayuda.txt")));
				Assert.That(salida, Is.Zero);
			});
		}

		[Test(Description = "Al llamar a calculadora con -h se escribe AyudaCorta.txt")]
		public void Calculadora_AyudaCorta_EscribeElArchivo() {
			_args = ["-h"];

			int salida = CalculadoraDivisibilidadCLI.Main(_args);

			Assert.Multiple(() => {
				Assert.That(_lectorSalida.ToString()?[0..^Environment.NewLine.Length], Is.EqualTo(File.ReadAllText("AyudaCorta.txt"))); // Hay un \r\n de más
				Assert.That(salida, Is.Zero);
			});
		}

		[Test(Description = "Al llamar a la calculadora en modo directo inverso con base y divisor coprimos devuelve la regla por consola en el orden inverso")]
		public void Calculadora_DirectoTodosNombre_ArgumentosCorrectos_SalidaCeroYUnaRegla() {
			_args = ["-ad", "5", "13", "5","-n","Nombre"];
			ListSerie<ListSerie<long>> regla = new("Nombre");
			Calculos.ReglasDivisibilidad(regla, 5, 5, 13); //Genera 2^cantidad reglas
			string[] lineasReglas = ReglasToArray(regla);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);

			string[] lineasResultado = LineasDeWriter(_lectorSalida);
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
			_lectorEntrada?.Dispose();
			_lectorSalida.Close();
			Console.SetOut(_salidaOriginal);
			Console.SetIn(_entradaOriginal);
		}

		private static string[] LineasDeWriter(TextWriter writer) {
			return writer.ToString()?.Split(Environment.NewLine) ?? [];
		}
	}
}
