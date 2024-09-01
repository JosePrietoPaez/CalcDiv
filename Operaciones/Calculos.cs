using Operaciones.Recursos;
using System.Diagnostics.CodeAnalysis;
using static System.Math;
using System.Text;
using System.Numerics;

namespace Operaciones
{
	public static class Calculos {

		private static int _longitudPrimos = 0;
		private static readonly List<long> _primosCalculados = [];

		/// <summary>
		/// Esta propiedad devuelve primero clon de la lista de primos más larga que ha sido calculada para no tener que calcular todos los primos siempre.
		/// </summary>
		/// <remarks>
		/// Asegúrese de que PrimosHasta ha sido llamado con argumento mayor o igual al número que espera.
		/// </remarks>
		internal static List<long> PrimosCalculados => new(_primosCalculados);

		/// <summary>
		/// Devuelve true si el número es primo, si no false
		/// </summary>
		/// <remarks>
		/// Los números no naturales no se consideran primos, por lo que se devuelve false
		/// </remarks>
		/// <param name="numero"></param>
		/// <returns></returns>
		public static bool EsPrimo(long numero) {
			if (numero <= 1) return false; //Por definición
			if (numero == 2) return true; //Porque si no, no va
			long cont = 2;
			double raizT = ((long)Sqrt(numero)) + 1;
			while (cont < raizT && numero % cont != 0) {
				cont++;
			}
			bool primo = numero % cont != 0;
			return primo;
		}

		
		/// <summary>
		/// Devuelve el factorial de <c>@base</c>.
		/// </summary>
		public static long Factorial(long numero) {
			ArgumentOutOfRangeException.ThrowIfNegative(numero);
			long res = numero;
			while (numero > 1) {
				res *= numero;
				numero--;
			}
			return res;
		}

		/// <summary>
		/// Devuelve el máximo común divisor de <c>@base</c> y <c>segundo</c>.
		/// </summary>
		public static long Mcd(long primero, long segundo)
		{
			if (primero < 0 || segundo < 0) throw new ArgumentException("Los argumentos deben ser no negativos");
			if (primero < segundo) { //Necesitamos que @base sea mayor o igual que segundo
				(segundo, primero) = (primero, segundo);
			}
			while (segundo != 0) {
				(primero,segundo) = (segundo, primero % segundo);
			}
			return primero;
		}

		/// <summary>
		/// Indica si <c>primero</c> y <c>segundo</c> son coprimos.
		/// </summary>
		/// <remarks>
		/// Ambos parámetros deben ser mayores o iguales que 0.
		/// </remarks>
		/// <returns>
		/// <c>true</c> si primero y segundo son coprimos, si no <c>false</c>.
		/// </returns>
		public static bool SonCoprimos(long primero, long segundo) {
			return Mcd(primero,segundo) == 1;
		}

		/// <summary>
		/// Devuelve una serie que contiene la descomposición en números primos de <c>divisor</c>.
		/// </summary>
		/// <remarks>
		/// Los elementos de la serie son los exponentes de los números primos en orden ascendente hasta llegar al último con coeficiente positivo.
		/// </remarks>
		public static List<long> DescompsicionEnPrimos(long numero) {
			if (numero < 0) throw new ArgumentException("El argumento debe ser positivo");
			List<long> primos = PrimosHasta(numero);
			List<long> res = new(primos.Count);
			while (res.Count < primos.Count) res.Add(0);

			Descomposicion(primos, res, numero);

			while (res[^1] == 0) res.RemoveAt(res.Count - 1);

			return res;
		}

		/// <summary>
		/// Calcula el mínimo común múltiplo de <c>primero</c> y <c>segundo</c>.
		/// </summary>
		/// <remarks>
		/// <c>primero</c> y <c>segundo</c> deben ser no negativos.
		/// </remarks>
		/// <exception cref="ArgumentException"></exception>
		/// <returns>
		/// El mínimo número positivo que es múltiplo de los segundo argumentos.
		/// </returns>
		public static long Mcm(long primero, long segundo) {
			if (primero < 0 || segundo < 0) throw new ArgumentException("Los argumentos deben ser no negativos");
			if (primero == 0 || segundo == 0) return 0;
			return primero * segundo / Mcd(primero, segundo);
		}

