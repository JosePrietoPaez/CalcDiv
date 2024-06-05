using System;
using System.Collections;
using System.IO;
using CommandLine;
using System.Text;
using Listas;
using Operaciones;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static class CalculadoraDivisibilidadCLI {
		
		//Inicialización de variables privadas
		private const int SALIDA_CORRECTA = 0, SALIDA_ERROR = 1, SALIDA_VOLUNTARIA = 2, SALIDA_FRACASO_EXPANDIDA = 3;
		private const string ERROR_NUEMRICO = "El divisor, base y el número de coeficientes deben ser números enteros positivos\r\ndivisor y base deben ser mayor que 1"
			,SALIDA = "FIN", ERROR_PRIMO = "La base y el divisor deben ser coprimos";
		private static bool salir = false; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int salida = SALIDA_CORRECTA; //Salida del programa
		private static Flags? flags;

		/// <summary>
		/// Este método calcula la regla de divisibilidad, obteniendo los datos desde la consola o desde los argumentos.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>
		/// <list type="bullet">
		/// <item>0, ejecución correcta</item>
		/// <item>1, argumentos incorrectos en modo directo</item>
		/// <item>2, el usuario sale voluntariamente durante el diálogo</item>
		/// <item>3, fracaso al encontrar una regla alternativa en modo directo</item>
		/// </list>
		/// </returns>
		public static int Main(string[] args) {
			var resultado = Parser.Default.ParseArguments<Flags>(args) //Parsea los argumentos
				.WithParsed(options => { flags = options; Console.Error.WriteLine(options.Dump()); })
				.WithNotParsed(errors => { salida = SALIDA_ERROR; Console.Error.WriteLine(errors); });
			if (salida == SALIDA_ERROR) {
				return salida;
			}
			return salida;
		}

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// booleano que indica si ha conseguido calcular la regla.
		/// </returns>
		private static bool IntentarDirecto() { //Intenta dar las reglas de forma directa, devuelve true si lo consigue y false si hay algún error
			return true;
		}

		/// <summary>
		/// Lógica de la aplicación en modo diálogo.
		/// </summary>
		/// <remarks>
		/// Véase la documentación de 
		/// <see cref="Main(string[])"/>
		/// para ver el significado de la salida.
		/// </remarks>
		/// <returns>
		/// Código de la salida de la aplicación.
		/// </returns>
		private static int IniciarAplicacion() { //Si no se proporcionan los argumentos de forma directa, se establece un diálogo con el usuario para obtenerlos
			Console.WriteLine($"El programa se ejecutará de forma normal, escriba {SALIDA} para cerrarlo");
			try {
			} catch {
				salir = true;
				Console.WriteLine("Se ha interrumpido el programa.");
			}			
			return salir? SALIDA_VOLUNTARIA : 0;
		}

		/// <summary>
		/// Calcula las reglas con los argumentos proporcionados y el usuario lo ha pedido
		/// </summary>
		private static string StringReglasConNombre(long divisor, long @base, int coeficientes, int indice) {
			if (flags.Nombre is not null) {
				return ObtenerReglas(divisor, @base, coeficientes, flags.Nombre);
			} else {
				return ObtenerReglas(divisor, @base, coeficientes);
			}
		}

		/// <summary>
		/// Devuelve la posición del primer null de un array, si no hay se devuelve la longitud
		/// </summary>
		private static int PrimerNullOFin(object[] arr) {
			int i = 0;
			while (i < arr.Length && arr[i] != null) i++;
			return i;
		}

		private static void ObtenerDeUsuario(out long dato, long minimo, string mensaje) {
			string? linea = Console.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo) {
				Console.WriteLine(mensaje);
				linea = Console.ReadLine();
			}
			if (linea == SALIDA) throw new Exception("Se ha detenido el programa");
		}

		private static void ObtenerDeUsuario(out int dato, long minimo, string mensaje) {
			string? linea = Console.ReadLine();
			while (!int.TryParse(linea, out dato) || dato < minimo) {
				Console.WriteLine(mensaje);
				linea = Console.ReadLine();
				if (linea == SALIDA) throw new Exception("Se ha detenido el programa");
			}
		}

		private static void ObtenerDeUsuarioCoprimo(out long dato, long minimo, string mensaje, long coprimo) {
			string? linea = Console.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo || CalculosEstatico.Mcd(dato,coprimo) > 1) {
				Console.WriteLine(mensaje);
				linea = Console.ReadLine();
				if (linea == SALIDA) throw new Exception("Se ha detenido el programa");
			}
		}

		private static void ObtenerDeUsuario(out bool dato) {
			string? linea = Console.ReadLine();
			while (linea is null) {
				linea = Console.ReadLine();
			}
			if (linea == SALIDA) throw new Exception("Se ha detenido el programa");
			dato = linea.ToLower().StartsWith('s');
		}

		private static string ObtenerReglas(long divisor, long @base, int coeficientes, string nombre = "") {
			string resultado;
			if (flags.Todos) { //Si se piden las 2^coeficientes reglas
				ListSerie<ListSerie<long>> series = new();
				CalculosEstatico.ReglasDivisibilidad(series, divisor, coeficientes,@base);
				if (nombre != "") {
					foreach (var serie in series) {
						serie.Nombre = nombre;
					}
				}
				resultado = SerieRectangularString(series);
			} else {
				ListSerie<long> serie = new(nombre);
				CalculosEstatico.ReglaDivisibilidadOptima(serie, divisor, coeficientes, @base);
				resultado = StringSerieConFlags(serie);
			}
			return resultado;
		}

		private static void EscribirArchivo(string ruta) {
			string texto = File.ReadAllText(ruta);
			Console.WriteLine(texto);
		}

		//Copia los elementos entre inicioCopia y finCopia de original a los primeros elementos de copia
		private static void CopiarArrayParcial(string[] original, string[] copia, int inicioCopia, int finCopia) {
			int indice = 0;
			while (indice < copia.Length && inicioCopia + indice <= finCopia && indice + inicioCopia < original.Length) {
				copia[indice] = original[inicioCopia + indice++];
			}
		}

		/// <summary>
		/// Busca el índice donde comienzan los argumentos.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>
		/// Índice donde están los argumentos
		/// </returns>
		private static int InicioArgs(string[] args) {
			int indice = 0;
			while (args.Length > indice && !args[indice].StartsWith('-')) {
				indice++;
			}
			if (indice == args.Length) {
				indice = -1;
			}
			return indice;
		}

		private static string SerieRectangularString(ListSerie<ListSerie<long>> serie) {
			StringBuilder stringBuilder = new();
			foreach (var item in serie) {
				stringBuilder.Append(StringSerieConFlags(item)).Append('\n');
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Devuelve el ToString adecuado de <c>serie</c>.
		/// </summary>
		/// <param name="serie"></param>
		/// <returns>
		/// String que representa a <c>serie</c> dependiendo de los flags.
		/// </returns>
		private static string StringSerieConFlags(ISerie<long> serie) {
			if (flags.Inverso) {
				return flags.Nombre is not null ? serie.ToStringCompletoInverso() : serie.ToStringInverso();
			}
			return flags.Nombre is not null ? serie.ToStringCompleto() : serie.ToString()??"";
		}
	}
}
