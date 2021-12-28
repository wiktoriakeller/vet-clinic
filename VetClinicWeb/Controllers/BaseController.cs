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
    }
}
