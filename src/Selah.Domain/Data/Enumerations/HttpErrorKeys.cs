using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selah.Domain.Data.Dictionaries
{
    public enum HttpErrorKeys
    {
        ResourceNotFound,
        InvalidLoginAttempt,
        UnknownError,
        UnauthorizedAccessToResource,
        PasswordResetMismatch
    }
}
