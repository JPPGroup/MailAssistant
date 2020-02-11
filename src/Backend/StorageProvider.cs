using Jpp.Common.Backend;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Jpp.AddIn.MailAssistant.Backend
{
    internal class StorageProvider : IStorageProvider
    {
        public void CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> OpenFileForRead(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> OpenFileForWrite(string path, bool createIfMissing)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> OpenSharedForRead(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> OpenSharedForWrite(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public Task CopyFile(string origin, string destination)
        {
            throw new NotImplementedException();
        }
    }
}
