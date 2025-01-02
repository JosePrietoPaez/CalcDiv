using Operaciones;
using static ModosEjecucion.Recursos.TextoEjecucion;

namespace ModosEjecucion {
	public class ModoVarias : IModoEjecucion {

		private static readonly StringWriter _writerDesecho = new();

		public Salida Ejecutar(TextWriter salida, TextWriter error, IOpciones opciones) {
			OpcionesVarias varias = (OpcionesVarias) opciones;
			Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion, IRegla)> generadora = ModoDirecto.SeleccionarFuncionYAjustarFlags(varias);
			Func<IRegla, long, long, int, List<(TextWriter, string, bool)>> consumidora = SeleccionarConsumidora(varias, salida, error);
			Func<List<object>, List<(TextWriter, string, bool)>> final = SeleccionarFinal(varias, salida, error);
			return EjectutarVarias(generadora, consumidora, final, varias.Divisores, varias.Bases, varias.Longitud ?? 1, varias, salida, error);
		}

		private static Salida EjectutarVarias(Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion, IRegla)> generadora,
			Func<IRegla, long, long, int, List<(TextWriter, string, bool)>> consumidora,
			Func<List<object>, List<(TextWriter, string, bool)>> final,
			long[] divisores, long[] bases, int longitud,
			OpcionesVarias flags,
			TextWriter textSalida, TextWriter textError) {

			Salida salida = new(EstadoEjecucion.CORRECTA);
			bool hayFallo = false, hayExito = false;
			List<IRegla> reglas = new(divisores.Length * bases.Length); // Contendrá las listas o listas de listas
			foreach (long divisor in divisores) {
				foreach (long @base in bases) {
					(salida.Estado, IRegla nuevoElemento) = generadora(divisor, @base, longitud, flags); // La divisibilidad se maneja en el método
					if (!hayExito && salida.Estado == EstadoEjecucion.CORRECTA) {
						hayExito = true;
					} else if (!hayFallo && salida.Estado != EstadoEjecucion.CORRECTA) {
						hayFallo = true;
					}
					if (!hayExito) continue;
					reglas.Add(nuevoElemento);
					salida.Mensajes.AddRange(consumidora(nuevoElemento, divisor, @base, longitud));
				}
			}
			salida.Mensajes.AddRange(final(reglas.Select(o => o as object).ToList()));
			ComprobarErrorVarias(textError, salida, hayFallo, hayExito);
			return salida;
		}

		private static void ComprobarErrorVarias(TextWriter textError, Salida salida, bool hayFallo, bool hayExito) {
			if (!hayExito) { // Si no hay reglas, no se escriben
				salida.Mensajes.Add((textError, VariasMensajeErrorTotal, true));
				salida.Estado = EstadoEjecucion.VARIAS_ERROR_TOTAL;
			} else {
				if (hayFallo) {
					salida.Mensajes.Add((textError, VariasMensajeError, true));
					salida.Estado = EstadoEjecucion.VARIAS_ERROR;
				}
			}
		}

		private static Func<IRegla, long, long, int, List<(TextWriter, string, bool)>> SeleccionarConsumidora(OpcionesVarias flags, TextWriter salida, TextWriter error) {
			Func<IRegla, long, long, int, List<(TextWriter, string, bool)>> funcion;
			if (flags.JSON) {
				funcion = (_,_,_,_) => [(_writerDesecho, string.Empty, true)];
			} else {
				funcion = (o, divisor, @base, longitud) => ReglaConMensaje(Salida.ObjetoAString(o), divisor, @base, salida, error);
				if ((flags as IOpcionesGlobales).DividendoList.Count != 0) {
					Func<IRegla, long, long, int, List<(TextWriter, string, bool)>> funcionAuxiliar = funcion;
					funcion = (o, divisor, @base, longitud) => {
						var resultado = funcionAuxiliar(o, divisor, @base, longitud);
						resultado.Add((salida,
							o.AplicarVariosDividendos((flags as IOpcionesGlobales).DividendoList) ?? ObjetoNuloMensaje,
							true));
						return resultado;
					};
				}
			}
			return funcion;
		}

		/// <summary>
		/// Devuelve una lista con strings y los writers por los que deben ser escritos.
		/// </summary>
		internal static List<(TextWriter, string, bool)> ReglaConMensaje(string reglaCoeficientes, long divisor, long @base, TextWriter salida, TextWriter error) {
			return [(error, string.Format(MensajeParametrosDirecto, divisor, @base), true)
				, (salida, reglaCoeficientes, true)];
		}

		private static Func<List<object>, List<(TextWriter, string, bool)>> SeleccionarFinal(OpcionesVarias flags, TextWriter salida, TextWriter error) {
			Func<List<object>, List<(TextWriter, string, bool)>> funcion;
			if (flags.JSON) {
				funcion = (lista) => [(salida, Salida.ObjetoAString(lista, true), true)];
				if ((flags as IOpcionesGlobales).DividendoList.Count != 0) {
					Func<List<object>, List<(TextWriter, string, bool)>> funcionAuxiliar = funcion;
					funcion = (lista) => {
						var resultado = funcionAuxiliar(lista);
						foreach (var obj in lista) {
							resultado.Add((salida, (obj as IRegla)!.AplicarVariosDividendos((flags as IOpcionesGlobales).DividendoList), true));
						}
						return resultado;
					};
				}
			} else {
				funcion = (_) => [(_writerDesecho, string.Empty, true)];
			}
			return funcion;
		}

		public (EstadoEjecucion, IEnumerable<IRegla>) CalcularRegla(IOpciones opciones) {
			OpcionesVarias varias = (OpcionesVarias)opciones;
			Func<long, long, int, IOpcionesGlobales, (EstadoEjecucion, IRegla)> generadora = ModoDirecto.SeleccionarFuncionYAjustarFlags(varias);
			bool hayFallo = false, hayExito = false;
			Salida salida = new();
			List<IRegla> reglas = [];
			foreach (long divisor in varias.Divisores) {
				foreach (long @base in varias.Bases) {
					(EstadoEjecucion estado, IRegla regla) = generadora(divisor, @base, varias.Longitud ?? 1, varias);
					reglas.Add(regla);
					if (!hayExito && estado == EstadoEjecucion.CORRECTA) {
						hayExito = true;
					} else if (!hayFallo && estado != EstadoEjecucion.CORRECTA) {
						hayFallo = true;
					}
				}
			}
			ComprobarErrorVarias(_writerDesecho, salida, hayFallo, hayExito);
			return (salida.Estado, reglas);
		}
	}
}
