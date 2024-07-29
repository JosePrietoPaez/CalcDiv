namespace Operaciones
{
	public static class OperacionesListas {

		/// <summary>
		/// Escribe, a partir de la posición <c>0</c>, las potencias de <c>base</c> en aumento desde <c>0</c> hasta <c>fin</c>.
		/// </summary>
		/// <remarks>
		/// Ambos <c>0</c> y <c>fin</c> están incluidos.
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>0</c> y no puede ser menor que 0.
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <param name="longitud">exponente de la última potencia</param>
		/// <param name="mod">modulo que aplicar a la potencia</param>
		/// <param name="base">la base de la potencia</param>
		/// <returns>
		/// Lista con los coeficientes de las potencias
		/// </returns>
		public static List<long> PotenciaModProgresiva(long @base, long mod, int longitud) {
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mod, nameof(mod));
			ArgumentOutOfRangeException.ThrowIfNegative(longitud, nameof(longitud));
			List<long> resultado = new(longitud);
			long num = 1;
			for (int i = 0; i < longitud; i++) {
				num = Calculos.ProductoMod(num, @base, mod);
				resultado.Insert(i, num);
			}
			return resultado;
		}

		/// <summary>
		/// Indica si <c>arr</c> solo tiene valores <c>false</c>.
		/// </summary>
		/// <returns>
		/// <c>true</c> si <c>arr</c> no tiene ningún valor a <c>true</c>.
		/// </returns>
		public static bool ArrayFalso(bool[] arr) {
			bool hayTrue = false;
			for (int i = 0; i < arr.Length && !hayTrue; i++) hayTrue |= arr[i];
			return !hayTrue;
		}

		/// <summary>
		/// Incrementa el valor de <c>arr</c> en 1 de la misma forma que con un número binario.
		/// </summary>
		/// <remarks>
		/// Puede producir overflow, haciendo que todos los valores se cambien a <c>false</c>.
		/// </remarks>
		public static void IncrementarArray(bool[] arr) {
			int i = 0;
			bool seguir = true;
			while (i < arr.Length && seguir) {
				seguir = arr[i];
				arr[i] = !arr[i];
				if (!arr[i]) i++;
			}
		}
	}
}
