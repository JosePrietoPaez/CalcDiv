using Operaciones.Recursos;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	public class ReglaCompuesta : ReglaAplicable {

		private readonly List<IRegla> _reglas = [];
		private readonly long _base, _divisor = 1;
		private readonly List<long> _potencias, _primos;

		public ReglaCompuesta(long divisor, long @base) {
			List<long> potencias = Calculos.DescompsicionEnPrimos(divisor)
				, primos = Calculos.PrimosCalculados[0..potencias.Count]; // Puede ser de mayor o igual longitud
			for (int i = potencias.Count - 1; i >= 0; i--) {
				if (potencias[i] == 0) {
					potencias.RemoveAt(i);
					primos.RemoveAt(i);
				}
			}
			_potencias = potencias;
			_primos = primos;
			_base = @base;
			_divisor = divisor;
			for (int i = 0; i < potencias.Count; i++) {
				long divisorLista = Calculos.PotenciaEntera(primos[i], potencias[i]);
				_reglas.Add(IRegla.GenerarReglaPorTipo(divisorLista, @base));
			}
		
		}

		[JsonPropertyName("rule-explained")]
		public override string ReglaExplicada => string.Format(TextoCalculos.ReglaExplicadaCompuesta, Divisor);

		[JsonPropertyName("base")]
		public override long Base => _base;

		[JsonPropertyName("divisor")]
		public override long Divisor => _divisor;

		[JsonPropertyName("prime-factors")]
		public List<long> FactoresPrimos => new(_primos);

		[JsonPropertyName("powers")]
		public List<long> Potencias => new(_potencias);

		[JsonIgnore]
		public List<IRegla> Subreglas => new(_reglas);

		[JsonPropertyName("subrules")]
		[JsonInclude]
		private List<object> SubreglasJSON => new(_reglas);

		[JsonPropertyName("type")]
		public override CasosDivisibilidad Tipo => CasosDivisibilidad.COMPOSITE_RULE;

		[JsonPropertyName("error")]
		public override string Error => TextoCalculos.MensajeErrorNinguno;

		protected override BigInteger ObtenerNuevoDividendo(BigInteger dividendo, StringBuilder sb) {
			throw new NotImplementedException();
		}

		public override string AplicarRegla(BigInteger dividendo) {
			BigInteger dividendoOriginal = dividendo;
			StringBuilder sb = new();
			InsertarMensajeBase(sb, dividendo);
			bool salirBucle = false;
			for (int i = 0; i < _reglas.Count && !salirBucle; i++) {
				IRegla regla = _reglas[i];
				sb.AppendLine(regla.AplicarRegla(dividendo));
				salirBucle = (dividendo % regla.Divisor) != 0;
				dividendo /= regla.Divisor;
			}
			if (salirBucle) {
				sb.AppendFormat(TextoCalculos.MensajeAplicarCompuestaFracaso, LongAStringCondicional(dividendoOriginal), _divisor).AppendLine();
			} else {
				sb.AppendFormat(TextoCalculos.MensajeAplicarCompuestaExito, LongAStringCondicional(dividendoOriginal), _divisor).AppendLine();
			}
			return sb.ToString();
		}

		public override string ToString() {
			return ReglaExplicada;
		}
	}
}
