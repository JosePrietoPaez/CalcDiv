using NUnit.Framework;
using Operaciones;
using System;
using System.Globalization;
using System.Numerics;

namespace TestReglas {
	[TestFixture]
	public class ReglaCifrasTests {

		private readonly CultureInfo _culturaActual = CultureInfo.CurrentCulture;

		[Test]
		public void Generar_DevuelveReglaDeCifras() {
			// Arrange
			long @base = 15, divisor = 75;
			var listaValores = new List<BigInteger>([0, 75, 150]);

			// Act
			var reglaCifras = IRegla.GenerarReglaPorTipo(divisor, @base);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCifras.Tipo, Is.EqualTo(CasosDivisibilidad.DIGITS));
				Assert.That(reglaCifras.Base, Is.EqualTo(@base));
				Assert.That(reglaCifras.Divisor, Is.EqualTo(divisor));
				var cifras = reglaCifras as ReglaCifras;
				Assert.That(cifras!.Cifras, Is.EqualTo(2));
				Assert.That(cifras!.CasosPermitidos, Is.EquivalentTo(listaValores));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo no es divisible.")]
		public void AplicarRegla_Fracaso_ExplicaFallo([Values(34L, -4234231897L, 67482364876L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaCifras = IRegla.GenerarReglaPorTipo(10, 20);

			// Act
			var result = reglaCifras.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCifras, Is.TypeOf<ReglaCifras>());
				Assert.That(result, Does.Contain(", therefore the original dividend, " + dividendo + ", also isn't."));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo es divisible.")]
		public void AplicarRegla_Exito_ExplicaExito([Values(480L, -31764823640L, 6482745765720L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaCifras = IRegla.GenerarReglaPorTipo(10, 20);

			// Act
			var result = reglaCifras.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCifras, Is.TypeOf<ReglaCifras>());
				Assert.That(result, Does.Contain(", therefore the original dividend, " + dividendo + ", also is."));
			});
		}

		[Test(Description = "ReglaCifras explica como aplicarla.")]
		public void ToString_ExplicaRegla() {
			// Arrange
			var reglaCifras = IRegla.GenerarReglaPorTipo(10, 20);

			// Act
			var result = reglaCifras.ToString();

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCifras, Is.TypeOf<ReglaCifras>());
				Assert.That(result, Does.Contain(reglaCifras.ReglaExplicada).And.EndsWith((reglaCifras as ReglaCifras)!.CasosPermitidosString));
			});
		}

		[TearDown]
		public void TearDown() {
			Thread.CurrentThread.CurrentCulture = _culturaActual;
			Thread.CurrentThread.CurrentUICulture = _culturaActual;
		}
	}
}
