using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace VetClinicWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected string GetExceptionMessage(int number)
        {
            switch (number)
            {
                case 2292:
                    return "is used in diffrent table";
                default:
                    return "can't be modified or deleted";
            }
        }
    }
}
