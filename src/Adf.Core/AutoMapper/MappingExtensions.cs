using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using global::Adf.Core.Reflection;
using global::AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Adf.Core.AutoMapper
{
    
    
        /// <summary>
        /// Represents the extensions to map entity to model and vise versa
        /// </summary>
        public static class MappingExtensions
        {
            /////// <summary>
            /////// Converts an object to another using AutoMapper library. Creates a new object of <typeparamref name="TDestination"/>.
            /////// There must be a mapping between objects before calling this method.
            /////// </summary>
            /////// <typeparam name="TDestination">Type of the destination object</typeparam>
            /////// <param name="source">Source object</param>
            ////public static TDestination MapTo<TDestination>(this object source)
            ////{
            ////    return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
            ////}
            /////// <summary>
            /////// Execute a mapping from the source object to the existing destination object
            /////// There must be a mapping between objects before calling this method.
            /////// </summary>
            /////// <typeparam name="TSource">Source type</typeparam>
            /////// <typeparam name="TDestination">Destination type</typeparam>
            /////// <param name="source">Source object</param>
            /////// <param name="destination">Destination object</param>
            /////// <returns></returns>
            ////public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            ////{
            ////    return AutoMapperConfiguration.Mapper.Map(source, destination);
            ////}
            public static IMappingExpression<TDestination, TMember> Ignore<TDestination, TMember, TResult>(this IMappingExpression<TDestination, TMember> mappingExpression, Expression<Func<TMember, TResult>> destinationMember)
            {
                return mappingExpression.ForMember(destinationMember, opts => opts.Ignore());
            }
            public static void RegisterMapperProfiles(this IServiceCollection services, List<Assembly> assemblies)
            {
                var profileTypes = ReflectionUtils.GetAssemblySubClassesByBaseType(typeof(Profile), assemblies);
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    foreach (var profileType in profileTypes)
                    {
                        mc.AddProfile((Profile)ReflectionUtils.CreateInstanceFromType(profileType));
                    }
                });
                IMapper mapper = mappingConfig.CreateMapper();
                services.AddSingleton(mapper);
            }
        }
    

}
