using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class Medicamento : EntidadeBase
    {
        public Medicamento(string nome, string descricao, string fornecedor, int quantidade, int quantidadeCritica, int id) 
        { 
            this.nome = nome;
            this.descricao = descricao;
            this.fornecedor = fornecedor;
            this.quantidade = quantidade;
            this.quantidadeCritica = quantidadeCritica;
            this.id = id;
        }

    }
}
