using NUnit.Framework;
using Operaciones;
using System;
using System.Numerics;

namespace TestReglas {
	[TestFixture]
	public class ReglaCeroTests {

		[Test(Description = "ReglaCero tiene divisor uno al generarse.")]
		public void Generar_DevuelveReglaConDivisorUnoYBaseCorrecta() {
			// Arrange
			long @base = 10, divisor = 0;

			// Act
			var reglaCero = IRegla.GenerarReglaPorTipo(divisor, @base);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaCero.Base, Is.EqualTo(10));
				Assert.That(reglaCero.Divisor, Is.Zero);
				Assert.That(reglaCero.Tipo, Is.EqualTo(CasosDivisibilidad.DIVISOR_ZERO));
			});
		}

		[Test(Description = "ReglaCero explica que no hace falta aplicarla.")]
		public void AplicarRegla_ExplicaRegla() {
			// Arrange
			var reglaCero = IRegla.GenerarReglaPorTipo(0, 10);
			BigInteger dividendo = 100;

			// Act
			var result = reglaCero.AplicarRegla(
				dividendo);

			// Assert
			Assert.That(result, Is.EqualTo(Operaciones.Recursos.TextoCalculos.ReglaExplicadaCero));
		}

		[Test(Description = "ReglaCero explica que no hace falta aplicarla.")]
		public void ToString_ExplicaRegla() {
			// Arrange
			var reglaCero = IRegla.GenerarReglaPorTipo(0, 10);

			// Act
			var result = reglaCero.ToString();

			// Assert
			Assert.That(result, Is.EqualTo(Operaciones.Recursos.TextoCalculos.ReglaExplicadaCero));
		}
	}
}
