using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Logic
{
    public class ObjectNotFoundException : Exception
    {
        public int Id { get; }
        public Type Type {  get; }
        public ObjectNotFoundException(int id, Type type) : base()
        { 
            Id = id;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type.Name} with ID of {Id} was not found";
        }
    }
}
