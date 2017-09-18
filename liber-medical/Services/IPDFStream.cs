using System.IO;

namespace libermedical.Services
{
    public interface IPDFStream
    {
        Stream GetStream(string filename);
        void Save(string filename, byte[] data);
    }
}
