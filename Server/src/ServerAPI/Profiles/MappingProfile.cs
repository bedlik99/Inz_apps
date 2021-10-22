using AutoMapper;
using ServerAPI.DTOs;
using ServerAPI.Entities;
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
			CreateMap<RegisteredUser, RegisteredLabUserDTO>()
				.ForMember(m => m.Email, c => c.MapFrom(s => s.Email))
				.ForMember(m => m.UniqueCode, c => c.MapFrom(s => s.UniqueCode));

			CreateMap<RecordedEvent, RecordedEventDTO>()
				.ForMember(m => m.RegistryContent, c => c.MapFrom(s => s.RegistryContent));
		}
	}
}
