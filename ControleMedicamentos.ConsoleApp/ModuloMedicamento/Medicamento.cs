namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    internal class Medicamento
    {
        public string nome { get; set; }
        public string descricao { get; set; } 
        public string fornecedor { get; set; }
        public int quantidade { get; set; }
        public int quantidadeCritica { get; set; }

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
