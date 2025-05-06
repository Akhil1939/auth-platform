using System.ComponentModel.DataAnnotations;

namespace API.Utils.Attributes;

public class NoProfanityAttribute : ValidationAttribute
{
    private readonly string[] _bannedWords = { "spam", "scam", "offensive" };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string message)
        {
            foreach (string word in _bannedWords)
            {
                if (message.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult($"Message contains inappropriate word: {word}");
                }
            }
        }
        return ValidationResult.Success;
    }
}