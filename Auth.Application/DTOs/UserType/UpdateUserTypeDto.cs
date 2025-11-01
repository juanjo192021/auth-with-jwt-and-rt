using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.DTOs.UserType
{
    public class UpdateUserTypeDto: CreateUserTypeDto
    {
        public int Id { get; set; } = 0!;
    }
}
