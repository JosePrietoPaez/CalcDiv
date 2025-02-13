﻿using System;

namespace ModosEjecucion {
	public class SalidaException : Exception {
		public SalidaException() : base() { }

		public SalidaException(string message) : base(message) { }

		public SalidaException(string message, Exception innerException) : base(message, innerException) { }
	}
}
