﻿using Operaciones.Recursos;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace Operaciones {
	/// <summary>
	/// Los objetos de esta clase representan reglas de divisibilidad que se basan en reducir las cifras del dividendo.
	/// </summary>
	/// <remarks>
	/// Las reglas de este tipo solo pueden usarse una vez.
	/// </remarks>
	public class ReglaCifras : ReglaAplicable {

		private readonly long _divisor, _base;
		private readonly int _cifras;
		public const int POSIBLES_MENORES = 256;

		internal ReglaCifras(long divisor, long @base, int cifras) {
			ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 1);
			ArgumentOutOfRangeException.ThrowIfLessThan(@base, 2);
			ArgumentOutOfRangeException.ThrowIfLessThan(cifras, 1);
			_divisor = divisor;
			_base = @base;
			_cifras = cifras;
			long iteraciones = Calculos.PotenciaEntera(Base, Cifras) / Divisor;
			if (iteraciones <= POSIBLES_MENORES) {
				_stringCifras = CalcularMenoresCifras();
				_stringError = TextoCalculos.MensajeErrorNinguno;
			} else {
				_stringCifras = TextoCalculos.MensajeCifrasDemasiadas;
				_stringError = TextoCalculos.MensajeErrorDemasiadasPosibilidades;
			}
		}

		[JsonPropertyName("rule-explained")]
		public override string ReglaExplicada => string.Format(TextoCalculos.CalculosExtendidaMensajeCifrasPrincipio, _divisor, _base)
			+ Environment.NewLine
			+ string.Format(TextoCalculos.ReglaExplicadaCifras, Divisor, Cifras, Base);

		[JsonPropertyName("base")]
		public override long Base => _base;

		[JsonPropertyName("divisor")]
		public override long Divisor => _divisor;

		[JsonPropertyName("digits-used")]
		public int Cifras => _cifras;

		[JsonPropertyName("allowed-lowest-digits")]
		public List<BigInteger> CasosPermitidos {
			get {
				return new(_menoresPermitidos);
			}
		}

		[JsonIgnore]
		public string CasosPermitidosString => _stringCifras;

		private readonly List<BigInteger> _menoresPermitidos = [0];

		private readonly string _stringCifras, _stringError;

		private string CalcularMenoresCifras() {
			long iteraciones = Calculos.PotenciaEntera(Base, Cifras) / Divisor;
			StringBuilder resultado = new();
			resultado.Append('[');
			for (int i = 1; i < iteraciones; i++) {
				_menoresPermitidos.Add(Divisor * i);
				resultado.Append(',').Append(_menoresPermitidos.Select(LongAStringCondicional));
			}
			resultado.Append(']');
			return resultado.ToString();
		}

		[JsonPropertyName("type")]
		public override CasosDivisibilidad Tipo => CasosDivisibilidad.DIGITS;

		[JsonPropertyName("error")]
		public override string Error => _stringError;

		public override string AplicarRegla(BigInteger dividendo) {
			StringBuilder sb = new();
			sb.AppendFormat(TextoCalculos.MensajeAplicarCifrasInicio, Divisor, Base, dividendo, Cifras).AppendLine();
			InsertarMensajeBase(sb, dividendo);
			BigInteger cifrasUsadas = ObtenerNuevoDividendo(dividendo, sb);
			InsertarMensajeFin(sb, dividendo, cifrasUsadas);
			return sb.ToString();
		}

		public override string ToString() {
			return ReglaExplicada + Environment.NewLine
				+ string.Format(TextoCalculos.MensajePosiblesCifras, CasosPermitidosString, Base);
		}

		protected override BigInteger ObtenerNuevoDividendo(BigInteger dividendo, StringBuilder sb) {
			BigInteger cifrasUsadas = dividendo % Calculos.PotenciaEntera(Base, Cifras);
			sb.AppendFormat(TextoCalculos.MensajeAplicarCifrasSeparar, Cifras, LongAStringCondicional(cifrasUsadas)).AppendLine();
			return cifrasUsadas;
		}
	}
}
