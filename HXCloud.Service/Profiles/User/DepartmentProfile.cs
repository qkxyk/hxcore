using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentAddViewModel, DepartmentModel>().ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentType, opt => opt.MapFrom(src => (DepartmentType)src.DepartmentType));
            CreateMap<DepartmentUpdateViewModel, DepartmentModel>().ForMember(dest => dest.ModifyTime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name));            //.ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<DepartmentModel, DepartmentData>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DepartmentName))
               .ForMember(dest => dest.Child, opt => opt.Ignore());
        }
        /*
                /// <summary>
                /// 递归创建类型间的映射关系 (Recursively create mappings between types)
                ///created by cqwang
                /// </summary>
                /// <param name="sourceType"></param>
                /// <param name="destinationType"></param>
                public static void CreateNestedMappers(Type sourceType, Type destinationType)
                {
                    PropertyInfo[] sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    PropertyInfo[] destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var destinationProperty in destinationProperties)
                    {
                        PropertyInfo sourceProperty = sourceProperties.FirstOrDefault(prop => NameMatches(prop.Name, destinationProperty.Name));
                        if (sourceProperty == null)
                            continue;

                        Type sourcePropertyType = sourceProperty.PropertyType;
                        Type destinationPropertyType = destinationProperty.PropertyType;


                        if (destinationPropertyType.IsGenericType)
                        {
                            Type destinationGenericType = destinationPropertyType.GetGenericArguments()[0];
                            if (Filter(destinationGenericType))
                                continue;

                            Type sourceGenericType = sourcePropertyType.GetGenericArguments()[0];
                            CreateMappers(sourceGenericType, destinationGenericType);
                        }
                        else
                        {
                            if (Filter(destinationPropertyType))
                                continue;
                            CreateMappers(sourcePropertyType, destinationPropertyType);
                        }
                    }

                    Mapper.CreateMap(sourceType, destinationType);
                }

                /// <summary>
                /// 过滤 (Filter)
                /// </summary>
                /// <param name="type"></param>
                /// <returns></returns>
                static bool Filter(Type type)
                {
                    return type.IsPrimitive || NoPrimitiveTypes.Contains(type.Name);
                }

                static readonly HashSet<string> NoPrimitiveTypes = new HashSet<string>() { "String", "DateTime", "Decimal" };

                private static bool NameMatches(string memberName, string nameToMatch)
                {
                    return String.Compare(memberName, nameToMatch, StringComparison.OrdinalIgnoreCase) == 0;
                }
                */
    }
}
