using ModosEjecucion;
using System.Numerics;
using static Operaciones.Calculos;
using static ModosEjecucion.Recursos.TextoEjecucion;
using static ModosEjecucionInterno.Recursos.TextoEjecucionInterno;
using Operaciones;

namespace ModosEjecucionInterno {
	/// <summary>
	/// Esta clase contiene la lógica para el modo diálogo
	/// </summary>
	public class ModoDialogo : IModoEjecucion {

		private const string SALIDA_DIALOGO = "/";

		private static bool GestionarOpcionesDialogo(OpcionesDialogo flags) {
			bool saltarPreguntaExtra = flags.ReglasCoeficientes;
			if (flags.Base.HasValue && flags.Divisor.HasValue) {
				if (!SonCoprimos(flags.Base.Value, flags.Divisor.Value)) {
					if (flags.ReglasCoeficientes) throw new ArgumentException(ErrorDivisorCoprimo, nameof(flags.Divisor));
					flags.ReglasCoeficientes = false; // No se pueden usar las de coeficientes, como se cambia después de evaluar para sinFlags, se sigue preguntando
					saltarPreguntaExtra = true;
				} else {
					ArgumentOutOfRangeException.ThrowIfLessThan(flags.Base.Value, 2, nameof(flags.Base));
					ArgumentOutOfRangeException.ThrowIfLessThan(flags.Divisor.Value, 2, nameof(flags.Divisor));
				}
			}
			return saltarPreguntaExtra;
		}

		/// <summary>
		/// Lógica de la aplicación en modo diálogo.
		/// </summary>
		/// <remarks>
		/// Véase la documentación de 
		/// <see cref="CalcDivCLI.Main(string[])"/>
		/// para ver el significado de la salida.
		/// </remarks>
		/// <returns>
		/// Código de la salida de la aplicación.
		/// </returns>
		public Output Ejecutar(TextWriter salida, TextWriter error, IOpciones opciones) {
			OpcionesDialogo flags = (OpcionesDialogo) opciones;
			Console.Error.WriteLine(MensajeInicioDialogo, SALIDA_DIALOGO);
			bool sinFlags = flags.FlagsInactivos, salir = true;
			Output resultadoSalida = new(ExitState.NO_ERROR);
			static bool esSoY(char letra) => letra == 's' | letra == 'S' | letra == 'y' | letra == 'Y';
			try {
				bool saltarPreguntaExtra = GestionarOpcionesDialogo(flags);
				bool saltarPreguntaDividendo = flags.Dividendo!.Any();
				salir = EjecutarDialogo(salida, error, flags, sinFlags, salir, resultadoSalida, saltarPreguntaExtra, saltarPreguntaDividendo);
			}
			// Si decide salir, se saldrá por este catch
			catch (SalidaException) {
				resultadoSalida.Estado = ExitState.VOLUNTARY_EXIT;
				Console.Error.WriteLine(Environment.NewLine + MensajeDialogoInterrumpido);
			}
			// Si ocurre otro error se saldrá por este catch
			catch (Exception e) {
				resultadoSalida.Estado = ExitState.ERROR;
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				Console.Error.WriteLine(DialogoExcepcionInesperada);
			}
			return resultadoSalida;

			static bool EjecutarDialogo(TextWriter salida, TextWriter error,
				OpcionesDialogo flags, bool sinFlags, bool salir, Output resultadoSalida,
				bool saltarPreguntaExtra, bool saltarPreguntaDividendo) {
				do {
					//Si no tiene flags las pedirá durante la ejecución
					if (sinFlags) {

						//Le una tecla de entrada reglasObj lanza excepción si es la _salida
						if (!saltarPreguntaExtra) {
							flags.ReglasCoeficientes = ObtenerDeUsuario(MensajeDialogoExtendido, esSoY);
						}

						flags.JSON = ObtenerDeUsuario(MensajeDialogoJson, esSoY);
					}

					var (mensaje, regla) =
						FlujoDatosRegla(flags.Divisor, flags.Base, flags.Longitud, flags);

					resultadoSalida.EscribirReglaPorConsola(mensaje + Environment.NewLine, regla.Divisor, regla.Base, salida, error);
					resultadoSalida.EscribirMensajes();
					resultadoSalida.Mensajes.Clear();

					if (!flags.SaltarExplicacion) {

						if (!saltarPreguntaDividendo) {
							flags.Dividendo = [ObtenerDeUsuario(MensajeDialogoExplicar)];
							bool parar = string.IsNullOrEmpty(flags.Dividendo.First());

							while (!parar) {
								if (!BigInteger.TryParse(flags.Dividendo.First(), out BigInteger valor)) {
									valor = ObtenerDeUsuario(MensajeDialogoNoValido, MensajeDialogoExplicarFallido);
								}

								resultadoSalida.Mensajes.Add((error, Environment.NewLine, true));
								resultadoSalida.Mensajes.Add((salida, regla.AplicarRegla(valor), false));
								resultadoSalida.Mensajes.Add((error, Environment.NewLine, true));
								resultadoSalida.EscribirMensajes();
								resultadoSalida.Mensajes.Clear();

								flags.Dividendo = [ObtenerDeUsuario(MensajeDialogoExplicar)];
								parar = string.IsNullOrEmpty(flags.Dividendo.First());
							}
						} else { // Se aplican directamente
							List<BigInteger> lista = flags.Dividendo!.Select(BigInteger.Parse).ToList();

							resultadoSalida.Mensajes.Add((error, Environment.NewLine, true));
							resultadoSalida.Mensajes.Add((salida, regla.AplicarVariosDividendos(lista), false));
							resultadoSalida.Mensajes.Add((error, Environment.NewLine, true));
							resultadoSalida.EscribirMensajes();
							resultadoSalida.Mensajes.Clear();
						}
					}

					if (!flags.AnularBucle) {
						salir = !ObtenerDeUsuario(MensajeDialogoRepetir, esSoY);
					}

					resultadoSalida.Estado = ExitState.NO_ERROR;

				} while (!salir);
				return salir;
			}
		}

