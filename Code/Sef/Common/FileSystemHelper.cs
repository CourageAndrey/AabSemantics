using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Sef.Localization;

namespace Sef.Common
{
	public static class FileSystemHelper
	{
		#region ShellAPI

		public static void ShellExecute(this String exe, String parameters = null, Boolean waitForExit = false)
		{
			var process = new Process { StartInfo = { FileName = exe, Arguments = parameters } };
			process.Start();
			if (waitForExit)
			{
				process.WaitForExit();
			}
		}

		public static void ShellOpen(this String file)
		{
			var info = new ShellExecuteInfo();
			info.Size = Marshal.SizeOf(info);
			info.File = file;
			info.Show = SW_NORMAL;
			if (!ShellExecuteEx(ref info))
			{
				Int32 error = Marshal.GetLastWin32Error();
				if (error != WIN32_ERROR_CANCELLED)
				{
					throw Marshal.GetExceptionForHR(error);
				}
			}
		}

		private const Int32 SW_NORMAL = 1;
		private const Int32 WIN32_ERROR_CANCELLED = 1223;

		[DllImport("shell32.dll", SetLastError = true)]
		private extern static Boolean ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

        // ReSharper disable UnusedField.Compiler
        // ReSharper disable NotAccessedField.Local
        [Serializable]
		private struct ShellExecuteInfo
		{
			public Int32 Size;
			public UInt32 Mask;
			public IntPtr hwnd;
			public String Verb;
			public String File;
			public String Parameters;
			public String Directory;
			public UInt32 Show;
			public IntPtr InstApp;
			public IntPtr IDList;
			public String Class;
			public IntPtr hkeyClass;
			public UInt32 HotKey;
			public IntPtr Icon;
			public IntPtr Monitor;
		}
        // ReSharper restore NotAccessedField.Local
        // ReSharper restore UnusedField.Compiler

		#endregion

		#region Handle waiting

