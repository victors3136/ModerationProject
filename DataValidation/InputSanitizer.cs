using System.Text.RegularExpressions;

namespace ModerationProject.DataValidation
{
    public interface IValidator
    {
        string Apply(string toValidate);
    }
    public class SQLValidator : IValidator
    {
        private static readonly Regex tautologyRegex = new("\\b(.*)\\s*=\\s*\\1\\b", RegexOptions.None);
        static private string CheckForBatched(string toValidate) 
        {
            if (toValidate.Contains(';'))
            {
                Console.WriteLine("Spotted \";\"");
                toValidate = toValidate.Split(';')[0];
            }
            return toValidate;
        }
        static private string CheckForTautology(string toValidate)
        {
            Match checkForTautology = tautologyRegex.Match(toValidate);
            if (checkForTautology.Success)
            {
                Console.WriteLine("Spotted \"{0}\"", checkForTautology.Value);
                toValidate = toValidate.Replace(checkForTautology.Value, "");
            }
            return toValidate;
        }
        public string Apply(string toValidate)
        {
            Console.WriteLine("Input: {0}", toValidate);
            toValidate = CheckForBatched(toValidate);
            Console.WriteLine("After step 1: {0}", toValidate);
            toValidate = CheckForTautology(toValidate);
            Console.WriteLine("After step 2: {0}", toValidate);
            return toValidate;
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
