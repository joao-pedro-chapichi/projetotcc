using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace projetotcc.Model
{
    public class ModelRegistro
    {
        private int _idRegistro;
        private DateTime _data;
        private int _id;
        private string _acao;
        private DateTime _dataInicio;
        private DateTime _dataFim;

        public int IdRegistro
        {
            get { return _idRegistro; }
            set { _idRegistro = value; }
        }

        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Acao
        {
            get { return _acao; }
            set { _acao = value; }
        }

        public DateTime DataInicio
        {
            get { return _dataInicio; }
            set { _dataInicio = value; }
        }

        public DateTime DataFim
        {
            get { return _dataFim; }
            set { _dataFim = value; }
        }

    }
}
