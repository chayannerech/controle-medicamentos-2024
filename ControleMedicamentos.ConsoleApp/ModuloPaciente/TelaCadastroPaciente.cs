using ControleMedicamentos.ConsoleApp.ModuloPaciente;

namespace Controlepacientes.ConsoleApp.ModuloPaciente
{
    internal class TelaCadastroPaciente
    {
        RepositorioPaciente repositorioPaciente = new RepositorioPaciente();

        public void MenuPaciente(ref bool sair)
        {
            bool repetir = false;
            do
            {
                CabecalhoPaciente();
                Console.WriteLine("------------------------------------------------");

                repetir = false;
                string opcao = RecebeString("\n        1. Cadastrar um novo paciente\n\t    2. Pesquisar paciente\n\t   3. Visualizar pacientes\n\t      4. Editar paciente\n\t     5. Excluir paciente\n\n\t R. Retornar ao menu principal\n\t\t   S. Sair\n------------------------------------------------\n\n Digite: ");
                switch (opcao)
                {
                    case "1": CadastroPaciente(ref sair); break;
                    case "2": PesquisaPaciente(ref sair, ref repetir); break;
                    case "3": VisualizarPaciente(ref sair, ref repetir); break;
                    case "4": EditarPaciente(ref sair, ref repetir); break;
                    case "5": ExcluirPaciente(ref sair, ref repetir); break;
                    case "S": sair = true; break;
                    case "R": break;
                    default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
                }
            }
            while (repetir);
        }
        public void CadastroPaciente(ref bool sair)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string nome = RecebeString("\n Informe o nome: ");
            int index = repositorioPaciente.PacienteJaExiste(nome);
            if (index != -1) PacienteJaExiste(repositorioPaciente, index, ref sair);
            else
            {
                string cpf = RecebeString(" Informe a descrição: ");
                string endereco = RecebeString(" Informe o fornecedor: ");
                string cartaoSUS = RecebeString(" Informe a quantidade em estoque: ");
                repositorioPaciente.Cadastrar(nome, cpf, endereco, cartaoSUS);
                RealizadoComSucesso("cadastrado");
            }
        }
        public void PesquisaPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            string PesquisarIndex = RecebeString("\t\t    Pesquisar\n------------------------------------------------\n\n Informe o nome do paciente: ");
            int index = repositorioPaciente.PesquisarIndex(PesquisarIndex);

            if (index != -1) Pesquisa(repositorioPaciente, index);
            else PacienteNaoExiste();

            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void VisualizarPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t  Visualizar");

