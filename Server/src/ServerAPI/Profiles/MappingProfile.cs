using AutoMapper;
using ServerAPI.DTOs;
using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Profiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisteredUser, RegisteredUserDTO>()
				.ForMember(m => m.IndexNr, c => c.MapFrom(s => s.IndexNum))
				.ForMember(m => m.UniqueCode, c => c.MapFrom(s => s.UniqueCode));

			CreateMap<RecordedEvent, RecordedEventDTO>()
				.ForMember(m => m.RegistryContent, c => c.MapFrom(s => s.RegistryContent));
		}
	}
}
