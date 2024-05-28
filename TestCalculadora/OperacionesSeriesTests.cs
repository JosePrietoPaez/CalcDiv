using Listas;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Operaciones;
using System;

namespace TestCalculadora {
	[TestFixture]
	public class OperacionesSeriesTests {

		private static readonly int LONGITUD = 10;
		
		[Test(Description = "PotenciaModProgresiva lanza excepción si el módulo es cero")] //Iba a hacer que comprobase negativos, pero al parecer el operador % funciona con divisores negativos
		public void PotenciaModProgresiva_ModuloCero_LanzaExcepcion() {
			// Arrange
			IListaDinamica<long> serie = new ListSerie<long>() {
				Longitud = LONGITUD //Longitud tiene un setter, pone el elemento por defecto de la clase en las posiciones nuevas
			};
			long @base = 10;
			long mod = 0;
			int fin = 5;
			int pos = 0;

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => OperacionesSeries.PotenciaModProgresiva(
				serie,
				@base,
				mod,
				fin,
				pos));
		}

		[TestCase(0,TestName = "PotenciaModProgresiva lanza excepción si la última potencia es cero")]
		[TestCase(-1,TestName = "PotenciaModProgresiva lanza excepción si la última potencia es negativa")]
		public void PotenciaModProgresiva_FinNoPositivo_LanzaExcepcion(int argumento) {
			// Arrange
			IListaDinamica<long> serie = new ListSerie<long>() {
				Longitud = LONGITUD
			};
			long @base = 10;
			long mod = 3;
			int fin = argumento;
			int pos = 0;

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => OperacionesSeries.PotenciaModProgresiva(
				serie,
				@base,
				mod,
				fin,
				pos));
		}

		[TestCase(-1,TestName = "PotenciaModProgresiva lanza excepción si la posición es negativa")]
		[TestCase(100,TestName = "PotenciaModProgresiva lanza excepción si la posición mayor a la longitud")] //Si es igual a la longitud funciona
		public void PotenciaModProgresiva_PosicionInvalida_LanzaExcepcion(int argumento) {
			// Arrange
			IListaDinamica<long> serie = new ListSerie<long>() {
				Longitud = LONGITUD
			};
			long @base = 10;
			long mod = 3;
			int fin = 5;
			int pos = argumento;

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => OperacionesSeries.PotenciaModProgresiva(
				serie,
				@base,
				mod,
				fin,
				pos));
		}

		[Test(Description = "Cuando los argumentos son correctos, PotenciaModProgresiva inserta el resto las potencias de base desde 1 a fin entre mod en serie, a partir de pos")]
		public void PotenciaModProgresiva_ArgumentosValidos_IntroduceLasPotencias() {
			// Arrange
			IListaDinamica<long> serie = new ListSerie<long>() {
				Longitud = LONGITUD
			};
			long @base = 11;
			long mod = 7;
			int fin = 6;
			int pos = 4;

			// Act
			OperacionesSeries.PotenciaModProgresiva(serie,
				@base,
				mod,
				fin,
				pos);

			// Assert
			Assert.That(serie, Has.Exactly(10).EqualTo(0)); // No se cumplirá si base es 0
			Assert.That(serie, Has.Property("Longitud").EqualTo(LONGITUD + fin));
			for (int i = 0; i < fin; i++) { //Necesito comprobar que se cumple para todos los elementos
				long elementoI = CalculosEstatico.PotenciaEntera(@base, i+1) % mod; //No se usa PotenciaEntera en la función, pero lo hace más sencillo.
				Assert.That(serie, Has.ItemAt(pos + i).EqualTo(elementoI));
			}
		}

		[Test]
		public void ArrayFalso_StateUnderTest_ExpectedBehavior() {
			// Arrange
			bool[] arr = new bool[10];

			// Act
			var result = OperacionesSeries.ArrayFalso(
				arr);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void IncrementarArray_StateUnderTest_ExpectedBehavior() {
			// Arrange
			bool[] arr = new bool[10];

			// Act
			OperacionesSeries.IncrementarArray(
				arr);

			// Assert
			Assert.Fail();
		}
	}
}
