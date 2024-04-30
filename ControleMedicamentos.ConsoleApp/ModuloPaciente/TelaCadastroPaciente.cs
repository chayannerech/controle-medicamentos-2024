using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
namespace Controlepacientes.ConsoleApp.ModuloPaciente
{
    internal class TelaCadastroPaciente : TelaBase
    {
        public TelaCadastroPaciente(RepositorioBase repositorio, string nome, string NOME)
        {
            this.repositorio = repositorio;
            tipoEntidade = nome;
            tipoEntidadePlural = NOME;
        }

        protected override void CadastrarItem(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("PACIENTE");
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            int index = repositorio.EsteItemExiste(nome);
            if (index != -1) ItemJaExiste(ref sair, ref repetir);
            else
            {
                string cpf = RecebeString(" Informe o cpf: ");
                string endereco = RecebeString(" Informe o endereço: ");
                string cartaoSUS = RecebeString(" Informe o cartão SUS: ");
                var novoPaciente = new Paciente(nome, cpf, endereco, cartaoSUS, repositorio.contador + 1);

                repositorio.Cadastrar(novoPaciente);
                RealizadoComSucesso("cadastrado");
            }
        }

        #region Métodos Auxiliares
        #region Visualizar
        protected override void CabecalhoVisualizar() => Notificação(ConsoleColor.Blue, "\n--------------------------------------------------------------\n ID\t| Nome\t| CPF\t\t| Endereço\t| Cartão SUS\n--------------------------------------------------------------\n");
        protected override void ListaItensParaVisualizar(int i) => Console.Write($" {repositorio.entidade[i].id}\t| {repositorio.entidade[i].nome}\t| {repositorio.entidade[i].cpf}\t\t| {repositorio.entidade[i].endereco}\t\t| {repositorio.entidade[i].cartaoSUS}\n--------------------------------------------------------------\n");
        #endregion
        #region Edição
        protected override void MenuEdicao(ref bool sair, ref bool repetir, TelaBase telaMedicamento, TelaBase telaPaciente, int indexEditar)
        {
            CabecalhoEntidade("PACIENTES");
            var objetoAuxiliar = repositorio.entidade[indexEditar]; bool editado = true;
            VisualizarParaEdicao(objetoAuxiliar);
            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. CPF\n 3. endereço\n 4. cartão SUS\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1":
                    string nome = RecebeString("\n Informe o novo nome: ");
                    if (repositorio.EsteItemExiste(nome) == -1) objetoAuxiliar.nome = nome;
                    else
                    {
                        ItemJaExiste(ref sair, ref repetir);
                        editado = false;
                    }
                    break;
                case "2": objetoAuxiliar.cpf = RecebeString("\n Informe o novo CPF: "); break;
                case "3": objetoAuxiliar.endereco = RecebeString("\n Informe o novo endereço: "); break;
                case "4": objetoAuxiliar.cartaoSUS = RecebeString("\n Informe o novo cartão SUS: "); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
            if (opcao == "") repetir = true;

            if (!sair && !repetir)
            {
                var editarPaciente = new Paciente(objetoAuxiliar.nome, objetoAuxiliar.cpf, objetoAuxiliar.endereco, objetoAuxiliar.cartaoSUS, objetoAuxiliar.id);
                repositorio.Editar(editarPaciente, indexEditar);
                if (editado) RealizadoComSucesso("editado");
            }
        }
        protected override void VisualizarParaEdicao(EntidadeBase objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            CabecalhoVisualizar();
            Console.Write($" {objetoAuxiliar.id}\t| {objetoAuxiliar.nome}\t| {objetoAuxiliar.cpf}\t\t| {objetoAuxiliar.endereco}\t\t| {objetoAuxiliar.cartaoSUS}\n--------------------------------------------------------------\n\n");
        }
        #endregion
        #endregion
    }
}
