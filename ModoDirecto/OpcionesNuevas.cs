using CommandLine;
using CommandLine.Text;
using ModosEjecucion.Recursos;
using Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Operaciones.Calculos;

namespace ModosEjecucion {

	public interface IOpciones {
		[Usage(ApplicationAlias = "CalcDiv.exe")]
		public static IEnumerable<Example> Examples {
			get {
				return [
					new(TextoEjecucion.EjemploReglaUno, new OpcionesDirecto {Base = 12, Divisor = 7, Longitud = 2}),
					new(TextoEjecucion.EjemploReglaJsonExtendido, new OpcionesDirecto {Base = 16, Divisor = 13, ReglasCoeficientes = false, JSON = true}),
					new(TextoEjecucion.EjemploReglaDialogo, new OpcionesDialogo{ }),
					new(TextoEjecucion.EjemploReglaVarias, new OpcionesVarias{ VariasReglas = ["7,100,41", "10,8"], ReglasCoeficientes = false, Dividendo = ["6341,289"] })
				]; 
			}
		}
	}

	[Verb("dialog", false, HelpText = "HelpVerbDialog", ResourceType = typeof(TextoEjecucion))]
	public class OpcionesDialogo : IOpcionesGlobales {

		[Option("base", MetaValue = "LONG"
			, HelpText = "HelpBaseDialogo"
			, ResourceType = typeof(TextoEjecucion))]
		public long? Base { get; set; }

		[Option("divisor", MetaValue = "LONG"
			, HelpText = "HelpDivisorDialogo"
			, ResourceType = typeof(TextoEjecucion))]
		public long? Divisor { get; set; }

