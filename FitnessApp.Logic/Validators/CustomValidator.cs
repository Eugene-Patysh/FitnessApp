using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class CustomValidator<T> : ICustomValidator<T>
    {
        private readonly IValidator<T> _validator;
        public CustomValidator(IValidator<T> validator)
        {
            _validator = validator;
        }
        public void Validate(T objectDto, string ruleSetName)
        {
            var validationResult = _validator.Validate(objectDto, v => v.IncludeRulesNotInRuleSet().IncludeRuleSets(ruleSetName)); //IncludeAllRuleSets IncludeRuleSets("*")
            if (!validationResult.IsValid)
                throw new Exception(validationResult.ToString());
        }
    }
}
