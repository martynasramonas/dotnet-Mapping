﻿using AutoMapper;

namespace Affecto.Mapping.AutoMapper
{
    public abstract class TwoWayMappingProfile<TSource, TDestination> : MappingProfile<TSource, TDestination>, ITwoWayMappingProfile<TSource, TDestination>
    {
        protected TwoWayMappingProfile()
        {
            IMappingExpression<TDestination, TSource> destinationToSourceMap = CreateMap<TDestination, TSource>();
            ConfigureMapping(destinationToSourceMap);
        }

        public new ITwoWayMapper<TSource, TDestination> CreateMapper(IMapper mapper)
        {
            return new TwoWayMapper(mapper);
        }

        protected abstract void ConfigureMapping(IMappingExpression<TDestination, TSource> map);

        public class TwoWayMapper : Mapper, ITwoWayMapper<TSource, TDestination>
        {
            public TwoWayMapper(IMapper mapper)
                : base(mapper)
            {
            }

            public virtual TSource Map(TDestination source)
            {
                return mapper.Map<TDestination, TSource>(source);
            }

            public virtual TSource Map(TDestination source, params (string parameterName, object parameterValue)[] parameters)
            {
                return mapper.Map<TDestination, TSource>(source, opt =>
                {
                    if (parameters != null)
                    {
                        foreach ((string parameterName, object parameterValue) parameter in parameters)
                        {
                            if (!string.IsNullOrWhiteSpace(parameter.parameterName) && parameter.parameterValue != null)
                            {
                                opt.Items.Add(parameter.parameterName, parameter.parameterValue);
                            }
                        }
                    }
                });
            }
        }
    }
}