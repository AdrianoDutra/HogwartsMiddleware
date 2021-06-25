using System;

namespace Hogwarts.Middleware.Dtos
{
    public class CharacterResultDto
    {
        public Guid id { get; set; }
       
        public string name { get; set; }

        
        public string role { get; set; }

        
        public string school { get; set; }

       
        public string house { get; set; }

        public string patronus { get; set; }
    }
}