            if (repositorioPaciente.NaoHaPacientes()) SemPacientes(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioPaciente);
                ParaRetornarAoMenu(ref sair, ref repetir);
            }
        }
        public void EditarPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t    Editar");

            if (repositorioPaciente.NaoHaPacientes()) SemPacientes(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioPaciente);
                int indexEditar = repositorioPaciente.PesquisarIndex(RecebeString("\n Informe o nome do paciente a editar: "));

                if (indexEditar != -1) NomeValidoParaEdicao(repositorioPaciente, indexEditar, ref sair, ref repetir);
                else
                {
                    PacienteNaoExiste();
                    ParaRetornarAoMenu(ref sair, ref repetir);
                }
            }
        }
        public void ExcluirPaciente(ref bool sair, ref bool repetir)
        {
            CabecalhoPaciente();
            Console.WriteLine("\t\t    Excluir");

            if (repositorioPaciente.NaoHaPacientes()) SemPacientes(ref sair, ref repetir);
            else
            {
                Visualizar(repositorioPaciente);
                int indexExcluir = repositorioPaciente.PesquisarIndex(RecebeString("\n Informe o nome do paciente a excluir: "));

                if (indexExcluir != -1)
                {
                    repositorioPaciente.Excluir(indexExcluir);
                    RealizadoComSucesso("excluido");
                }
                else
                {
                    PacienteNaoExiste();
                    ParaRetornarAoMenu(ref sair, ref repetir);
                }
            }
        }

        //Auxiliar Menu
        public void CabecalhoPaciente()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------\n  Controle de Medicamentos dos Postos de Saúde\n------------------------------------------------");
            Console.WriteLine("\t    GESTÃO DE PACIENTES");
        }
        public string RecebeString(string texto)
        {
            Console.Write(texto);
            Console.ResetColor();
            return Console.ReadLine().ToUpper();
        }
        public void OpcaoInvalida(ref string opcao, ref bool sair, ref bool repetir)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            opcao = RecebeString("\n Opção inválida. 'Enter' para tentar novamente ou 'S' para sair: ");

            if (opcao == "S") sair = true;
            else if (opcao != "") OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        //Auxiliar Cadastro
        public void RealizadoComSucesso(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n       Paciente {texto} com sucesso!");
            Console.ResetColor();
            string opcao = RecebeString("     'Enter' para voltar ao menu principal ");
        }
        public void PacienteJaExiste(RepositorioPaciente repositorioPaciente, int index, ref bool sair)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Este paciente já existe!");
            Console.ResetColor();
            bool repetir = false;
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        //Auxiliar Pesquisa
        public void Pesquisa(RepositorioPaciente repositorioPaciente, int index) => Console.WriteLine($"\n paciente = {repositorioPaciente.pacientes[index].nome}\n CPF = {repositorioPaciente.pacientes[index].cpf}\n Endereço = {repositorioPaciente.pacientes[index].endereco}\n Cartão SUS = {repositorioPaciente.pacientes[index].cartaoSUS}\n");
        public void ParaRetornarAoMenu(ref bool sair, ref bool repetir)
        {
            string opcao = RecebeString("\n     'Enter' para voltar ao Menu Principal\n    'R' para voltar ao Menu de pacientes\n\t       'S' para Sair\n\n\t\t   Digite: ");
            if (opcao == "") ;
            else if (opcao == "R") repetir = true;
            else if (opcao == "S") sair = true;
            else OpcaoInvalida(ref opcao, ref sair, ref repetir);
        }
        public void PacienteNaoExiste()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\t   Paciente não existente!");
            Console.ResetColor();
        }
        //Auxiliar Visualizar
        public void SemPacientes(ref bool sair, ref bool repetir)
        {
            Console.WriteLine("------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("      Não existem pacientes cadastrados");
            Console.ResetColor();
            ParaRetornarAoMenu(ref sair, ref repetir);
        }
        public void Visualizar(RepositorioPaciente repositorioPaciente)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n------------------------------------------------------\n Nome\t| CPF\t\t| Endereço\t| Cartão SUS\n------------------------------------------------------");
            Console.ResetColor();

            for (int i = 0; i < repositorioPaciente.pacientes.Length; i++)
            {
                if (repositorioPaciente.pacientes[i] != null) Console.Write($" {repositorioPaciente.pacientes[i].nome}\t| {repositorioPaciente.pacientes[i].cpf}\t\t| {repositorioPaciente.pacientes[i].endereco}\t\t| {repositorioPaciente.pacientes[i].cartaoSUS}\n------------------------------------------------------\n");
            }
        }
        //Auxiliar Editar
        public void NomeValidoParaEdicao(RepositorioPaciente repositorioPaciente, int indexEditar, ref bool sair, ref bool repetir)
        {
            var objetoAuxiliar = repositorioPaciente.pacientes[indexEditar];

            MenuParaEdicao(ref sair, ref repetir, objetoAuxiliar);
            if (!sair && !repetir)
            {
                repositorioPaciente.Editar(objetoAuxiliar.nome, objetoAuxiliar.cpf, objetoAuxiliar.endereco, objetoAuxiliar.cartaoSUS, indexEditar);
                RealizadoComSucesso("editado");
            }
        }
        public void MenuParaEdicao(ref bool sair, ref bool repetir, Paciente objetoAuxiliar)
        {
            Console.Clear();
            CabecalhoPaciente();
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. nome\n 2. CPF\n 3. endereço\n 4. cartão SUS\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1": objetoAuxiliar.nome = RecebeString("\n Informe o novo nome: "); break;
                case "2": objetoAuxiliar.cpf = RecebeString("\n Informe o novo CPF: "); break;
                case "3": objetoAuxiliar.endereco = RecebeString("\n Informe o novo endereço: "); break;
                case "4": objetoAuxiliar.cartaoSUS = RecebeString("\n Informe o novo cartão SUS: "); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref opcao, ref sair, ref repetir); break;
            }
            if (opcao == "") repetir = true;
        }
        public void VisualizarParaEdicao(Paciente objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n------------------------------------------------\n Nome\t| CPF\t| Endereço\t| Cartão SUS\n------------------------------------------------");
            Console.ResetColor();

            Console.Write($" {objetoAuxiliar.nome}\t| {objetoAuxiliar.cpf}\t\t| {objetoAuxiliar.endereco}\t\t| {objetoAuxiliar.cartaoSUS}\n------------------------------------------------\n\n");
        }
    }
}
