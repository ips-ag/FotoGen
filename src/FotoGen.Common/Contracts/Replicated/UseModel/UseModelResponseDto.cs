using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoGen.Common.Contracts.Replicated.UseModel
{
    public class UseModelResponseDto
    {
        public string StreamUrl { get; set; } = default!;
        public string OutputFormat { get; set; } = default!;
    }
}
