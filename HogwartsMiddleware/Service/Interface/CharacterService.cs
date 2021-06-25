
using Hogwarts.Middleware.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hogwarts.Middleware.Interfaces.Service
{
    public interface ICharacterService
    {
        Task<CharacterResultDto> Get(Guid id);
        Task<IEnumerable<CharacterResultDto>> GetAllCharacterHouse(string house);
        Task<CharacterResultDto> Post(CharacterInsertDto character);
        Task<CharacterResultDto> Put(CharacterUpdateDto character);
        Task<bool> Delete(Guid id);

        Task<bool> GetHousesApiPotter(Guid id);
    }
}
