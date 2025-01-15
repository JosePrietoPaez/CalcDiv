using NUnit.Framework;
using Operaciones;
using System.Globalization;

namespace TestReglas {
	[TestFixture]
	public class ReglaRestarTests {

		private readonly CultureInfo _culturaActual = CultureInfo.CurrentCulture;

		[Test(Description = "ReglaUno tiene divisor uno al generarse.")]
		public void Generar_DevuelveReglaConDivisorYBaseCorrectos() {
			// Arrange
			long @base = 10, divisor = 101;

			// Act
			var regla = IRegla.GenerarReglaPorTipo(divisor, @base);

			Assert.Multiple(() => {
				// Assert
				Assert.That(regla.Base, Is.EqualTo(@base));
				Assert.That(regla.Divisor, Is.EqualTo(divisor));
				Assert.That(regla.Tipo, Is.EqualTo(CasosDivisibilidad.SUBTRACT_BLOCKS));
				var reglaSumar = regla as ReglaRestar;
				Assert.That(reglaSumar!.Longitud, Is.EqualTo(2));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo no es divisible.")]
		public void AplicarRegla_Fracaso_ExplicaFallo([Values(34L, -4234231897L, 67482364876L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaRestar = IRegla.GenerarReglaPorTipo(101, 10);

			// Act
			var result = reglaRestar.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaRestar, Is.TypeOf<ReglaRestar>());
				Assert.That(result, Does.Contain(", therefore the original dividend, " + dividendo + ", also isn't."));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo es divisible.")]
		public void AplicarRegla_Exito_ExplicaExito([Values(1010L, -58200442L, 5878244642L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaRestar = IRegla.GenerarReglaPorTipo(101, 10);

			// Act
			var result = reglaRestar.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaRestar, Is.TypeOf<ReglaRestar>());
				Assert.That(result, Does.Contain(", therefore the original dividend, " + dividendo + ", also is."));
			});
		}

		[Test]
		public void ToString_ExplicaRegla() {
			// Arrange
			var reglaRestar = IRegla.GenerarReglaPorTipo(11, 10);

			// Act
			var result = reglaRestar.ToString();

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaRestar, Is.TypeOf<ReglaRestar>());
				Assert.That(result, Is.EqualTo(reglaRestar.ReglaExplicada));
			});
		}

		[TearDown]
		public void TearDown() {
			Thread.CurrentThread.CurrentCulture = _culturaActual;
			Thread.CurrentThread.CurrentUICulture = _culturaActual;
		}
	}
}
