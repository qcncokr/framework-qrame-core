using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Qrame.CoreFX.Diagnostics
{
	public sealed class FileStreamWithBackup : FileStream
	{
		public long MaxFileLength
		{
			get
			{
				return maxFileLength;
			}
		}

		public int MaxFileCount
		{
			get { return maxFileCount; }
		}
		public bool CanSplitData
		{
			get
			{
				return isSplitData;
			}
			set
			{
				isSplitData = value;
			}
		}

		private long maxFileLength;
		private int maxFileCount;
		private string logFileDirectory;
		private string fileBase;
		private string fileExtension;
		private int fileDecimals;
		private bool isSplitData;
		private int nextFileIndex;

		// FileStreamWithBackup fs = new FileStreamWithBackup(Assembly.GetEntryAssembly()?.GetName().Name + "Trace.log");
		// fs.CanSplitData = false;
        // TextWriterTraceListener listener = new TextWriterTraceListener(fs);
		// Trace.AutoFlush = true;
        // Trace.Listeners.Add(listener);
        // Trace.Assert(true, "Assertion that should not appear");
        // Trace.Assert(false, "Assertion that should appear in a trace file");
        // Trace.WriteLine(123, "Category 1");
        // Trace.WriteLineIf(true, 456, "Category 2");
        // Trace.WriteLineIf(false, 789, "Category 3 (should not appear)");
		public FileStreamWithBackup(string filePath, long maxFileLength = 104857600, int maxFileCount = 10, FileMode mode = FileMode.Append) : base(filePath, BaseFileMode(mode), FileAccess.Write)
		{
			Init(filePath, maxFileLength, maxFileCount, mode);
		}

		public override bool CanRead { get { return false; } }

		public override void Write(byte[] array, int offset, int count)
		{
			int actualCount = Math.Min(count, array.GetLength(0));
			if (Position + actualCount <= maxFileLength)
			{
				base.Write(array, offset, count);
			}
			else
			{
				if (CanSplitData == true)
				{
					int partialCount = (int)(Math.Max(maxFileLength, Position) - Position);
					base.Write(array, offset, partialCount);
					offset += partialCount;
					count = actualCount - partialCount;
				}
				else
				{
					if (count > maxFileLength)
					{
						// Buffer size exceeds maximum file length
						return;
					}
				}

				BackupAndResetStream();
				Write(array, offset, count);
			}
		}

		private void Init(string filePath, long maxFileLength, int maxFileCount, FileMode mode)
		{
			if (maxFileLength <= 0)
			{
				maxFileLength = 104857600;
			}

			if (maxFileCount <= 0)
			{
				maxFileCount = 10;
			}

			this.maxFileLength = maxFileLength;
			this.maxFileCount = maxFileCount;
			isSplitData = true;

			string fullPath = Path.GetFullPath(filePath);
			logFileDirectory = Path.GetDirectoryName(fullPath);
			fileBase = Path.GetFileNameWithoutExtension(fullPath);
			fileExtension = Path.GetExtension(fullPath);

			fileDecimals = 1;
			int decimalBase = 10;
			while (decimalBase < this.maxFileCount)
			{
				++fileDecimals;
				decimalBase *= 10;
			}

			switch (mode)
			{
				case FileMode.Create:
				case FileMode.CreateNew:
				case FileMode.Truncate:
					for (int i = 0; i < this.maxFileCount; ++i)
					{
						string file = GetBackupFileName(i);
						if (File.Exists(file) == true)
						{
							File.Delete(file);
						}
					}
					break;

				default:
					for (int i = 0; i < this.maxFileCount; ++i)
					{
						if (File.Exists(GetBackupFileName(i)) == true)
							nextFileIndex = i + 1;
					}
					if (nextFileIndex == this.maxFileCount)
						nextFileIndex = 0;
					Seek(0, SeekOrigin.End);
					break;
			}
		}

		private void BackupAndResetStream()
		{
			Flush();
			File.Copy(Name, GetBackupFileName(nextFileIndex), true);
			SetLength(0);

			++nextFileIndex;

			if (nextFileIndex >= maxFileCount)
			{
				nextFileIndex = 0;
			}
		}

		private string GetBackupFileName(int index)
		{
			StringBuilder format = new StringBuilder();
			format.AppendFormat("D{0}", fileDecimals);
			StringBuilder sb = new StringBuilder();
			if (fileExtension.Length > 0)
			{
				sb.AppendFormat("{0}{1}{2}", fileBase, index.ToString(format.ToString()), fileExtension);
			}
			else
			{
				sb.AppendFormat("{0}{1}", fileBase, index.ToString(format.ToString()));
			}
			return Path.Combine(logFileDirectory, sb.ToString());
		}

		private static FileMode BaseFileMode(FileMode mode)
		{
			return mode == FileMode.Append ? FileMode.OpenOrCreate : mode;
		}
	}

	// TextWriterTraceListenerWithDateTime listener = new TextWriterTraceListenerWithDateTime("MyTrace.txt");
	// Trace.Listeners.Add(listener);
	// Trace.WriteLine(123, "Category 1");
	public class TextWriterTraceListenerWithDateTime : TextWriterTraceListener
	{
		public override void WriteLine(string message)
		{
			base.Write(DateTime.Now.ToString() + " ");
			base.WriteLine(message);
		}
	}
}