		/// <summary>
		/// Separa la obtención de los datos de la regla del resto del diálogo para mejorar la lectura
		/// </summary>
		/// <param name="divisorNull"></param>
		/// <param name="baseNull"></param>
		/// <param name="longitudNull"></param>
		/// <param name="flags"></param>
		private static (string Mensaje, IRegla regla) FlujoDatosRegla(
			long? divisorNull, long? baseNull, int? longitudNull, OpcionesDialogo flags) {
			long divisor, @base;
			int longitud = 1;
			// Se carga la base primero, necesaria en todos los casos
			@base = ObtenerValorODefecto(baseNull,
				() => ObtenerDeUsuarioLong(2, ErrorBase, MensajeDialogoBase));

			if (!flags.ReglasCoeficientes) {
				divisor = ObtenerValorODefecto(divisorNull,
					() => ObtenerDeUsuarioLong(2, ErrorDivisor, MensajeDialogoDivisor));

			} else {
				divisor = ObtenerValorODefecto(divisorNull,
					() => ObtenerDeUsuarioCoprimo(2, @base, ErrorDivisorCoprimo, MensajeDialogoDivisor));

				longitud = ObtenerValorODefecto(longitudNull,
					() => ObtenerDeUsuario(0, ErrorCoeficientes, MensajeDialogoCoeficientes));
			}

			return ((IOpcionesGlobales)flags).ObtenerReglas(divisor, @base, longitud, flags.JSON);
		}

		#region ObtenerSHUTTHEFUCKUP

		private static T ObtenerValorODefecto<T>(T? valorDefecto, Func<T> funcionCasoNulo) where T : struct {
			return valorDefecto is null ? funcionCasoNulo() : valorDefecto.Value;
		}

		private static bool ObtenerDeUsuario(string mensaje, Func<char,bool> comparador) {
			Console.Error.Write(mensaje);
			char entrada;
			if (Console.IsInputRedirected) {
				entrada = (char)Console.In.Read();
			} else {
				entrada = Console.ReadKey().KeyChar; //Necesario usar la consola
			}
			LanzarExcepcionSiSalida(entrada.ToString());
			Console.Error.WriteLine();
			return comparador(entrada);
		}

		private static long ObtenerDeUsuarioLong(long minimo, string mensajeError, string mensajePregunta) {
			long dato;
			Console.Error.Write(mensajePregunta);
			string? linea = Console.In.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo) {
				LanzarExcepcionSiSalida(linea);
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.In.ReadLine();
			}
			Console.Error.WriteLine();
			return dato;
		}

		private static BigInteger ObtenerDeUsuario(string mensajeError, string mensajePregunta) {
			BigInteger dato;
			Console.Error.Write(mensajePregunta);
			string? linea = Console.In.ReadLine();
			while (!BigInteger.TryParse(linea, out dato)) {
				LanzarExcepcionSiSalida(linea);
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.In.ReadLine();
			}
			Console.Error.WriteLine();
			return dato;
		}

		private static string ObtenerDeUsuario(string mensajePregunta) {
			Console.Error.Write(mensajePregunta);
			string? linea = Console.In.ReadLine();
			LanzarExcepcionSiSalida(linea);
			while (linea is null) {
				Console.Error.WriteLine(Environment.NewLine + ObjetoNuloMensaje);
				Console.Error.Write(mensajePregunta);
				linea = Console.In.ReadLine();
			}
			Console.Error.WriteLine();
			return linea;
		}

		private static int ObtenerDeUsuario(long minimo, string mensajeError, string mensajePregunta) {
			int dato;
			Console.Error.Write(mensajePregunta);
			string? linea = Console.In.ReadLine();
			while (!int.TryParse(linea, out dato) || dato < minimo) {
				LanzarExcepcionSiSalida(linea);
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.In.ReadLine();
			}
			Console.Error.WriteLine();
			return dato;	
		}

		private static long ObtenerDeUsuarioCoprimo(long minimo, long coprimo, string mensajeError, string mensajePregunta) {
			long dato;
			Console.Error.Write(mensajePregunta);
			string? linea = Console.In.ReadLine();
			while (!long.TryParse(linea, out dato) || dato < minimo || !SonCoprimos(dato, coprimo)) {
				LanzarExcepcionSiSalida(linea);
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.In.ReadLine();
			}
			Console.Error.WriteLine();
			return dato;
		}

		private static void LanzarExcepcionSiSalida(string? linea) {
			if (linea == SALIDA_DIALOGO) throw new SalidaException(MensajeSalidaVoluntaria);
		}

		public (ExitState, IEnumerable<IRegla>) CalcularRegla(IOpciones opciones) {
			throw new NotImplementedException();
		}

		#endregion

	}
}
