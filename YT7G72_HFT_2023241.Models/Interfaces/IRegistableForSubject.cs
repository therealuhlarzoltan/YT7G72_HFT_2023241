using System.Collections.Generic;

namespace YT7G72_HFT_2023241.Models
{
    public interface IRegistableForSubject
    {
        ICollection<Subject> RegisteredSubjects { get; set; }
    }
}