		/// <summary>
		/// Devuelve una lista con los números primos hasta <c>divisor</c> incluido
		/// </summary>
		public static List<long> PrimosHasta(long numero) {
			long cont;
			List<long> serie = [];
			if (_longitudPrimos == 0) { //Si no se ha calculado ninguno hace los cálculos completos
				if (numero >= 2) {
					serie.Add(2L);
				}
				if (numero <= 2) {
					return serie;
				}
				cont = 3;
				_primosCalculados.Add(2L); //Es más complicado si se introduce en el primer if
				_longitudPrimos = 1;
			} else {
				for (int i = 0; i < _primosCalculados.Count && _primosCalculados[i] <= numero; i++) { //Se insertan todos los que se necesiten para no tener que buscarlos otra vez
					serie.Add(_primosCalculados[i]);
				}
				cont = _primosCalculados[^1] + 2; //Se le suma 2 para evitar que se inserte otra vez en el while
			}

			while (cont <= numero) {
				if (EsPrimo(cont)) {
					serie.Add(cont);
					_primosCalculados.Add(cont);
				}
				cont += 2;
			}
			
			return serie;
		}

		/// <summary>
		/// Calcula el inverso multiplicativo de <c>a</c> en módulo <c>m</c>.
		/// </summary>
		/// <remarks>
		/// Devuelve 0 si no hay inverso.<para>
		/// El resultado nunca es negativo
		/// </para>
		/// </remarks>
		/// <returns>
		/// a^-1 talque a * a^-1 equivale a 1 (mód m)
		/// </returns>
		public static long InversoMod(long a, long m) {
			long c1 = 1;
			long c2 = -(m / a); //coeficiente de a y b respectivamente
			long t1 = 0;
			long t2 = 1; //coeficientes penultima corrida
			long r = m % a; //residuo, asignamos 1 como condicion de entrada
			long x = a, y = r, c;

			while (r != 0) {
				c = x / y; //cociente
				r = x % y; //residuo

				// guardamos valores temporales de los coeficientes
				// multiplicamos los coeficiente por -1*cociente de la division
				c1 *= -c;
				c2 *= -c;

				//sumamos la corrida anterior
				c1 += t1;
				c2 += t2;

				//actualizamos corrida anterior
				t1 = -(c1 - t1) / c;
				t2 = -(c2 - t2) / c;
				x = y;
				y = r;
			}
			if (x == 1) { //En Wikipedia no usaba returns
				return t2 < 0 ? t2 + m : t2;
			} else {
				return 0;
			}
		}

		/**
		 * Calcula la progresión aritmética que empieza en inicio, acaba en fin y tiene divisor números
		 * @param inicio primer número del sumatorio
		 * @param fin último número del sumatorio
		 * @param divisor número de elementos de la progresión
		 * @return valor de la progresión aritmética
		 */
		public static long SumatorioIntervalo(long inicio, long fin, long numero) {
			return (inicio + fin) * numero / 2;
		}

		/// <summary>
		/// Devuelve el mínimo de los absolutos entre <c>primero</c> y <c>segundo</c>.
		/// </summary>
		public static long MinAbs(long primero, long segundo) {
			if (Min(Abs(primero), Abs(segundo)) == Abs(primero))
				return primero;
			return segundo;
		}

		/// <summary>
		/// Calcula el número de cifras de <c>numero</c> en base <c>@base</c>.
		/// </summary>
		public static byte Cifras(long numero, long @base) { //El valor máximo será 64 en base 2
			if (numero == 0) return 1;
			return (byte)Ceiling(Log(Abs(numero) + 1L) / Log(@base));
		}

		/// <summary>
		/// Calcula el número de cifras de <c>numero</c> en base <c>@base</c>.
		/// </summary>
		public static long Cifras(BigInteger numero, long @base) { //El valor máximo será 64 en base 2
			if (numero == 0) return 1;
			return (long)Ceiling(BigInteger.Log(BigInteger.Abs(numero) + 1L) / Log(@base));
		}

		/// <summary>
		/// Calcula el número de cifras de <c>numero</c> en base <c>@base</c>.
		/// </summary>
		public static short Cifras(double numero, double @base) { //El valor máximo será 1024 en base dos, creo
			if (numero == 0) return 1;
			return (short)Ceiling(Log(BitIncrement(Abs(numero))) / Log(@base));
		}

