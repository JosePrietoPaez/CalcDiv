﻿using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;

namespace ProgramaDivisibilidad {
	/// <summary>
	/// Esta clase contiene la lógica para el modo diálogo
	/// </summary>
	class ModoDialogo : IModoEjecucion {

		private const string SALIDA_DIALOGO = "/";

		private static bool GestionarOpcionesDialogo(OpcionesDialogo flags) {
			bool saltarPreguntaExtra = flags.TipoExtra;
			if (flags.BaseDialogo.HasValue && flags.DivisorDialogo.HasValue) {
				if (!SonCoprimos(flags.BaseDialogo.Value, flags.DivisorDialogo.Value)) {
					if (!flags.TipoExtra && !flags.FlagsInactivos) throw new ArgumentException(ErrorDivisorCoprimo, nameof(flags.DivisorDialogo));
					flags.TipoExtra = true; // No se pueden usar las de coeficientes, como se cambia después de evaluar para sinFlags, se sigue preguntando
					saltarPreguntaExtra = true;
				} else {
					ArgumentOutOfRangeException.ThrowIfLessThan(flags.BaseDialogo.Value, 2, nameof(flags.BaseDialogo));
					ArgumentOutOfRangeException.ThrowIfLessThan(flags.DivisorDialogo.Value, 2, nameof(flags.DivisorDialogo));
				}
			}
			if (flags.LongitudDialogo.HasValue) {
				ArgumentOutOfRangeException.ThrowIfLessThan(flags.LongitudDialogo.Value, 1, nameof(flags.LongitudDialogo));
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
		public Salida Ejecutar(IOpciones opciones) {
			OpcionesDialogo flags = (OpcionesDialogo) opciones;
			Console.Error.WriteLine(MensajeInicioDialogo, SALIDA_DIALOGO);
			bool sinFlags = flags.FlagsInactivos, salir = true;
			Salida resultadoSalida = Salida.CORRECTA;
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
						FlujoDatosRegla(flags.DivisorDialogo, flags.BaseDialogo, flags.LongitudDialogo, sinFlags, flags);

					CalcDivCLI.EscribirReglaPorConsola(Mensaje + Environment.NewLine, Divisor, Base, Longitud);

					if (!flags.AnularBucle) {
						salir = !ObtenerDeUsuario(MensajeDialogoRepetir, esS);
					}
					resultadoSalida = Salida.CORRECTA;

				} while (!salir);
			}
			// Si decide salir, se saldrá por este catch
			catch (SalidaException) {
				resultadoSalida = Salida.VOLUNTARIA;
				Console.Error.WriteLine(Environment.NewLine + MensajeDialogoInterrumpido);
			}
			// Si ocurre otro error se saldrá por este catch
			catch (Exception e) {
				resultadoSalida = Salida.ERROR;
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

				if (sinFlags) {

					flags.Todos = ObtenerDeUsuario(MensajeDialogoTodas, c => c == 's' | c == 'S');
				}
			}

			return (flags.ObtenerReglas(divisor, @base, longitud), divisor, @base, longitud);
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