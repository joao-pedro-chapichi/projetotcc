using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projetotcc.Model
{
    public class ModelFuncionario
    {
        // Declarando os atributos
        private int _id;
        private string _nome;

        // Encapsulamento dos atributos
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }


    }
}
