using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Extension methods for the DirectoryInfo class
    /// </summary>
    public static class DirectoryInfoExtensions
    {

        /// <summary>
        /// Gets all files in the @this matching one of the several (!) supplied patterns (instead of just one in the regular implementation).
        /// </summary>
        /// <param name="@this">The @this.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns>The matching files</returns>
        /// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.</remarks>
        /// <example>
        /// <code>
        /// var files = @this.GetFiles("*.txt", "*.xml");
        /// </code></example>
        public static FileInfo[] GetFiles(this DirectoryInfo @this, params string[] patterns)
        {
            var files = new List<FileInfo>();
            foreach (var pattern in patterns)
            {
                files.AddRange(@this.GetFiles(pattern));
            }
            return files.ToArray();
        }

        /// <summary>
        /// Searches the provided @this recursively and returns the first file matching the provided pattern.
        /// </summary>
        /// <param name="@this">The @this.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The found file</returns>
        /// <example>
        /// <code>
        /// var @this = new DirectoryInfo(@"c:\");
        /// var file = @this.FindFileRecursive("win.ini");
        /// </code></example>
        public static FileInfo FindFileRecursive(this DirectoryInfo @this, string pattern)
        {
            var files = @this.GetFiles(pattern);
            if (files.Length > 0) return files[0];

            foreach (var subDirectory in @this.GetDirectories())
            {
                var foundFile = subDirectory.FindFileRecursive(pattern);
                if (foundFile != null) return foundFile;
            }
            return null;
        }

        /// <summary>
        /// Searches the provided @this recursively and returns the first file matching to the provided predicate.
        /// </summary>
        /// <param name="@this">The @this.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The found file</returns>
        /// <example>
        /// <code>
        /// var @this = new DirectoryInfo(@"c:\");
        /// var file = @this.FindFileRecursive(f => f.Extension == ".ini");
        /// </code></example>
        public static FileInfo FindFileRecursive(this DirectoryInfo @this, Func<FileInfo, bool> predicate)
        {
            foreach (var file in @this.GetFiles())
            {
                if (predicate(file)) return file;
            }

            foreach (var subDirectory in @this.GetDirectories())
            {
                var foundFile = subDirectory.FindFileRecursive(predicate);
                if (foundFile != null) return foundFile;
            }
            return null;
        }

        /// <summary>
        /// Searches the provided @this recursively and returns the all files matching the provided pattern.
        /// </summary>
        /// <param name="@this">The @this.</param>
        /// <param name="pattern">The pattern.</param>
        /// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.</remarks>
        /// <returns>The found files</returns>
        /// <example>
        /// <code>
        /// var @this = new DirectoryInfo(@"c:\");
        /// var files = @this.FindFilesRecursive("*.ini");
        /// </code></example>
        public static FileInfo[] FindFilesRecursive(this DirectoryInfo @this, string pattern)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(@this, pattern, foundFiles);
            return foundFiles.ToArray();
        }

        private static void FindFilesRecursive(DirectoryInfo @this, string pattern, List<FileInfo> foundFiles)
        {
            foundFiles.AddRange(@this.GetFiles(pattern));
            @this.GetDirectories().ForEach(d => FindFilesRecursive(d, pattern, foundFiles));
        }

        /// <summary>
        /// Searches the provided @this recursively and returns the all files matching to the provided predicate.
        /// </summary>
        /// <param name="@this">The @this.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The found files</returns>
        /// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.</remarks>
        /// <example>
        /// <code>
        /// var @this = new DirectoryInfo(@"c:\");
        /// var files = @this.FindFilesRecursive(f => f.Extension == ".ini");
        /// </code></example>
        public static FileInfo[] FindFilesRecursive(this DirectoryInfo @this, Func<FileInfo, bool> predicate)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(@this, predicate, foundFiles);
            return foundFiles.ToArray();
        }

        private static void FindFilesRecursive(DirectoryInfo @this, Func<FileInfo, bool> predicate, List<FileInfo> foundFiles)
        {
            foundFiles.AddRange(@this.GetFiles().Where(predicate));
            @this.GetDirectories().ForEach(d => FindFilesRecursive(d, predicate, foundFiles));
        }

        /// <summary>
        /// DirectoryInfo내의 모든 파일을 지정한 경로로 복사 합니다.
        /// </summary>
        /// <param name="source">DirectoryInfo 타입</param>
        /// <param name="destination">복사 대상 경로</param>
        /// <param name="recursive">하위 폴더 복사여부</param>
        public static void CopyTo(this DirectoryInfo source, string destination, bool recursive)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            DirectoryInfo target = new DirectoryInfo(destination);
            if (!source.Exists)
            {
                throw new DirectoryNotFoundException("Source @this not found: " + source.FullName);
            }
            if (!target.Exists)
            {
                target.Create();
            }

            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }

            if (!recursive)
            {
                return;
            }

            foreach (var @this in source.GetDirectories())
            {
                CopyTo(@this, Path.Combine(target.FullName, @this.Name), recursive);
            }
        }

        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName);
        }

        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
        }

        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory, entryNameEncoding);
        }

        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName);
        }

        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName, compressionLevel, includeBaseDirectory);
        }

        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName, compressionLevel, includeBaseDirectory, entryNameEncoding);
        }
    }
}
