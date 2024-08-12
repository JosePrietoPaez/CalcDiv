using Operaciones;
using static Operaciones.Calculos;
using static ProgramaDivisibilidad.Recursos.TextoResource;

namespace ProgramaDivisibilidad {
	public static partial class CalculadoraDivisibilidadCLI {

		/// <summary>
		/// Lógica de la aplicación en modo directo.
		/// </summary>
		/// <returns>
		/// booleano que indica si ha conseguido calcular la regla.
		/// </returns>
		private static void IntentarDirecto() { //Intenta dar las reglas de forma directa, cambia _salida para mostrar el error
			_salida = SALIDA_CORRECTA;
			Func<long, long, int, (int, object)> funcionEjecutada = SeleccionarFuncionYAjustarFlags();
			if (flags.VariasReglas?.Any() ?? false) {
				flags.DatosRegla = [1,1,1];
				_salida = EjectutarVarias(funcionEjecutada, flags.ListaDivisores, flags.ListaBases, flags.CoeficientesVarias);
			} else {
				if (flags.DivisorDirecto < 2 || flags.BaseDirecto < 2 || !SonCoprimos(flags.DivisorDirecto, flags.BaseDirecto)) {
					_escritorError.WriteLine(ErrorDivisorCoprimo);
					_escritorError.WriteLine(ErrorBase);
					_salida = SALIDA_ERROR;
				} else {
					if (flags.DatosRegla.Count == 2) flags.Directo = flags.Directo!.Append(1);
					(_salida, object elementoCreado) = funcionEjecutada(flags.DivisorDirecto, flags.BaseDirecto, flags.LongitudDirecta);
					string textoResultado = ObjetoAString(elementoCreado);
					EscribirReglaPorWriter(textoResultado, _escritorSalida, _escritorError, flags.DivisorDirecto, flags.BaseDirecto, flags.LongitudDirecta);
					if (!SonCoprimos(flags.DivisorDirecto, flags.BaseDirecto)) {
						_escritorError.WriteLine(ErrorPrimo);
					}
					if (flags.Dividendo?.Any() ?? false)
						foreach (long dividendo in flags.Dividendo) {
							_escritorSalida.WriteLine(((Regla)elementoCreado).AplicarRegla(dividendo));
						}
				}
			}
		}

		private static int EjectutarVarias(Func<long, long, int, (int,object)> funcionEjecutada, long[] divisores, long[] bases, int coeficiente) {
			int valorEjecucion = SALIDA_CORRECTA;
			bool hayFallo = false, hayExito = false;
			List<(long divisor, long @base)> tuplas = []; // Sacrifica espacio, pero debería mejorar la eficiencia
			List<object> reglas = new(divisores.Length * bases.Length); // Contendrá las listas o listas de listas
			foreach (long divisor in divisores) {
				foreach (long @base in bases) {
					tuplas.Add((divisor, @base));
					flags.DatosRegla = [divisor, @base, coeficiente];
					(valorEjecucion, object nuevoElemento) = funcionEjecutada(divisor, @base, coeficiente); // La divisibilidad se maneja en el método
					reglas.Add(nuevoElemento);
					if (!hayExito && valorEjecucion == SALIDA_CORRECTA) {
						hayExito = true;
					} else if (!hayFallo && valorEjecucion != SALIDA_CORRECTA) {
						hayFallo = true;
					}
				}
			}
			if (!hayExito) { // Si no hay reglas, no se escriben
				_escritorError.WriteLine(VariasMensajeErrorTotal);
				valorEjecucion = SALIDA_VARIAS_ERROR_TOTAL;
			} else {
				if (flags.ActivarMensajesIntermedios) {
					IntercalarMensajesParametros(reglas, tuplas, coeficiente);
				} else {
					string resultadoString = ObjetoAString(reglas);
					_escritorSalida.WriteLine(resultadoString);
				}
				if (hayFallo) {
					_escritorError.WriteLine(VariasMensajeError);
					valorEjecucion = SALIDA_VARIAS_ERROR;
				}
			}
			return valorEjecucion;
		}

		private static void IntercalarMensajesParametros(List<object> reglas, List<(long divisor,long @base)> tuplas, int coeficiente) {
			string salidaReglas = ObjetoAString(reglas).Trim();
			string[] lineas = salidaReglas.Split(Environment.NewLine);
			int indiceLinea = 0, lineasPorRegla = flags.Todos ? PotenciaEntera(2, coeficiente) : 1
				, indiceTupla = -1, saltados = 0;
			while (indiceLinea < lineas.Length) {
				if ((indiceLinea - saltados) % lineasPorRegla == 0) {
					indiceTupla++;
				}
				_escritorError.WriteLine(MensajeParametrosDirecto, tuplas[indiceTupla].divisor, tuplas[indiceTupla].@base, coeficiente);
				if (lineas[indiceLinea].Equals(string.Empty)) { // Si la regla falla se salta
					if (!SonCoprimos(tuplas[indiceTupla].divisor, tuplas[indiceTupla].@base)) {
						_escritorError.WriteLine(ErrorPrimo);
					}
					indiceLinea++;
					saltados++;
					continue;
				}
				if (flags.Todos) { // Porque cada regla tendrá varias líneas se escriben todas antes de ir a la siguentes
					for (int j = 0; j < lineasPorRegla; j++, indiceLinea++) {
						if (indiceLinea >= lineas.Length - 1) {
							_escritorSalida.Write(lineas[indiceLinea]);
						} else {
							_escritorSalida.WriteLine(lineas[indiceLinea]);
						}
					}
				} else {
					if (indiceLinea >= lineas.Length - 1) {
						_escritorSalida.Write(lineas[indiceLinea++]);
					} else {
						_escritorSalida.WriteLine(lineas[indiceLinea++]);
					}
				}
			}
			_escritorError.WriteLine();
		}

		private static Func<long, long, int, (int, object)> SeleccionarFuncionYAjustarFlags() {
			Func<long, long, int, (int, object)> resultado;
			if (flags.TipoExtra) { // Para separar la funcion de las llamadas en VariasReglas
				resultado = CrearReglaExtra;
			} else {
				resultado = CrearReglaCoeficientes;
			}
			return resultado;
		}

		private static (int, object) CrearReglaCoeficientes(long divisor, long @base, int coefientes) {
			int salida;
			object elementoCreado = new Regla(divisor, @base, coefientes);
			if (!SonCoprimos(divisor,@base)) {
				salida = SALIDA_ERROR;
			} else if (flags.Todos) {
				List<Regla> listaAuxiliar = ReglasDivisibilidad(divisor, coefientes, @base);
				foreach (var item in listaAuxiliar) {
					item.Nombre = flags.Nombre;
				}
				elementoCreado = listaAuxiliar;
				salida = SALIDA_CORRECTA;
			} else {
				Regla reglaAuxiliar = ReglaDivisibilidadOptima(divisor, coefientes, @base);
				reglaAuxiliar.Nombre = flags.Nombre!;
				elementoCreado = reglaAuxiliar;
				salida = SALIDA_CORRECTA;
			}
			return (salida, elementoCreado);
		}

		private static (int, object) CrearReglaExtra(long divisor, long @base, int coefientes) {
			var resultado = ReglaDivisibilidadExtendida(divisor, @base);
			int salida;
			if (resultado.Item1) {
				salida = SALIDA_CORRECTA;
			} else {
				salida = SALIDA_FRACASO_EXPANDIDA;
			}
			return (salida, resultado.Item2);
		}
	}
}
