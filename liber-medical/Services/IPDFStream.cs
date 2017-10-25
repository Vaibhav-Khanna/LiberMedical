using System.IO;

namespace libermedical.Services
{
    public interface IPDFStream
    {
        Stream GetStream(string path);
        void Save(string filename, byte[] data);
    }
}
