using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AabSemantics.Utils
{
	/// <summary>
	/// Asynchronous file access. The <see cref="File"/> shortcuts are synchronous on the platforms
	/// this library targets, so the streams are opened for overlapped access here and the bytes are
	/// moved with the asynchronous methods, which releases the calling thread for the round trip.
	/// </summary>
	internal static class AsyncFile
	{
		private const Int32 BufferSize = 4096;

		/// <summary>Reads a whole file.</summary>
		/// <param name="fileName">Path to read from.</param>
		/// <param name="cancellationToken">Cancels waiting for the disk.</param>
		/// <returns>The file's content.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static async Task<Byte[]> ReadAllBytesAsync(String fileName, CancellationToken cancellationToken = default)
		{
			using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
			{
				var bytes = new Byte[file.Length];

				int read = 0;
				while (read < bytes.Length)
				{
					int chunk = await file.ReadAsync(bytes, read, bytes.Length - read, cancellationToken).ConfigureAwait(false);
					if (chunk == 0)
					{
						// the file has been truncated since it was opened
						Array.Resize(ref bytes, read);
						break;
					}
					read += chunk;
				}

				return bytes;
			}
		}

		/// <summary>Writes a whole file, overwriting it.</summary>
		/// <param name="fileName">Path to write to.</param>
		/// <param name="bytes">Content to write.</param>
		/// <param name="cancellationToken">Cancels waiting for the disk.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static async Task WriteAllBytesAsync(String fileName, Byte[] bytes, CancellationToken cancellationToken = default)
		{
			using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, useAsync: true))
			{
				await file.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
