using System;

namespace EscuelaWeb.Models
{
    public abstract class ObjetoEscuelaBase
    {
        public string Id { get; set; }
        
        public virtual string Nombre { get; set; } //Virtual->que puede ser reescrito por las clases hijos.

        public ObjetoEscuelaBase()
        {
            
        }

        public override string ToString()
        {
            return $"{Nombre},{Id}";
        }
    }
}