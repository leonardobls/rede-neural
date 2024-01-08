
using System;
using RedeNeural.dtos;
using RedeNeural.Controllers;
using projeto_de_redes;
using System.Collections;

namespace RedeNeural
{
    internal class Program
    {

        static void Main(string[] args)
        {

            List<FutebolDTO> data = FileReaderController.GetContent();

            NormalizaDados(data);

            double[,] MatrizEntradaIntermediario = new double[Globals.NUMERO_NODOS_INTERMEDIARIOS, Globals.NUMERO_NODOS_ENTRADA];
            double[,] MatrizIntermediariosSaida = new double[3, Globals.NUMERO_NODOS_INTERMEDIARIOS];

            DefinePesosAleatorios(MatrizEntradaIntermediario, MatrizIntermediariosSaida);

            NeuronioDTO[] VetorEtradas = new NeuronioDTO[Globals.NUMERO_NODOS_ENTRADA];
            NeuronioDTO[] VetorIntermediario = new NeuronioDTO[Globals.NUMERO_NODOS_INTERMEDIARIOS];
            NeuronioDTO[] VetorSaidas = new NeuronioDTO[3];

            for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
            {
                VetorIntermediario[i] = new NeuronioDTO();
            }

            for (int i = 0; i < 3; i++)
            {
                VetorSaidas[i] = new NeuronioDTO();
            }

            int totalDataCount = data.Count;
            int trainingDataCount = (int)(totalDataCount * 0.8); // 80% of the data
            int testingDataCount = totalDataCount - trainingDataCount; // 20% of the data

            List<FutebolDTO> dataTraining = new();
            List<FutebolDTO> dataTest = new();
            List<FutebolDTO> dataTestRandom = new();


            dataTraining = data.Take(trainingDataCount).ToList();

            Random random = new Random();

            for(int i =testingDataCount; i < data.Count; i++){
                dataTest.Add(data[i]);
            }

             for(int i =testingDataCount; i < data.Count; i++){
                 int randomIndex = random.Next(dataTest.Count);
                dataTestRandom.Add(dataTest[randomIndex]);
            }

            // for (var i = 0; i < data.Count; i++)
            // {
            //     if((i + 1) % 5 == 0){
            //           dataTest.Add(data[i]);
            //     }else{
            //          dataTraining.Add(data[i]);
            //     }
            // }


            for (int epocas = 0; epocas < 800; epocas++)
            {
                foreach (FutebolDTO item in dataTraining)
                {

                    VetorEtradas = new NeuronioDTO[]{
                        //new() { Valor = item.HTHG! },
                        //new() { Valor = item.HTAG! },
                        new() { Valor = item.HS! },
                        new() { Valor = item.AS! },
                        new() { Valor = item.HST! },
                        new() { Valor = item.AST! },
                        new() { Valor = item.HC! },
                        new() { Valor = item.AC! },
                        new() { Valor = item.HF! },
                        new() { Valor = item.AF! },
                        new() { Valor = item.HY! },
                        new() { Valor = item.AY! },
                    };

                    SomatorioIntermediario(VetorEtradas, MatrizEntradaIntermediario, VetorIntermediario);
                    AtivacaoIntermediario(VetorIntermediario);

                    SomatorioSaidas(VetorIntermediario, MatrizIntermediariosSaida, VetorSaidas);
                    AtivacaoSaidas(VetorSaidas);

                    CalculaErroSaidas(item.FTR!, VetorSaidas);
                    CalculaErroIntermediario(VetorSaidas, VetorIntermediario, MatrizIntermediariosSaida);

                    AjustaPesoIntermediarioSaida(VetorSaidas, VetorIntermediario, MatrizIntermediariosSaida);
                    AjustaPesoEntradaIntermediario(VetorEtradas, VetorIntermediario, MatrizEntradaIntermediario);

                }

                Console.WriteLine($"\nMATRIZ ENTRADA X INTERMEDIARIO N {epocas} ---- \n");

                    for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
                    {
                        for (int j = 0; j < Globals.NUMERO_NODOS_ENTRADA; j++)
                        {
                            string formattedValue = MatrizEntradaIntermediario[i, j].ToString("F2").PadLeft(8);
                            Console.Write(formattedValue);
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine("\n//--------------------------------------------------------------------------- \n");

                    Console.WriteLine($"\nMATRIZ INTERMEDIARIO X SAIDA N {epocas} ------ \n");

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < Globals.NUMERO_NODOS_INTERMEDIARIOS; j++)
                        {
                            string formattedValue = MatrizIntermediariosSaida[i, j].ToString("F2").PadLeft(8);
                            Console.Write(formattedValue);
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine("\n//--------------------------------------------------------------------------- \n");


            }

            int[,] MatrizConfusao = new int[3, 3];
            int counter = 0;

            int linha = 0, coluna = 0, acertos = 0;


            foreach (var item in dataTestRandom)
            {   //faz o teste com 1200 linhas (20%)
                Console.WriteLine($"Teste {counter}");

                string saidaEsperada = item.FTR!;
                VetorEtradas[0].Valor = item.HS!;
                VetorEtradas[1].Valor = item.AS!;
                VetorEtradas[2].Valor = item.HST!;
                VetorEtradas[3].Valor = item.AST!;
                VetorEtradas[4].Valor = item.HC!;
                VetorEtradas[5].Valor = item.AC!;
                VetorEtradas[6].Valor = item.HF!;
                VetorEtradas[7].Valor = item.AF!;
                VetorEtradas[8].Valor = item.HY!;
                VetorEtradas[9].Valor = item.AY!;
                //VetorEtradas[10].Valor = item.HTHG!;
                //VetorEtradas[11].Valor = item.HTAG!;

                string saidaAlgoritmo = TestaEntradaAtual(VetorEtradas, MatrizEntradaIntermediario, MatrizIntermediariosSaida, VetorIntermediario, VetorSaidas);

                Console.WriteLine($"Resultado do algoritmo: {saidaAlgoritmo} | Resultado esperado: {saidaEsperada}");

                if (saidaEsperada.Equals("H"))
                {
                    linha = 0;
                }
                else if (saidaEsperada.Equals("D"))
                {
                    linha = 1;
                }
                else if (saidaEsperada.Equals("A"))
                {
                    linha = 2;
                }

                if (saidaAlgoritmo.Equals("H"))
                {
                    coluna = 0;
                }
                else if (saidaAlgoritmo.Equals("D"))
                {
                    coluna = 1;
                }
                else if (saidaAlgoritmo.Equals("A"))
                {
                    coluna = 2;
                }

                if(linha == coluna) acertos++;

                MatrizConfusao[linha, coluna] += 1;

                counter++;
            }


            Console.WriteLine("\nMATRIZ DE CONFUSÃO -------------------------------------------");

            for (int n = 0; n < 3; n++)
            {
                for (int k = 0; k < 3; k++)
                {
                    string formattedValue = MatrizConfusao[n, k].ToString().PadLeft(8);
                    Console.Write(formattedValue);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nACURÁCIA -------------------------------------------");

            Console.WriteLine($"{((decimal)acertos/(decimal)dataTestRandom.Count) * 100}%");

            Console.WriteLine("\nRECALL -------------------------------------------");

            for (int n = 0; n < 3; n++)
            {
                double instancias = 0;
                for (int k = 0; k < 3; k++)
                {

                    instancias += MatrizConfusao[n, k];
                }

                double recall = 0;

                if(n == 0){
                    recall = MatrizConfusao[0,0] / instancias;
                    Console.WriteLine($"H: {(recall)}");
                }

                if(n == 1){
                    recall = MatrizConfusao[1,1] / instancias;
                    Console.WriteLine($"D: {recall}");
                }

                if(n == 2){
                    recall = MatrizConfusao[2,2] / instancias;
                    Console.WriteLine($"A: {recall}");
                }
            }

            Console.WriteLine("\nPRECISÃO -------------------------------------------");

            for (int n = 0; n < 3; n++)
            {
                double instancias = 0;
                for (int k = 0; k < 3; k++)
                {

                    instancias += MatrizConfusao[k, n];
                }

                double precisao = 0;

                if(n == 0){
                    precisao = MatrizConfusao[0,0] / instancias;
                    Console.WriteLine($"H: {(precisao)}");
                }

                if(n == 1){
                    precisao = MatrizConfusao[1,1] / instancias;
                    Console.WriteLine($"D: {precisao}");
                }

                if(n == 2){
                    precisao = MatrizConfusao[2,2] / instancias;
                    Console.WriteLine($"A: {precisao}");
                }
            }

               Console.WriteLine("\nF1 SCORE -------------------------------------------");

            for (int n = 0; n < 3; n++)
            {
                double instanciasR = 0;
                double instanciasP = 0;
                for (int k = 0; k < 3; k++)
                {

                    instanciasR += MatrizConfusao[n, k];
                    instanciasP += MatrizConfusao[k, n];
                }

                double precisao = 0;
                double recall = 0;

                if(n == 0){
                    recall = MatrizConfusao[0,0] / instanciasR;
                    precisao = MatrizConfusao[0,0] / instanciasP;
                    Console.WriteLine($"H: {2*recall*precisao/recall+precisao}");
                }

                if(n == 1){
                    recall = MatrizConfusao[1,1] / instanciasR;
                    precisao = MatrizConfusao[1,1] / instanciasP;
                    Console.WriteLine($"D: {2*recall*precisao/recall+precisao}");
                }

                if(n == 2){
                    recall = MatrizConfusao[2,2] / instanciasR;
                    precisao = MatrizConfusao[2,2] / instanciasP;
                    Console.WriteLine($"A: {2*recall*precisao/recall+precisao}");
                }
            }
        }

        static void NormalizaDados(List<FutebolDTO> data)
        {
            double HSm = data.Min(x => x.HS);
            double ASm = data.Min(x => x.AS);
            double HSTm = data.Min(x => x.HST);
            double ASTm = data.Min(x => x.AST);
            double HCm = data.Min(x => x.HC);
            double ACm = data.Min(x => x.AC);
            double HFm = data.Min(x => x.HF);
            double AFm = data.Min(x => x.AF);
            double HYm = data.Min(x => x.HY);
            double AYm = data.Min(x => x.AY);

            double HSM = data.Max(x => x.HS);
            double ASM = data.Max(x => x.AS);
            double HSTM = data.Max(x => x.HST);
            double ASTM = data.Max(x => x.AST);
            double HCM = data.Max(x => x.HC);
            double ACM = data.Max(x => x.AC);
            double HFM = data.Max(x => x.HF);
            double AFM = data.Max(x => x.AF);
            double HYM = data.Max(x => x.HY);
            double AYM = data.Max(x => x.AY);

            foreach (FutebolDTO item in data)
            {
                item.HS = (item.HS - HSm)/(HSM - HSm);
                item.AS = (item.AS - ASm)/(ASM - ASm);
                item.HST = (item.HST - HSTm)/(HSTM - HSTm);
                item.AST = (item.AST - ASTm)/(ASTM - ASTm);
                item.HC = (item.HC - HCm)/(HCM - HCm);
                item.AC = (item.AC - ACm)/(ACM - ACm);
                item.HF = (item.HF - HFm)/(HFM - HFm);
                item.AF = (item.AF - AFm)/(AFM - AFm);
                item.HY = (item.HY - HYm)/(HYM - HYm);
                item.AY = (item.AY - AYm)/(AYM - AYm);
                /*item.AS /= Math.Sqrt( AS);
                item.HST /= Math.Sqrt( HST);
                item.AST /= Math.Sqrt( AST);
                item.HC /= Math.Sqrt( HC);
                item.AC /= Math.Sqrt( AC);
                item.HF /= Math.Sqrt( HF);
                item.AF /= Math.Sqrt( AF);
                item.HY /= Math.Sqrt( HY);
                item.AY /= Math.Sqrt( AY);*/
            }
        }

        static void DefinePesosAleatorios(double[,] MatrizEntradaIntermediario, double[,] MatrizIntermediarioSaida)
        {

            Random rand = new();

            for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
            {
                for (int j = 0; j < Globals.NUMERO_NODOS_ENTRADA; j++)
                {
                    double randomValue = (double)rand.NextDouble();

                    MatrizEntradaIntermediario[i, j] = randomValue;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Globals.NUMERO_NODOS_INTERMEDIARIOS; j++)
                {
                    double randomValue = (double)rand.NextDouble();

                    MatrizIntermediarioSaida[i, j] = randomValue;
                }
            }

            Console.WriteLine("\nMATRIZ DE PESOS DA CAMADA ENTRADA X INTERMEDIARIO COM VALORES ALEATORIOS ---- \n");

            for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
            {
                for (int j = 0; j < Globals.NUMERO_NODOS_ENTRADA; j++)
                {
                    string formattedValue = MatrizEntradaIntermediario[i, j].ToString("F2").PadLeft(8);
                    Console.Write(formattedValue);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n//--------------------------------------------------------------------------- \n");

            Console.WriteLine("\nMATRIZ DE PESOS DA CAMADA INTERMEDIARIO X SAIDA COM VALORES ALEATORIOS ------ \n");

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Globals.NUMERO_NODOS_INTERMEDIARIOS; j++)
                {
                    string formattedValue = MatrizIntermediarioSaida[i, j].ToString("F2").PadLeft(8);
                    Console.Write(formattedValue);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n//--------------------------------------------------------------------------- \n");
        }

        static void SomatorioIntermediario(NeuronioDTO[] VetorEntradas, double[,] matrixOfWheights, NeuronioDTO[] VetorIntermediario)
        {

            for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
            {
                double somatorio = 0;
                for (int j = 0; j < Globals.NUMERO_NODOS_ENTRADA; j++)
                {
                    somatorio += VetorEntradas[j].Valor * matrixOfWheights[i, j];
                }

                VetorIntermediario[i].Valor = somatorio;
            }
        }

        static void AtivacaoIntermediario(NeuronioDTO[] VetorIntermediario)
        {
            foreach (var item in VetorIntermediario)
            {
                item.Valor = 1.0 / (1.0 + Math.Exp(-item.Valor!));
            }
        }

        static void SomatorioSaidas(NeuronioDTO[] VetorIntermediario, double[,] matrixOfWheights, NeuronioDTO[] VetorSaidas)
        {
            double somatorio;
            for (int i = 0; i < 3; i++)
            {   //para cada neuronio de saida...
                somatorio = 0;
                for (int j = 0; j < Globals.NUMERO_NODOS_INTERMEDIARIOS; j++)
                {
                    somatorio += VetorIntermediario[j].Valor * matrixOfWheights[i, j];
                }

                VetorSaidas[i].Valor = somatorio;
            }
        }

        static void AtivacaoSaidas(NeuronioDTO[] VetorSaidas)
        {
            foreach (var item in VetorSaidas)
            {
                item.Valor = 1.0 / (1.0 + Math.Exp(-item.Valor!));
            }
        }

        static void CalculaErroSaidas(string resultadoEsperado, NeuronioDTO[] VetorSaidas)
        {
            if (resultadoEsperado == "H")
            {
                VetorSaidas[0].Error = VetorSaidas[0].Valor * (1 - VetorSaidas[0].Valor) * (1 - VetorSaidas[0].Valor);
                VetorSaidas[1].Error = VetorSaidas[1].Valor * (1 - VetorSaidas[1].Valor) * (0 - VetorSaidas[1].Valor);
                VetorSaidas[2].Error = VetorSaidas[2].Valor * (1 - VetorSaidas[2].Valor) * (0 - VetorSaidas[2].Valor);
            }
            else if (resultadoEsperado == "D")
            {
                VetorSaidas[0].Error = VetorSaidas[0].Valor * (1 - VetorSaidas[0].Valor) * (0 - VetorSaidas[0].Valor);
                VetorSaidas[1].Error = VetorSaidas[1].Valor * (1 - VetorSaidas[1].Valor) * (1 - VetorSaidas[1].Valor);
                VetorSaidas[2].Error = VetorSaidas[2].Valor * (1 - VetorSaidas[2].Valor) * (0 - VetorSaidas[2].Valor);
            }
            else if (resultadoEsperado == "A")
            {
                VetorSaidas[0].Error = VetorSaidas[0].Valor * (1 - VetorSaidas[0].Valor) * (0 - VetorSaidas[0].Valor);
                VetorSaidas[1].Error = VetorSaidas[1].Valor * (1 - VetorSaidas[1].Valor) * (0 - VetorSaidas[1].Valor);
                VetorSaidas[2].Error = VetorSaidas[2].Valor * (1 - VetorSaidas[2].Valor) * (1 - VetorSaidas[2].Valor);
            }
        }

        static void CalculaErroIntermediario(NeuronioDTO[] VetorSaidas, NeuronioDTO[] VetorIntermediarios, double[,] matrixOfWheights)
        {
            for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
            {
                double fatorErro = 0;
                for (int j = 0; j < 3; j++)
                {
                    fatorErro += VetorSaidas[j].Error * matrixOfWheights[j, i];
                }
                VetorIntermediarios[i].Error = VetorIntermediarios[i].Valor * (1 - VetorIntermediarios[i].Valor) * fatorErro;
            }
        }

        static void AjustaPesoIntermediarioSaida(NeuronioDTO[] VetorSaidas, NeuronioDTO[] VetorIntermediarios, double[,] matrixOfWheights)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Globals.NUMERO_NODOS_INTERMEDIARIOS; j++)
                {
                    //Novo_peso = Peso_anterior+Taxa_aprendizagem*Sa�da_neur�nio_anterior*Erro_neuronio_posterior
                    matrixOfWheights[i, j] = matrixOfWheights[i, j] + Globals.TAXA_APRENDIZAGEM * VetorIntermediarios[j].Valor * VetorSaidas[i].Error;
                }
            }
        }

        static void AjustaPesoEntradaIntermediario(NeuronioDTO[] VetorEntradas, NeuronioDTO[] VetorIntermediarios, double[,] matrixOfWheights)
        {
            for (int i = 0; i < Globals.NUMERO_NODOS_INTERMEDIARIOS; i++)
            {
                for (int j = 0; j < Globals.NUMERO_NODOS_ENTRADA; j++)
                {
                    //Novo_peso = Peso_anterior+Taxa_aprendizagem*Sa�da_neur�nio_anterior*Erro_neuronio_posterior
                    matrixOfWheights[i, j] = matrixOfWheights[i, j] + Globals.TAXA_APRENDIZAGEM * VetorEntradas[j].Valor * VetorIntermediarios[i].Error;
                }
            }
        }

        static string TestaEntradaAtual(NeuronioDTO[] VetorEntradas, double[,] matrixEntradaIntermediario, double[,] matrixIntermediarioSaida, NeuronioDTO[] VetorIntermediario, NeuronioDTO[] VetorSaidas)
        {
            SomatorioIntermediario(VetorEntradas, matrixEntradaIntermediario, VetorIntermediario);
            AtivacaoIntermediario(VetorIntermediario);
            SomatorioSaidas(VetorIntermediario, matrixIntermediarioSaida, VetorSaidas);
            AtivacaoSaidas(VetorSaidas);

            double maior = 0;
            int resultado;

            // O vencedor eh o time da casa
            maior = VetorSaidas[0].Valor;
            resultado = 0;

            // O resultado foi empate
            if (maior < VetorSaidas[1].Valor)
            {
                maior = VetorSaidas[1].Valor;
                resultado = 1;
            }

            // O vencedor eh o time visitante
            if (maior < VetorSaidas[2].Valor)
            {
                maior = VetorSaidas[2].Valor;
                resultado = 2;
            }

            if (resultado == 0) return ("H");
            else if (resultado == 1) return ("D");
            else return ("A");
        }
    }
}

