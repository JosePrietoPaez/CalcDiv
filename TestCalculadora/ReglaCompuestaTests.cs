using NUnit.Framework;
using Operaciones;
using System;
using System.Globalization;
using System.Numerics;

namespace TestReglas {
	[TestFixture]
	public class ReglaCompuestaTests {

		private readonly CultureInfo _culturaActual = CultureInfo.CurrentCulture;

		[Test]
		public void Generar_DevuelveReglaCompuesta() {
			// Arrange
			long @base = 10, divisor = 780;
			var listaPotencias = new List<long>([2,1,1,1]);
			var listaPrimos = new List<long>([2,3,5,13]);
			var listaTipos = new List<CasosDivisibilidad>(
				[CasosDivisibilidad.DIGITS
				, CasosDivisibilidad.ADD_BLOCKS
				, CasosDivisibilidad.DIGITS
				, CasosDivisibilidad.SUBSTRACT_BLOCKS]);

			// Act
			var regla = IRegla.GenerarReglaPorTipo(divisor, @base);

			Assert.Multiple(() => {
				// Assert
				Assert.That(regla.Tipo, Is.EqualTo(CasosDivisibilidad.COMPOSITE_RULE));
				Assert.That(regla.Base, Is.EqualTo(@base));
				Assert.That(regla.Divisor, Is.EqualTo(divisor));
				var reglaCompuesta = regla as ReglaCompuesta;
				Assert.That(reglaCompuesta!.Subreglas.Select(reg => reg.Tipo), Is.EquivalentTo(listaTipos));
				Assert.That(reglaCompuesta!.FactoresPrimos, Is.EquivalentTo(listaPrimos));
				Assert.That(reglaCompuesta!.Potencias, Is.EquivalentTo(listaPotencias));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo no es divisible.")]
		public void AplicarRegla_Fracaso_ExplicaFallo([Values(34L, -4234231897L, 67482364876L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaCompuesta = IRegla.GenerarReglaPorTipo(60, 10);

			// Act
			var result = reglaCompuesta.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCompuesta, Is.TypeOf<ReglaCompuesta>());
				Assert.That(result, Does.Contain(", therefore, it is not divisible by " + 60 + "."));
			});
		}

		[Test(Description = "AplicarRegla explicará el proceso y podrá decidir que el dividendo es divisible.")]
		public void AplicarRegla_Exito_ExplicaExito([Values(480L, -31764823680L, 6482745765720L)] long dividendo) {
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			// Arrange
			var reglaCompuesta = IRegla.GenerarReglaPorTipo(60, 10);

			// Act
			var result = reglaCompuesta.AplicarRegla(
				dividendo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCompuesta, Is.TypeOf<ReglaCompuesta>());
				Assert.That(result, Does.Contain(", therefore, it is divisible by " + 60 + "."));
			});
		}

		[Test]
		public void ToString_ExplicaRegla() {
			// Arrange
			var reglaCompuesta = IRegla.GenerarReglaPorTipo(30, 10);

			// Act
			var result = reglaCompuesta.ToString();

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCompuesta, Is.TypeOf<ReglaCompuesta>());
				Assert.That(result, Is.EqualTo(reglaCompuesta.ReglaExplicada));
			});
		}

		[TearDown]
		public void TearDown() {
			Thread.CurrentThread.CurrentCulture = _culturaActual;
			Thread.CurrentThread.CurrentUICulture = _culturaActual;
		}
	}
}
