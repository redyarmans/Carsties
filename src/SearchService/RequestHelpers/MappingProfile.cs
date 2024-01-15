using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers
{
    public class MappingProfile : Profile
    {
        protected internal MappingProfile(string profileName) : base(profileName)
        {
            CreateMap<AuctionCreated, Item>();
        }
    }
}