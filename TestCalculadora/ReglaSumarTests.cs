using NUnit.Framework;
using Operaciones;
using System.Globalization;

namespace TestReglas {
	[TestFixture]
	public class ReglaSumarTests {

		private readonly CultureInfo _culturaActual = CultureInfo.CurrentCulture;

		[Test(Description = "ReglaUno tiene divisor uno al generarse.")]
		public void Generar_DevuelveReglaConDivisorYBaseCorrectos() {
			// Arrange
			long @base = 10, divisor = 99;

			// Act
			var regla = IRegla.GenerarReglaPorTipo(divisor, @base);

			Assert.Multiple(() => {
				// Assert
				Assert.That(regla.Base, Is.EqualTo(@base));
				Assert.That(regla.Divisor, Is.EqualTo(divisor));
				Assert.That(regla.Tipo, Is.EqualTo(CasosDivisibilidad.ADD_BLOCKS));
				var reglaSumar = regla as ReglaSumar;
				Assert.That(reglaSumar!.Longitud, Is.EqualTo(2));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo no es divisible.")]
		public void AplicarRegla_Fracaso_ExplicaFallo([Values(34L, -4234231897L, 67482364876L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaSumar = IRegla.GenerarReglaPorTipo(99, 10);

			// Act
			var result = reglaSumar.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaSumar, Is.TypeOf<ReglaSumar>());
				Assert.That(result, Does.Contain(", therefore the original dividend, " + dividendo + ", also isn't."));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo es divisible.")]
		public void AplicarRegla_Exito_ExplicaExito([Values(990L, -94880709L, 9393190191L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaSumar = IRegla.GenerarReglaPorTipo(99, 10);

			// Act
			var result = reglaSumar.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaSumar, Is.TypeOf<ReglaSumar>());
				Assert.That(result, Does.Contain(", therefore the original dividend, " + dividendo + ", also is."));
			});
		}

		[Test]
		public void ToString_ExplicaRegla() {
			// Arrange
			var reglaSumar = IRegla.GenerarReglaPorTipo(9, 10);

			// Act
			var result = reglaSumar.ToString();

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaSumar, Is.TypeOf<ReglaSumar>());
				Assert.That(result, Is.EqualTo(reglaSumar.ReglaExplicada));
			});
		}

		[TearDown]
		public void TearDown() {
			Thread.CurrentThread.CurrentCulture = _culturaActual;
			Thread.CurrentThread.CurrentUICulture = _culturaActual;
		}
	}
}
