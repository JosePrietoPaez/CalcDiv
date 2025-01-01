using Operaciones;
using System.Numerics;
using static Operaciones.Calculos;
using static ModosEjecucion.Recursos.TextoEjecucion;

namespace ModosEjecucion {
	public class ModoDirecto : IModoEjecucion {

		private readonly Salida _estadoSalida = new();

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// <see cref="Salida"/> que indica si ha conseguido calcular la regla.
		/// </returns>
		public Salida Ejecutar(TextWriter salida, TextWriter error, IOpciones opciones) { //Intenta dar las reglas de forma directa, cambia _salida para mostrar el error
			OpcionesDirecto flags = (OpcionesDirecto)opciones;
			Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion, IRegla)> generadora = SeleccionarFuncionYAjustarFlags(flags);
			return GestionarErrorYUsarDatos(flags, flags.Base, flags.Divisor, flags.Longitud ?? 1, generadora, salida, error);
		}

		private Salida GestionarErrorYUsarDatos(OpcionesDirecto flags, long @base, long divisor, int longitud
			, Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion, IRegla)> generadora
			, TextWriter salida, TextWriter error) {
			if (!flags.TipoExtra && (divisor < 2 || @base < 2 || !SonCoprimos(divisor, @base))) {
				_estadoSalida.Mensajes.Add((error, ErrorDivisorCoprimo, true));
				_estadoSalida.Mensajes.Add((error, ErrorBase, true));
				_estadoSalida.Estado = EstadoEjecucion.ERROR;
			} else {
				(_estadoSalida.Estado, object? elementoCreado) = generadora(divisor, @base, longitud, flags);
				string textoResultado = Salida.ObjetoAString(elementoCreado, flags.JSON);
				if (elementoCreado is null) {
					throw new NullReferenceException(ErrorReglaNula);
				}
				_estadoSalida.EscribirReglaPorConsola(textoResultado, divisor, @base, salida, error);
				if (elementoCreado is not ReglaCoeficientes) {
					//Por si hiciera falta
				} else {
					if (!SonCoprimos(divisor, @base)) {
						_estadoSalida.Mensajes.Add((error, ErrorPrimo, true));
					}
				}
				if (flags.Dividendo?.Any() ?? false) {
					_estadoSalida.Mensajes.Add((salida, Environment.NewLine, true));
					AplicarReglaPorObjeto(elementoCreado, flags, _estadoSalida, salida);
				}
			}
			return _estadoSalida;
		}

		internal static void AplicarReglaPorObjeto(object? regla, IOpcionesGlobales flags, Salida salida, TextWriter text) {
			if (regla is null) return;
			AplicarReglaDivisibilidad((IRegla)regla, flags.DividendoList, salida, text);
		}

		internal static Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion,IRegla)> SeleccionarFuncionYAjustarFlags(IOpcionesGlobales flags) {
			Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion, IRegla)> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			} else {
				resultado = CrearReglaCoeficientes;
			}
			return resultado;
		}

		internal static (EstadoEjecucion, IRegla) CrearReglaCoeficientes(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			EstadoEjecucion salida;
			IRegla elementoCreado;
			if (!SonCoprimos(divisor,@base)) {
				salida = EstadoEjecucion.ERROR;
				elementoCreado = new ReglaCoeficientes(divisor, @base, coefientes);
			} else {
				ReglaCoeficientes reglaAuxiliar = ReglaDivisibilidadOptima(divisor, coefientes, @base);
				elementoCreado = reglaAuxiliar;
				salida = EstadoEjecucion.CORRECTA;
			}
			return (salida, elementoCreado);
		}

		internal static (EstadoEjecucion, IRegla) CrearReglaExtra(long divisor, long @base, int coefientes, IOpcionesGlobales flags) {
			var resultado = ReglaDivisibilidadExtendida(divisor, @base);
			EstadoEjecucion salida;
			if (resultado.Item1) {
				salida = EstadoEjecucion.CORRECTA;
			} else {
				salida = EstadoEjecucion.FRACASO_EXPANDIDA;
			}
			return (salida, resultado.Item2);
		}

		private static void AplicarReglaDivisibilidad(IRegla regla, IEnumerable<BigInteger> dividendos, Salida salida, TextWriter text) {
			foreach (BigInteger dividendo in dividendos) {
				salida.Mensajes.Add((text, regla.AplicarRegla(dividendo), true));
			}
		}
	}
}
