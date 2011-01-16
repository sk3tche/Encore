using System.Diagnostics.CodeAnalysis;
using Trinity.Core.Security;

namespace Trinity.Encore.Game.Security
{
    /// <summary>
    /// Should not be assigned to any entity. This is a permission reserved
    /// for the console.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711", Justification = "No.")]
    public sealed class ConsolePermission : Permission
    {
    }
}
