using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace RockyToy.Contracts.Common.Helpers
{
	public static class IoHelper
	{
		/// <summary>
		///   ref: https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp
		/// </summary>
		public static void CopyFolder(string sourcePath, string destinationPath, IProgress<string> progress, CancellationToken ct)
		{
			//Now Create all of the directories
			foreach (var dirPath in Directory.GetDirectories(sourcePath, "*",
				SearchOption.AllDirectories))
				Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

			//Copy all the files & Replaces any files with the same name
			foreach (var newPath in Directory.GetFiles(sourcePath, "*.*",
				SearchOption.AllDirectories))
			{
				progress?.Report($"Copying {Path.GetFileName(newPath)} ...");
				ct.ThrowIfCancellationRequested();
				File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
			}
		}

		public static void DeleteFolder(string path, IProgress<string> progress)
		{
			var allFiles = GetFiles(path).ToList();
			foreach (var f in allFiles)
			{
				progress?.Report($"Deleting {Path.GetFileName(f)} ...");
				try
				{
					File.Delete(f);
				}
				catch
				{
					// ignored
				}
			}

			try
			{
				Directory.Delete(path, true);
			}
			catch
			{
				// ignored
			}
		}

		/// <summary>
		///   ref: https://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetFiles(string path)
		{
			var queue = new Queue<string>();
			queue.Enqueue(path);
			while (queue.Count > 0)
			{
				path = queue.Dequeue();
				try
				{
					foreach (var subDir in Directory.GetDirectories(path)) queue.Enqueue(subDir);
				}
				catch
				{
					// ignored
				}

				string[] files = null;
				try
				{
					files = Directory.GetFiles(path);
				}
				catch
				{
					// ignored
				}

				if (files == null)
					continue;

				foreach (var t in files) yield return t;
			}
		}

		/// <summary>
		///   ref: https://stackoverflow.com/questions/703281/getting-path-relative-to-the-current-working-directory/703290#703290
		/// </summary>
		/// <param name="filespec"></param>
		/// <param name="folder"></param>
		/// <param name="dirSeparatorChar"></param>
		/// <returns></returns>
		public static string GetRelativePath(string filespec, string folder, char dirSeparatorChar)
		{
			// Folders must end in a slash
			if (!folder.EndsWith(dirSeparatorChar.ToString()))
				folder += dirSeparatorChar;

			if (!filespec.StartsWith(folder))
				throw new IOException($"{filespec} is not within {folder}");

			return filespec.Substring(folder.Length);
		}
	}
}