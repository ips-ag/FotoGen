using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FotoGen.Common;
using MediatR;

namespace FotoGen.Application.UseCases.GeneratePhoto
{
    public class GeneratePhotoCommand : IRequest<BaseResponse<GeneratePhotoResponse?>>
    {
        public string ModelName { get; set; } = default!;
        public string Prompt { get; set; } = default!;
    }
}
