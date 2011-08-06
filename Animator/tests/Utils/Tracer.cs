using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Animator.Common;

namespace Animator.Tests.Utils
{

	#region Tracer

	internal static class Tracer
	{

		#region TracerSection

		internal class TracerSection : IDisposable
		{

			#region TraceSectionResult

			public enum TraceSectionResult
			{
				None,
				Success,
				Error,
				OtherEnded
			}

			#endregion

			#region Static / Constant

			#endregion

			#region Fields

			private TraceSectionResult _Result;

			public string Category;
			public string Name;

			public string BeginMessage;
			public string EndMessage;
			public string SuccessMessage;
			public string ErrorMessage;

			public object[] MessageArgs;

			#endregion

			#region Properties

			public TraceSectionResult Result
			{
				get { return this._Result; }
			}

			public bool Ended
			{
				get { return this._Result > TraceSectionResult.None; }
			}

			#endregion

			#region Constructors

			public TracerSection(string category, string beginMessage = null, string endMessage = null, string successMessage = null, string errorMessage = null, object[] messageArgs = null, bool begin = true)
			{
				this.Category = category;
				this.BeginMessage = beginMessage;
				this.EndMessage = endMessage;
				this.SuccessMessage = successMessage;
				this.ErrorMessage = errorMessage;
				this.MessageArgs = messageArgs;
				if(begin)
					this.Begin();
			}

			public TracerSection(string category, string name, bool begin = true)
			{
				this.Category = category;
				this.Name = name;
				if(begin)
					this.Begin();
			}

			#endregion

			#region Methods

			public bool TryCatch(Action action, bool rethrow = true)
			{
				return this.TryCatch<Exception>(action, rethrow);
			}

			public bool TryCatch<TException>(Action action, bool rethrow = true)
				where TException : Exception
			{
				if(this.Ended)
					throw new InvalidOperationException();
				try
				{
					action();
					this.Succeed();
					return true;
				}
				catch(TException ex)
				{
					this.Error(ex);
					if(rethrow)
						throw;
					return false;
				}
			}

			private string GetMessage(string custom, string namePrefix)
			{
				if(custom != null)
					return String.Format(custom, this.MessageArgs);
				return String.Format("{0} {1}", namePrefix, this.Name ?? "Section");
			}

			public void Begin()
			{
				if(this.Ended)
					throw new InvalidOperationException();
				CategoryLine(this.Category, this.GetMessage(this.BeginMessage, "BEGIN"));
				Trace.Indent();
			}

			public void EndOther()
			{
				this.SetResult(TraceSectionResult.OtherEnded);
			}

			public void Error(Exception ex)
			{
				if(this.Ended)
					throw new InvalidOperationException();
				if(ex == null)
				{
					this.Error();
				}
				else
				{
					CategoryLine(this.Category, "{1}: {0}", ex.GetType().FullName, ex.Message);
					Trace.Indent();
					CategoryLine(this.Category, "Stack Trace: \n{0}", ex.StackTrace);
					Trace.Unindent();
				}
			}

			public void Error()
			{
				this.SetResult(TraceSectionResult.Error);
			}

			public void Succeed()
			{
				this.SetResult(TraceSectionResult.Success);
			}

			private void SetResult(TraceSectionResult result)
			{
				if(this.Ended)
					throw new InvalidOperationException();
				Trace.Unindent();
				switch(result)
				{
				case TraceSectionResult.Success:
					CategoryLine(this.Category, this.GetMessage(this.SuccessMessage, "SUCCESS"));
					break;
				case TraceSectionResult.Error:
					CategoryLine(this.Category, this.GetMessage(this.ErrorMessage, "ERROR"));
					break;
				case TraceSectionResult.OtherEnded:
					CategoryLine(this.Category, this.GetMessage(this.EndMessage, "ENDED (Other)"));
					break;
				}
				this._Result = result;
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				if(!this.Ended)
					this.Succeed();
				GC.SuppressFinalize(this);
			}

			#endregion
		}

		#endregion

		public static bool TryCatchCategorySection(string category, Action action, string beginMessage = null, string endMessage = null, string successMessage = null, string errorMessage = null, object[] messageArgs = null, bool rethrow = true)
		{
			var section = new TracerSection(category, beginMessage, endMessage, successMessage, errorMessage, messageArgs);
			return section.TryCatch(action, rethrow);
		}

		public static bool TryCatchSection(Action action, string beginMessage = null, string endMessage = null, string successMessage = null, string errorMessage = null, object[] messageArgs = null, bool rethrow = true)
		{
			var section = new TracerSection(null, beginMessage, endMessage, successMessage, errorMessage, messageArgs);
			return section.TryCatch(action, rethrow);
		}

		public static bool TryCatchCategorySection(string category, string name, Action action, bool rethrow = true)
		{
			return TryCatchCategorySection<Exception>(category, name, action, rethrow);
		}

		public static bool TryCatchCategorySection<TException>(string category, string name, Action action, bool rethrow = true)
			where TException : Exception
		{
			var section = new TracerSection(category, name, begin: true);
			return section.TryCatch<TException>(action, rethrow);
		}

		public static bool TryCatchSection(string name, Action action, bool rethrow = true)
		{
			var section = new TracerSection(null, name, begin: true);
			return section.TryCatch(action, rethrow);
		}

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		internal static void CategoryIndent(string category, string message, params object[] args)
		{
			CategoryLine(category, message, args);
			Trace.Indent();
		}

		internal static void CategoryUnindent(string category, string message, params object[] args)
		{
			Trace.Unindent();
			CategoryLine(category, message, args);
		}

		internal static void CategoryLine(string category, string message, params object[] args)
		{
			Trace.WriteLine(String.Format(message, args), category);
		}

		internal static void Line(string message, params object[] args)
		{
			Trace.WriteLine(String.Format(message, args));
		}

		internal static void Line()
		{
			Trace.WriteLine(String.Empty);
		}

		internal static IDisposable Section(string beginMessage, string endMessage, params object[] args)
		{
			return CategorySection(null, beginMessage, endMessage, args);
		}

		internal static IDisposable CategorySection(string category)
		{
			return CategorySection(category, null, null);
		}

		internal static IDisposable CategorySection(string category, string beginMessage)
		{
			return CategorySection(category, beginMessage, null);
		}

		internal static IDisposable CategorySection(string category, string beginMessage, string endMessage, params object[] args)
		{
			CategoryLine(category, beginMessage ?? "Begin", args);
			Trace.Indent();
			return new ActionBlock(
				() =>
				{
					Trace.Unindent();
					CategoryLine(category, endMessage ?? "End", args);
				});
		}
	}

	#endregion

}
