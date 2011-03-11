﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CleanBuildOutput
{
	class Program
	{
		static int Main(string[] args)
		{
			//Console.Error.WriteLine("args length: {0}", args.Length);
			//for(var i = 0; i < args.Length; i++)
			//    Console.Error.WriteLine("arg {0}: '{1}'", i, args[i]);
			//return 1;
			if(args.Length != 2)
				return Usage();
			var projectDir = args[0];
			var projectName = args[1];
			if(String.IsNullOrEmpty(projectDir) || String.IsNullOrEmpty(projectName))
				return Usage();
			if(projectDir[projectDir.Length - 1] != Path.DirectorySeparatorChar)
				projectDir = projectDir + Path.DirectorySeparatorChar;
			Console.WriteLine("ProjectDir: '{0}' ProjectName: '{1}'", projectDir, projectName);
			var files = Directory.GetFiles(projectDir + "bin", "*.*", SearchOption.AllDirectories)
				//.Where(f => !f.ToLower().Contains(projectName + ".dll") && !f.ToLower().Contains(projectName + ".pdb"))
				.Where(f => !f.EndsWith(projectName + ".dll", StringComparison.OrdinalIgnoreCase) &&
							!f.EndsWith(projectName + ".pdb", StringComparison.OrdinalIgnoreCase) &&
							!f.EndsWith(projectName + ".xml", StringComparison.OrdinalIgnoreCase))
				.ToList();
			//foreach(var file in files)
			//    Console.Error.WriteLine("file: '{0}'", file);
			//return 1;
			files.AddRange(Directory.GetFiles(projectDir + "obj", "*.*", SearchOption.AllDirectories));
			foreach(var file in files)
			{
				Console.WriteLine("Deleting file '{0}'...", file);
				File.Delete(file);
			}
			return 0;
		}

		static int Usage()
		{
			Console.Error.WriteLine("Usage: CleanBuildOutput.exe PROJECT_DIR PROJECT_NAME");
			return 1;
		}
	}
}
