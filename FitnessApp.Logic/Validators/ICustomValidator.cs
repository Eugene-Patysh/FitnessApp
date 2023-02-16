namespace FitnessApp.Logic.Validators
{
    public interface ICustomValidator<T>
    {
        public void Validate(T objectDto, string ruleSetName);
        public bool IsValid(T objectDto, string ruleSetName, out string message);
    }
}
