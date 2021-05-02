using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IMemoryCacheService
    {
        string Get(string key);

        void Set(string key, string value);
    }
}