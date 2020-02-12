using System;
using System.Threading.Tasks;

namespace AppAppartamenti.Files
{
    public interface ISaveFile
    {
        Task<string> SaveFiles(string filename, byte[] bytes);
    }
}
