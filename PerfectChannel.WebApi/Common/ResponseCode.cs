using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Common
{
    public enum ResponseCode
    {
        Success = 200,
        ErrorServer = 300,
        ErrorValidation = 400,
        TimeOut = 500
    }
}
