﻿/*
    Copyright (C) 2011 de4dot@gmail.com

    This file is part of de4dot.

    de4dot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    de4dot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with de4dot.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;
using de4dot.deobfuscators;

namespace de4dot {
	public class Program {
		static IList<IDeobfuscatorInfo> deobfuscatorInfos = createDeobfuscatorInfos();

		static IList<IDeobfuscatorInfo> createDeobfuscatorInfos() {
			return new List<IDeobfuscatorInfo> {
				new de4dot.deobfuscators.Unknown.DeobfuscatorInfo(),
				new de4dot.deobfuscators.CliSecure.DeobfuscatorInfo(),
				new de4dot.deobfuscators.Dotfuscator.DeobfuscatorInfo(),
				new de4dot.deobfuscators.Eazfuscator.DeobfuscatorInfo(),
				new de4dot.deobfuscators.SmartAssembly.DeobfuscatorInfo(),
			};
		}

		public static int main(StartUpArch startUpArch, string[] args) {
			Utils.startUpArch = startUpArch;

			try {
				Console.OutputEncoding = new UTF8Encoding(false);

				Log.n("");
				Log.n("de4dot v{0} (BETA) Copyright (C) 2011 de4dot@gmail.com", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
				Log.n("Latest version and source code: https://github.com/0xd4d/de4dot");
				Log.n("");

				var options = new FilesDeobfuscator.Options();
				parseCommandLine(args, options);
				new FilesDeobfuscator(options).doIt();
			}
			catch (UserException ex) {
				Log.e("ERROR: {0}", ex.Message);
			}
			catch (Exception ex) {
				var line = new string('-', 78);
				Log.e("\n\nERROR: Caught an exception:\n\n");
				Log.e(line);
				Log.e("Message: {0}", ex.Message);
				Log.e("Type: {0}", ex.GetType());
				Log.e(line);
				Log.e("\n\nStack trace:\n{0}", ex.StackTrace);
				return 1;
			}

			return 0;
		}

		static void parseCommandLine(string[] args, FilesDeobfuscator.Options options) {
			new CommandLineParser(deobfuscatorInfos, options).parse(args);

			Log.vv("Args:");
			Log.indent();
			foreach (var arg in args)
				Log.vv("{0}", Utils.toCsharpString(arg));
			Log.deIndent();
		}
	}
}