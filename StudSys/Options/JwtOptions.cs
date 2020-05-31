using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StudSys.Options
{
    public class JwtOptions
    {
        public const string Issuer = "EvaSys Issuer"; // издатель токенов
        public const string Audience = "EvaSys Client"; // потребитель токена
        const string Key = "1qwedsaf894DFskg0564SfslfkHGEEr194";
        public const int LifeTime = 1;


        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
