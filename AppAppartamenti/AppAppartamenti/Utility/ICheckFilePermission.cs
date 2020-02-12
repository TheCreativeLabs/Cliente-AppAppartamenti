using System;
using System.Threading.Tasks;

namespace AppAppartamenti.Files
{
    public interface ICheckFilePermission
    {
        Task<bool> CheckPermission();
    }
}
