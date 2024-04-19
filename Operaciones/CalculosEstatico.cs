using Listas;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Operaciones
{
	public static class CalculosEstatico {

		private static int _longitudPrimos = 0;
		private static readonly IListaDinamica<long> _primosCalculados = new ListSerie<long>();

		/// <summary>
		/// Esta propiedad devuelve un clon de la lista de primos más larga que ha sido calculada para no tener que calcular todos los primos siempre
		/// </summary>
		/// <remarks>
		/// Asegúrese de que PrimosHasta ha sido llamado con argumento mayor o igual al número que espera
		/// </remarks>
		public static IListaDinamica<long> PrimosCalculados => _primosCalculados.ClonarDinamica();
		public static bool EsPrimo(long raiz) {
			if (raiz <= 1) return false; //Por definición
			if (raiz == 2) return true; //Porque si no, no va
			long cont = 2;
			double raizT = ((long)Math.Sqrt(raiz)) + 1;
			while (cont < raizT && raiz % cont != 0) {
				cont++;
			}
			bool primo = raiz % cont != 0;
			return primo;
		}

		/**
		 * Calcula el factorial de raiz
		 * @return raiz!
		 */
		public static long Factorial(long raiz) {
			ArgumentOutOfRangeException.ThrowIfNegative(raiz);
			long res = raiz;
			while (raiz > 1) {
				res *= raiz;
				raiz--;
			}
			return res;
		}

		/**
		 * Devuelve el máximo común divisor de raiz y segundo
		 * <p></p>
		 * Se usa el algoritmo de Euclidess
		 * @return mcd(raiz, segundo)
		 */
		public static long Mcd(long raiz, long segundo)
		{
			if (raiz < segundo) { //Necesitamos que raiz sea mayor o igual que segundo
				(segundo, raiz) = (raiz, segundo);
			}
			while (segundo != 0) {
				long aux = raiz;
				raiz = segundo;
				segundo = aux % segundo;
			}
			return raiz;
		}

		/**
		 * Devuelve una serie que contiene la descomposición en números primos de {@code num}
		 * <p>Los elementos de la serie son los exponentes de los números primos en orden ascendente</p>
		 */
		public static IListaDinamica<long> DescompsicionEnPrimos(long num) {
			IListaDinamica<long> primos = PrimosHasta(num/2);
			ListSerie<long> res = new(primos.Longitud) {
				FuncionDeGeneracion = num => 0L,
				Longitud = primos.Longitud
			};
			Calculos.Descomposicion(primos, res, num);
			while (res.UltimoElemento == 0L) {
				res.BorrarUltimo();
			}
			return res;
		}

		/**
		 * Calcula el mínimo común múltiplo de {@code primero} y {@code segundo}
		 * @return mcm(primero,segundo)
		 */
		public static long Mcm(long primero, long segundo) {
			if (primero < 0 || segundo < 0) throw new ArgumentException("Los argumentos deben ser no negativos");
			if (primero == 0 || segundo == 0) return 0;
			return primero * segundo / Mcd(primero, segundo);
		}

		/**
		 * Devuelve una serie con los números primos hasta num incluido
		 * <p>La serie tendrá nombre nulo</p>
		 */
		public static IListaDinamica<long> PrimosHasta(long num) {
			long cont;
			ListSerie<long> serie = new();
			if (_longitudPrimos == 0) { //Si no se ha calculado ninguno hace los cálculos completos
				if (num >= 2) {
					serie.InsertarUltimo(2L);
				}
				if (num <= 2) {
					return serie;
				}
				cont = 3;
				_primosCalculados.Insertar(2L); //Es más complicado si se introduce en el primer if
				_longitudPrimos = 1;
			} else {
				for (int i = 0; _primosCalculados[i] <= num && i < _primosCalculados.Longitud; i++) { //Se insertan todos los que se necesiten para no tener que buscarlos otra vez
					serie.InsertarUltimo(_primosCalculados[i]);
				}
				cont = _primosCalculados.UltimoElemento + 2; //Se le suma 2 para evitar que se inserte otra vez en el while
			}

			while (cont <= num) {
				if (EsPrimo(cont)) {
					serie.InsertarUltimo(cont);
					_primosCalculados.InsertarUltimo(cont);
				}
				cont += 2;
			}
			
			return serie;
		}

		/**
		 * Calcula el inverso multiplicativo de a en módulo m
		 * <p></p>
		 * Copiado del artículo de <a href="https://es.wikipedia.org/wiki/Inverso_multiplicativo">
		 *     Wikipedia</a><p></p>
		 * Devuelve 0 si no hay inverso, es decir, mcd(a,m) &ne; 1
		 * El resultado nunca es negativo
		 * @param a número cuyo inverso queremos calcular
		 * @param m módulo que se usa para el cálculo
		 * @return a^-1, talque a*a^-1 &equiv; 1 (mód m)
		 */
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
		 * Calcula la progresión aritmética que empieza en inicio, acaba en fin y tiene num números
		 * @param inicio primer número del sumatorio
		 * @param fin último número del sumatorio
		 * @param num número de elementos de la progresión
		 * @return valor de la progresión aritmética
		 */
		public static long SumatorioIntervalo(long inicio, long fin, long num) {
			return (inicio + fin) * num / 2;
		}

		/**
		 * Devuelve el mínimo de los absolutos entre {@code un} y {@code dos}
		 * @return min{|{@code un}|,|{@code dos}|}
		 */
		public static long MinAbs(long un, long dos) {
			if (Math.Min(Math.Abs(un), Math.Abs(dos)) == Math.Abs(un)) {
				return un;
			}
			return dos;
		}

		/**
		 * Calcula el número de cifras de {@code num} con la raiz indicada
		 * @param raiz raiz del número
		 * @return número de cifras de {@code num} en raiz {@code raiz}
		 */
		public static short Cifras(long num, long raiz) {
			if (num == 0) return 1;
			return (short)Math.Ceiling(Math.Log(Math.Abs(num) + 1L) / Math.Log(raiz));
		}

		/**
		 * Calcula el número de cifras de parte entera de {@code num} con la raiz indicada
		 * @param raiz raiz del número
		 * @return número de cifras de {@code num} en raiz {@code raiz}
		 */
		public static short Cifras(double num, double raiz) {
			if (num == 0) return 1;
			return (short)Math.Ceiling(Math.Log(Math.BitIncrement(Math.Abs(num))) / Math.Log(raiz));
		}

		/**
		 * Calcula el número de cifras de {@code num} en raiz diez
		 * <p>Equivalente a {@code cifras(num,10)}</p>
		 * @return número de cifras de {@code num} en raiz 10
		 */
		public static short Cifras10(long num) {
			return Cifras(num, 10);
		}

		/**
		 * Calcula la cifra en la posición {@code pos} de {@code num} en la raiz {@code raiz}
		 * <p>La cifra 0 es la menos significativa</p>
		 * La posición debe estar dentro del número
		 * @param pos cifra que calcular
		 * @param raiz raiz del número
		 * @return cifra {@code pos} de {@code num} en raiz {@code raiz}
		 */
		public static byte Cifra(long num, long pos, long raiz) {
			if (pos >= Cifras(num, raiz) || pos < 0) throw new ArgumentException("La posición debe ser una cifra del número");
			return (byte)(num / (long)Math.Pow(raiz, pos) % raiz);
		}

		/**
		 * Devuelve un objeto {@code String} que contiene {@code num} escrito con subíndices de sus cifras en raiz 10
		 * @return {@code num} en subíndices
		 */
		public static String NumASubindice(long num) {
			StringBuilder res = new();
			short cifras = Cifras(num, 10);
			for (int i = cifras - 1; i >= 0; i--) {
				res.Append(CifraASubindice(Cifra(num, i, 10)));
			}
			return res.ToString();
		}

		public static char CifraASubindice(long num) { return (char)(num + 0x2080); }

		/**
		 * Calcula {@code num}^{@code exp} en módulo {@code raiz} multiplicando {@code num} por sí mismo y haciendo el módulo en cada paso
		 * <p>Intenta evitar el overflow que se produciría al hacer la potencia antes del módulo,
		 * pero esto no se garantiza</p>
		 * @param num raiz de la potencia
		 * @param exp exponente
		 * @param raiz raiz del módulo
		 * @return {@code num}^{@code exp} en módulo {@code raiz}
		 */
		public static long PotenciaMod(long num, long exp, long raiz) {
			long res = 1;
			num %= raiz;
			while (exp > 0) {
				res = ProductoMod(res, num, raiz);
				exp--;
			}
			return res;
		}

		public static long ProductoMod(long fac1, long fac2, long raiz) {
			return fac1 * fac2 % raiz;
		}

		/**
		 * Devuelve todas las reglas de divisibilidad de {@code num} en raiz {@code raiz} con {@code cantidad} coeficientes cuyo valor absoluto no supera raiz
		 * <p>reglas se borra antes de calcular las reglas</p>
		 * @param reglas serie donde guardar las series que guardan las reglas
		 * @param cantidad número de coeficientes de la regla
		 * @param raiz raiz que se usa para crear la regla
		 */
		public static void ReglasDivisibilidad(ISerie<ISerie<long>> reglas, long num, int cantidad, long raiz) {
			reglas.BorrarTodos(); //Se borra la serie
			String nombre = reglas.Nombre;
			reglas.InsertarUltimo(new ListSerie<long>(nombre, cantidad));
			ReglaDivisibilidadBase(reglas[0], num, cantidad, raiz); //Se calcula la regla de positivos
			ISerie<long> aux, atajo = reglas[0]; //aux guarda la serie que se añadirá y atajo es un atajo
			bool[] producto = new bool[cantidad]; //guarda si el elemento i de atajo se le resta num
			OperacionesSeries.IncrementarArray(producto); //Como el primero ya está se incrementa
			do {
				aux = new ListSerie<long>(nombre, cantidad);
				for (int i = 0; i < atajo.Longitud; i++) {
					aux.InsertarUltimo(atajo[i] - (producto[i] ? num : 0)); //Se añade en la serie de regla
				}
				reglas.InsertarUltimo(aux);
				OperacionesSeries.IncrementarArray(producto); //Itera el bucle
			} while (!OperacionesSeries.ArrayFalso(producto));
		}
		/**
		 * Calcula la regla de divisibilidad de {@code num} en raiz {@code raiz} con {@code cantidad} coeficiente
		 * tal que todos los coeficientes tienen el menor valor absoluto y la guarda en serie
		 * <p>{@code serie} se borra antes de crear la regla</p>
		 * @param serie serie donde guardar la regla
		 * @param cantidad número de coeficientes de la regla
		 * @param raiz raiz que se usa para crear la regla
		 */
		public static void ReglaDivisibilidadOptima(IListaDinamica<long> serie, long num, int cantidad, long raiz) {
			ReglaDivisibilidadBase(serie, num, cantidad, raiz);
			for (int i = 0; i < serie.Longitud; i++) {
				serie[i] = MinAbs(serie[i], serie[i] - num);
			}
		}

		/**
		 * Calcula una regla de divisibilidad de {@code num} en raiz {@code raiz} con {@code cantidad} coeficientes
		 * y la guarda en serie
		 * <p>{@code serie} se borra antes de crear la regla</p>
		 * Los coeficientes son positivos siempre
		 * @param serie serie donde guardar la regla
		 * @param cantidad número de coeficientes de la regla
		 * @param raiz raiz que se usa para crear la regla
		 */
		public static void ReglaDivisibilidadBase(IListaDinamica<long> serie, long num, int cantidad, long raiz) {
			if (num < 0 || raiz < 0 || cantidad < 0) throw new ArgumentException("Los argumentos no pueden ser negativos");
			long inv = InversoMod(raiz, num);
			if (inv == 0) throw new ArithmeticException("num debe ser coprimo con raiz");
			serie.Vacia = true;
			OperacionesSeries.PotenciaModProgresiva(serie, inv, num, cantidad, 0);
		}

		public static (bool,string) ReglaDivisibilidadExtendida(long divisor, long raiz) {
			var caso = CasoEspecialRegla(divisor, raiz);
			string mensaje = caso.caso switch {
				CasosDivisibilidad.MIRAR_CIFRAS => CrearMensajeDivisibilidad($"{divisor} está compuesto de potencias de divisores de {raiz}",
					$"sus primeras {caso.informacion} cifras son múltiplo de {divisor}",
					raiz, divisor),
				CasosDivisibilidad.UNO => "Todos los enteros son divisibles entre uno.",
				CasosDivisibilidad.RESTAR_BLOQUES => CrearMensajeDivisibilidad($"{divisor} es {raiz} elevado a {caso.informacion}",
					$"al separar sus cifras en grupos de {caso.informacion} desde las unidades, la diferencia de la suma de los grupos pares" +
					$" y la de los grupos impares es múltiplo de {divisor}",
					raiz,divisor),
				CasosDivisibilidad.SUMAR_BLOQUES => CrearMensajeDivisibilidad($"{divisor} es {raiz} elevado a {caso.informacion}",
					$"al separar sus cifras en grupos de {caso.informacion} desde las unidades, la suma de los grupos es múltiplo de {divisor}",
					raiz, divisor),
				CasosDivisibilidad.CERO => "No se le puede aplicar la relación de divisibilidad a cero.",
				_ => "No se ha encontrado ninguna regla alternativa, aplique la regla calculada.",
			};
			return (caso.caso != CasosDivisibilidad.USAR_NORMAL, mensaje);
		}

		[return: NotNull]
		private static (CasosDivisibilidad caso,int informacion) CasoEspecialRegla(long divisor, long raiz) {
			if (divisor == 0) return (CasosDivisibilidad.CERO, -1);
			if (divisor == 1) return (CasosDivisibilidad.UNO, -1);

			IListaDinamica<long> descomposicionDivisor = DescompsicionEnPrimos(divisor),
				descomposicionRaiz = DescompsicionEnPrimos(raiz);
			var tuplaCaso = ProductoDePotenciasDeBases(descomposicionDivisor, descomposicionRaiz, raiz);

			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.MIRAR_CIFRAS,tuplaCaso.dato);
			tuplaCaso = UnoMasQuePotencia(divisor, raiz);
			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.RESTAR_BLOQUES, tuplaCaso.dato);
			tuplaCaso = UnoMenosQuePotencia(divisor, raiz);
			if (tuplaCaso.cumpleCondicion) return (CasosDivisibilidad.SUMAR_BLOQUES, tuplaCaso.dato);
			return (CasosDivisibilidad.USAR_NORMAL,-1);
		}

		/// <summary>
		/// Comprueba si divisor es una potencia de raiz menos uno, o una de los divisores de raiz menos uno, lo que permite usar una regla distinta
		/// </summary>
		/// <returns></returns>
		private static (bool, int) UnoMenosQuePotencia(long divisor, long raiz) {
			if (Mcd(divisor, raiz) > 1) return (false, -1);
			long raizModulo = raiz % divisor, raizInicial = raizModulo;
			int potencia = 1;
			bool restoUnoRaiz = raiz % divisor == 1;
			do {
				raizModulo *= raizModulo;
				raizModulo %= divisor; //Para evitar overflow se calcula el modulo divisor
				restoUnoRaiz |= raizModulo == 1;
				potencia++;
			} while (!restoUnoRaiz && raizModulo != raizInicial);
			return (restoUnoRaiz, potencia);
		}

		/// <summary>
		/// Comprueba si divisor es una potencia de raiz más uno, o uno de sus divisores, lo que permite usar una regla distinta
		/// </summary>
		/// <returns></returns>
		private static (bool,int) UnoMasQuePotencia(long divisor, long raiz) {
			if (Mcd(divisor, raiz) > 1) return (false, -1);
			long raizModulo = raiz % divisor, raizInicial = raizModulo;
			int potencia = 1;
			bool restoUnoRaiz = divisor % raiz == 1;
			do {
				raizModulo *= raizModulo;
				raizModulo %= divisor; //Para evitar overflow se calcula el modulo divisor
				restoUnoRaiz |= raizModulo + 1 == divisor;
				potencia++;
			} while (!restoUnoRaiz && raizModulo != raizInicial);
			return (restoUnoRaiz, potencia);
		}

		private static (bool cumpleCondicion,int dato) ProductoDePotenciasDeBases(ILista<long> divisor, ILista<long> raiz, long valorRaiz) {
			int maxPotencia = 0;
			for (int i = 0; i < Math.Min(divisor.Longitud,raiz.Longitud); i++) { //Comprueba que todos los valores de divisor estén en raiz
				//Si en divisor es mayor que cero, pero en raiz es cero, tiene un componente primo que no esta en raiz y no se le puede aplicar la regla
				if (divisor[i] == 0 || raiz[i] != 0) return (false, -1);
				maxPotencia = Math.Max(maxPotencia, raiz[i] == 0 ? 0 : (int)Math.Ceiling(divisor[i] / (double)raiz[i]));
			}
			return (true, maxPotencia);
		}

		private static string CrearMensajeDivisibilidad(string motivo, string condicion, long raiz, long divisor) {
			return $"{motivo}.\nUn número en base {raiz} será múltiplo de {divisor} si {condicion}.";
		}

		public static string ToStringCompleto<T>(this ISerie<T> lista) {
			if (lista.Longitud == 0) return "Serie vacía";
			StringBuilder sb = new();
			for (int i = 0; i < lista.Longitud; i++) {
				sb.Append(lista.Nombre).Append(NumASubindice(i)).Append('=').Append(lista[i]);
				if (i + 1 != lista.Longitud) {
					sb.Append(", ");
				}
			}
			return sb.ToString();
		}

		public static string ToStringCompletoInverso<T>(this ISerie<T> lista) {
			if (lista.Longitud == 0) return "Serie vacía";
			StringBuilder sb = new();
			for (int i = lista.Longitud - 1; i >= 0; i--) {
				sb.Append(lista.Nombre).Append(NumASubindice(i)).Append('=').Append(lista[i]);
				if (i > 0) {
					sb.Append(", ");
				}
			}
			return sb.ToString();
		}
	}
}
