using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projetotcc.Model
{
    public class ModelFuncionario
    {

        private int _id;


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }


    }
}
