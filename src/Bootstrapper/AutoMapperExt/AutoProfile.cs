using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Nofdev.Bootstrapper.AutoMapperExt
{
    public class AutoProfile : Profile
    {
        public string[] DtoPostfixes { get; set; } =  new[] { "InputDTO","DTO" };

        public void Configure(Assembly dtoAssembly, Assembly entityAssembly)
        {
            var dtoTypes = dtoAssembly.GetTypes().Where(m => DtoPostfixes.Any(t => m.Name.EndsWith(t))).ToList();
            var entityTypes = entityAssembly.GetTypes().ToList();
            dtoTypes.ForEach(t =>
            {
                var matchedEntityTypes = entityTypes.Where(m => t.Name.StartsWith(m.Name)).ToList();
                foreach (var entityType in matchedEntityTypes)
                {
                    foreach (var postfix in DtoPostfixes)
                    {
                        if (entityType.Name + postfix == t.Name)
                        {
                            CreateMap(t, entityType);
                            CreateMap(entityType, t);
                        }
                    }
                }
            });
        }
    }
}
