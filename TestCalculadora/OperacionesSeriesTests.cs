using Operaciones;

namespace TestCalculadora {
	[TestFixture]
	public class OperacionesSeriesTests {

		[TestFixture]
		public class PotenciaModProgresivaTests {

			[TestCase(0, TestName = "PotenciaModProgresiva lanza excepción si el módulo es 0")]
			[TestCase(-1, TestName = "PotenciaModProgresiva lanza excepción si el módulo es negativo")]
			public void PotenciaModProgresiva_ModuloNoPositivo_LanzaExcepcion(long mod) {
				long @base = 5;
				int longitud = 4;

				Assert.Throws<ArgumentOutOfRangeException>(() => OperacionesListas.PotenciaModProgresiva(@base, mod, longitud));
			}

			[TestCase(-1, TestName = "PotenciaModProgresiva lanza excepción si la longitud es negativa")]
			public void PotenciaModProgresiva_LongitudNegativa_LanzaExcepcion(int longitud) {
				long mod = 5, @base = 5;

				Assert.Throws<ArgumentOutOfRangeException>(() => OperacionesListas.PotenciaModProgresiva(@base, mod, longitud));
			}

			[TestCase(0)]
			[TestCase(1)]
			[TestCase(2)]
			[TestCase(5)]
			[TestCase(-3)]
			public void PotenciaModProgresiva_ArgumentosCorrectos_DevuelveListaConPotencias(int @base) {
				long mod = 10;
				int longitud = 10;

				List<long> resultado = OperacionesListas.PotenciaModProgresiva(@base, mod, longitud);

				for (int i = 0; i < resultado.Count; i++) {
					Assert.That(resultado[i], Is.EqualTo(Calculos.ProductoMod(Calculos.PotenciaEntera(@base, i), @base, mod)));
				}
			}
		}

		[Test]
		public void ArrayFalso_ArraySinTrue_DevuelveTrue() {
			// Arrange
			bool[] arr = new bool[10];

			// Act
			var result = OperacionesListas.ArrayFalso(
				arr);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void ArrayFalso_ArrayConTrue_DevuelveFalse() {
			// Arrange
			bool[] arr = {true,false,false,false,false};

			// Act
			var result = OperacionesListas.ArrayFalso(
				arr);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void IncrementarArray_ArrayFalso_PrimerElementoTrue() {
			// Arrange
			int longitud = 10;
			bool[] arr = new bool[longitud];

			// Act
			OperacionesListas.IncrementarArray(
				arr);

			// Assert
			Assert.Multiple(() => {
				Assert.That(arr, Has.Length.EqualTo(longitud));
				Assert.That(arr[0], Is.True);
			});
		}

		[Test]
		public void IncrementarArray_ArrayPrimeroTrue_PrimerElementoFalseSegundoTrue() {
			// Arrange
			int longitud = 10;
			bool[] arr = [true,false,false,false,false,false,false,false,false,false];

			// Act
			OperacionesListas.IncrementarArray(
				arr);

			// Assert
			Assert.Multiple(() => {
				Assert.That(arr, Has.Length.EqualTo(longitud));
				Assert.That(arr[0], Is.False);
				Assert.That(arr[1], Is.True);
			});
		}

		[Test]
		public void IncrementarArray_ArrayPrimeroSegundoTrue_PrimerSegundoElementoFalseSegundoTrue() {
			// Arrange
			int longitud = 10;
			bool[] arr = [true, true, false, false, false, false, false, false, false, false];

			// Act
			OperacionesListas.IncrementarArray(
				arr);

			// Assert
			Assert.Multiple(() => {
				Assert.That(arr, Has.Length.EqualTo(longitud));
				Assert.That(arr[0], Is.False);
				Assert.That(arr[1], Is.False);
				Assert.That(arr[2], Is.True);
			});
		}

		[Test]
		public void IncrementarArray_ArrayTrue_DevuelveArrayFalse() {
			// Arrange
			int longitud = 10;
			bool[] arr = [true, true, true, true, true, true, true, true, true, true];

			// Act
			OperacionesListas.IncrementarArray(
				arr);

			// Assert
			Assert.Multiple(() => {
				Assert.That(arr, Has.Length.EqualTo(longitud));
				Assert.That(arr, Has.All.EqualTo(false));
			});
		}

	}
}