		[Option("no-loop"
			, HelpText = "HelpAnularBucle"
			, ResourceType = typeof(TextoEjecucion))]
		public bool AnularBucle { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -s está activa.
		/// </summary>
		[Option('s', longName: "simple-dialog"
			, HelpText = "HelpSaltar"
			, ResourceType = typeof(TextoEjecucion))]
		public bool DialogoSencillo { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción --skip-explanation está activa.
		/// </summary>
		[Option("skip-explanation"
			, HelpText = "HelpExplicacion"
			, ResourceType = typeof(TextoEjecucion))]
		public bool SaltarExplicacion { get; set; }

		/// <summary>
		/// Esta propiedad devuelve si todos los flags, excepto los de valores de diálogo están inactivos.
		/// </summary>
		public bool FlagsInactivos => !(DialogoSencillo || JSON || ReglasCoeficientes || SaltarExplicacion);

		public bool ReglasCoeficientes { get; set; }
		public bool JSON { get; set; }
		public IEnumerable<string>? Dividendo { get; set; }
		public int? Longitud { get; set; }
	}

	[Verb("single", true, HelpText = "HelpVerbSingle", ResourceType = typeof(TextoEjecucion))]
	public class OpcionesDirecto : IOpcionesGlobales {

		/// <summary>
		/// Devuelve el divisor pasado por parámetro.
		/// </summary>
		[Value(0, MetaName = "divisor", MetaValue = "LONG"
			, Required = true
			, HelpText = "HelpDivisor"
			, ResourceType = typeof(TextoEjecucion))]
		public long Divisor { get; set; }

		/// <summary>
		/// Devuelve la base pasada por parámetro.
		/// </summary>
		[Value(1, MetaName = "base", MetaValue = "LONG"
			, Default = 10
			, Required = false
			, HelpText = "HelpBase"
			, ResourceType = typeof(TextoEjecucion))]
		public long Base { get; set; }

		public bool ReglasCoeficientes { get; set; }
		public bool JSON { get; set; }
		public IEnumerable<string>? Dividendo { get; set; }
		public int? Longitud { get; set; }
	}

	[Verb("multiple", false, HelpText = "HelpTextMultiple", ResourceType = typeof(TextoEjecucion))]
	public class OpcionesVarias : IOpcionesGlobales {

		/// <summary>
		/// Esta propiedad obtiene la información de las reglas para calcularlas.
		/// </summary>
		[Value(0, MetaName = "divisor-base", MetaValue = "[LONG] [LONG]"
			, Min = 1
			, Max = 2
			, HelpText = "HelpVarias"
			, ResourceType = typeof(TextoEjecucion))]
		public IEnumerable<string> VariasReglas { get; set; } = [""];

		private const char SEPARADOR = ',';
		private long[]? _listaDivisores = null
			, _listaBases = null;

		/// <summary>
		/// Devuelve la lista de divisores de las reglas pasada a multiple
		/// </summary>
		public long[] Divisores { 
			get {
				if (_listaDivisores == null) {
					string[] lista = VariasReglas!.First().Split(SEPARADOR);
					long[] result = ParsearStringsLong(lista, TextoEjecucion.ErrorBase);
					_listaDivisores = [.. result];
					return result;
				} else {
					return [.. _listaDivisores];
				}
			}
		}

		/// <summary>
		/// Devuelve la lista de bases pasada a multiple
		/// </summary>
		public long[] Bases {
			get {
				if (_listaBases == null) {
					if (VariasReglas.Count() == 1) {
						_listaBases = [10];
						return [10];
					} else {
						string[] lista = VariasReglas!.ElementAt(1).Split(SEPARADOR);
						long[] result = ParsearStringsLong(lista, TextoEjecucion.ErrorBase);
						_listaBases = [.. result];
						return result;
					}
				} else {
					return [.. _listaBases];
				}
			}
		}

		public bool ReglasCoeficientes { get; set; }
		public bool JSON { get; set; }
		public IEnumerable<string>? Dividendo { get; set; }
		public int? Longitud { get; set; }

		private static long[] ParsearStringsLong(string[] numeros, string mensajeError = "") {
			long[] result = [];
			foreach (string s in numeros) {
				if (long.TryParse(s, out long numeroParseado)) {
					if (!result.Contains(numeroParseado)) {
						result = [.. result, numeroParseado];
					}
				} else {
					throw new FormatException(mensajeError);
				}
			}
			return result;
		}

	}

	[Verb("manual", false, HelpText = "HelpVerbManual", ResourceType = typeof(TextoEjecucion))]
	public class OpcionesManual : IOpciones {

		[Value(0, Hidden = true, Default = "")]
		public string? Show { get; set; }

	}
	
	public interface IOpcionesGlobales : IOpciones {

		/// <summary>
		/// Esta propiedad indica si la opción -x está activa.
		/// </summary>
		[Option('c', longName: "coefficient-rules"
			, HelpText = "HelpExtendido"
			, ResourceType = typeof(TextoEjecucion)
			, SetName = "varied"
			, Default = false)]
		public bool ReglasCoeficientes { get; set; }

		/// <summary>
		/// Esta propiedad indica si la opción -j está activa.
		/// </summary>
		[Option('j', longName: "json"
			, HelpText = "HelpJson"
			, ResourceType = typeof(TextoEjecucion))]
		public bool JSON { get; set; }

		[Option('d', longName: "dividend", MetaValue = "[LONG]"
			, Separator = ','
			, HelpText = "HelpDividendo"
			, ResourceType = typeof(TextoEjecucion))]
		public IEnumerable<string>? Dividendo { get; set; }

		/// <summary>
		/// Devuelve la longitud de las reglas de coeficientes
		/// </summary>
		[Option("length", MetaValue = "INT"
			, HelpText = "HelpLongitud"
			, ResourceType = typeof(TextoEjecucion))]
		public int? Longitud { get; set; }
		public virtual List<BigInteger> DividendoList => Dividendo?.Select(BigInteger.Parse).ToList() ?? [];

		/// <summary>
		/// Obtiene el string de una regla de coeficientes a partir de su base, divisor y coeficientes.
		/// </summary>
		/// <param name="divisor"></param>
		/// <param name="base"></param>
		/// <param name="longitud"></param>
		/// <returns>
		/// String apropiado para la regla según los datos proporcionados.
		/// </returns>
		public (string, IRegla) ObtenerReglas(long divisor, long @base, int longitud, bool json) {
			string resultado;
			IRegla regla;
			if (ReglasCoeficientes) {
				regla = ReglaDivisibilidadOptima(divisor, longitud, @base);
			} else {
				regla = ReglaDivisibilidadExtendida(divisor, @base).Item2;
			}
			resultado = Salida.ObjetoAString(regla,json);
			return (resultado, regla);
		}

	}
}
