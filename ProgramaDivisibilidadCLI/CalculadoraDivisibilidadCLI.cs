using System;
using System.Collections;
using System.IO;
using CommandLine;
using System.Text;
using Listas;
using Operaciones;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProgramaDivisibilidad {

	/// <summary>
	/// Esta clase contiene el método principal de la aplicación
	/// </summary>
	public static class CalculadoraDivisibilidadCLI {
		
		//Inicialización de variables privadas
		private const int SALIDA_CORRECTA = 0, SALIDA_ERROR = 1, SALIDA_ENTRADA_MALFORMADA = 2, SALIDA_VOLUNTARIA = 3, SALIDA_FRACASO_EXPANDIDA = 4;
		private const string ERROR_NUEMRICO = "El divisor, base y el número de coeficientes deben ser números enteros positivos\r\ndivisor y base deben ser mayor que 1"
			,SALIDA = "FIN", ERROR_PRIMO = "La base y el divisor deben ser coprimos";
		private static bool salir = false; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int salida = SALIDA_CORRECTA; //Salida del programa
		private static Flags? flags;
		private static readonly JsonSerializerOptions opcionesJson = new() { WriteIndented=true };

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
				.WithParsed(options => { flags = options;
					//Console.Error.WriteLine(options.Dump());
				})
				.WithNotParsed(errors => { salida = SALIDA_ENTRADA_MALFORMADA;
					//Console.Error.WriteLine(errors);
				});
			if (salida == SALIDA_ENTRADA_MALFORMADA || flags is null) {
				return SALIDA_ENTRADA_MALFORMADA;
			}
			if (flags.Ayuda) {
				EscribirArchivo("Ayuda.txt");
			}
			if (flags.Directo.Count() > 1) { //Si se ha activado el modo directo
				IntentarDirecto();
			} else {
				IniciarAplicacion();
			}
			return salida;
		}

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// booleano que indica si ha conseguido calcular la regla.
		/// </returns>
		private static void IntentarDirecto() { //Intenta dar las reglas de forma directa, cambia salida para mostrar el error
			List<long> datos = flags.Directo.ToList();
			if (datos.Count == 2) datos.Add(1);
			(bool exitoExtendido, string mensajeRegla, int informacion) reglas = (false,"",-1);
			if (flags.Extendido) {
				reglas = Calculos.ReglaDivisibilidadExtendida(datos[0], datos[1]);
				Console.WriteLine(reglas.mensajeRegla);
			}
			if (!reglas.exitoExtendido && Calculos.Mcd(datos[0], datos[1]) == 1) { //Si falla al obtener reglas alternativas o no se ha buscado, y base y divisor son coprimos
				Console.Error.WriteLine($"Divisor: {datos[0]}, Base: {datos[1]}, Coeficientes: {datos[2]}");
				CalcularReglaCoeficientes(datos[0], datos[1], (int)datos[2]);
			} else if (Calculos.Mcd(datos[0], datos[1]) != 1) { //Si el divisor y la base no son coprimos
				Console.Error.WriteLine($"Divisor: {datos[0]}, Base: {datos[1]}, Coeficientes: {datos[2]}");
				Console.Error.WriteLine(ERROR_PRIMO);
				salida = SALIDA_ERROR;
			} else { //Si se ha obtenido una regla alternativa
				salida = SALIDA_CORRECTA;
			}
		}

		/// <summary>
		/// Calcula las reglas de coeficientes especificadas en flags con los argumentos
		/// </summary>
		/// <remarks>
		/// Escribe por consola lo que sea necesario
		/// </remarks>
		private static void CalcularReglaCoeficientes(long divisor, long @base, int coeficientes) {
			string salidaConsola = "";
			if (flags.Todos) {
				ListSerie<ListSerie<long>> reglaSerie = new(Calculos.PotenciaEntera(2, coeficientes)) {
					Nombre = flags.Nombre
				};
				Calculos.ReglasDivisibilidad(reglaSerie, divisor, coeficientes, @base);
				Console.Error.WriteLine("Se han calculado las reglas.");
				foreach (var regla in reglaSerie) {
					regla.Nombre = flags.Nombre;
				}
				if (flags.JSON) {
					salidaConsola = Serializar(reglaSerie);
				} else {
					salidaConsola = SerieRectangularString(reglaSerie);
				}
			} else {
				ListSerie<long> regla = new(flags.Nombre);
				Calculos.ReglaDivisibilidadOptima(regla,divisor,coeficientes,@base);
				Console.Error.WriteLine("Se ha calculado la regla.");
				salidaConsola = StringSerieConFlags(regla);
			}
			salida = SALIDA_CORRECTA;
			Console.Out.WriteLine(salidaConsola);
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
		private static void IniciarAplicacion() { //Si no se proporcionan los argumentos de forma directa, se establece un diálogo con el usuario para obtenerlos
			Console.WriteLine($"Se pedirá que introduzca los datos por consola, escriba {SALIDA} para cerrar el programa");
			bool sinFlags = SinFlags();
			long @base, divisor;
			int coeficientes;
			char eleccionExtendido = 'c',eleccionJson = 's',eleccionTodos = 's';
			string nombre = "";
			static bool esC(char letra) => letra == 'c' || letra == 'C';
			static bool esS(char letra) => letra == 's' || letra == 'S';
			try {
				do {

					if (sinFlags) {
						Console.Error.Write("¿Quiere obtener reglas de coeficientes, o reglas de otros tipos? Pulse c para obtener reglas de coeficientes u otra letra para otras letras: ");
						eleccionExtendido = Console.ReadKey().KeyChar;
						Console.Error.WriteLine();

						if (esC(eleccionExtendido)) {
							Console.Error.Write("¿Quiere que la lista se escriba en JSON? En caso afirmativo pulse s, si no, pulse otra letra: ");
							eleccionJson = Console.ReadKey().KeyChar;
							Console.Error.WriteLine();
						}
						
					}

					if (!esC(eleccionExtendido)) {

						Console.Error.Write("Escriba la base de los números: ");
						ObtenerDeUsuario(out @base, 2, "La base debe ser un entero mayor que 1");

						Console.Error.Write("Escriba el divisor de la regla: ");
						ObtenerDeUsuario(out divisor, 0, "El divisor debe ser un entero no negativo" + Environment.NewLine +
							"Las reglas de divisibilidad de números negativos son equivalentes a la de los positivos");

						Console.WriteLine(Calculos.ReglaDivisibilidadExtendida(divisor,@base).Item2);

					} else {

						Console.Error.Write("Escriba la base de los números: ");
						ObtenerDeUsuario(out @base, 2, "La base debe ser un entero mayor que 1");

						Console.Error.Write("Escriba el divisor de la regla: ");
						ObtenerDeUsuarioCoprimo(out divisor, 0, "El divisor debe ser un entero no negativo y coprimo con la base" + Environment.NewLine +
							"Las reglas de divisibilidad de números negativos son equivalentes a la de los positivos" + Environment.NewLine +
							"Dos números son coprimos entre sí si su máximo común divisor es 1", @base);

						Console.Error.Write("Escriba el número de coeficientes de la regla: ");
						ObtenerDeUsuario(out coeficientes, 1, "Debe ser un entero positivo");

						if (sinFlags) {
							Console.Error.Write("¿Quiere recibir todas las reglas? En caso afirmativo, pulse s, si no, pulse otra letra: ");
							eleccionTodos = Console.ReadKey().KeyChar;
							Console.Error.WriteLine();

							Console.Error.WriteLine("Escriba el nombre de la regla, puede dejarlo vacío: ");
							nombre = Console.ReadLine() ?? "";
							Console.Error.WriteLine();
						}

						if (esS(eleccionTodos)) {
							Console.Error.WriteLine("Reglas obtenidas: ");
							if (esS(eleccionJson)) {
								ListSerie<ListSerie<long>> series = new(nombre);
								Calculos.ReglasDivisibilidad(series, divisor, coeficientes, @base);
								Console.Write(Serializar(series));
							} else {
								Console.Write(ObtenerReglas(divisor, @base, coeficientes, nombre));
							}
						} else {
							Console.Error.WriteLine("Regla obtenida: ");
							ListSerie<long> regla = new(nombre);
							Calculos.ReglaDivisibilidadOptima(regla, divisor, coeficientes, @base);
							Console.Write(StringSerieConFlags(regla,esS(eleccionJson),nombre == ""));
						}
						Console.Error.WriteLine();

					}

					Console.Error.Write("¿Quiere calcular otra regla? En caso afirmativo, pulse s: ");
					salir = !esS(Console.ReadKey().KeyChar);
					Console.Error.WriteLine();

				} while (!salir);
			} catch {
				salir = true;
				Console.WriteLine("Se ha interrumpido el programa.");
			}			
			salida = salir? SALIDA_VOLUNTARIA : SALIDA_CORRECTA;
		}

		private static bool SinFlags() {
			return !(flags.Todos || flags.SaltarPreguntas || flags.JSON || flags.Extendido || flags.Nombre.Length == 0 || flags.Directo.Any());
		}

		/// <summary>
		/// Calcula las reglas con los argumentos proporcionados y el usuario lo ha pedido
		/// </summary>
		private static string StringReglasConNombre(long divisor, long @base, int coeficientes, int indice) {
			if (flags.Nombre == string.Empty) {
				return ObtenerReglas(divisor, @base, coeficientes, flags.Nombre);
			} else {
				return ObtenerReglas(divisor, @base, coeficientes);
			}
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
			while (!long.TryParse(linea, out dato) || dato < minimo || Calculos.Mcd(dato,coprimo) > 1) {
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
				Calculos.ReglasDivisibilidad(series, divisor, coeficientes,@base);
				if (nombre != "") {
					foreach (var serie in series) {
						serie.Nombre = nombre;
					}
				}
				resultado = SerieRectangularString(series);
			} else {
				ListSerie<long> serie = new(nombre);
				Calculos.ReglaDivisibilidadOptima(serie, divisor, coeficientes, @base);
				resultado = StringSerieConFlags(serie);
			}
			return resultado;
		}

		private static void EscribirArchivo(string ruta) {
			string texto = File.ReadAllText(ruta);
			Console.WriteLine(texto);
		}

		private static string SerieRectangularString(ListSerie<ListSerie<long>> serie) {
			if (flags.JSON) {
				return Serializar(serie);
			}
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
		private static string StringSerieConFlags(ListSerie<long> serie) {
			if (flags.JSON) {
				return Serializar(serie);
			}
			return serie.Nombre != string.Empty ? serie.ToStringCompleto() : serie.ToString()??"";
		}
		private static string StringSerieConFlags(ListSerie<long> serie, bool json, bool nombre) {
			if (json) {
				return Serializar(serie);
			}
			return nombre ? serie.ToStringCompleto() : serie.ToString() ?? "";
		}

		private static string Serializar(object o) {
			string res = "{" + Environment.NewLine;
			if (o is ListSerie<long> lista) {
				res += "\"Nombre\" : \"" + lista.Nombre + "\", " + Environment.NewLine + "\"Lista\" : ";
			} else if (o is ListSerie<ListSerie<long>> otra) {
				res += "\"Nombre\" : \"" + otra.Nombre + "\", " + Environment.NewLine + "\"Lista\" : ";
			}
			res += JsonSerializer.Serialize(o, opcionesJson) + Environment.NewLine + "}";
			return res;
		}
	}
}
