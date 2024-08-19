using Operaciones;

namespace TestCalculadora
{

	[TestFixture]
	public class CalculosTests
	{

		[TestCase(-1, TestName = "EsPrimo devuelve false si se le pasa un número negativo")]
		[TestCase(0, TestName = "EsPrimo devuelve false si se le pasa cero")]
		public void EsPrimo_ArgumentoNoPositivo_DevuelveFalse(long argumento)
		{
			// Arrange
			long numero = argumento;

			// Act
			var result = Calculos.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "EsPrimo devuelve false si se le pasa uno")] //No aparece en el explorador de pruebas
		public void EsPrimo_Uno_DevuelveFalse()
		{
			// Arrange
			long numero = 1;

			// Act
			var result = Calculos.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "EsPrimo devuelve false si se le pasa un número compuesto")]
		public void EsPrimo_ArgumentoCompuesto_DevuelveFalse()
		{
			// Arrange
			long numero = 12;

			// Act
			var result = Calculos.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test(Description = "EsPrimo devuelve true si se le pasa un número compuesto")]
		public void EsPrimo_ArgumentoPrimo_DevuelveTrue()
		{
			// Arrange
			long numero = 7;

			// Act
			var result = Calculos.EsPrimo(
				numero);

			// Assert
			Assert.That(result, Is.True);
		}

		[TestFixture]
		public class McdTest
		{
			[Test(Description = "Mcd de dos números coprimos devuelve 1")]
			public void Mcd_Coprimos_DevuelveProducto()
			{
				// Arrange

				long raiz = 3;
				long segundo = 16;

				// Act
				var result = Calculos.Mcd(
					raiz,
					segundo);

				// Assert
				Assert.That(result, Is.EqualTo(1));
			}

			[Test(Description = "Mcd devuelve el comun divisor de dos números primos multiplicados por otro")]
			public void Mcd_NoCoprimos_NoDevuelveProducto()
			{
				// Arrange

				long raiz = 2 * 3;
				long segundo = 5 * 3;

				// Act
				var result = Calculos.Mcd(
					raiz,
					segundo);

				// Assert
				Assert.That(result, Is.EqualTo(3));
			}

			[Test(Description = "Mcd de una raiz y su potencia devuelve la raiz")]
			public void Mcd_RaizYPotencia_DevuelvePotencia()
			{
				// Arrange

				long raiz = 2;
				long potencia = 8;

				// Act
				var result = Calculos.Mcd(
					raiz,
					potencia);

				// Assert
				Assert.That(result, Is.EqualTo(raiz));
			}

			[Test(Description = "Mcd de cero y otro número devuelve el otro número")]
			public void Mcd_CeroYOtro_DevuelveOtro()
			{
				// Arrange

				long raiz = 0;
				long segundo = 4;

				// Act
				var result = Calculos.Mcd(
					raiz,
					segundo);

				// Assert
				Assert.That(result, Is.EqualTo(segundo));
			}

			[Test(Description = "Mcd de números negativos lanza una excepción")]
			public void Mcd_NumeroNegativo_LanzaExcepcion()
			{
				// Arrange

				long raiz = -1;
				long segundo = -2;

				// Assert
				Assert.Throws<ArgumentException>(() =>
					// Act
					Calculos.Mcd(
						raiz,
						segundo
					)
				);
			}
		}

		[TestFixture]
		public class SonCoprimosTest {

			[TestCase(10,-3, TestName = "SonCoprimos lanza excepción si segundo es negativo")]
			[TestCase(-10,-3, TestName = "SonCoprimos lanza excepción si primero y segundo son negativos")]
			[TestCase(-10,3, TestName = "SonCoprimos lanza excepción si primero es negativo")]
			public void SonCoprimos_ArgumentosNegativos_LanzaExcepcion(long primero, long segundo) {
				long argumento1 = primero
					,argumento2 = segundo;

				Assert.Throws<ArgumentException>(() => Calculos.SonCoprimos(argumento1, argumento2));
			}

			[Test(Description = "SonCoprimos devuelve false si los argumentos no lo son")]
			public void SonCoprimos_ArgumentosNoCoprimos_DevuelveFalse() {
				long primero = 15, segundo = 35;

				bool resultado = Calculos.SonCoprimos(primero, segundo);

				Assert.That(resultado, Is.False);
			}

			[Test(Description = "SonCoprimos devuelve false si los argumentos no lo son")]
			public void SonCoprimos_ArgumentosCoprimos_DevuelveTrue() {
				long primero = 101, segundo = 4023;

				bool resultado = Calculos.SonCoprimos(primero, segundo);

				Assert.That(resultado, Is.True);
			}
		}

		[TestFixture]
		public class DescompsicionEnPrimosTest
		{
			[Test(Description = "DescomposicionEnPrimos de un número negativo lanza una excepción")]
			public void DescompsicionEnPrimos_NumeroNegativo_LanzaExcepcion()
			{
				// Arrange

				long num = -1;

				// Assert
				Assert.Throws<ArgumentException>(() =>
					// Act
					Calculos.DescompsicionEnPrimos(num)
				);
			}

			[Test(Description = "DescomposicionEnPrimos de un primo devuelve la lista correcta")]
			public void DescompsicionEnPrimos_NumeroPrimo_DevuelveListaDePotencias()
			{
				// Arrange

				long num = 3;

				// Act
				var result = Calculos.DescompsicionEnPrimos(
					num);

				// Assert
				List<long> primos = [0, 1];
				Assert.That(result, Is.EqualTo(primos));
			}

			[Test(Description = "DescomposicionEnPrimos de un primo devuelve la lista correcta")]
			public void DescompsicionEnPrimos_NumeroPrimoCaso2_DevuelveListaDePotencias() {
				// Arrange

				long num = 700;

				// Act
				var result = Calculos.DescompsicionEnPrimos(
					num);

				// Assert
				List<long> primos = [2, 0, 2, 1];
				Assert.That(result, Is.EqualTo(primos));
			}

			[Test(Description = "DescomposicionEnPrimos de un número compuesto devuelve la lista correcta")]
			public void DescompsicionEnPrimos_NumeroCompuesto_DevuelveListaDePotencias()
			{
				// Arrange
				long num = 18;

				// Act
				var result = Calculos.DescompsicionEnPrimos(
					num);

				// Assert
				// Calculamos el resultado
				List<long> primos = Calculos.PrimosHasta(num);
				long res = 1;
				var eprimos = primos.GetEnumerator();
				var eresult = result.GetEnumerator();
				while (eprimos.MoveNext() && eresult.MoveNext())
				{
					res *= (long)Math.Pow(eprimos.Current, eresult.Current);
				}
				Assert.That(res, Is.EqualTo(num));
			}
		}

		[Test(Description = "La lista que devuelve el metodo debe contener todos los numeros primos incluido el numero propio")]
		public void PrimosHasta_ValoresSimples_DevuelveListaDePrimos()
		{
			long num = 18;
			// Act
			List<long> primos = Calculos.PrimosHasta(num);
			long[] numerosPrim = [2, 3, 5, 7, 11, 13, 17, 18];
			
			int contador = 0;
			foreach(long value in primos){
				Assert.That(value, Is.EqualTo(numerosPrim[contador]));
				contador++;
			}

		}

		[TestFixture]
		public class InversoModTest
		{
			[Test(Description = "InversoMod de 0 lanza una excepción")]
			public void InversoMod_Cero_LanzaExcepcion()
			{
				// Arrange

				long a = 0;
				long m = 1;

				// Assert
				Assert.Throws<DivideByZeroException>(() =>
					// Act
					Calculos.InversoMod(a, m)
				);
			}

			[Test(Description = "InversoMod de un número con inverso lo devuelve")]
			public void InversoMod_ConInverso_DevuelveInverso()
			{
				// Arrange

				long a = 3;
				long m = 4;

				// Act
				var result = Calculos.InversoMod(
					a,
					m);

				// Assert
				Assert.That(result, Is.EqualTo(3));
			}

			[Test(Description = "InversoMod de un número sin inverso devuelve cero")]
			public void InversoMod_SinInverso_DevuelveCero()
			{
				// Arrange

				long a = 2;
				long m = 4;

				// Act
				var result = Calculos.InversoMod(
					a,
					m);

				// Assert
				Assert.That(result, Is.EqualTo(0));
			}
		}

		[TestFixture]
		public class MinAbsTest {

			[Test(Description = "Devuelve aquel numero que su absoluto sea menor si este es positivo")]
			public void MinAbs_MenorPositivo_DevuelveElMenor()
			{
				// Arrange
				long un = -11;
				long dos = 2;

				// Act
				var result = Calculos.MinAbs(
					un,
					dos);

				Assert.That(result,Is.EqualTo(dos));

			}

			[Test(Description = "Devuelve aquel numero que su absoluto sea menor si este es negativo")]
			public void MinAbs_MenorNegativo_DevuelveElMenor() {
				// Arrange
				long un = -11;
				long dos = 12;

				// Act
				var result = Calculos.MinAbs(
					un,
					dos);

				Assert.That(result, Is.EqualTo(un));

			}

		}

		[TestFixture]
		public class CifrasTest {
			[TestCase(10, TestName = "Cifras en base 10 devuelve el entero superior o igual al logaritmo en base 10")]
			[TestCase(2, TestName = "Cifras en base 2 devuelve el entero superior o igual al logaritmo en base 2")]
			[TestCase(3, TestName = "Cifras en base 10 devuelve el entero superior o igual al logaritmo en base 3")]
			public void Cifras_NumeroBasePositivos_DevuelveLogaritmo(long raiz)
			{
				// Arrange

				long num = 40;

				// Act
				var result = Calculos.Cifras(
					num,
					raiz);

				// Assert
				// Entero superior o igual al logaritmo en base de raiz
				long expected = (long) Math.Ceiling(Math.Log(num) / Math.Log(raiz));
				Assert.That(result, Is.EqualTo(expected));
			}

			[Test]
			public void Cifras_NumeroCero_DevuelveUno() {
				// Arrange

				long num = 0;

				// Act
				var result = Calculos.Cifras(
					num,
					10);

				// Assert
				Assert.That(result, Is.EqualTo(1));
			}
		}
		[TestFixture]
		public class CifraTest {
			[Test(Description = "Cifra en base 10 devuelve la cifra correcta")]
			public void Cifra_Base10_DevuelveCifraCorrecta()
			{
				// Arrange

				long num = 543210;
				byte pos = 3;
				long raiz = 10;

				// Act
				var result = Calculos.Cifra(
					num,
					pos,
					raiz);

				// Assert
				Assert.That(result, Is.EqualTo(pos));
			}

			[TestCase(2, TestName = "Cifra en base 2 devuelve la cifra correcta")]
			[TestCase(3, TestName = "Cifra en base 3 devuelve la cifra correcta")]
			public void Cifra_OtraBase_DevuelveCifraCorrecta(long raiz)
			{
				// Arrange

				long num = 18;

				// Act
				var ncifras = Calculos.Cifras(num, raiz);
				long[] cifras = new long[ncifras];
				for(byte i = 0; i < ncifras; i++) {
					cifras[i] = Calculos.Cifra(num, i, raiz);
				}

				// Calculo de numero a partir de cifras en base 10
				long pot = 1;
				long res = 0;
				for (int i = 0; i < ncifras; i++) {
					res += cifras[i] * pot;
					pot *= raiz;
				}

				// Assert
				Assert.Multiple(() => {
					Assert.That(cifras[ncifras - 1], Is.GreaterThan(0));
					Assert.That(res, Is.EqualTo(num));
				});
			}

			[TestCase(2, 5, TestName = "Cifra en base 2 devuelve 0 si el índice es demasiado grande")]
			[TestCase(10, 2, TestName = "Cifra en base 10 devuelve 0 si el índice es demasiado grande")]
			[TestCase(16, 3, TestName = "Cifra en base 16 devuelve 0 si el índice es demasiado grande")]
			public void Cifra_OtraBasePosicionExcesiva_DevuelveCero(long raiz, byte pos) {
				// Arrange
				long numero = 18;

				// Act
				var resultado = Calculos.Cifra(numero, pos, raiz);

				// Assert
				Assert.That(resultado, Is.Zero);
			}
		}

		[TestFixture]
		public class IntervaloCifrasTest {

			[Test]
			public void IntervaloCifras_InicioMayor_LanzaExcepcion() {
				// Arrange
				byte inicio = 1, fin = 0;
				long numero = 100, @base = 10;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(() => Calculos.IntervaloCifras(numero, @base, inicio, fin));
			}

			[TestCase(0)]
			[TestCase(1)]
			[TestCase(5)]
			public void IntervaloCifras_UnaCifra_DevuelveLaCifraIndicada(byte posicion) {
				// Arrange
				long @base = 16, numero = 0xFEDA0124;
				byte final = (byte)(posicion + 1);
				long esperado = Calculos.Cifra(numero, posicion, @base);

				// Act
				var resultado = Calculos.IntervaloCifras(numero, @base, posicion, final);

				// Assert
				Assert.That(resultado, Is.EqualTo(esperado));
			}

			[TestCase(0, 3)]
			[TestCase(1, 3)]
			[TestCase(5, 2)]
			public void IntervaloCifras_VariasCifras_DevuelveLasCifrasEnElIntervalo(byte posicion, byte longitud) {
				// Arrange
				long @base = 15, numero = 9876543210;
				byte final = (byte)(posicion + longitud);
				long esperado = 0;
				while (longitud > 0) {
					esperado += Calculos.Cifra(numero, (byte)(posicion + longitud - 1), @base) * Calculos.PotenciaEntera(@base, longitud - 1);
					longitud--;
				}

				// Act
				var resultado = Calculos.IntervaloCifras(numero, @base, posicion, final);

				// Assert
				Assert.That(resultado, Is.EqualTo(esperado));
			}

			[Test]
			public void IntervaloCifras_IntervaloFueraDeNumero_DevuelveCero() {
				// Arrange
				long @base = 10, numero = 1234;
				byte posicionInicial = 4, posicionFinal = 10;

				// Act
				var resultado = Calculos.IntervaloCifras(numero, @base, posicionInicial, posicionFinal);

				// Assert
				Assert.That(resultado, Is.Zero);
			}


		}

		[TestFixture]
		public class IntToStringNoAlphabetTest {

			[Test]
			public void IntoToStringNoAlphabet_ArgumentosCorrectos_DevuelveStringConCifrasYBase() {
				// Arrange
				long numero = 0xFEDCBA90
					, @base = 0x10;

				// Act
				var resultado = Calculos.LongToStringNoAlphabet(numero, @base);

				// Assert
				Assert.That(resultado, Is.EqualTo("[15,14,13,12,11,10,9,0](" + @base + ")"));
			}

		}

		[Test]
		public void NumASubindice_NumeroNoNegativo_DevuelveStringConElNumero() {
			// Arrange

			long num = 1234567890;
			string esperado = "₁₂₃₄₅₆₇₈₉₀";

			// Act
			var result = Calculos.NumASubindice(
				num);

			// Assert
			Assert.That(result,Is.EqualTo(esperado));
		}

		[Test]
		public void NumASubindice_NumeroNegativo_DevuelveStringConElNumero() {
			// Arrange

			long num = -1234567890;
			string esperado = "₋₁₂₃₄₅₆₇₈₉₀";

			// Act
			var result = Calculos.NumASubindice(
				num);

			// Assert
			Assert.That(result, Is.EqualTo(esperado));
		}

		[Test]
		public void CifraASubindice_PruebaDeTodoElDominio_DevuelveSubindicies() {
			// Arrange

			long[] num = [1,2,3,4,5,6,7,8,9,0];
			char[] subindices = ['₁', '₂', '₃', '₄', '₅', '₆', '₇', '₈', '₉','₀'];

			// Act
			var result = num.Select(Calculos.CifraASubindice).ToArray();

			// Assert
			Assert.That(result, Is.EqualTo(subindices));
		}

		[Test(Description ="Devuelve el valor en base raiz de fact1 * fact2")]
		public void ProductoMod_longValues_DevuelveElModulo_EnBase_raiz()
		{
			// Arrange


			long fac1 = 12;
			long fac2 = 18;
			long raiz = 7;

			// Act
			var result = Calculos.ProductoMod(
				fac1,
				fac2,
				raiz);

			Assert.That(result,Is.EqualTo(6));
		}

		[Test(Description = "ProductMod lanza excepción si raiz es 0")]
		public void ProductoMod_Cero_LanzaExcepcion()
		{
			// Arrange

			long fac1 = 8;
			long fac2 = 2;
			long raiz = 0;

			// Act

			// Act
			Assert.Throws<DivideByZeroException>(() =>
			Calculos.ProductoMod(
				fac1,
				fac2,
				raiz));
		}

		[Test(Description ="Devuelve de forma acertada el valor de la base según su exponente")]
		public void PotenciaEntera_LongValues_ValorDeLaPotencia()
		{
			// Arrange

			long @base = 4;
			long exp = 8;

			// Act
			var result = Calculos.PotenciaEntera(
				@base,
				exp);

			Assert.That(result, Is.EqualTo(65536));

		}

		[TestFixture]
		public class ReglasDivisibilidadTests {

			[Test(Description = "ReglasDivisibilidad lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglasDivisibilidad_ArgumentosNoValidos_LanzaExcepcion(
				[Values(1, -1)] long divisor, [Values(0, -1)] int longitud, [Values(1, -1)] long @base) {
				Assume.That(divisor <= 1 || longitud <= 0 || @base <= 1);

				Assert.Throws<ArgumentOutOfRangeException>(() => Calculos.ReglasDivisibilidad(divisor, longitud, @base));
			}

			[Test(Description = "ReglasDivisibilidad lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglasDivisibilidad_ArgumentosNoCoprimos_LanzaExcepcion(
				[Values(2, 4)] long divisor, [Values(6, 10)] long @base) {
				int longitud = 2;

				Assert.Throws<ArithmeticException>(() => Calculos.ReglasDivisibilidad(divisor, longitud, @base));
			}

			[Test(Description = "ReglasDivisibilidad lanza excepción")]
			public void ReglasDivisibilidad_ArgumentosCoprimos_DevuelveTodasLasReglasDeDivisibilidad(
				[Values(6, 15)] long divisor, [Values(13,77)] long @base) {
				int longitud = 5;
				long inverso = Calculos.InversoMod(@base, divisor);
				List<long> listaPotencias = [];
				for (int i = 0; i < longitud; i++) {
					listaPotencias.Add(Calculos.PotenciaEntera(inverso, i) % divisor);
				}

				List<ReglaCoeficientes> resultado = Calculos.ReglasDivisibilidad(divisor, longitud, @base);

				Assert.Multiple(() => {
					Assert.That(resultado, Has.Count.EqualTo(Calculos.PotenciaEntera(2, longitud)));
					Assert.That(resultado, Is.Unique);
					Assert.That(resultado, Is.All.Property("Coeficientes").All.Matches<long>(coeficiente => listaPotencias.Contains(coeficiente) || listaPotencias.Contains(coeficiente + divisor)));
				});
			}

		}

		[TestFixture]
		public class ReglaDivisibilidadOptimaTests {

			[Test(Description = "ReglaDivisibilidadOptima lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglaDivisibilidadOptima_ArgumentosNoValidos_LanzaExcepcion(
				[Values(1, -1)] long divisor, [Values(0, -1)] int longitud, [Values(1, -1)] long @base) {
				Assume.That(divisor <= 1 || longitud <= 0 || @base <= 1);

				Assert.Throws<ArgumentOutOfRangeException>(() => Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base));
			}

			[Test(Description = "ReglaDivisibilidadOptima lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglaDivisibilidadOptima_ArgumentosNoCoprimos_LanzaExcepcion(
				[Values(2, 4)] long divisor, [Values(6, 10)] long @base) {
				int longitud = 2;

				Assert.Throws<ArithmeticException>(() => Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base));
			}


			[Test(Description = "ReglaDivisibilidadOptima lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglaDivisibilidadOptima_ArgumentosCoprimos_DevuelveRegla(
				[Values(3, 7)] long divisor, [Values(10, 20)] long @base) {
				int longitud = 5;
				long inverso = Calculos.InversoMod(@base, divisor);

				ReglaCoeficientes resultado = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);

				Assert.Multiple(() => {
					Assert.That(resultado.Longitud, Is.EqualTo(longitud));
					Assert.That(resultado.Coeficientes, Has.Count.EqualTo(longitud));
					Assert.That(resultado.Base, Is.EqualTo(@base));
					Assert.That(resultado.Divisor, Is.EqualTo(divisor));
					for (int i = 0; i < resultado.Coeficientes.Count; i++) {
						long potencia = Calculos.PotenciaEntera(inverso, i + 1) % divisor;
						Assert.That(resultado.Coeficientes[i], Is.EqualTo(Calculos.MinAbs(potencia, potencia - divisor)));
					}
				});
			}

		}

		[TestFixture]
		public class ReglaDivisibilidadBaseTests {

			[Test(Description = "ReglaDivisibilidadBase lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglaDivisibilidadBase_ArgumentosNoValidos_LanzaExcepcion(
				[Values(1,-1)] long divisor, [Values(0,-1)] int longitud, [Values(1,-1)] long @base) {
				Assume.That(divisor <= 1 || longitud <= 0 || @base <= 1);

				Assert.Throws<ArgumentOutOfRangeException>(() => Calculos.ReglaDivisibilidadBase(divisor, longitud, @base));
			}

			[Test(Description = "ReglaDivisibilidadBase lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglaDivisibilidadBase_ArgumentosNoCoprimos_LanzaExcepcion(
				[Values(2,4)] long divisor, [Values(6,10)] long @base) {
				int longitud = 2;

				Assert.Throws<ArithmeticException>(() => Calculos.ReglaDivisibilidadBase(divisor, longitud, @base));
			}


			[Test(Description = "ReglaDivisibilidadBase lanza excepción si base o divisor son menores que 2 o longitud no es positiva"), Combinatorial]
			public void ReglaDivisibilidadBase_ArgumentosCoprimos_DevuelveLista(
				[Values(3, 7)] long divisor, [Values(10, 20)] long @base) {
				int longitud = 5;
				long inverso = Calculos.InversoMod(@base,divisor);

				List<long> resultado = Calculos.ReglaDivisibilidadBase(divisor, longitud, @base);

				Assert.That(resultado, Has.Count.EqualTo(longitud));
				for (int i = 0; i < resultado.Count; i++) {
					Assert.That(resultado[i], Is.EqualTo(Calculos.PotenciaEntera(inverso, i + 1) % divisor));
				}
			}

		}

		[TestFixture]
		public class ReglaDivisibilidadExtendidaTests
		{

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que el divisor no puede ser cero y true")]
			public void ReglaDivisibilidadExtendida_DivisorCero_MensajeCero()
			{
				// Arrange

				long divisor = 0;
				long raiz = 3;

				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.True);
			}

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que uno divide a todos los enteros y true")]
			public void ReglaDivisibilidadExtendida_DivisorUno_MensajeUno()
			{
				// Arrange

				long divisor = 1;
				long raiz = 3;

				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.True);
			}

			[Test(Description = "ReglaDivisibiladExtendida devuelve un mensaje indicando que se deben sumar bloques de cifras del número si el divisor es múltiplo de la base o una potencia menos uno y true")]
			[TestCase()]
			[TestCase(13, 3, 3)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMenos1PotenciaMayorQue1_MensajeSumarBloques(long divisor = -1, long raiz = -1, int potencia = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 7;
					raiz = 2;
					potencia = 3;
				}
				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.True);
			}

			[Test(Description = "ReglaDivisibiladExtendida devuelve un mensaje indicando que se deben sumar las cifras del número si el divisor es múltiplo de la base menos uno y true")]
			[TestCase()]
			[TestCase(3, 10)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMenos1Potencia1_MensajeSumarBloques(long divisor = -1, long raiz = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 5;
					raiz = 16;
				}
				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.True);
			}


			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que se deben restar bloques de cifras del número si el divisor es múltiplo de la base o una potencia más uno y true")]
			[TestCase()]
			[TestCase(257, 16, 2)]
			[TestCase(9, 2, 3)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMas1PotenciaMayorQue1_CasoRestarBloques(long divisor = -1, long raiz = -1, int potencia = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 101;
					raiz = 10;
					potencia = 2;
				}
				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.True);
			}

			[Test(Description = "ReglaDivisibiladExtendida devuelve un mensaje indicando que se deben restar la suma de las cifras pares con la suma de las impares del número si el divisor es múltiplo de la base más uno y true")]
			[TestCase()]
			[TestCase(17, 16)]
			[TestCase(3, 2)]
			public void ReglaDivisibilidadExtendida_DivisorMultiploRaizMas1Potencia1_CasoRestarBloques(long divisor = -1, long raiz = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 11;
					raiz = 10;
				}
				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.True);
			}

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que solo las primeras cifras importan para la divisibilidad si el divisor está compuesto de los factores primos de raiz y true")]
			[TestCase()]
			[TestCase(32, 16, 2)]
			[TestCase(16, 2, 4)]
			public void ReglaDivisibilidadExtendida_DivisorCompuestoDePotenciasDeRaiz_CasoMirarCifras(long divisor = -1, long raiz = -1, int cifras = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 5;
					raiz = 10;
					cifras = 1;
				}

				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.EqualTo(true));
			}

			[Test(Description = "ReglaDivisibilidadExtendida lanza excepción si la base es menor que 2")]
			public void ReglaDivisibilidadExtendida_BaseMenorQue2_LanzaExcepcion()
			{
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz));

			}

			[Test(Description = "ReglaDivisibilidadExtendida lanza excepción si el divisor es menor que 2")]
			public void ReglaDivisibilidadExtendida_DivisorNegativo_LanzaExcepcion()
			{
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz));

			}

			[Test(Description = "ReglaDivisibilidadExtendida devuelve un mensaje indicando que no se ha encontrado una regla alternativa si no puede encontrar y false")]
			public void ReglaDivisibilidadExtendida_NoEncuentra_CasoNormal()
			{
				// Arrange
				long divisor = 789293; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = 31233;

				// Act
				var result = Calculos.ReglaDivisibilidadExtendida(
					divisor,
					raiz);

				// Assert
				Assert.That(result.Item1, Is.EqualTo(false));
			}
		}

		[TestFixture]
		public class CasoEspecialReglaTests
		{

			[Test(Description = "CasoEspecialRegla devuelve el caso CERO e información -1 si el divisor es 0")]
			public void CasoEspecialRegla_DivisorCero_CasoEsCeroInformacionMenos1()
			{
				// Arrange

				long divisor = 0;
				long raiz = 3;

				// Act
				var result = Calculos.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.DIVISOR_ZERO));
					Assert.That(result.informacion, Is.EqualTo(-1));
				});
			}

			[Test(Description = "CasoEspecialRegla devuelve el caso UNO e información -1 si el divisor es 1")]
			public void CasoEspecialRegla_DivisorUno_CasoEsUnoInformacionMenos1()
			{
				// Arrange

				long divisor = 1;
				long raiz = 3;

				// Act
				var result = Calculos.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.DIVISOR_ONE));
					Assert.That(result.informacion, Is.EqualTo(-1));
				});
			}

			[Test(Description = "CasoEspecialRegla devuelve el caso SUMAR_BLOQUES si el divisor es múltiplo de la base o una potencia menos uno y devuelve el tamaño de los bloques")]
			[TestCase()]
			[TestCase(5, 16, 1)]
			[TestCase(7, 2, 3)]
			public void CasoEspecialRegla_DivisorMultiploRaizMenos1_CasoSumarBloques(long divisor = -1, long raiz = -1, int potencia = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 9;
					raiz = 10;
					potencia = 1;
				}
				// Act
				var result = Calculos.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.ADD_BLOCKS));
					Assert.That(result.informacion, Is.EqualTo(potencia));
				});
			}


			[Test(Description = "CasoEspecialRegla devuelve el caso RESTAR_BLOQUES si el divisor es múltiplo de la base o una potencia más uno y devuelve el tamaño de los bloques")]
			[TestCase()]
			[TestCase(17, 16, 1)]
			[TestCase(9, 2, 3)]
			public void CasoEspecialRegla_DivisorMultiploRaizMas1_CasoRestarBloques(long divisor = -1, long raiz = -1, int potencia = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 11;
					raiz = 10;
					potencia = 1;
				}

				// Act
				var result = Calculos.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.SUBSTRACT_BLOCKS));
					Assert.That(result.informacion, Is.EqualTo(potencia));
				});
			}

			[Test(Description = "CasoEspecialRegla devuelve el caso MIRAR_CIFRAS si el divisor está compuesto de los divisores de la base y devuelve las cifras que se deben considerar")]
			[TestCase()]
			[TestCase(32, 16, 2)]
			[TestCase(16, 2, 4)]
			public void CasoEspecialRegla_DivisorCompuestoDePotenciasDeRaiz_CasoMirarCifras(long divisor = -1, long raiz = -1, int cifras = -1)
			{
				// Arrange - para que haya un arrange
				if (divisor == -1)
				{
					divisor = 5;
					raiz = 10;
					cifras = 1;
				}

				// Act
				var result = Calculos.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.DIGITS));
					Assert.That(result.informacion, Is.EqualTo(cifras));
				});
			}

			[Test(Description = "CasoEspecialRegla lanza excepción si la base es menor que 2")]
			public void CasoEspecialRegla_BaseMenorQue2_LanzaExcepcion()
			{
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => Calculos.CasoEspecialRegla(
					divisor,
					raiz));

			}

			[Test(Description = "CasoEspecialRegla lanza excepción si el divisor es menor que 2")]
			public void CasoEspecialRegla_DivisorNegativo_LanzaExcepcion()
			{
				// Arrange
				long divisor = 10; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = -1;

				// Act
				Assert.Throws<ArgumentOutOfRangeException>(
					() => Calculos.CasoEspecialRegla(
					divisor,
					raiz));

			}

			[Test(Description = "CasoEspecialRegla devuelve el caso USAR_NORMAL e información -1 si ninguno de los casos especiales se aplica")]
			public void CasoEspecialRegla_NoEncuentra_CasoNormal()
			{
				// Arrange
				long divisor = 789293; // No significa que no pueda existir otra regla para estos valores, pero no se pueden encontrar usando longs
				long raiz = 31233;

				// Act
				var result = Calculos.CasoEspecialRegla(
					divisor,
					raiz);

				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(result.caso, Is.EqualTo(CasosDivisibilidad.COEFFICIENTS));
					Assert.That(result.informacion, Is.EqualTo(-1));
				});
			}
		}
	}
}
