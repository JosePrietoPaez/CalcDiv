using Listas;
using NUnit.Framework;
using Operaciones;
using System;

namespace TestCalculadora {

	[TestFixture]
	public class CalculosEstaticoTests {

		[TestCase(-1,TestName = "EsPrimo devuelve false si se le pasa un número negativo")]
		[TestCase(0,TestName = "EsPrimo devuelve false si se le pasa cero")]
		public void EsPrimo_ArgumentoNoPositivo_DevuelveFalse(long argumento) {
			// Arrange
			long numero = argumento;

			// Act
			var result = CalculosEstatico.EsPrimo(
				numero);

			// Assert
			Assert.That(result,Is.False);
		}

		[Test(Description = "EsPrimo devuelve false si se le pasa uno")] //No aparece en el explorador de pruebas
		public void EsPrimo_Uno_DevuelveFalse() {
			// Arrange
			long numero = 1;

			// Act
			var result = CalculosEstatico.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "EsPrimo devuelve false si se le pasa un número compuesto")]
		public void EsPrimo_ArgumentoCompuesto_DevuelveFalse() {
			// Arrange
			long numero = 12;

			// Act
			var result = CalculosEstatico.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "EsPrimo devuelve true si se le pasa un número compuesto")]
		public void EsPrimo_ArgumentoPrimo_DevuelveTrue() {
			// Arrange
			long numero = 7;

			// Act
			var result = CalculosEstatico.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void Mcd_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long raiz = 0;
			long segundo = 0;

			// Act
			var result = CalculosEstatico.Mcd(
				raiz,
				segundo);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void DescompsicionEnPrimos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long num = 0;

			// Act
			var result = CalculosEstatico.DescompsicionEnPrimos(
				num);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void PrimosHasta_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long num = 0;

			// Act
			var result = CalculosEstatico.PrimosHasta(
				num);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void InversoMod_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long a = 0;
			long m = 0;

			// Act
			var result = CalculosEstatico.InversoMod(
				a,
				m);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void MinAbs_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long un = 0;
			long dos = 0;

			// Act
			var result = CalculosEstatico.MinAbs(
				un,
				dos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Cifras_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long num = 0;
			long raiz = 0;

			// Act
			var result = CalculosEstatico.Cifras(
				num,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Cifra_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long num = 0;
			long pos = 0;
			long raiz = 0;

			// Act
			var result = CalculosEstatico.Cifra(
				num,
				pos,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void NumASubindice_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long num = 0;

			// Act
			var result = CalculosEstatico.NumASubindice(
				num);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void CifraASubindice_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long num = 0;

			// Act
			var result = CalculosEstatico.CifraASubindice(
				num);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ProductoMod_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long fac1 = 0;
			long fac2 = 0;
			long raiz = 0;

			// Act
			var result = CalculosEstatico.ProductoMod(
				fac1,
				fac2,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ReglasDivisibilidad_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			ISerie<ISerie<long>> reglas = new ListSerie<ISerie<long>>();
			long num = 0;
			int cantidad = 0;
			long raiz = 0;

			// Act
			CalculosEstatico.ReglasDivisibilidad(
				reglas,
				num,
				cantidad,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ReglaDivisibilidadOptima_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			IListaDinamica<long> serie = new ListSerie<long>();
			long num = 0;
			int cantidad = 0;
			long raiz = 0;

			// Act
			CalculosEstatico.ReglaDivisibilidadOptima(
				serie,
				num,
				cantidad,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ReglaDivisibilidadBase_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			IListaDinamica<long> serie = new ListSerie<long>();
			long num = 0;
			int cantidad = 0;
			long raiz = 0;

			// Act
			CalculosEstatico.ReglaDivisibilidadBase(
				serie,
				num,
				cantidad,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void PotenciaEntera_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long @base = 0;
			long exp = 0;

			// Act
			var result = CalculosEstatico.PotenciaEntera(
				@base,
				exp);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ReglaDivisibilidadExtendida_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long divisor = 0;
			long raiz = 0;

			// Act
			var result = CalculosEstatico.ReglaDivisibilidadExtendida(
				divisor,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void CasoEspecialRegla_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			long divisor = 0;
			long raiz = 0;

			// Act
			var result = CalculosEstatico.CasoEspecialRegla(
				divisor,
				raiz);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ToStringCompleto_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			ISerie<int> lista = null;

			// Act
			var result = CalculosEstatico.ToStringCompleto(
				lista);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ToStringCompletoInverso_StateUnderTest_ExpectedBehavior() {
			// Arrange
			
			ISerie<int> lista = null;

			// Act
			var result = CalculosEstatico.ToStringCompletoInverso(
				lista);

			// Assert
			Assert.Fail();
		}
	}
}