		/// <summary>
		/// Calcula el número de cifras de <c>numero</c> en base 10.
		/// </summary>
		public static byte Cifras10(long numero) => Cifras(numero, 10);

		/// <summary>
		/// Calcula el número de cifras de <c>numero</c> en base 10.
		/// </summary>
		public static short Cifras10(double numero) => Cifras(numero, 10);

		/// <summary>
		/// Calcula la cifra en la posición <c>pos</c> de <c>numero</c> en base <c>@base</c>.
		/// </summary>
		/// <remarks>
		/// La cifra 0 es la menos significativa.
		/// </remarks>
		/// <returns>
		/// Cifra <c>pos</c> de <c>numero</c> en base <c>@base</c>
		/// </returns>
		public static long Cifra(long numero, int pos, long @base) {
			if (pos < 0) throw new ArgumentException("La posición debe ser una cifra del número");
			if (pos >= Cifras(numero, @base)) return 0; //Suponemos que las cifras de la izquierda son 0
			return Abs(numero) / PotenciaEntera(@base,pos) % @base;	
		}

		/// <summary>
		/// Calcula la cifra en la posición <c>pos</c> de <c>numero</c> en base <c>@base</c>.
		/// </summary>
		/// <remarks>
		/// La cifra 0 es la menos significativa.
		/// </remarks>
		/// <returns>
		/// Cifra <c>pos</c> de <c>numero</c> en base <c>@base</c>
		/// </returns>
		public static BigInteger Cifra(BigInteger numero, int pos, long @base) {
			if (pos < 0) throw new ArgumentException("La posición debe ser una cifra del número");
			if (pos >= Cifras(numero, @base)) return 0; //Suponemos que las cifras de la izquierda son 0
			return BigInteger.Abs(numero) / PotenciaEntera(@base,pos) % @base;
		}

		/// <summary>
		/// Devuelve un número con las cifras de <c>numero</c> entre las posiciones <c>cifraInicio</c> inclusive y <c>cifraFin</c> no inclusive en la base indicada
		/// </summary>
		/// <param name="numero">Número del que se sacarán las cifras</param>
		/// <param name="base">Base de la que se obtendrán las cifras</param>
		/// <param name="cifraInicio">Primera cifra que obtener</param>
		/// <param name="cifraFin">Posición posterior a la última</param>
		/// <returns>
		/// Cifras en el intervalo indica
		/// </returns>
		public static long IntervaloCifras(long numero, long @base, int cifraInicio, int cifraFin) {
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cifraInicio, cifraFin, nameof(cifraInicio));
			return Abs(numero) / PotenciaEntera(@base,cifraInicio) % PotenciaEntera(@base, cifraFin - cifraInicio);
		}

