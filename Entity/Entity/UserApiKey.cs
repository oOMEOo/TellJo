using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entity
{
    public class UserApiKey
    {
        [Key]
        public int ID { get; set; }

        [NotNull]
        public string ApiKey { get; set; }

        [NotNull]
        [StringLength(maximumLength: 450)]
        public string UserId { get; set; }
    }
}
