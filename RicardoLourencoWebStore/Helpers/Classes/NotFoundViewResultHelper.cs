using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Classes
{
    public class NotFoundViewResultHelper : ViewResult
    {
        public NotFoundViewResultHelper(string viewName)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
