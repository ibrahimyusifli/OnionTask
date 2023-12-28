using ProniaOnion202.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Application.Abstractions.Services
{
    internal interface IAuthenticationService
    {
        Task Register(RegisterDto dto);
        Task Login(LoginDto dto);

    }
}
