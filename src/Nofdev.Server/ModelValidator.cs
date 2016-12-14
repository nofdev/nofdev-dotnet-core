using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nofdev.Server
{
    public class ModelValidator : IObjectModelValidator
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ValidatorCache _validatorCache;
        private readonly IModelValidatorProvider _validatorProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultObjectValidator"/>.
        /// </summary>
        /// <param name="modelMetadataProvider">The <see cref="IModelMetadataProvider"/>.</param>
        /// <param name="validatorProviders">The list of <see cref="IModelValidatorProvider"/>.</param>
        public ModelValidator(
            IModelMetadataProvider modelMetadataProvider,
            IList<IModelValidatorProvider> validatorProviders)
        {
            if (modelMetadataProvider == null)
            {
                throw new ArgumentNullException(nameof(modelMetadataProvider));
            }

            if (validatorProviders == null)
            {
                throw new ArgumentNullException(nameof(validatorProviders));
            }

            _modelMetadataProvider = modelMetadataProvider;
            _validatorCache = new ValidatorCache();

            _validatorProvider = new CompositeModelValidatorProvider(validatorProviders);
        }
        #region Implementation of IObjectModelValidator

        public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix, object model)
        {
            var visitor = new ValidationVisitor(
                actionContext,
                _validatorProvider,
                _validatorCache,
                _modelMetadataProvider,
                validationState);

            var metadata = model == null ? null : _modelMetadataProvider.GetMetadataForType(model.GetType());
            visitor.Validate(metadata, prefix, model);
        }

        #endregion
    }
}
