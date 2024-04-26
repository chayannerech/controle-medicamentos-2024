using ControleMedicamentos.ConsoleApp.Compartilhado;
namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class Medicamento : Entidades
    {
        public Medicamento(string nome, string descricao, string fornecedor, int quantidade, int quantidadeCritica) 
        { 
            this.nome = nome;
            this.descricao = descricao;
            this.fornecedor = fornecedor;
            this.quantidade = quantidade;
            this.quantidadeCritica = quantidadeCritica;
        }

    }
}
