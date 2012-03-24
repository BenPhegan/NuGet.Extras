using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReplacementFileSystem;
using System.IO;

namespace NuGet.Extras.Tests.TestObjects
{
    public class NugetMockFileSystem : MockFileSystem, IFileSystem
    {
        public void AddFile(string path, System.IO.Stream stream)
        {
            var file = MockFileSystemInfo.CreateFileObject(path, stream);
            this.AddMockFile(file);
        }

        public DateTimeOffset GetCreated(string path)
        {
            throw new NotImplementedException();
        }

        public new IEnumerable<string> GetDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public new IEnumerable<string> GetFiles(string path, string filter)
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IFileSystem.GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastModified(string path)
        {
            throw new NotImplementedException();
        }

        public ILogger Logger
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual Stream OpenFile(string path)
        {
            return base.OpenFile(path,FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        public string Root
        {
            get { throw new NotImplementedException(); }
        }
    }
}
