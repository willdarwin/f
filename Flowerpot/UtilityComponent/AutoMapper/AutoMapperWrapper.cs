using System;
using AutoMapper;
using AutoMapper.Mappers;

namespace UtilityComponent.AutoMapper
{
    public class AutoMapperWrapper
    {
        private readonly Func<ConfigurationStore> _configurationInit; //= (Func<ConfigurationStore>)(() => new ConfigurationStore((ITypeMapFactory)new TypeMapFactory(), MapperRegistry.AllMappers()));
        private Lazy<ConfigurationStore> _configuration; //= new Lazy<ConfigurationStore>(_configurationInit);
        private readonly Func<IMappingEngine> _mappingEngineInit; //= (Func<IMappingEngine>)(() => (IMappingEngine)new MappingEngine((IConfigurationProvider)_configuration.Value));
        private Lazy<IMappingEngine> _mappingEngine; //= new Lazy<IMappingEngine>(_mappingEngineInit);

        public AutoMapperWrapper()
        {
            _configurationInit = (Func<ConfigurationStore>)(() => new ConfigurationStore((ITypeMapFactory)new TypeMapFactory(), MapperRegistry.AllMappers()));
            _configuration = new Lazy<ConfigurationStore>(_configurationInit);
            _mappingEngineInit = (Func<IMappingEngine>)(() => (IMappingEngine)new MappingEngine((IConfigurationProvider)_configuration.Value));
            _mappingEngine = new Lazy<IMappingEngine>(_mappingEngineInit);
        }

        public IConfiguration Configuration
        {
            get
            {
                return (IConfiguration)ConfigurationProvider;
            }
        }

        IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return (IConfigurationProvider)_configuration.Value;
            }
        }

        public IMappingEngine Engine
        {
            get
            {
                return _mappingEngine.Value;
            }
        }

        public void Initialize(Action<IConfiguration> action)
        {
            Reset();
            action(Configuration);
            Configuration.Seal();
        }

        public void Reset()
        {
            _configuration = new Lazy<ConfigurationStore>(_configurationInit);
            _mappingEngine = new Lazy<IMappingEngine>(_mappingEngineInit);
        }

        public TDestination Map<TDestination>(object source)
        {
            return Engine.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Engine.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Engine.Map<TSource, TDestination>(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions> opts)
        {
            return Engine.Map<TSource, TDestination>(source, destination, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions> opts)
        {
            return Engine.Map<TSource, TDestination>(source, opts);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Engine.Map(source, sourceType, destinationType);
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            return Engine.Map(source, sourceType, destinationType, opts);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return Engine.Map(source, destination, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            return Engine.Map(source, destination, sourceType, destinationType, opts);
        }
    }
}

