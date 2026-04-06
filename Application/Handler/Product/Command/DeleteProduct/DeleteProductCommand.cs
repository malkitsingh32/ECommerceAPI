using Common.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handler.Product.Command.DeleteProduct
{
    public class DeleteProductCommand : IRequest<CommonResultResponseDto<string>>
    {
        public int ProductId { get; set; }
    }
}
