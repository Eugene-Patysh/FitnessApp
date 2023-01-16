namespace FitnessApp.Logic.Validators
{
    public interface ICustomValidator<T>
    {
        public void Validate(T objectDto, string ruleSetName);
    }
}