		/// <summary>
		/// Devuelve un número con las cifras de <c>numero</c> entre las posiciones <c>cifraInicio</c> inclusive y <c>cifraFin</c> no inclusive en la base indicada
		/// </summary>
		/// <param name="numero">Número del que se sacarán las cifras</param>
		/// <param name="base">Base de la que se obtendrán las cifras</param>
		/// <param name="cifraInicio">Primera cifra que obtener</param>
		/// <param name="cifraFin">Posición posterior a la última</param>
		/// <returns>
		/// Cifras en el intervalo indica
		/// </returns>
		public static BigInteger IntervaloCifras(BigInteger numero, long @base, int cifraInicio, int cifraFin) {
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cifraInicio, cifraFin, nameof(cifraInicio));
			return BigInteger.Abs(numero) / PotenciaEnteraGrande(@base,cifraInicio) % PotenciaEnteraGrande(@base, cifraFin - cifraInicio);
		}

		/**
		 * Devuelve primero objeto {@code String} que contiene {@code numero} escrito con subíndices de sus cifras en @base 10
		 * @return {@code numero} en subíndices
		 */
		public static string NumASubindice(long numero) {
			StringBuilder res = new();
			if (numero < 0) {
				res.Append('₋');
			}
			byte cifras = Cifras(numero, 10);
			for (byte i = cifras; i > 0; i--) {
				res.Append(CifraASubindice(Cifra(numero, (byte)(i - 1), 10)));
			}
			return res.ToString();
		}

		public static char CifraASubindice(long numero) { return (char)(numero + 0x2080); }

		/// <summary>
		/// An optimized method using an array as buffer instead of 
		/// string concatenation. This is faster for return values having 
		/// a length > 1.
		/// </summary>
		/// <remarks>
		/// Basado en https://stackoverflow.com/a/923814/22934376.
		/// </remarks>
		public static string LongToStringFast(BigInteger value, char[] baseChars) {
			// Es 64 para el caso máximo en base 2 y uno más para negativo
			uint targetBase = (uint)baseChars.Length;
			int cifras = (int)Cifras(value, targetBase) + 1, i = cifras;
			char[] buffer = new char[i];
			bool ponerSigno = value.Sign == -1;
			if (ponerSigno)
				value = BigInteger.Abs(value); //Para quitarle el signo, jódase el caso máximo

			do {
				buffer[--i] = baseChars[(int)(value % targetBase)];
				value /= targetBase;
			}
			while (value > 0);

			if (ponerSigno)
				buffer[--i] = '-';

			char[] result = new char[cifras - i];
			Array.Copy(buffer, i, result, 0, cifras - i);

			if (targetBase == 10) {
				return new string(result, 0, cifras - i);
			}
			return new string(result, 0, cifras - i)
			 //Se supone que no debería hacer esto para que vaya rápido, pero necesito poner la base
			 + '(' + targetBase + ')';

		}

		/// <summary>
		/// Sobrecarga que permite poner la base y usa un string predeterminado para las cifras.
		/// </summary>
		/// <remarks>
		/// Para más detalles: <see cref="LongToStringFast(BigInteger, char[])"/>
		/// <para>
		/// El máximo de <c>base</c> por defecto es <c>64</c>.
		/// </para>
		/// </remarks>
		public static string LongToStringFast(BigInteger value, long @base) => LongToStringFast(value, BASE_64_STRING.ToCharArray()[0..(int)@base]);

		/// <summary>
		/// Devuelve un string que representa a un <see cref="long"/> en una base arbitraria.
		/// </summary>
		/// <remarks>
		/// El formato es: "[Cifra(Longitud),Cifra(Longitud-1),...,Cifra(0)](base)".
		/// <para>
		/// Por ejemplo: "[15,12,3,0](16)" para <c>0xFC30</c> y base 16.
		/// </para>
		/// </remarks>
		/// <param name="value"></param>
		/// <param name="base"></param>
		/// <returns>
		/// String equivalente a <c>value</c>, con las cifras en decimal.
		/// </returns>
		public static string LongToStringNoAlphabet(long value, long @base) {
			long[] cifras = new long[Cifras(value, @base)];
			for (byte i = 0; i < cifras.Length; i++) {
				cifras[i] = Cifra(value, cifras.Length - i - 1, @base);
			}
			return (value < 0 ? "-" : "") + '[' + string.Join(',', cifras) + "](" + @base + ")";
		}

		/// <summary>
		/// Devuelve un string que representa a un <see cref="long"/> en una base arbitraria.
		/// </summary>
		/// <remarks>
		/// El formato es: "[Cifra(Longitud),Cifra(Longitud-1),...,Cifra(0)](base)".
		/// <para>
		/// Por ejemplo: "[15,12,3,0](16)" para <c>0xFC30</c> y base 16.
		/// </para>
		/// </remarks>
		/// <param name="value"></param>
		/// <param name="base"></param>
		/// <returns>
		/// String equivalente a <c>value</c>, con las cifras en decimal.
		/// </returns>
		public static string LongToStringNoAlphabet(BigInteger value, long @base) {
			BigInteger[] cifras = new BigInteger[Cifras(value, @base)];
			for (byte i = 0; i < cifras.Length; i++) {
				cifras[i] = Cifra(value, cifras.Length - i - 1, @base);
			}
			return (value < 0 ? "-" : "") + '[' + string.Join(',', cifras) + "](" + @base + ")";
		}

		public const string BASE_64_STRING = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_";

		/**
		 * Calcula {@code numero}^{@code exp} en módulo {@code @base} multiplicando {@code numero} por sí mismo y haciendo el módulo en cada paso
		 * <p>Intenta evitar el overflow que se produciría al hacer la potencia antes del módulo,
		 * pero esto no se garantiza</p>
		 * @param numero @base de la potencia
		 * @param exp exponente
		 * @param @base @base del módulo
		 * @return {@code numero}^{@code exp} en módulo {@code @base}
		 */
		public static long PotenciaMod(long numero, long exp, long @base) {
			long res = 1;
			numero %= @base;
			while (exp > 0) {
				res = ProductoMod(res, numero, @base);
				exp--;
			}
			return res;
		}

		/// <summary>
		/// Calcula el producto de <c>fac1</c> y <c>fac2</c> en módulo <c>@base</c>
		/// </summary>
		/// <returns>
		/// <c>fac1 * fac2 % @base</c>
		/// </returns>
		/// <exception cref="DivideByZeroException"></exception>
		public static long ProductoMod(long fac1, long fac2, long @base) {
			if (@base == 0) throw new DivideByZeroException("No se puede calcular el resto con divisor 0");
			return fac1 * fac2 % @base;
		}

		//Guarda en desc la descomposicion de numero, usando los primos de primos
		internal static void Descomposicion(List<long> primos, List<long> desc, long numero) {
			for (int j = 0; j < primos.Count && numero > 1; j++) {
				while (numero % primos[j] == 0) {
					numero /= primos[j];
					desc[j]++;
				}
			}
		}

		/// <summary>
		/// Devuelve todas las reglas de divisibilidad de <c>divisor</c> en base <c>@base</c> con longitud <c>longitud</c> cuyo valor absoluto no supera <c>@base</c>.
		/// </summary>
		/// <returns>
		/// <see cref="List{T}"/> de <see cref="ReglaCoeficientes"/> con todas las combinaciones de reglas.
		/// </returns>
		public static List<ReglaCoeficientes> ReglasDivisibilidad(long divisor, int longitud, long @base) {
			List<long> reglaInicial = ReglaDivisibilidadBase(divisor, longitud, @base), //Se calcula la regla de positivos
				listaAuxiliar = [];
			for (int i = 0; i < longitud; i++) {
				listaAuxiliar.Add(0);
			}
			bool[] producto = new bool[longitud]; //guarda si el elemento i de atajo se le resta divisor
			int iteracionesMaximas = PotenciaEntera(2, longitud), //Antes se usaba ArrayFalso, pero eso significaba una operación O(longitud) en cada iteración
				iteracionesActuales = 1;
			List<ReglaCoeficientes> reglas = new(iteracionesMaximas) {
				new(divisor, @base, reglaInicial)
			};
			static long restaCondicional(long coeficiente, long divisorResta, bool resta) => resta ? coeficiente - divisorResta : coeficiente;
			OperacionesListas.IncrementarArray(producto); //Como el primero ya está se incrementa
			do {
				for (int i = 0; i < reglaInicial.Count; i++) {
					listaAuxiliar[i] = restaCondicional(reglaInicial[i], divisor, producto[i]);
				}
				reglas.Add(new ReglaCoeficientes(divisor, @base, new List<long>(listaAuxiliar)));
				OperacionesListas.IncrementarArray(producto); //Itera el bucle
				iteracionesActuales++;
			} while (iteracionesMaximas > iteracionesActuales);
			return reglas;
		}

		/// <summary>
		/// Calcula la regla de divisibilidad óptima de <c>divisor</c> para la base <c>@base</c> con longitud <c>longitud</c>.
		/// </summary>
		/// <remarks>
		/// <c>divisor</c> y <c>@base</c> deben ser coprimos
		/// </remarks>
		public static ReglaCoeficientes ReglaDivisibilidadOptima(long divisor, int longitud, long @base) {
			List<long> regla = ReglaDivisibilidadBase(divisor, longitud, @base);
			for (int i = 0; i < regla.Count; i++) {
				regla[i] = MinAbs(regla[i], regla[i] - divisor);
			}
			return new(divisor, @base, regla);
		}

		/// <summary>
		/// Calcula la regla de divisibilidad positiva de <c>divisor</c> para la base <c>@base</c> con longitud <c>longitud</c>.
		/// </summary>
		/// <remarks>
		/// <c>divisor</c> y <c>@base</c> deben ser coprimos
		/// </remarks>
		public static List<long> ReglaDivisibilidadBase(long divisor, int longitud, long @base) {
			ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 2, nameof(divisor));
			ArgumentOutOfRangeException.ThrowIfLessThan(@base, 2, nameof(@base));
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(longitud, nameof(longitud));
			long inv = InversoMod(@base, divisor);
			if (inv == 0) throw new ArithmeticException("numero debe ser coprimo con @base");
			return OperacionesListas.PotenciaModProgresiva(inv, divisor, longitud);
		}

		/// <summary>
		/// Calcula la potencia de enteros de forma eficiente.
		/// </summary>
		/// <remarks>
		/// <c>exp</c> debe ser positivo.
		/// <para>
		/// Obtenido de https://stackoverflow.com/a/2065303/22934376
		/// </para>
		/// </remarks>
		/// <param name="base"></param>
		/// <param name="exp"></param>
		/// <returns>
		/// <c>base</c> ^ <c>exp</c>
		/// </returns>
		public static long PotenciaEntera(long @base, long exp) {
			long result = 1;
			while (exp > 0) {
				if ((exp & 1) != 0) {
					checked {
						result *= @base;
					}
				}
				exp >>= 1;
				@base *= @base;
			}
			return result;
		}

		/// <summary>
		/// Calcula la potencia de enteros de forma eficiente.
		/// </summary>
		/// <remarks>
		/// <c>exp</c> debe ser positivo.
		/// <para>
		/// Obtenido de https://stackoverflow.com/a/2065303/22934376
		/// </para>
		/// </remarks>
		/// <param name="base"></param>
		/// <param name="exp"></param>
		/// <returns>
		/// <c>base</c> ^ <c>exp</c>
		/// </returns>
		public static BigInteger PotenciaEnteraGrande(BigInteger @base, long exp) {
			BigInteger result = 1;
			while (exp > 0) {
				if ((exp & 1) != 0) {
					result *= @base;
				}
				exp >>= 1;
				@base *= @base;
			}
			return result;
		}

		/// <summary>
		/// Calcula la potencia de enteros de forma eficiente.
		/// </summary>
		/// <remarks>
		/// <c>exp</c> debe ser positivo.
		/// <para>
		/// Obtenido de https://stackoverflow.com/a/2065303/22934376
		/// </para>
		/// </remarks>
		/// <param name="base"></param>
		/// <param name="exp"></param>
		/// <returns>
		/// <c>base</c> ^ <c>exp</c>
		/// </returns>
		public static int PotenciaEntera(int @base, int exp) {
			int result = 1;
			while (exp > 0) {
				if ((exp & 1) != 0) {
					checked {
						result *= @base;
					}
				}
				exp >>= 1;
				@base *= @base;
			}
			return result;
		}

		/// <summary>
		/// Devuelve una tupla indicando si se ha encontrado una regla alternativa y una explicación de como aplicarla.
		/// </summary>
		/// <remarks>
		/// <c>divisor</c> y <c>@base</c> deben ser mayores que uno.
		/// <para>
		/// Si no se encuentra una regla debería usarse <see cref="ReglasDivisibilidad(long, int, long)"/>
		/// para calcularlas.
		/// </para>
		/// El divisor debe ser mayor o igual que 2.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <param name="divisor">Divisor de la regla</param>
		/// <param name="base">Base para aplicar la regla</param>
		/// <returns>
		/// Tupla con primero booleano indicando el éxito del método, segundo la regla con su información inicializada.
		/// </returns>
		public static (bool, IRegla) ReglaDivisibilidadExtendida(long divisor, long @base) {
			var caso = CasoEspecialRegla(divisor, @base);
			IRegla reglaExtra = IRegla.GenerarReglaPorTipo(caso.caso, divisor, @base, caso.informacion);
			return (caso.caso != CasosDivisibilidad.COEFFICIENTS, reglaExtra);
		}

		/// <summary>
		/// Devuelve una tupla que representa la regla de divisibilidad encontrada para <c>divisor</c> y <c>@base</c>.
		/// </summary>
		/// <remarks>
		/// Para obtener los mensajes use <see cref="ReglaDivisibilidadExtendida(long, long)"/> en su lugar.
		/// <para>
		/// Este método es público porque el flujo normal de la aplicación solo devuelve mensajes.
		/// </para>
		/// El divisor debe ser mayor o igual que 2.
		/// <list type="bullet">
		/// <listheader>Significado de <c>informacion</c> según el caso:</listheader>
		/// <item>CERO, UNO y USAR_NORMAL</item> - <description>Ninguno, siempre es -1 ya que no se requiere más información</description>
		/// <item>MIRAR_CIFRAS</item> - <description>Cifras que se deben considerar</description>
		/// <item>RESTAR_BLOQUES y SUMAR_BLOQUES</item> - <description>Longitud de los bloques para agrupar las cifras</description>
		/// </list>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <param name="divisor"></param>
		/// <param name="base"></param>
		/// <returns>
		/// Tupla con primero caso y primero entero con información
		/// </returns>
		[return: NotNull]
		public static (CasosDivisibilidad caso,int informacion) CasoEspecialRegla(long divisor, long @base) {
			ArgumentOutOfRangeException.ThrowIfLessThan(@base,2,nameof(@base));
			ArgumentOutOfRangeException.ThrowIfLessThan(@base,0, nameof(@base));

			if (divisor == 0) return (CasosDivisibilidad.DIVISOR_ZERO, -1);
			if (divisor == 1) return (CasosDivisibilidad.DIVISOR_ONE, -1);

			List<long> descomposicionDivisor = DescompsicionEnPrimos(divisor),
				descomposicionRaiz = DescompsicionEnPrimos(@base);
			while (descomposicionDivisor.Count > descomposicionRaiz.Count)
				descomposicionRaiz.Add(0);

			while (descomposicionDivisor.Count < descomposicionRaiz.Count)
				descomposicionDivisor.Add(0);

			var tuplaCaso = ProductoDePotenciasDeBases(descomposicionDivisor, descomposicionRaiz);

			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.DIGITS, tuplaCaso.dato);
			tuplaCaso = UnoMenosQuePotencia(divisor, @base);
			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.SUBSTRACT_BLOCKS, tuplaCaso.dato);
			tuplaCaso = UnoMasQuePotencia(divisor, @base);
			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.ADD_BLOCKS, tuplaCaso.dato);
			return (CasosDivisibilidad.COEFFICIENTS,-1);
		}

		/// <summary>
		/// Comprueba si divisor es una potencia de @base menos uno, o una de los divisores de @base menos uno, lo que permite usar una regla distinta
		/// </summary>
		/// <returns></returns>
		private static (bool, int) UnoMenosQuePotencia(long divisor, long @base) {
			if (!SonCoprimos(divisor, @base)) return (false, -1);
			List<long> modulos = []; // Para evitar que entre en primero bucle
			long raizModulo = @base % divisor;
			int potencia = 1;
			bool restoDivisorMenosUno = @base % divisor == divisor - 1;
			if (!restoDivisorMenosUno)
				do {
					modulos.Add(raizModulo);
					raizModulo *= modulos[0];
					raizModulo %= divisor; //Para evitar overflow se calcula el modulo divisor
					restoDivisorMenosUno |= raizModulo == divisor - 1;
					potencia++;
				} while (!restoDivisorMenosUno && !modulos.Contains(raizModulo));
			return (restoDivisorMenosUno, potencia);
		}

		/// <summary>
		/// Comprueba si divisor es una potencia de @base más uno, o uno de sus divisores, lo que permite usar una regla distinta
		/// </summary>
		/// <returns></returns>
		private static (bool,int) UnoMasQuePotencia(long divisor, long @base) {
			if (!SonCoprimos(divisor, @base)) return (false, -1);
			List<long> modulos = []; // Para evitar que entre en primero bucle
			long raizModulo = @base % divisor;
			int potencia = 1;
			bool restoUno = @base % divisor == 1;
			if (!restoUno)
				do {
					modulos.Add(raizModulo);
					raizModulo *= modulos[0];
					raizModulo %= divisor; //Para evitar overflow se calcula el modulo divisor
					restoUno |= raizModulo == 1;
					potencia++;
				} while (!restoUno && !modulos.Contains(raizModulo));
			return (restoUno, potencia);
		}

		private static (bool cumpleCondicion,int dato) ProductoDePotenciasDeBases(List<long> divisor, List<long> @base) {
			if (@base.Count == 0) return (false, -1);
			int maxPotencia = 0;
			for (int i = 0; i < Min(divisor.Count,@base.Count); i++) { //Comprueba que todos los valores de divisor estén en @base
				//Si en divisor es mayor que cero, pero en @base es cero, tiene primero componente primo que no esta en @base y no se le puede aplicar la regla
				if (divisor[i] != 0 && @base[i] == 0) return (false, -1);
				maxPotencia = Max(maxPotencia, @base[i] == 0 ? 0 : (int)Ceiling(divisor[i] / (double)@base[i]));
			}
			return (true, maxPotencia);
		}
	}
}
