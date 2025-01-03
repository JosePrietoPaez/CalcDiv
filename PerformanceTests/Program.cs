// See https://aka.ms/new-console-template for more information
using Operaciones;
using System.Diagnostics;
using System.IO;

string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
List<string> results = [];

void DumpResults(TextWriter writer) {
	foreach (var result in results) {
		writer.WriteLine(result);
	}
	results.Clear();
}
void Single_Coefficient_Performance_BaseIncrement() {
	long divisor = 7;
	int longitud = 1;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Coefficient_Performance_BaseIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Base, Tiempo, Regla");
	for (long @base = 9; @base < 10000000L; @base += 2) {
		if (!Calculos.SonCoprimos(divisor, @base)) continue;
		stopwatch.Restart();
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		stopwatch.Stop();
		results.Add($"{@base},{stopwatch.ElapsedTicks},{regla}");
		if (results.Count == 1000) {
			DumpResults(writer);
		}
	}
	DumpResults(writer);
}

void Single_Coefficient_Performance_DivisorIncrement() {
	long @base = 10;
	int longitud = 1;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Coefficient_Performance_DivisorIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Divisor, Tiempo, Regla");
	for (long divisor = 3; divisor < 10000000L; divisor += 2) {
		if (!Calculos.SonCoprimos(divisor, @base)) continue;
		stopwatch.Restart();
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		stopwatch.Stop();
		results.Add($"{divisor},{stopwatch.ElapsedTicks},{regla}");
		if (results.Count == 1000) {
			DumpResults(writer);
		}
	}
	DumpResults(writer);
}

void Single_Coefficient_Performance_LengthIncrement() {
	long divisor = 7, @base = 10;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Coefficient_Performance_LengthIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Longitud, Tiempo, Regla");
	for (int longitud = 1; longitud < 10000; longitud +=5) {
		stopwatch.Restart();
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		stopwatch.Stop();
		results.Add($"{longitud},{stopwatch.ElapsedTicks},{regla.Longitud}");
		if (results.Count == 1000) {
			DumpResults(writer);
		}
	}
	DumpResults(writer);
}

void Single_Varied_Performance_BaseIncrement() {
	long divisor = 10;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Varied_Performance_BaseIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Base, Tiempo, Tipo");
	for (long @base = 3; @base < 100000L; @base += 2) {
		if (!Calculos.SonCoprimos(divisor, @base)) continue;
		stopwatch.Restart();
		(bool exito, IRegla regla) = Calculos.ReglaDivisibilidadExtendida(divisor, @base);
		stopwatch.Stop();
		results.Add($"{@base},{stopwatch.ElapsedTicks},{regla.Tipo}");
		if (results.Count == 1000) {
			DumpResults(writer);
		}
	}
}

Single_Coefficient_Performance_BaseIncrement();
Console.WriteLine("Base increment finished");
Single_Coefficient_Performance_DivisorIncrement();
Console.WriteLine("Divisor increment finished");
Single_Coefficient_Performance_LengthIncrement();
Console.WriteLine("Length increment finished");
Single_Varied_Performance_BaseIncrement();
Console.WriteLine("Varied base increment finished");
