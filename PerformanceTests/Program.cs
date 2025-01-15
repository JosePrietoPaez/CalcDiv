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

const int MAX_SIZE = 100;

void Single_Coefficient_Performance_BaseIncrement() {
	long divisor = 7;
	int longitud = 1;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Coefficient_Performance_BaseIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Base, Tiempo, Regla");
	for (long @base = 2; @base < 10000000L; @base *= 2) {
		if (!Calculos.SonCoprimos(divisor, @base)) continue;
		stopwatch.Restart();
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		stopwatch.Stop();
		results.Add($"{@base},{stopwatch.ElapsedTicks},{regla}");
		if (results.Count == MAX_SIZE) {
			DumpResults(writer);
		}
	}
	DumpResults(writer);
}

void Single_Coefficient_Performance_DivisorIncrement() {
	long @base = 11;
	int longitud = 1;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Coefficient_Performance_DivisorIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Divisor, Tiempo, Regla");
	for (long divisor = 2; divisor < 10000000L; divisor *= 2) {
		if (!Calculos.SonCoprimos(divisor, @base)) continue;
		stopwatch.Restart();
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		stopwatch.Stop();
		results.Add($"{divisor},{stopwatch.ElapsedTicks},{regla}");
		if (results.Count == MAX_SIZE) {
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
	for (int longitud = 1; longitud < 100000; longitud +=5) {
		stopwatch.Restart();
		ReglaCoeficientes regla = Calculos.ReglaDivisibilidadOptima(divisor, longitud, @base);
		stopwatch.Stop();
		results.Add($"{longitud},{stopwatch.ElapsedTicks},{regla.Longitud}");
		if (results.Count == MAX_SIZE) {
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
	for (long @base = 2; @base < 10000000L; @base = (long)(@base * 1.3) + 1) {
		stopwatch.Restart();
		(bool exito, IRegla regla) = Calculos.ReglaDivisibilidadExtendida(divisor, @base);
		stopwatch.Stop();
		results.Add($"{@base},{stopwatch.ElapsedTicks},{regla.Tipo}");
		if (results.Count == MAX_SIZE) {
			DumpResults(writer);
		}
	}
	DumpResults(writer);
}

void Single_Varied_Performance_DivisorIncrement() {
	long @base = 10;
	Stopwatch stopwatch = new();
	string path = filePath + "/Single_Varied_Performance_DivisorIncrement.csv";
	File.Create(path).Close();
	using TextWriter writer = new StreamWriter(path);
	writer.WriteLine("Divisor, Tiempo, Tipo");
	for (long divisor = 2, i = 2; divisor < 1000000L; divisor = (long)Math.Pow(i++,2)) {
		stopwatch.Restart();
		(bool exito, IRegla regla) = Calculos.ReglaDivisibilidadExtendida(divisor, @base);
		stopwatch.Stop();
		results.Add($"{divisor},{stopwatch.ElapsedTicks},{regla.Tipo}");
		DumpResults(writer);
		writer.Flush();
	}
}

//Single_Coefficient_Performance_BaseIncrement();
//Console.WriteLine("Base increment finished");
//Single_Coefficient_Performance_DivisorIncrement();
//Console.WriteLine("Divisor increment finished");
//Single_Coefficient_Performance_LengthIncrement();
//Console.WriteLine("Length increment finished");
//Single_Varied_Performance_BaseIncrement();
//Console.WriteLine("Varied base increment finished");
Single_Varied_Performance_DivisorIncrement();
Console.WriteLine("Varied divisor increment finished");
