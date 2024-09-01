using NUnit.Framework;
using Operaciones;
using System;
using System.Numerics;

namespace TestReglas {
	[TestFixture]
	public class ReglaUnoTests {

		[Test(Description = "ReglaUno tiene divisor uno al generarse.")]
		public void Generar_DevuelveReglaConDivisorUnoYBaseCorrecta() {
			// Arrange
			long @base = 10, divisor = 1;

			// Act
			var reglaUno = IRegla.GenerarReglaPorTipo(divisor, @base);

			Assert.Multiple(() => {
				// Assert
				Assert.That(reglaUno.Base, Is.EqualTo(10));
				Assert.That(reglaUno.Divisor, Is.EqualTo(1));
				Assert.That(reglaUno.Tipo, Is.EqualTo(CasosDivisibilidad.DIVISOR_ONE));
			});
		}

		[Test(Description = "ReglaUno explica que no hace falta aplicarla.")]
		public void AplicarRegla_ExplicaRegla() {
			// Arrange
			var reglaUno = IRegla.GenerarReglaPorTipo(1, 10);
			BigInteger dividendo = 100;

			// Act
			var result = reglaUno.AplicarRegla(
				dividendo);

			// Assert
			Assert.That(result, Is.EqualTo(Operaciones.Recursos.TextoCalculos.ReglaExplicadaUno));
		}

		[Test(Description = "ReglaUno explica que no hace falta aplicarla.")]
		public void ToString_ExplicaRegla() {
			// Arrange
			var reglaUno = IRegla.GenerarReglaPorTipo(1, 10);

			// Act
			var result = reglaUno.ToString();

			// Assert
			Assert.That(result, Is.EqualTo(Operaciones.Recursos.TextoCalculos.ReglaExplicadaUno));
		}
	}
}
