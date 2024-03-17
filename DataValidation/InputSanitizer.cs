using System.Text.RegularExpressions;

namespace ModerationProject.DataValidation
{
    public interface IValidator
    {
        string Apply(string toValidate);
    }
    public class SQLValidator : IValidator
    {
        private static readonly Regex tautologyRegex = new("(.*)\\s*=\\s*\\1", RegexOptions.None);
        static private string CheckForBatched(string toValidate) 
        {
            if (toValidate.Contains(';'))
            {
                Console.WriteLine("Potential SQL attack spotted ", toValidate);
                toValidate = toValidate.Split(';')[0];
            }
            return toValidate;
        }
        static private string CheckForTautology(string toValidate)
        {
            Match checkForTautology = tautologyRegex.Match(toValidate);
            if (checkForTautology.Success)
            {
                Console.WriteLine("Found {0} at position {1}", checkForTautology.Value, checkForTautology.Index);
                toValidate = toValidate.Replace(checkForTautology.Value, "");
            }
            return toValidate;
        }
        public string Apply(string toValidate)
        {   
            return CheckForTautology(CheckForBatched(toValidate));
        }
    }
    public class CensoringValidator : IValidator
    {
        public string Apply(string toValidate)
        {
            throw new NotImplementedException();
        }
    }
    public class PhoneNrValidator : IValidator
    {
        public string Apply(string toValidate)
        {
            throw new NotImplementedException();
        }
    }
    public class EmailAddressValidator : IValidator
    {
        public string Apply(string toValidate)
        {
            throw new NotImplementedException();
        }
    }
    public class DateTimeValidator : IValidator
    {
        public string Apply(string toValidate)
        {
            throw new NotImplementedException();
        }
    }
    static public class ValidatorConstructor
    {
        public enum Mode
        {
            SAFE_SQL_COMMAND,
            CENSOR_FORBIDDEN_WORDS,
            VALIDATE_PHONE_NUMBER,
            VALIDATE_EMAIL_ADDRESS,
            VALIDATE_DATE_TIME
        }
        static public IValidator BuildValidator
            (Mode mode) => mode switch
            {
                Mode.SAFE_SQL_COMMAND => new SQLValidator(),
                Mode.CENSOR_FORBIDDEN_WORDS => new CensoringValidator(), /// should recieve a db connection string
                Mode.VALIDATE_PHONE_NUMBER => new PhoneNrValidator(),
                Mode.VALIDATE_EMAIL_ADDRESS => new EmailAddressValidator(),
                Mode.VALIDATE_DATE_TIME => new DateTimeValidator(),
                _ => throw new Exception()
            };
    }
}
