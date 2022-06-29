using System.ComponentModel.DataAnnotations;

namespace Project.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}