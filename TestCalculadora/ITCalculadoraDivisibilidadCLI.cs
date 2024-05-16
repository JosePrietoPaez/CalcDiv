using Listas;
using Operaciones;
using ProgramaDivisibilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			CalculosEstatico.ReglaDivisibilidadOptima(regla, 7, 4, 10);

			int salida = CalculadoraDivisibilidadCLI.Main(_args);
			string[] lineas = LineasDeWriter(_lectorSalida);

			Assert.Multiple(() => {
				Assert.That(lineas[1], Is.EqualTo(regla.ToString())); //La segunda porque primero se escriben los argumentos
				Assert.That(salida, Is.Zero);
			});
		}

		[Test(Description = "Al llamar a calculadora con -H se escribe Ayuda.txt")]
		public void Calculadora_Ayuda_EscribeElArchivo() {
			_args = ["-H"];

			int salida = CalculadoraDivisibilidadCLI.Main(_args);

			Assert.Multiple(() => {
				Assert.That(_lectorSalida.ToString()?[0..^2], // Hay un \r\n de más
					Is.EqualTo(File.ReadAllText("Ayuda.txt")));
				Assert.That(salida, Is.Zero);
			});
		}

		[Test(Description = "Al llamar a calculadora con -h se escribe AyudaCorta.txt")]
		public void Calculadora_AyudaCorta_EscribeElArchivo() {
			_args = ["-h"];

			int salida = CalculadoraDivisibilidadCLI.Main(_args);

			Assert.Multiple(() => {
				Assert.That(_lectorSalida.ToString()?[0..^2], Is.EqualTo(File.ReadAllText("AyudaCorta.txt"))); // Hay un \r\n de más
				Assert.That(salida, Is.Zero);
			});
		}

		[TearDown]
		public void RestaurarEntradaYSalida() {
			_lectorSalida.Close();
			Console.SetOut(_salidaOriginal);
			Console.SetIn(_entradaOriginal);
		}

		private static string[] LineasDeWriter(TextWriter writer) {
			return writer.ToString()?.Replace(Environment.NewLine, "\n").Split('\n') ?? [];
		}
	}
}
