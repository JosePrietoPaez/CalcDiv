﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProgramaDivisibilidad.Recursos {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TextoResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TextoResource() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProgramaDivisibilidad.Recursos.TextoResource", typeof(TextoResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a CalcDivCLI Help:
        ///
        ///- Usage: CalcDivCLI.exe [-&lt;short option name&gt;|--&lt;long option name&gt; [&lt;mandatory arguments ...&gt; [&lt;optional arguments&gt;...]]
        ///
        ///Example: *CalcDiv.exe --json --direct-output 7 10 --named-rule Nombre*
        ///Equivalent to: *CalcDiv.exe -jd 7 10 -n Nombre*
        ///Calculates the divisibility rule of 7 in base 10, gives it the name Nombre and outputs it in JSON.
        ///
        ///- Program options, can be concatenated after a dash: *CalcDiv.exe -ajd 3 4 1* o *CalcDiv.exe -H*, use --help for further info.
        ///
        ///-H: shows this  [resto de la cadena truncado]&quot;;.
        /// </summary>
        public static string Ayuda {
            get {
                return ResourceManager.GetString("Ayuda", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a - Usage: CalcDivCLI.exe [-&amp;lt;short option name&amp;gt;|--&amp;lt;long option name&amp;gt; [&amp;lt;mandatory arguments ...&amp;gt; [&amp;lt;optional arguments&amp;gt;...]]
        ///
        ///Example: *CalcDiv.exe --json --direct-output 7 10 --named-rule Nombre*
        ///Equivalent to: *CalcDiv.exe -jd 7 10 -n Nombre*
        ///Calculates the divisibility rule of 7 in base 10, gives it the name Nombre and outputs it in JSON.
        ///
        ///Use --help for more information about options and arguments or -H for more information about this application..
        /// </summary>
        public static string AyudaCorta {
            get {
                return ResourceManager.GetString("AyudaCorta", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a An unexpected error ocurred, consider creating a new Issue on GitHub if it isn&apos;t present, include the trace above and the arguments provided:
        ///https://github.com/JosePrietoPaez/CalcDiv/issues.
        /// </summary>
        public static string DialogoExcepcionInesperada {
            get {
                return ResourceManager.GetString("DialogoExcepcionInesperada", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Cannot calculate this rule due to an unexpected error, try running with -x option, like: .
        /// </summary>
        public static string DirectoReferirExtendidoErrorInesperado {
            get {
                return ResourceManager.GetString("DirectoReferirExtendidoErrorInesperado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Cannot calculate coefficient rule, the divisor is composed of the prime factors of the base, a rule can be calculated running with -x option, like: .
        /// </summary>
        public static string DirectoReferirExtendidoPotencias {
            get {
                return ResourceManager.GetString("DirectoReferirExtendidoPotencias", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Coefficient rule can be calculated, however, in this case it is recommended to run with -x option, like: .
        /// </summary>
        public static string DirectoReferirExtendidoUsable {
            get {
                return ResourceManager.GetString("DirectoReferirExtendidoUsable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Coefficient rule can be calculated, in this case it is not recommended to use the -x option..
        /// </summary>
        public static string DirectoReferirExtendidoValido {
            get {
                return ResourceManager.GetString("DirectoReferirExtendidoValido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Find rule for seven in base twelve, using two coefficients..
        /// </summary>
        public static string EjemploReglaUno {
            get {
                return ResourceManager.GetString("EjemploReglaUno", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The base must be an integer greater than one..
        /// </summary>
        public static string ErrorBase {
            get {
                return ResourceManager.GetString("ErrorBase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The base must be an integer greater than one and coprime with the divisor..
        /// </summary>
        public static string ErrorBaseCoprima {
            get {
                return ResourceManager.GetString("ErrorBaseCoprima", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The number of coefficients must be a positive integer..
        /// </summary>
        public static string ErrorCoeficientes {
            get {
                return ResourceManager.GetString("ErrorCoeficientes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor must be a non-negative integer.
        ///Keep in mind that numbers divisible by a negative number are divisible by its absolute value..
        /// </summary>
        public static string ErrorDivisor {
            get {
                return ResourceManager.GetString("ErrorDivisor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor must be a positive integer greater than one and coprime with the base.
        ///Two numbers are coprime if their greatest common divisor is greater than one..
        /// </summary>
        public static string ErrorDivisorCoprimo {
            get {
                return ResourceManager.GetString("ErrorDivisorCoprimo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor must be a non-negative integer..
        /// </summary>
        public static string ErrorDivisorExtra {
            get {
                return ResourceManager.GetString("ErrorDivisorExtra", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor, the base and coefficient amount must be positive integers..
        /// </summary>
        public static string ErrorNumerico {
            get {
                return ResourceManager.GetString("ErrorNumerico", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor and the base are not coprime.
        ///Calculating a coefficient in this case will not be implemented.
        ///Run this program with the option-x to obtain a different type of rule, or activate it in dialog mode..
        /// </summary>
        public static string ErrorPrimo {
            get {
                return ResourceManager.GetString("ErrorPrimo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor and the base are not coprime.
        ///Calculating a coefficient in this case will not be implemented..
        /// </summary>
        public static string ErrorPrimoExtra {
            get {
                return ResourceManager.GetString("ErrorPrimoExtra", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a A rule was null when calculated, cannot continue execution..
        /// </summary>
        public static string ErrorReglaNula {
            get {
                return ResourceManager.GetString("ErrorReglaNula", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Unexpected error while starting application..
        /// </summary>
        public static string ErrorTipoVerbo {
            get {
                return ResourceManager.GetString("ErrorTipoVerbo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Cancels dialog mode&apos;s looping behavior. Will one calculate rules once..
        /// </summary>
        public static string HelpAnularBucle {
            get {
                return ResourceManager.GetString("HelpAnularBucle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs a longer help document explaining this application more in-depth..
        /// </summary>
        public static string HelpAyuda {
            get {
                return ResourceManager.GetString("HelpAyuda", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs short document with information on how to run this program..
        /// </summary>
        public static string HelpAyudaCorta {
            get {
                return ResourceManager.GetString("HelpAyudaCorta", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets a value for the base the rule will be applied in.
        ///Defaults to ten, must be greater than one..
        /// </summary>
        public static string HelpBase {
            get {
                return ResourceManager.GetString("HelpBase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets a default base for dialog, skipping input.
        ///Must be a valid integer greater than 1..
        /// </summary>
        public static string HelpBaseDialogo {
            get {
                return ResourceManager.GetString("HelpBaseDialogo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The divisor, base, and optionally the number of coefficients as arguments, not as dialog input..
        /// </summary>
        public static string HelpDirecto {
            get {
                return ResourceManager.GetString("HelpDirecto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets the dividends to apply the rule is successfully obtained.
        ///Must be valid 64-bit integers separated by commas.
        ///The absolute value will be used, this does not affect the result..
        /// </summary>
        public static string HelpDividendo {
            get {
                return ResourceManager.GetString("HelpDividendo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets the divisor to find its divisibility rule.
        ///Obligatory, must be a valid 64-bit non-negative integer..
        /// </summary>
        public static string HelpDivisor {
            get {
                return ResourceManager.GetString("HelpDivisor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets a default divisor for dialog, skipping input.
        ///Must be a valid 64-bit non-negative integer.
        ///If the base is specified, will default to extra rules, unless the divisor and base are coprime..
        /// </summary>
        public static string HelpDivisorDialogo {
            get {
                return ResourceManager.GetString("HelpDivisorDialogo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets the divisor to find its divisibility rule.
        ///Obligatory, must be a list of valid 64-bit non-negative integers..
        /// </summary>
        public static string HelpDivisorVarias {
            get {
                return ResourceManager.GetString("HelpDivisorVarias", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a May return other types of rules, also outputs explanations on how to apply them..
        /// </summary>
        public static string HelpExtendido {
            get {
                return ResourceManager.GetString("HelpExtendido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs coefficient rule as a JSON object, changes made by other flags are not ignored..
        /// </summary>
        public static string HelpJson {
            get {
                return ResourceManager.GetString("HelpJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets a value for the length of the rule if it is a coefficient rule.
        ///Defaults to one, must be a valid positive 32-bit integer..
        /// </summary>
        public static string HelpLongitud {
            get {
                return ResourceManager.GetString("HelpLongitud", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Sets a default rule length for dialog, skipping input.
        ///Must be a valid 32-bit positive integer.
        ///Defaults to coefficient rules, unless extra rules are activated..
        /// </summary>
        public static string HelpLongitudDialogo {
            get {
                return ResourceManager.GetString("HelpLongitudDialogo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Gives the coefficient rule a name, which is included in its output..
        /// </summary>
        public static string HelpNombre {
            get {
                return ResourceManager.GetString("HelpNombre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Without any other options, skips dialogue related to other options, does not affect output..
        /// </summary>
        public static string HelpSaltar {
            get {
                return ResourceManager.GetString("HelpSaltar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs all the valid combinations of rules given a list of divisors, bases and lengths if they are coefficient rules.
        ///The elements in the same lists are separated by commas..
        /// </summary>
        public static string HelpTextMultiple {
            get {
                return ResourceManager.GetString("HelpTextMultiple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs all coefficient rules with elements whose absolute value is less than the divisor..
        /// </summary>
        public static string HelpTodos {
            get {
                return ResourceManager.GetString("HelpTodos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Recieves multiple bases, divisors and coefficients as arguments and outputs the rule formed by every (divisor,base,coefficient) tuple.
        ///All bases, divisors and coefficients must be valid.
        ///Activates direct output mode..
        /// </summary>
        public static string HelpVarias {
            get {
                return ResourceManager.GetString("HelpVarias", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Starts a dialog to create divisibility rules..
        /// </summary>
        public static string HelpVerbDialog {
            get {
                return ResourceManager.GetString("HelpVerbDialog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs the help for this application..
        /// </summary>
        public static string HelpVerbHelp {
            get {
                return ResourceManager.GetString("HelpVerbHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Outputs a single rule given its divisor, base and length, if it is a coefficient rule..
        /// </summary>
        public static string HelpVerbSingle {
            get {
                return ResourceManager.GetString("HelpVerbSingle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Base of the rule: .
        /// </summary>
        public static string MensajeDialogoBase {
            get {
                return ResourceManager.GetString("MensajeDialogoBase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Number of coefficients in the rule: .
        /// </summary>
        public static string MensajeDialogoCoeficientes {
            get {
                return ResourceManager.GetString("MensajeDialogoCoeficientes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Divisor of the rule: .
        /// </summary>
        public static string MensajeDialogoDivisor {
            get {
                return ResourceManager.GetString("MensajeDialogoDivisor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Type &quot;s&quot; to obtain coefficient rules, type a different letter to obtain rules: .
        /// </summary>
        public static string MensajeDialogoExtendido {
            get {
                return ResourceManager.GetString("MensajeDialogoExtendido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The program was interrupted..
        /// </summary>
        public static string MensajeDialogoInterrumpido {
            get {
                return ResourceManager.GetString("MensajeDialogoInterrumpido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Type &quot;s&quot; to output the rule as a JSON, if the rule is not going to be parsed type another letter: .
        /// </summary>
        public static string MensajeDialogoJson {
            get {
                return ResourceManager.GetString("MensajeDialogoJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Name of the rule, can be left empty: .
        /// </summary>
        public static string MensajeDialogoRegla {
            get {
                return ResourceManager.GetString("MensajeDialogoRegla", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Type &quot;s&quot; to calculate another rule: .
        /// </summary>
        public static string MensajeDialogoRepetir {
            get {
                return ResourceManager.GetString("MensajeDialogoRepetir", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Obtained the following rule: .
        /// </summary>
        public static string MensajeDialogoResultado {
            get {
                return ResourceManager.GetString("MensajeDialogoResultado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Type &quot;s&quot; to output all the rules derived from the original, type a different letter to obtain the minimum rule: .
        /// </summary>
        public static string MensajeDialogoTodas {
            get {
                return ResourceManager.GetString("MensajeDialogoTodas", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The rule was calculated..
        /// </summary>
        public static string MensajeFinDirecto {
            get {
                return ResourceManager.GetString("MensajeFinDirecto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You will be asked to input data throught the console, write {0} to interrupt the program..
        /// </summary>
        public static string MensajeInicioDialogo {
            get {
                return ResourceManager.GetString("MensajeInicioDialogo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Divisor: {0}, Base: {1}..
        /// </summary>
        public static string MensajeParametrosDirecto {
            get {
                return ResourceManager.GetString("MensajeParametrosDirecto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Exit message has been detected, execution was interrupted..
        /// </summary>
        public static string MensajeSalidaVoluntaria {
            get {
                return ResourceManager.GetString("MensajeSalidaVoluntaria", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Object is null..
        /// </summary>
        public static string ObjetoNuloMensaje {
            get {
                return ResourceManager.GetString("ObjetoNuloMensaje", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a option.
        /// </summary>
        public static string OptionGroupWord {
            get {
                return ResourceManager.GetString("OptionGroupWord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Option &apos;{0}&apos; is defined in the wrong format..
        /// </summary>
        public static string SentenceBadFormatConversionErrorOption {
            get {
                return ResourceManager.GetString("SentenceBadFormatConversionErrorOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a A value not bound to option name is defined with a bad format..
        /// </summary>
        public static string SentenceBadFormatConversionErrorValue {
            get {
                return ResourceManager.GetString("SentenceBadFormatConversionErrorValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Token &apos;{0}&apos; is not recognized..
        /// </summary>
        public static string SentenceBadFormatTokenError {
            get {
                return ResourceManager.GetString("SentenceBadFormatTokenError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Verb &apos;{0}&apos; is not recognized..
        /// </summary>
        public static string SentenceBadVerbSelectedError {
            get {
                return ResourceManager.GetString("SentenceBadVerbSelectedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a ERROR(S):.
        /// </summary>
        public static string SentenceErrorsHeadingText {
            get {
                return ResourceManager.GetString("SentenceErrorsHeadingText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Display this help screen..
        /// </summary>
        public static string SentenceHelpCommandTextOption {
            get {
                return ResourceManager.GetString("SentenceHelpCommandTextOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Display more information on a specific command..
        /// </summary>
        public static string SentenceHelpCommandTextVerb {
            get {
                return ResourceManager.GetString("SentenceHelpCommandTextVerb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Required option &apos;{0}&apos; is missing..
        /// </summary>
        public static string SentenceMissingRequiredOptionError {
            get {
                return ResourceManager.GetString("SentenceMissingRequiredOptionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a A required value not bound to option name is missing..
        /// </summary>
        public static string SentenceMissingRequiredValueError {
            get {
                return ResourceManager.GetString("SentenceMissingRequiredValueError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Option &apos;{0}&apos; has no value..
        /// </summary>
        public static string SentenceMissingValueOptionError {
            get {
                return ResourceManager.GetString("SentenceMissingValueOptionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Options: {0} are not compatible with {1}..
        /// </summary>
        public static string SentenceMutuallyExclusiveSetErrors {
            get {
                return ResourceManager.GetString("SentenceMutuallyExclusiveSetErrors", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No verb selected..
        /// </summary>
        public static string SentenceNoVerbSelectedError {
            get {
                return ResourceManager.GetString("SentenceNoVerbSelectedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Option &apos;{0}&apos; is defined multiple times..
        /// </summary>
        public static string SentenceRepeatedOptionError {
            get {
                return ResourceManager.GetString("SentenceRepeatedOptionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Required..
        /// </summary>
        public static string SentenceRequiredWord {
            get {
                return ResourceManager.GetString("SentenceRequiredWord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a A sequence option &apos;{0}&apos; is defined with fewer or more items than required..
        /// </summary>
        public static string SentenceSequenceOutOfRangeErrorOption {
            get {
                return ResourceManager.GetString("SentenceSequenceOutOfRangeErrorOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a A sequence value not bound to option name is defined with fewer items than required..
        /// </summary>
        public static string SentenceSequenceOutOfRangeErrorValue {
            get {
                return ResourceManager.GetString("SentenceSequenceOutOfRangeErrorValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Error setting value to option &apos;{0}&apos;: {1}.
        /// </summary>
        public static string SentenceSetValueExceptionError {
            get {
                return ResourceManager.GetString("SentenceSetValueExceptionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Option &apos;{0}&apos; is unknown..
        /// </summary>
        public static string SentenceUnknownOptionError {
            get {
                return ResourceManager.GetString("SentenceUnknownOptionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a USAGE:.
        /// </summary>
        public static string SentenceUsageHeadingText {
            get {
                return ResourceManager.GetString("SentenceUsageHeadingText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Display version information..
        /// </summary>
        public static string SentenceVersionCommandText {
            get {
                return ResourceManager.GetString("SentenceVersionCommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Some rules couldn&apos;t be calculated.
        ///Probably caused by a divisor, base pair not being coprime.
        ///Check the error stream for more information..
        /// </summary>
        public static string VariasMensajeError {
            get {
                return ResourceManager.GetString("VariasMensajeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No rules were calculated.
        ///Might be caused by none of the divisors being coprime with the bases.
        ///Check the error stream for more information..
        /// </summary>
        public static string VariasMensajeErrorTotal {
            get {
                return ResourceManager.GetString("VariasMensajeErrorTotal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The rule wasn&apos;t calculated, should be caused bv the divisor and base not being coprime.
        ///This rule can be ignored..
        /// </summary>
        public static string VariasMensajeVacio {
            get {
                return ResourceManager.GetString("VariasMensajeVacio", resourceCulture);
            }
        }
    }
}
