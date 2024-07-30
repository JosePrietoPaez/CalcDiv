using Operaciones.Recursos;
using System.Diagnostics.CodeAnalysis;
using static System.Math;
using System.Text;

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
		public static long Factorial(long num) {
			ArgumentOutOfRangeException.ThrowIfNegative(num);
			long res = num;
			while (num > 1) {
				res *= num;
				num--;
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
		public static List<long> DescompsicionEnPrimos(long num) {
			if (num < 0) throw new ArgumentException("El argumento debe ser positivo");
			List<long> primos = PrimosHasta(num);
			List<long> res = new(primos.Count);
			while (res.Count < primos.Count) res.Add(0);

			Descomposicion(primos, res, num);

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
		public static List<long> PrimosHasta(long num) {
			long cont;
			List<long> serie = [];
			if (_longitudPrimos == 0) { //Si no se ha calculado ninguno hace los cálculos completos
				if (num >= 2) {
					serie.Add(2L);
				}
				if (num <= 2) {
					return serie;
				}
				cont = 3;
				_primosCalculados.Add(2L); //Es más complicado si se introduce en el primer if
				_longitudPrimos = 1;
			} else {
				for (int i = 0; i < _primosCalculados.Count && _primosCalculados[i] <= num; i++) { //Se insertan todos los que se necesiten para no tener que buscarlos otra vez
					serie.Add(_primosCalculados[i]);
				}
				cont = _primosCalculados[^1] + 2; //Se le suma 2 para evitar que se inserte otra vez en el while
			}

			while (cont <= num) {
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
		public static long SumatorioIntervalo(long inicio, long fin, long num) {
			return (inicio + fin) * num / 2;
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
		/// Calcula el número de cifras de <c>divisor</c> en base <c>@base</c>.
		/// </summary>
		public static short Cifras(long num, long @base) { 
			if (num == 0) return 1;
			return (short)Ceiling(Log(Abs(num) + 1L) / Log(@base));
		}

		/// <summary>
		/// Calcula el número de cifras de <c>divisor</c> en base <c>@base</c>.
		/// </summary>
		public static short Cifras(double num, double @base) {
			if (num == 0) return 1;
			return (short)Ceiling(Log(BitIncrement(Abs(num))) / Log(@base));
		}
		
		/// <summary>
		/// Calcula el número de cifras de <c>divisor</c> en base 10.
		/// </summary>
		public static short Cifras10(long num) {
			return Cifras(num, 10);
		}

		/// <summary>
		/// Calcula la cifra en la posición <c>pos</c> de <c>divisor</c> en base <c>@base</c>.
		/// </summary>
		/// <remarks>
		/// La cifra 0 es la menos significativa.
		/// </remarks>
		/// <returns>
		/// Cifra <c>pos</c> de <c>divisor</c> en base <c>@base</c>
		/// </returns>
		public static short Cifra(long num, long pos, long @base) { //Recemos para que nadie ponga primero número de base mayor a 32767
			if (pos >= Cifras(num, @base) || pos < 0) throw new ArgumentException("La posición debe ser una cifra del número");
			return (short)(Abs(num) / PotenciaEntera(@base,pos) % @base);	
		}

		/**
		 * Devuelve primero objeto {@code String} que contiene {@code divisor} escrito con subíndices de sus cifras en @base 10
		 * @return {@code divisor} en subíndices
		 */
		public static string NumASubindice(long num) {
			StringBuilder res = new();
			if (num < 0) {
				res.Append('₋');
			}
			short cifras = Cifras(num, 10);
			for (int i = cifras - 1; i >= 0; i--) {
				res.Append(CifraASubindice(Cifra(num, i, 10)));
			}
			return res.ToString();
		}

		public static char CifraASubindice(long num) { return (char)(num + 0x2080); }

		/**
		 * Calcula {@code divisor}^{@code exp} en módulo {@code @base} multiplicando {@code divisor} por sí mismo y haciendo el módulo en cada paso
		 * <p>Intenta evitar el overflow que se produciría al hacer la potencia antes del módulo,
		 * pero esto no se garantiza</p>
		 * @param divisor @base de la potencia
		 * @param exp exponente
		 * @param @base @base del módulo
		 * @return {@code divisor}^{@code exp} en módulo {@code @base}
		 */
		public static long PotenciaMod(long num, long exp, long @base) {
			long res = 1;
			num %= @base;
			while (exp > 0) {
				res = ProductoMod(res, num, @base);
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

		//Guarda en desc la descomposicion de divisor, usando los primos de primos
		internal static void Descomposicion(List<long> primos, List<long> desc, long num) {
			for (int j = 0; j < primos.Count && num > 1; j++) {
				while (num % primos[j] == 0) {
					num /= primos[j];
					desc[j]++;
				}
			}
		}

		/// <summary>
		/// Devuelve todas las reglas de divisibilidad de <c>divisor</c> en base <c>@base</c> con longitud <c>longitud</c> cuyo valor absoluto no supera <c>@base</c>.
		/// </summary>
		/// <returns>
		/// <see cref="List"/> de <see cref="Regla"/> con todas las combinaciones de reglas.
		/// </returns>
		public static List<Regla> ReglasDivisibilidad(long divisor, int longitud, long @base) {
			List<long> reglaInicial = ReglaDivisibilidadBase(divisor, longitud, @base), //Se calcula la regla de positivos
				listaAuxiliar = [];
			for (int i = 0; i < longitud; i++) {
				listaAuxiliar.Add(0);
			}
			bool[] producto = new bool[longitud]; //guarda si el elemento i de atajo se le resta divisor
			int iteracionesMaximas = PotenciaEntera(2, longitud), //Antes se usaba ArrayFalso, pero eso significaba una operación O(longitud) en cada iteración
				iteracionesActuales = 1;
			List<Regla> reglas = new(iteracionesMaximas) {
				new(divisor, @base, reglaInicial)
			};
			Func<long, long, bool, long> restaCondicional = (coeficiente, divisorResta, resta) => resta ? coeficiente - divisorResta : coeficiente;
			OperacionesListas.IncrementarArray(producto); //Como el primero ya está se incrementa
			do {
				for (int i = 0; i < reglaInicial.Count; i++) {
					listaAuxiliar[i] = restaCondicional(reglaInicial[i], divisor, producto[i]);
				}
				reglas.Add(new Regla(divisor, @base, new List<long>(listaAuxiliar)));
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
		public static Regla ReglaDivisibilidadOptima(long divisor, int longitud, long @base) {
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
			if (inv == 0) throw new ArithmeticException("num debe ser coprimo con @base");
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
		/// <param name="divisor"></param>
		/// <param name="@base"></param>
		/// <returns>
		/// Tupla con primero booleano indicando el éxito del método, segundo mensaje explicando la regla obtenida y tercero con información.
		/// </returns>
		public static (bool,string,int) ReglaDivisibilidadExtendida(long divisor, long @base) {
			var caso = CasoEspecialRegla(divisor, @base);
			string potencia = "";
			try {
				long resultado = -1;
				if (caso.caso == CasosDivisibilidad.RESTAR_BLOQUES) {
					resultado = PotenciaEntera(@base, caso.informacion) + 1;
				} else if (caso.caso == CasosDivisibilidad.SUMAR_BLOQUES) {
					resultado = PotenciaEntera(@base, caso.informacion) - 1;
				}
				potencia = resultado.ToString(); //Puede dar overflow si el exponente es grande
			} catch (OverflowException) {
				potencia = TextoCalculos.CalculosExtendidaMensajeExceso;
			}
			string mensaje;
			mensaje = caso.caso switch {
				CasosDivisibilidad.MIRAR_CIFRAS => CrearMensajeDivisibilidad(divisor + TextoCalculos.CalculosExtendidaMensajeCifrasPrincipio + @base,
					string.Format(TextoCalculos.CalculosExtendidaMensajeCifrasFinal,caso.informacion) + divisor,
					@base, divisor),
				CasosDivisibilidad.UNO => TextoCalculos.CalculosExtendidaMensajeUno,
				CasosDivisibilidad.RESTAR_BLOQUES => CrearMensajeDivisibilidad(divisor + string.Format(TextoCalculos.CalculosExtendidaRestarPrincipio,@base,caso.informacion,potencia),
					string.Format(TextoCalculos.CalculosExtendidaRestarFinal,caso.informacion) + divisor,
					@base,divisor),
				CasosDivisibilidad.SUMAR_BLOQUES => CrearMensajeDivisibilidad(divisor + string.Format(TextoCalculos.CalculosExtendidaSumarPrincipio,@base,caso.informacion,potencia),
					string.Format(TextoCalculos.CalculosExtendidaSumarFinal,caso.informacion) + divisor,
					@base, divisor),
				CasosDivisibilidad.CERO => TextoCalculos.CalculosExtendidaMensajeCero,
				_ => TextoCalculos.CalculosExtendidaMensajeFracaso,
			};
			if (caso.informacion == 1) {
				mensaje = MensajeParaDatoConValorUno(caso.caso,divisor,@base,mensaje);
			}
			
			return (caso.caso != CasosDivisibilidad.USAR_NORMAL, mensaje,caso.informacion);
		}

		private static string MensajeParaDatoConValorUno(CasosDivisibilidad caso, long divisor, long @base, string mensajeInicial) {
			return caso switch {
				CasosDivisibilidad.RESTAR_BLOQUES => CrearMensajeDivisibilidad(divisor + string.Format(TextoCalculos.CalculosValorUnoRestarPrincipio,@base),
					TextoCalculos.CalculosValorUnoRestarFinal + divisor,
					@base, divisor),
				CasosDivisibilidad.SUMAR_BLOQUES => CrearMensajeDivisibilidad(divisor + string.Format(TextoCalculos.CalculosValorUnoSumarPrincipio,@base),
					TextoCalculos.CalculosValorUnoSumarFinal + divisor,
					@base, divisor),
				_ => mensajeInicial
			};
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
		/// <param name="@base"></param>
		/// <returns>
		/// Tupla con primero caso y primero entero con información
		/// </returns>
		[return: NotNull]
		public static (CasosDivisibilidad caso,int informacion) CasoEspecialRegla(long divisor, long @base) {
			ArgumentOutOfRangeException.ThrowIfLessThan(@base,2,nameof(@base));
			ArgumentOutOfRangeException.ThrowIfLessThan(@base,0, nameof(@base));

			if (divisor == 0) return (CasosDivisibilidad.CERO, -1);
			if (divisor == 1) return (CasosDivisibilidad.UNO, -1);

			List<long> descomposicionDivisor = DescompsicionEnPrimos(divisor),
				descomposicionRaiz = DescompsicionEnPrimos(@base);
			while (descomposicionDivisor.Count > descomposicionRaiz.Count)
				descomposicionRaiz.Add(0);

			while (descomposicionDivisor.Count < descomposicionRaiz.Count)
				descomposicionDivisor.Add(0);

			var tuplaCaso = ProductoDePotenciasDeBases(descomposicionDivisor, descomposicionRaiz, @base);

			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.MIRAR_CIFRAS, tuplaCaso.dato);
			tuplaCaso = UnoMenosQuePotencia(divisor, @base);
			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.RESTAR_BLOQUES, tuplaCaso.dato);
			tuplaCaso = UnoMasQuePotencia(divisor, @base);
			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.SUMAR_BLOQUES, tuplaCaso.dato);
			return (CasosDivisibilidad.USAR_NORMAL,-1);
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

		private static (bool cumpleCondicion,int dato) ProductoDePotenciasDeBases(List<long> divisor, List<long> @base, long valorRaiz) {
			if (@base.Count == 0) return (false, -1);
			int maxPotencia = 0;
			for (int i = 0; i < Min(divisor.Count,@base.Count); i++) { //Comprueba que todos los valores de divisor estén en @base
				//Si en divisor es mayor que cero, pero en @base es cero, tiene primero componente primo que no esta en @base y no se le puede aplicar la regla
				if (divisor[i] != 0 && @base[i] == 0) return (false, -1);
				maxPotencia = Max(maxPotencia, @base[i] == 0 ? 0 : (int)Ceiling(divisor[i] / (double)@base[i]));
			}
			return (true, maxPotencia);
		}

		private static string CrearMensajeDivisibilidad(string motivo, string condicion, long @base, long divisor) {
			//motivo\nUn entero en base @base será divisible entre divisor si condicion.
			return motivo + '.' + Environment.NewLine + string.Format(TextoCalculos.CalculosCrearMensaje,@base,divisor) + condicion + '.';
		}
	}
}
