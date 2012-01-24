using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region LoadCommandMappingsFileNode

	[PluginInfo(Name = CommandNames.Nodes.Mappings,
		Category = CommandNames.Categories.Command,
		Version = TEShared.Names.Versions.Add + TEShared.Names.AND + TEShared.Names.Versions.File,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		Help = "Add Command Mappings loaded from a file containing encoded mapping strings")]
	public sealed class LoadCommandMappingsFileNode : IPluginBase, IDisposable, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Import]
		private ILogger _Logger;

		[Config("Clear On Load", IsSingle = true, DefaultValue = 1)]
		private ISpread<bool> _ClearOnLoadConfig;

		[Config("Part Separator", IsSingle = true, DefaultString = "|")]
		private ISpread<string> _PartSeparatorConfig;

		[Input("File Path", StringType = StringType.Filename, FileMask = "Data files (*.dat)|*.dat|Text files (*.txt)|*.txt")]
		private ISpread<string> _FilePathInput;

		[Input("Load", IsBang = true, IsSingle = true)]
		private IDiffSpread<bool> _DoLoadInput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void DoLoad_Changed(IDiffSpread<bool> doLoadInput)
		{
			if(!doLoadInput[0])
				return;
			if(_ClearOnLoadConfig[0])
				CommandRegistry.ClearMappings();
			var seps = _PartSeparatorConfig.ToArray();
			if(String.IsNullOrEmpty(seps[0]))
			{
				_Logger.Log(new InvalidOperationException("Command mapping part separator is required"));
				return;
			}
			var count = 0;
			foreach(var filePath in _FilePathInput)
			{
				if(!String.IsNullOrEmpty(filePath))
				{
					try
					{
						var lines = File.ReadAllLines(filePath);
						foreach(var line in lines)
						{
							if(!String.IsNullOrEmpty(line))
							{
								var parts = line.Split(seps, StringSplitOptions.None);
								if(parts.Length >= 3)
								{
									var mapping = CommandUtil.ParseMapping(parts[0], parts[1], parts[2], _Logger);
									if(mapping != null)
									{
										CommandRegistry.AddMapping(mapping, _Logger);
										count++;
									}
								}
							}
						}
					}
					catch(FileNotFoundException ex)
					{
						_Logger.Log(ex);
					}
				}
			}
			_Logger.Log(LogType.Message, "Loaded {0} command mappings", count);
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_DoLoadInput.Changed += this.DoLoad_Changed;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_DoLoadInput != null)
				_DoLoadInput.Changed -= this.DoLoad_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
