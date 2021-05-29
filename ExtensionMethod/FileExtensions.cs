using System;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// Extension methods for the FileInfo and FileInfo-Array classes
    /// </summary>
    public static class FileExtensions
    {
        public static void OpenFileWithAssociatedApplication(string @this)
        {
            //// 기본 메모장 편집기 열기
            //System.Diagnostics.Process.Start(@"c:\textfile.txt");

            //// 기본 이미지 뷰어 열기
            //System.Diagnostics.Process.Start(@"c:\image.jpg");

            //// 기본 웹 브라우저 열기
            //System.Diagnostics.Process.Start("http://www.csharp-examples.net");

            //// 기본 pdf 리더기 열기
            //System.Diagnostics.Process.Start(@"c:\document.pdf");

            try
            {
                System.Diagnostics.Process.Start(@this);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("지정된 파일 이름이 없습니다.");
            }
            catch (Win32Exception e)
            {
                throw new Win32Exception("@this이 실행 파일(.exe)이 아닙니다.");
            }
            catch (Exception e)
            {
                throw new ObjectDisposedException("프로세스 개체가 이미 삭제된 것 같습니다.");
            }
        }

        /// <example>
        /// <code>
        /// var @this = new FileInfo(@"c:\test.txt");
        /// @this.Rename("test2.txt");
        /// </code></example>
        public static FileInfo Rename(this FileInfo @this, string newName)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(@this.FullName), newName);
            @this.MoveTo(filePath);
            return @this;
        }

        /// <example>
        /// <code>
        /// var @this = new FileInfo(@"c:\test.txt");
        /// @this.RenameFileWithoutExtension("test3");
        /// </code></example>
        public static FileInfo RenameFileWithoutExtension(this FileInfo @this, string newName)
        {
            var fileName = string.Concat(newName, @this.Extension);
            @this.Rename(fileName);
            return @this;
        }

        /// <example>
        /// <code>
        /// var @this = new FileInfo(@"c:\test.txt");
        /// @this.ChangeExtension("xml");
        /// </code></example>
        public static FileInfo ChangeExtension(this FileInfo @this, string newExtension)
        {
            var fileName = string.Concat(Path.GetFileNameWithoutExtension(@this.FullName), newExtension);
            @this.Rename(fileName);
            return @this;
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.ChangeExtensions("tmp");
        /// </code></example>
        public static FileInfo[] ChangeExtensions(this FileInfo[] files, string newExtension)
        {
            files.ForEach(f => f.ChangeExtension(newExtension));
            return files;
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.Delete()
        /// </code></example>
        public static void Delete(this FileInfo[] files)
        {
            files.Delete(true);
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.Delete()
        /// </code></example>
        public static void Delete(this FileInfo[] files, bool consolidateExceptions)
        {
            List<Exception> exceptions = null;

            foreach (var @this in files)
            {
                try
                {
                    @this.Delete();
                }
                catch (Exception e)
                {
                    if (consolidateExceptions)
                    {
                        if (exceptions == null) exceptions = new List<Exception>();
                        exceptions.Add(e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if ((exceptions != null) && (exceptions.Count > 0))
            {
                throw new Exception(
                    "Error while deleting one or several files, see InnerExceptions array for details."
                );
            }
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// var copiedFiles = files.CopyTo(@"c:\temp\");
        /// </code></example>
        public static FileInfo[] CopyTo(this FileInfo[] files, string targetPath)
        {
            return files.CopyTo(targetPath, true);
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// var copiedFiles = files.CopyTo(@"c:\temp\");
        /// </code></example>
        public static FileInfo[] CopyTo(this FileInfo[] files, string targetPath, bool consolidateExceptions)
        {
            var copiedfiles = new List<FileInfo>();
            List<Exception> exceptions = null;

            foreach (var @this in files)
            {
                try
                {
                    var fileName = Path.Combine(targetPath, @this.Name);
                    copiedfiles.Add(@this.CopyTo(fileName));
                }
                catch (Exception e)
                {
                    if (consolidateExceptions)
                    {
                        if (exceptions == null)
                            exceptions = new List<Exception>();
                        exceptions.Add(e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if ((exceptions != null) && (exceptions.Count > 0))
            {
                throw new Exception(
                    "Error while copying one or several files, see InnerExceptions array for details."
                );
            }

            return copiedfiles.ToArray();
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.MoveTo(@"c:\temp\");
        /// </code></example>
        public static FileInfo[] MoveTo(this FileInfo[] files, string targetPath)
        {
            return files.MoveTo(targetPath, true);
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.MoveTo(@"c:\temp\");
        /// </code></example>
        public static FileInfo[] MoveTo(this FileInfo[] files, string targetPath, bool consolidateExceptions)
        {
            List<Exception> exceptions = null;

            foreach (var @this in files)
            {
                try
                {
                    var fileName = Path.Combine(targetPath, @this.Name);
                    @this.MoveTo(fileName);
                }
                catch (Exception e)
                {
                    if (consolidateExceptions)
                    {
                        if (exceptions == null) exceptions = new List<Exception>();
                        exceptions.Add(e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if ((exceptions != null) && (exceptions.Count > 0))
            {
                throw new Exception(
                    "Error while moving one or several files, see InnerExceptions array for details."
                );
            }

            return files;
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.SetAttributes(FileAttributes.Archive);
        /// </code></example>
        public static FileInfo[] SetAttributes(this FileInfo[] files, FileAttributes attributes)
        {
            foreach (var @this in files)
            {
                @this.Attributes = attributes;
            }
            return files;
        }

        /// <example>
        /// <code>
        /// var files = directory.GetFiles("*.txt", "*.xml");
        /// files.SetAttributesAdditive(FileAttributes.Archive);
        /// </code></example>
        public static FileInfo[] SetAttributesAdditive(this FileInfo[] files, FileAttributes attributes)
        {
            foreach (var @this in files)
            {
                @this.Attributes = (@this.Attributes | attributes);
            }
            return files;
        }

        public static void ExtractZipFileToDirectory(this FileInfo @this, string destinationDirectoryName)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName);
        }

        public static void ExtractZipFileToDirectory(this FileInfo @this, string destinationDirectoryName, Encoding entryNameEncoding)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName, entryNameEncoding);
        }

        public static void ExtractZipFileToDirectory(this FileInfo @this, DirectoryInfo destinationDirectory)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectory.FullName);
        }

        public static void ExtractZipFileToDirectory(this FileInfo @this, DirectoryInfo destinationDirectory, Encoding entryNameEncoding)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectory.FullName, entryNameEncoding);
        }

        public static ZipArchive OpenReadZipFile(this FileInfo @this)
        {
            return ZipFile.OpenRead(@this.FullName);
        }

        public static ZipArchive OpenZipFile(this FileInfo @this, ZipArchiveMode mode)
        {
            return ZipFile.Open(@this.FullName, mode);
        }

        public static ZipArchive OpenZipFile(this FileInfo @this, ZipArchiveMode mode, Encoding entryNameEncoding)
        {
            return ZipFile.Open(@this.FullName, mode, entryNameEncoding);
        }
    }
}
