using Listas;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Operaciones;
using System;
using System.Text.RegularExpressions;

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

		[Test(Description = "Mcd de dos números coprimos devuelve 1")]
		public void Mcd_Coprimos_DevuelveProducto() {
			// Arrange
			
			long raiz = 3;
			long segundo = 16;

			// Act
			var result = CalculosEstatico.Mcd(
				raiz,
				segundo);

			// Assert
			Assert.That(result, Is.EqualTo(1));
		}

		[Test(Description = "Mcd devuelve el comun divisor de dos números primos multiplicados por otro")]
		public void Mcd_NoCoprimos_NoDevuelveProducto() {
			// Arrange
			
			long raiz = 2*3;
			long segundo = 5*3;

			// Act
			var result = CalculosEstatico.Mcd(
				raiz,
				segundo);

			// Assert
			Assert.That(result, Is.EqualTo(3));
		}

		[Test(Description = "Mcd de una raiz y su potencia devuelve la raiz")]
		public void Mcd_RaizYPotencia_DevuelvePotencia() {
			// Arrange
			
			long raiz = 2;
			long potencia = 8;

			// Act
			var result = CalculosEstatico.Mcd(
				raiz,
				potencia);

			// Assert
			Assert.That(result, Is.EqualTo(raiz));
		}

		[Test(Description = "Mcd de cero y otro número devuelve el otro número")]
		public void Mcd_CeroYOtro_DevuelveOtro() {
			// Arrange
			
			long raiz = 0;
			long segundo = 4;

			// Act
			var result = CalculosEstatico.Mcd(
				raiz,
				segundo);

			// Assert
			Assert.That(result, Is.EqualTo(segundo));
		}

		[Test(Description = "Mcd de números negativos lanza una excepción")]
		public void Mcd_NumeroNegativo_LanzaExcepcion() {
			// Arrange
			
			long raiz = -1;
			long segundo = -2;

			// Assert
			Assert.Throws<ArgumentException>( () =>
				// Act
				CalculosEstatico.Mcd(
					raiz,
					segundo
				)
			);
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

		[TestFixture]
		public class ReglasDivisibilidadTests {

			[Test(Description = "ReglasDivisibilidad lanza excepción si num es 0")]
			public void ReglasDivisibilidad_NumNoPositivo_LanzaExcepcion() {
				// Arrange

				ISerie<ISerie<long>> serie = new ListSerie<ISerie<long>>();
				long num = 0;
				int cantidad = 3;
				long raiz = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglasDivisibilidad(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglasDivisibilidad lanza excepción si num es 0")]
			public void ReglasDivisibilidad_CantidadNoPositiva_LanzaExcepcion() {
				// Arrange

				ISerie<ISerie<long>> serie = new ListSerie<ISerie<long>>();
				long num = 0;
				int cantidad = 3;
				long raiz = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglasDivisibilidad(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglasDivisibilidad lanza excepción si num es 0")]
			public void ReglasDivisibilidad_RaizMenorQue2_LanzaExcepcion() {
				// Arrange

				ISerie<ISerie<long>> serie = new ListSerie<ISerie<long>>();
				long num = 7;
				int cantidad = 3;
				long raiz = 1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglasDivisibilidad(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglasDivisibilidad devuelve una serie de series, cada serie contiene una regla distinta, con elementos equivalentes a los de ReglaDivisibilidadBase")]
			public void ReglasDivisibilidad_ArgumentosCorrectos_DevuelveListaDeReglas() {
				// Arrange

				ISerie<ISerie<long>> serie = new ListSerie<ISerie<long>>();
				ISerie<long> serieOptima = new ListSerie<long>();
				long num = 7;
				int cantidad = 3;
				long raiz = 10;
				CalculosEstatico.ReglaDivisibilidadOptima(
					serieOptima,
					num,
					cantidad,
					raiz);

				// Act
				CalculosEstatico.ReglasDivisibilidad(
					serie,
					num,
					cantidad,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(serie.Longitud, Is.EqualTo(CalculosEstatico.PotenciaEntera(2, cantidad)));
					Assert.That(serie.Select(lista => lista.Select(elemento => CalculosEstatico.MinAbs(elemento, elemento - num))) //Calcula la regla óptima de todas las reglas
						, Has.All.EquivalentTo(serieOptima)); //Todas deben ser la misma
				});
			}
		}

		[TestFixture]
		public class ReglaDivisibilidadOptimaTests {

			[Test(Description = "ReglaDivisibilidadOptima lanza excepción si num es 0")]
			public void ReglaDivisibilidadOptima_NumNoPositivo_LanzaExcepcion() {
				// Arrange

				IListaDinamica<long> serie = new ListSerie<long>();
				long num = 0;
				int cantidad = 3;
				long raiz = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglaDivisibilidadOptima(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglaDivisibilidadOptima lanza excepción si num es 0")]
			public void ReglaDivisibilidadOptima_CantidadNoPositiva_LanzaExcepcion() {
				// Arrange

				IListaDinamica<long> serie = new ListSerie<long>();
				long num = 0;
				int cantidad = 3;
				long raiz = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglaDivisibilidadOptima(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglaDivisibilidadOptima lanza excepción si num es 0")]
			public void ReglaDivisibilidadOptima_RaizMenorQue2_LanzaExcepcion() {
				// Arrange

				IListaDinamica<long> serie = new ListSerie<long>();
				long num = 7;
				int cantidad = 3;
				long raiz = 1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglaDivisibilidadOptima(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglaDivisibilidadOptima devuelve una lista con los mínimos absolutos entre los elementos de ReglaDivisibilidadBase y esos menos el divisor")]
			public void ReglaDivisibilidadOptima_ArgumentosCorrectos_LanzaExcepcion() {
				// Arrange

				IListaDinamica<long> serie = new ListSerie<long>(),
					serieAuxiliar = new ListSerie<long>();
				long num = 7;
				int cantidad = 3;
				long raiz = 10;
				CalculosEstatico.ReglaDivisibilidadBase(serieAuxiliar,num, cantidad, raiz); //Literalmente se calcula así
				IEnumerable<long> listaOptima = serieAuxiliar.Select(elemento => CalculosEstatico.MinAbs(elemento, elemento - num)); //Select es el equivalente a map de haskell

				// Act
				CalculosEstatico.ReglaDivisibilidadOptima(
					serie,
					num,
					cantidad,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(serie.Longitud, Is.EqualTo(cantidad));
					Assert.That(serie, Is.EquivalentTo(listaOptima));
				});

			}

		}

		[TestFixture]
		public class ReglaDivisibilidadBaseTests {

			[Test(Description ="ReglaDivisibilidadBase lanza excepción si num es 0")]
			public void ReglaDivisibilidadBase_NumNoPositivo_LanzaExcepcion() {
				// Arrange
			
				IListaDinamica<long> serie = new ListSerie<long>();
				long num = 0;
				int cantidad = 3;
				long raiz = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglaDivisibilidadBase(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglaDivisibilidadBase lanza excepción si num es 0")]
			public void ReglaDivisibilidadBase_CantidadNoPositiva_LanzaExcepcion() {
				// Arrange

				IListaDinamica<long> serie = new ListSerie<long>();
				long num = 0;
				int cantidad = 3;
				long raiz = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglaDivisibilidadBase(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglaDivisibilidadBase lanza excepción si num es 0")]
			public void ReglaDivisibilidadBase_RaizMenorQue2_LanzaExcepcion() {
				// Arrange

				IListaDinamica<long> serie = new ListSerie<long>();
				long num = 7;
				int cantidad = 3;
				long raiz = 1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() =>
				CalculosEstatico.ReglaDivisibilidadBase(
					serie,
					num,
					cantidad,
					raiz));
			}

			[Test(Description = "ReglaDivisibilidadBase lanza excepción si num es 0")]
			public void ReglaDivisibilidadBase_ArgumentosCorrectos_DevuelveRegla() {
				// Arrange

				long num = 13;
				int cantidad = 5;
				long raiz = 10;
				IListaDinamica<long> serie = new ListSerie<long>(),
					serieAssert = new ListSerie<long>();
				OperacionesSeries.PotenciaModProgresiva(serieAssert,CalculosEstatico.InversoMod(raiz, num), num, cantidad, 0);

				// Act
				CalculosEstatico.ReglaDivisibilidadBase(
					serie,
					num,
					cantidad,
					raiz);

				//Assert
				Assert.Multiple(() => {
					Assert.That(serie.Longitud, Is.EqualTo(cantidad));
					Assert.That(serie, Is.EqualTo(serieAssert));
				});
			}
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

		[TestFixture]
		public class ReglaDivisibilidadExtendidaTests {

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que el divisor no puede ser cero y true")]
			public void ReglaDivisibilidadExtendida_DivisorCero_MensajeCero() {
				// Arrange

				long divisor = 0;
				long raiz = 3;

				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.True);
					Assert.That(result.Item2, Is.EqualTo("No se le puede aplicar la relación de divisibilidad a cero."));
				});
			}

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que uno divide a todos los enteros y true")]
			public void ReglaDivisibilidadExtendida_DivisorUno_MensajeUno() {
				// Arrange

				long divisor = 1;
				long raiz = 3;

				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.True);
					Assert.That(result.Item2, Is.EqualTo("Todos los enteros son divisibles entre uno."));
				});
			}

			[Test(Description = "ReglaDivisibiladExtendida devuelve un mensaje indicando que se deben sumar bloques de cifras del número si el divisor es múltiplo de la base o una potencia menos uno y true")]
			[TestCase()]
			[TestCase(13, 3, 3)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMenos1PotenciaMayorQue1_MensajeSumarBloques(long divisor = -1, long raiz = -1, int potencia = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 7;
					raiz = 2;
					potencia = 3;
				}
				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.True);
					Assert.That(result.Item2, Is.EqualTo($"{divisor} es divisor de {raiz} elevado a {potencia} menos uno ({CalculosEstatico.PotenciaEntera(raiz,potencia)-1})" +
						$".\nUn número en base {raiz} será múltiplo de {divisor} si al separar sus cifras en grupos de {potencia} desde las unidades, la suma de los grupos es múltiplo de {divisor}."));
				});
			}

			[Test(Description = "ReglaDivisibiladExtendida devuelve un mensaje indicando que se deben sumar las cifras del número si el divisor es múltiplo de la base menos uno y true")]
			[TestCase()]
			[TestCase(3, 10)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMenos1Potencia1_MensajeSumarBloques(long divisor = -1, long raiz = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 5;
					raiz = 16;
				}
				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.True);
					Assert.That(result.Item2, Is.EqualTo($"{divisor} es divisor de {raiz} menos uno." +
						$"\nUn número en base {raiz} será múltiplo de {divisor} si la suma de sus cifras es múltiplo de {divisor}."));
				});
			}


			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que se deben restar bloques de cifras del número si el divisor es múltiplo de la base o una potencia más uno y true")]
			[TestCase()]
			[TestCase(257, 16, 2)]
			[TestCase(9, 2, 3)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMas1PotenciaMayorQue1_CasoRestarBloques(long divisor = -1, long raiz = -1, int potencia = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 101;
					raiz = 10;
					potencia = 2;
				}
				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.True);
					Assert.That(result.Item2, Is.EqualTo($"{divisor} es divisor de {raiz} elevado a {potencia} más uno ({CalculosEstatico.PotenciaEntera(raiz, potencia) + 1})" +
						$".\nUn número en base {raiz} será múltiplo de {divisor} si al separar sus cifras en grupos de {potencia} desde las unidades, la diferencia de la suma de los grupos pares y la de los grupos impares es múltiplo de {divisor}."));
				});
			}

			[Test(Description = "ReglaDivisibiladExtendida devuelve un mensaje indicando que se deben restar la suma de las cifras pares con la suma de las impares del número si el divisor es múltiplo de la base más uno y true")]
			[TestCase()]
			[TestCase(17, 16)]
			[TestCase(3, 2)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMas1Potencia1_CasoRestarBloques(long divisor = -1, long raiz = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 11;
					raiz = 10;
				}
				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.True);
					Assert.That(result.Item2, Is.EqualTo($"{divisor} es divisor de {raiz} más uno." +
						$"\nUn número en base {raiz} será múltiplo de {divisor} si la diferencia de la suma de las cifras pares con la de las impares es múltiplo de {divisor}."));
				});
			}

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que solo las primeras cifras importan para la divisibilidad si el divisor está compuesto de los factores primos de raiz y true")]
			[TestCase()]
			[TestCase(32, 16, 2)]
			[TestCase(16, 2, 4)]
			public void ReglaDivisibilidadExtendida_DivisorCompuestoDePotenciasDeRaiz_CasoMirarCifras(long divisor = -1, long raiz = -1, int cifras = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 5;
					raiz = 10;
					cifras = 1;
				}

				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.EqualTo(true));
					Assert.That(result.Item2, Is.EqualTo($"{divisor} está compuesto de potencias de los factores primos de {raiz}" +
						$".\nUn número en base {raiz} será múltiplo de {divisor} si sus primeras {cifras} cifras son múltiplo de {divisor}."));
				});
			}

			[Test(Description = "ReglaDivisibilidadExtendida lanza excepción si la base es menor que 2")]
			public void ReglaDivisibilidadExtendida_BaseMenorQue2_LanzaExcepcion() {
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz));

			}

			[Test(Description = "ReglaDivisibilidadExtendida lanza excepción si el divisor es menor que 2")]
			public void ReglaDivisibilidadExtendida_DivisorNegativo_LanzaExcepcion() {
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz));

			}

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que no se ha encontrado una regla alternativa si no puede encontrar y false")]
			public void ReglaDivisibilidadExtendida_NoEncuentra_CasoNormal() {
				// Arrange
				long divisor = 789293; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = 31233;

				// Act
				var result = CalculosEstatico.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.Item1, Is.EqualTo(false));
					Assert.That(result.Item2, Is.EqualTo("No se ha encontrado ninguna regla alternativa, aplique la regla calculada, si se puede calcular."));
				});
			}
		}
		
		[TestFixture]
		public class CasoEspecialReglaTests {

			[Test(Description = "CasoEspecialRegla devuelve el caso CERO e información -1 si el divisor es 0")]
			public void CasoEspecialRegla_DivisorCero_CasoEsCeroInformacionMenos1() {
				// Arrange
			
				long divisor = 0;
				long raiz = 3;

				// Act
				var result = CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz);
			
				// Assert
				Assert.Multiple(() => {
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.CERO));
					Assert.That(result.informacion, Is.EqualTo(-1));
				});
			}

			[Test(Description = "CasoEspecialRegla devuelve el caso UNO e información -1 si el divisor es 1")]
			public void CasoEspecialRegla_DivisorUno_CasoEsUnoInformacionMenos1() {
				// Arrange

				long divisor = 1;
				long raiz = 3;

				// Act
				var result = CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.UNO));
					Assert.That(result.informacion, Is.EqualTo(-1));
				});
			}

			[Test(Description = "CasoEspecialRegla devuelve el caso SUMAR_BLOQUES si el divisor es múltiplo de la base o una potencia menos uno y devuelve el tamaño de los bloques")]
			[TestCase()]
			[TestCase(5,16,1)]
			[TestCase(7,2,3)]
			public void CasoEspecialRegla_DivisorMultiploRaizMenos1_CasoSumarBloques(long divisor = -1, long raiz = -1, int potencia = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 9;
					raiz = 10;
					potencia = 1;
				}
				// Act
				var result = CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.SUMAR_BLOQUES));
					Assert.That(result.informacion, Is.EqualTo(potencia));
				});
			}


			[Test(Description = "CasoEspecialRegla devuelve el caso RESTAR_BLOQUES si el divisor es múltiplo de la base o una potencia más uno y devuelve el tamaño de los bloques")]
			[TestCase()]
			[TestCase(17, 16, 1)]
			[TestCase(9, 2, 3)]
			public void CasoEspecialRegla_DivisorMultiploRaizMas1_CasoRestarBloques(long divisor = -1, long raiz = -1, int potencia = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 11;
					raiz = 10;
					potencia = 1;
				}

				// Act
				var result = CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.RESTAR_BLOQUES));
					Assert.That(result.informacion, Is.EqualTo(potencia));
				});
			}

			[Test(Description = "CasoEspecialRegla devuelve el caso MIRAR_CIFRAS si el divisor está compuesto de los divisores de la base y devuelve las cifras que se deben considerar")]
			[TestCase()]
			[TestCase(32, 16, 2)]
			[TestCase(16, 2, 4)]
			public void CasoEspecialRegla_DivisorCompuestoDePotenciasDeRaiz_CasoMirarCifras(long divisor = -1, long raiz = -1, int cifras = -1) {
				// Arrange - para que haya un arrange
				if (divisor == -1) {
					divisor = 5;
					raiz = 10;
					cifras = 1;
				}

				// Act
				var result = CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.MIRAR_CIFRAS));
					Assert.That(result.informacion, Is.EqualTo(cifras));
				});
			}

			[Test(Description = "CasoEspecialRegla lanza excepción si la base es menor que 2")]
			public void CasoEspecialRegla_BaseMenorQue2_LanzaExcepcion() {
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz));
			
			}

			[Test(Description = "CasoEspecialRegla lanza excepción si el divisor es menor que 2")]
			public void CasoEspecialRegla_DivisorNegativo_LanzaExcepcion() {
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz));

			}

			[Test(Description = "CasoEspecialRegla devuelve el caso USAR_NORMAL e información -1 si ninguno de los casos especiales se aplica")]
			public void CasoEspecialRegla_NoEncuentra_CasoNormal() {
				// Arrange
				long divisor = 789293; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = 31233;

				// Act
				var result = CalculosEstatico.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() => {
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.USAR_NORMAL));
					Assert.That(result.informacion, Is.EqualTo(-1));
				});
			}
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