		public static Boolean WaitFile(this String fileName, Int32 delayMs = 1000, Int32? waitMs = null)
		{
			if (File.Exists(fileName))
			{
				Boolean error;
				Double waitingSpan = 0;
				do
				{
					try
					{
						using (new FileStream(fileName, FileMode.Open, FileAccess.Write))
						{
							error = false;
						}
					}
					catch
					{
						error = true;
						waitingSpan += delayMs;
						Thread.Sleep(delayMs);
					}
				} while (error && (waitMs.HasValue && waitingSpan < waitMs.Value));
				return !error;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region File operations

		#region Delete

		public static void DeleteAnyway(this FileSystemInfo target)
		{
			if (target is FileInfo)
			{
				deleteFile(target as FileInfo);
			}
			else if (target is DirectoryInfo)
			{
				deleteDirectory(target as DirectoryInfo);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static void DeleteFileSystemAnyway(this String target)
		{
			target.ToFileSystemInfo().DeleteAnyway();
		}

		private static void deleteDirectory(DirectoryInfo directory)
		{
			directory.Delete(true);
		}

		private static void deleteFile(FileInfo file)
		{
			file.Delete();
		}

		#endregion

		#region Copy

		public static void CopyAnyway(this FileSystemInfo target, String outputFolder)
		{
			if (target is FileInfo)
			{
				copyFile(target as FileInfo, outputFolder);
			}
			else if (target is DirectoryInfo)
			{
				copyDirectory(target as DirectoryInfo, outputFolder);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static void CopyFileSystemAnyway(this String target, String outputFolder)
		{
			target.ToFileSystemInfo().CopyAnyway(outputFolder);
		}

		private static void copyDirectory(DirectoryInfo directory, String outFolder)
		{
			var output = new DirectoryInfo(Path.Combine(outFolder, directory.Name));
			if (!output.Exists)
			{
				output.Create();
			}
			foreach (var childDirectory in directory.GetDirectories())
			{
				copyDirectory(childDirectory, output.FullName);
			}
			foreach (var childFile in directory.GetFiles())
			{
				copyFile(childFile, output.FullName);
			}
		}

		private static void copyFile(FileInfo file, String outFolder)
		{
			file.CopyTo(Path.Combine(outFolder, file.Name), true);
		}

		#endregion

		#region Move

		public static void MoveAnyway(this FileSystemInfo target, String outputFolder)
		{
			if (target is FileInfo)
			{
				moveFile(target as FileInfo, outputFolder);
			}
			else if (target is DirectoryInfo)
			{
				moveDirectory(target as DirectoryInfo, outputFolder);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static void MoveFileSystemAnyway(this String target, String outputFolder)
		{
			target.ToFileSystemInfo().MoveAnyway(outputFolder);
		}

		private static void moveDirectory(DirectoryInfo directory, String outFolder)
		{
			CopyAnyway(directory, outFolder);
			directory.DeleteAnyway();
		}

		private static void moveFile(FileInfo file, String outFolder)
		{
			file.MoveTo(Path.Combine(outFolder, file.Name));
		}

		#endregion

		#region FileSystem object

		public static FileSystemInfo ToFileSystemInfo(this String path)
		{
			return path.IsFolderPath() ? (FileSystemInfo) new DirectoryInfo(path) : new FileInfo(path);
		}

		public static bool IsFolderPath(this String path)
		{
			return Directory.Exists(path) && !File.Exists(path);
		}

		#endregion

		#endregion

		#region Size in bytes

		public static String FormatSize(this Int64 size, Boolean decimalPowers = false)
		{
			var powers = decimalPowers ? powers10 : powers2;
			Int32 idx = powers.Length - 1;
			while (idx >= 0)
			{
				if (size >= powers[idx])
				{
					return string.Format(
						(idx > 0) ? "{0:N1} {1}" : "{0:N0} {1}",
						(idx > 0) ? size / powers[idx] : size,
						names[idx].Invoke());
				}
				else
				{
					idx--;
				}
			}
			throw new ArgumentOutOfRangeException("size");
		}

		private static readonly Double[] powers2 = { 0d, Math.Pow(2, 10), Math.Pow(2, 20), Math.Pow(2, 30), Math.Pow(2, 40) };
		private static readonly Double[] powers10 = { 0d, Math.Pow(10, 3), Math.Pow(10, 6), Math.Pow(10, 9), Math.Pow(10, 12) };
		private static readonly Func<String>[] names =
		{
			() => Language.Current.Common.DataSizeB,
			() => Language.Current.Common.DataSizeKB,
			() => Language.Current.Common.DataSizeMB,
			() => Language.Current.Common.DataSizeGB,
			() => Language.Current.Common.DataSizeTB
		};

		#endregion

		#region Encoding

		private static readonly Dictionary<Byte[], Encoding> signatures = new Dictionary<Byte[], Encoding>
		{
			{ new byte[] { 0xFF, 0xFE, 0x00, 0x00 }, Encoding.UTF32 },
			{ new byte[] { 0x00, 0x00, 0xFE, 0xFF } , new UTF32Encoding(true, true) },
			{ new byte[] { 0xEF, 0xBB, 0xBF }, Encoding.UTF8 },
			{ new byte[] { 0xFF, 0xFE }, Encoding.Unicode },
			{ new byte[] { 0xFE, 0xFF }, Encoding.BigEndianUnicode },
		};
		private static readonly EqualityComparer<Byte> byteComparer;

		public static IEnumerable<Encoding> StronglyTypedEncodings
		{
			get { return signatures.Values; }
		}

		public static Encoding GetFileEncoding(this String path)
		{
			var fileInfo = new FileInfo(path);

			if (fileInfo.Length >= 3)
			{
				var signature = new Byte[4];
				using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					stream.Read(signature, 0, 4);
				}

				foreach (var sign in signatures.Keys)
				{
					if (equal(signature, sign))
					{
						return signatures[sign];
					}
				}
			}

			return Encoding.Default;
		}

		private static Boolean equal(Byte[] array4, Byte[] arraySign)
		{
			for (Int32 i = 0; i < arraySign.Length; i++)
			{
				if (!byteComparer.Equals(array4[i], arraySign[i]))
				{
					return false;
				}
			}
			return true;
		}

		static FileSystemHelper()
		{
			byteComparer = EqualityComparer<Byte>.Default;
		}

		#endregion
	}
}
