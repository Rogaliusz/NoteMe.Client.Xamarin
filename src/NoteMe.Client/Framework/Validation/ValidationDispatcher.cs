using System;
using FluentValidation;
using TinyIoC;

namespace NoteMe.Client.Framework.Validation
{
    public interface IValidationDispatcher
    {
        IValidator GetValidator(Type type);
    }   
    
    public class ValidationDispatcher : IValidationDispatcher
    {
        private readonly TinyIoCContainer _container;

        public ValidationDispatcher(TinyIoCContainer container)
        {
            _container = container;
        }
        
        public IValidator GetValidator(Type type)
        {
            try
            {

                var validatorType = typeof(IValidator<>).MakeGenericType(type);
                var validator = _container.Resolve(validatorType) as IValidator;

                return validator;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}