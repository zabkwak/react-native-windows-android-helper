using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React_Native_Controller {
	class Program {

		public delegate void Callback(string message);

		private static readonly Dictionary<string, string> VARIABLES = new Dictionary<string, string>() {
			{ "%HOMEPATH%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) }
		};

		static bool exited = false;

		private string _directory;
		private string _reactNativeCommandPath;

		static void Main(string[] args) {
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
			if (args.Length < 2) {
				Console.WriteLine("Invalid count of arguments");
				return;
			}
			string directory = args[0];
			string command = args[1];

			Program program = new Program(directory);
			if (command.Equals("start")) {
				program.Start();
			} else if (command.Equals("clean")) {
				program.Clean();
			} else {
				Console.WriteLine("Invalid command");
			}
			return;
		}

		static void OnProcessExit(object sender, EventArgs e) {
			Console.WriteLine("Process exit");
			exited = true;
			// process.Kill();
		}

		public Program(string directory) {
			_directory = _getPath(directory);
			_reactNativeCommandPath = _getPath(@"%HOMEPATH%\AppData\Roaming\npm\react-native.cmd");
		}

		public void Start() {
			_StartProcess("start", (startData) => {
				if (startData.Equals("React packager ready.")) {
					_StartProcess("run-android", (runData) => {
						if (runData.Equals("BUILD SUCCESSFUL")) {
							_StartProcess("log-android", null);
						}
					});
				}
			});
		}

		public void Clean() {
			Console.WriteLine("Clean not implemented");
		}

		private static string _getPath(string path) {
			foreach (KeyValuePair<string, string> pair in VARIABLES) {
				path = path.Replace(pair.Key, pair.Value);
			}
			return path;
		}

		private Process _StartProcess(string arguments, Callback callback) {
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo();

			process.StartInfo = new ProcessStartInfo {
				WorkingDirectory = _directory,
				FileName = _reactNativeCommandPath,
				Arguments = arguments,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardOutput = true
			};
			process.OutputDataReceived += (sender, a) => {
				if (!exited) {
					Console.WriteLine(a.Data);
					if (callback != null) {
						callback(a.Data);
					}
				}
			};

			process.Start();
			process.BeginOutputReadLine();

			process.WaitForExit();
			return process;
		}
	}
}
