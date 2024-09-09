using Operaciones;
using static ProgramaDivisibilidad.Recursos.TextoResource;

namespace ProgramaDivisibilidad {
	class ModoVarias : IModoEjecucion {

		private static readonly StringWriter _writerDesecho = new();

		public Salida Ejecutar(IOpciones opciones) {
			OpcionesVarias varias = (OpcionesVarias) opciones;
			Func<long, long, int, IOpcionesGlobales, (Salida, IRegla)> generadora = ModoDirecto.SeleccionarFuncionYAjustarFlags(varias);
			Func<IRegla, long, long, int, List<(TextWriter, string)>> consumidora = SeleccionarConsumidora(varias);
			Func<List<object>, List<(TextWriter, string)>> final = SeleccionarFinal(varias);
			return EjectutarVarias(generadora, consumidora, final, varias.Divisores, varias.Bases, varias.Longitud ?? 1, varias);
		}

		private static Salida EjectutarVarias(Func<long, long, int, IOpcionesGlobales, (Salida, IRegla)> generadora,
			Func<IRegla, long, long, int, List<(TextWriter, string)>> consumidora,
			Func<List<object>, List<(TextWriter, string)>> final,
			long[] divisores, long[] bases, int longitud,
			OpcionesVarias flags) {

			Salida valorEjecucion = Salida.CORRECTA;
			bool hayFallo = false, hayExito = false;
			List<IRegla> reglas = new(divisores.Length * bases.Length); // Contendrá las listas o listas de listas
			foreach (long divisor in divisores) {
				foreach (long @base in bases) {
					(valorEjecucion, IRegla nuevoElemento) = generadora(divisor, @base, longitud, flags); // La divisibilidad se maneja en el método
					if (!hayExito && valorEjecucion == Salida.CORRECTA) {
						hayExito = true;
					} else if (!hayFallo && valorEjecucion != Salida.CORRECTA) {
						hayFallo = true;
					}
					if (!hayExito) continue;
					reglas.Add(nuevoElemento);
					EscribirListaPorWriters(consumidora(nuevoElemento, divisor, @base, longitud));
				}
			}
			EscribirListaPorWriters(final(reglas.Select(o => o as object).ToList()));
			if (!hayExito) { // Si no hay reglas, no se escriben
				Console.Error.WriteLine(VariasMensajeErrorTotal);
				valorEjecucion = Salida.VARIAS_ERROR_TOTAL;
			} else {
				if (hayFallo) {
					Console.Error.WriteLine(VariasMensajeError);
					valorEjecucion = Salida.VARIAS_ERROR;
				}
			}
			return valorEjecucion;
		}

		private static Func<IRegla, long, long, int, List<(TextWriter, string)>> SeleccionarConsumidora(OpcionesVarias flags) {
			Func<IRegla, long, long, int, List<(TextWriter, string)>> funcion;
			if (flags.JSON) {
				funcion = (_,_,_,_) => [(_writerDesecho, string.Empty)];
			} else {
				funcion = (o, divisor, @base, longitud) => ReglaConMensaje(CalcDivCLI.ObjetoAString(o), divisor, @base);
				if ((flags as IOpcionesGlobales).DividendoList.Count != 0) {
					Func<IRegla, long, long, int, List<(TextWriter, string)>> funcionAuxiliar = funcion;
					funcion = (o, divisor, @base, longitud) => {
						var resultado = funcionAuxiliar(o, divisor, @base, longitud);
						resultado.Add((Console.Out,
							o.AplicarVariosDividendos((flags as IOpcionesGlobales).DividendoList) ?? ObjetoNuloMensaje));
						return resultado;
					};
				}
			}
			return funcion;
		}

		/// <summary>
		/// Devuelve una lista con strings y los writers por los que deben ser escritos.
		/// </summary>
		internal static List<(TextWriter, string)> ReglaConMensaje(string reglaCoeficientes, long divisor, long @base) {
			return [(Console.Error, string.Format(MensajeParametrosDirecto, divisor, @base))
				, (Console.Out, reglaCoeficientes)];
		}

		private static Func<List<object>, List<(TextWriter, string)>> SeleccionarFinal(OpcionesVarias flags) {
			Func<List<object>, List<(TextWriter, string)>> funcion;
			if (flags.JSON) {
				funcion = (lista) => [(Console.Out, CalcDivCLI.ObjetoAString(lista, true))];
				if ((flags as IOpcionesGlobales).DividendoList.Count != 0) {
					Func<List<object>, List<(TextWriter, string)>> funcionAuxiliar = funcion;
					funcion = (lista) => {
						var resultado = funcionAuxiliar(lista);
						foreach (var obj in lista) {
							resultado.Add((Console.Out, (obj as IRegla)!.AplicarVariosDividendos((flags as IOpcionesGlobales).DividendoList)));
						}
						return resultado;
					};
				}
			} else {
				funcion = (_) => [(_writerDesecho, string.Empty)];
			}
			return funcion;
		}

		private static void EscribirListaPorWriters(List<(TextWriter, string)> lista) {
			foreach (var (writer, linea) in lista) {
				if (linea.Length != 0) {
					writer.WriteLine(linea);
				}
			}
		}
	}
}
