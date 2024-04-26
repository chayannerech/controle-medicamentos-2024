using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class Requisicao : Entidades
    {
        public Requisicao(string medicamento, string paciente, int posologia, DateTime dataValidade, int id)
        {
            this.medicamento = medicamento;
            this.paciente = paciente;
            this.posologia = posologia;
            this.dataValidade = dataValidade;
            this.id = id;
        }
    }
}
