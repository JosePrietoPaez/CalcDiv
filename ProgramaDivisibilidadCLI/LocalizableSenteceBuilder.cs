using CommandLine;
using CommandLine.Text;
using ProgramaDivisibilidad.Recursos;

namespace ProgramaDivisibilidad {
	internal class LocalizableSentenceBuilder : SentenceBuilder {
		public override Func<string> RequiredWord {
			get { return () => TextoResource.SentenceRequiredWord; }
		}

		public override Func<string> ErrorsHeadingText {
			// Cannot be pluralized
			get { return () => TextoResource.SentenceErrorsHeadingText; }
		}

		public override Func<string> UsageHeadingText {
			get { return () => TextoResource.SentenceUsageHeadingText; }
		}

		public override Func<bool, string> HelpCommandText {
			get {
				return isOption => isOption
					? TextoResource.SentenceHelpCommandTextOption
					: TextoResource.SentenceHelpCommandTextVerb;
			}
		}

		public override Func<bool, string> VersionCommandText {
			get { return _ => TextoResource.SentenceVersionCommandText; }
		}

		public override Func<string> OptionGroupWord {
			get { return () => TextoResource.OptionGroupWord; }
		}

		public override Func<Error, string> FormatError {
			get {
				return error => {
					switch (error.Tag) {
						case ErrorType.BadFormatTokenError:
							return string.Format(TextoResource.SentenceBadFormatTokenError, ((BadFormatTokenError)error).Token);
						case ErrorType.MissingValueOptionError:
							return string.Format(TextoResource.SentenceMissingValueOptionError, ((MissingValueOptionError)error).NameInfo.NameText);
						case ErrorType.UnknownOptionError:
							return string.Format(TextoResource.SentenceUnknownOptionError, ((UnknownOptionError)error).Token);
						case ErrorType.MissingRequiredOptionError:
							var errMisssing = ((MissingRequiredOptionError)error);
							return errMisssing.NameInfo.Equals(NameInfo.EmptyName)
									   ? TextoResource.SentenceMissingRequiredOptionError
									   : string.Format(TextoResource.SentenceMissingRequiredOptionError, errMisssing.NameInfo.NameText);
						case ErrorType.BadFormatConversionError:
							var badFormat = ((BadFormatConversionError)error);
							return badFormat.NameInfo.Equals(NameInfo.EmptyName)
									   ? TextoResource.SentenceBadFormatConversionErrorValue
									   : string.Format(TextoResource.SentenceBadFormatConversionErrorOption, badFormat.NameInfo.NameText);
						case ErrorType.SequenceOutOfRangeError:
							var seqOutRange = ((SequenceOutOfRangeError)error);
							return seqOutRange.NameInfo.Equals(NameInfo.EmptyName)
									   ? TextoResource.SentenceSequenceOutOfRangeErrorValue
									   : string.Format(TextoResource.SentenceSequenceOutOfRangeErrorOption,
											seqOutRange.NameInfo.NameText);
						case ErrorType.BadVerbSelectedError:
							return string.Format(TextoResource.SentenceBadVerbSelectedError, ((BadVerbSelectedError)error).Token);
						case ErrorType.NoVerbSelectedError:
							return TextoResource.SentenceNoVerbSelectedError;
						case ErrorType.RepeatedOptionError:
							return string.Format(TextoResource.SentenceRepeatedOptionError, ((RepeatedOptionError)error).NameInfo.NameText);
						case ErrorType.SetValueExceptionError:
							var setValueError = (SetValueExceptionError)error;
							return string.Format(TextoResource.SentenceSetValueExceptionError, setValueError.NameInfo.NameText, setValueError.Exception.Message);
					}
					throw new InvalidOperationException();
				};
			}
		}

		public override Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors {
			get {
				return errors => {
					var bySet = from e in errors
								group e by e.SetName into g
								select new { SetName = g.Key, Errors = g.ToList() };

					var msgs = bySet.Select(
						set => {
							var names = string.Join(
								string.Empty,
								(from e in set.Errors select string.Format("'{0}', ", e.NameInfo.NameText)).ToArray());
							var namesCount = set.Errors.Count();

							var incompat = String.Join(
								String.Empty,
								(from x in
									 (from s in bySet where !s.SetName.Equals(set.SetName) from e in s.Errors select e)
									.Distinct()
								 select string.Format("'{0}', ", x.NameInfo.NameText)).ToArray());
							//TODO: Pluralize by namesCount
							return
								string.Format(TextoResource.SentenceMutuallyExclusiveSetErrors,
									names.Substring(0, names.Length - 2), incompat.Substring(0, incompat.Length - 2));
						}).ToArray();
					return string.Join(Environment.NewLine, msgs);
				};
			}
		}
	}
}
