using ModosEjecucion;
using System.Numerics;
using static Operaciones.Calculos;
using static ModosEjecucion.Recursos.TextoEjecucion;
using static ModosEjecucionInterno.Recursos.TextoEjecucionInterno;

namespace ModosEjecucionInterno {
	/// <summary>
	/// Esta clase contiene la lógica para el modo diálogo
	/// </summary>
	public class ModoDialogo : IModoEjecucion {

		private const string SALIDA_DIALOGO = "/";

		private static bool GestionarOpcionesDialogo(OpcionesDialogo flags) {
			bool saltarPreguntaExtra = flags.TipoExtra;
			if (flags.Base.HasValue && flags.Divisor.HasValue) {
				if (!SonCoprimos(flags.Base.Value, flags.Divisor.Value)) {
					if (!flags.TipoExtra && !flags.FlagsInactivos) throw new ArgumentException(ErrorDivisorCoprimo, nameof(flags.Divisor));
					flags.TipoExtra = true; // No se pueden usar las de coeficientes, como se cambia después de evaluar para sinFlags, se sigue preguntando
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
		public EstadoEjecucion Ejecutar(IOpciones opciones) {
			OpcionesDialogo flags = (OpcionesDialogo) opciones;
			Console.Error.WriteLine(MensajeInicioDialogo, SALIDA_DIALOGO);
			bool sinFlags = flags.FlagsInactivos, salir = true;
			EstadoEjecucion resultadoSalida = EstadoEjecucion.CORRECTA;
			static bool esS(char letra) => letra == 's' | letra == 'S';
			try {
				bool saltarPreguntaExtra = GestionarOpcionesDialogo(flags);
				do {
					//Si no tiene flags las pedirá durante la ejecución
					if (sinFlags) {

						//Le una tecla de entrada reglasObj lanza excepción si es la _salida
						if (!saltarPreguntaExtra) {
							flags.TipoExtra = !ObtenerDeUsuario(MensajeDialogoExtendido, esS);
						}
						
						flags.JSON = ObtenerDeUsuario(MensajeDialogoJson, esS);
					}

					var (Mensaje, Divisor, Base, Longitud) = 
						FlujoDatosRegla(flags.Divisor, flags.Base, flags.Longitud, sinFlags, flags);

					Salida.EscribirReglaPorConsola(Mensaje + Environment.NewLine, Divisor, Base);

					if (!flags.AnularBucle) {
						salir = !ObtenerDeUsuario(MensajeDialogoRepetir, esS);
					}
					resultadoSalida = EstadoEjecucion.CORRECTA;

				} while (!salir);
			}
			// Si decide salir, se saldrá por este catch
			catch (SalidaException) {
				resultadoSalida = EstadoEjecucion.VOLUNTARIA;
				Console.Error.WriteLine(Environment.NewLine + MensajeDialogoInterrumpido);
			}
			// Si ocurre otro error se saldrá por este catch
			catch (Exception e) {
				resultadoSalida = EstadoEjecucion.ERROR;
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				Console.Error.WriteLine(DialogoExcepcionInesperada);
			}
			return resultadoSalida;
		}

		/// <summary>
		/// Separa la obtención de los datos de la regla del resto del diálogo para mejorar la lectura
		/// </summary>
		/// <param name="divisorNull"></param>
		/// <param name="baseNull"></param>
		/// <param name="longitudNull"></param>
		/// <param name="sinFlags"></param>
		/// <param name="flags"></param>
		private static (string Mensaje, long Divisor, long Base, int Longitud) FlujoDatosRegla(
			long? divisorNull, long? baseNull, int? longitudNull, bool sinFlags, OpcionesDialogo flags) {
			long divisor, @base;
			int longitud = 1;
			// Se carga la base primero, necesaria en todos los casos
			@base = ObtenerValorODefecto(baseNull,
				() => ObtenerDeUsuarioLong(2, ErrorBase, MensajeDialogoBase));

			if (flags.TipoExtra) {
				divisor = ObtenerValorODefecto(divisorNull,
					() => ObtenerDeUsuarioLong(2, ErrorDivisor, MensajeDialogoDivisor));

			} else {
				divisor = ObtenerValorODefecto(divisorNull,
					() => ObtenerDeUsuarioCoprimo(2, @base, ErrorDivisorCoprimo, MensajeDialogoDivisor));

				longitud = ObtenerValorODefecto(longitudNull,
					() => ObtenerDeUsuario(0, ErrorCoeficientes, MensajeDialogoCoeficientes));
			}

			return (((IOpcionesGlobales)flags).ObtenerReglas(divisor, @base, longitud), divisor, @base, longitud);
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

		private static BigInteger ObtenerDeUsuario(BigInteger minimo, string mensajeError, string mensajePregunta) {
			BigInteger dato;
			Console.Error.Write(mensajePregunta);
			string? linea = Console.In.ReadLine();
			while (!BigInteger.TryParse(linea, out dato) || dato < minimo) {
				LanzarExcepcionSiSalida(linea);
				Console.Error.WriteLine(Environment.NewLine + mensajeError);
				Console.Error.Write(mensajePregunta);
				linea = Console.In.ReadLine();
			}
			Console.Error.WriteLine();
			return dato;
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

		#endregion

	}
}
