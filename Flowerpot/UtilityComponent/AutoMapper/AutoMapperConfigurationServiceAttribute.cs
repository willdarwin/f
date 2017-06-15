using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Collections.ObjectModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace UtilityComponent.AutoMapper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AutoMapperConfigurationServiceAttribute : AutoMapperConfigurationAttribute, IOperationBehavior, IServiceBehavior, IContractBehavior
    {
        public AutoMapperConfigurationServiceAttribute(Type profileType)
            : base(profileType)
        { }

        public AutoMapperConfigurationServiceAttribute(Type profileType, Type profileType2)
            : base(profileType, profileType2)
        { }

        public AutoMapperConfigurationServiceAttribute(Type profileType, Type profileType2, Type profileType3)
            : base(profileType, profileType2, profileType3)
        { }

        public AutoMapperConfigurationServiceAttribute(Type profileType, Type profileType2, Type profileType3, Type profileType4)
            : base(profileType, profileType2, profileType3, profileType4)
        { }

        #region IOperationBehavior
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation) { }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation) { }

        public void Validate(OperationDescription operationDescription) { }
        #endregion

        #region IServiceBehavior
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        #endregion

        #region IContractBehavior
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime) { }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime) { }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }
        #endregion
    }
}
