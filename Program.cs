using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;


namespace aula2
{

// Class Criação de arquivos 7z
    class Zipa{ 
        string arquivo;
        public string nome;
        public string local;
        public Zipa(string valor1, string valor2 = null, string valor3 = null){
            arquivo = valor1;
            nome = valor2;
            local = valor3;
        }
    
        public void criarArquivo(){
            if(local == "" || local == string.Empty || local == null){ Directory.CreateDirectory("c:\\BackUpBirks");  local = "c:\\BackUpBirks\\";}
            if(nome == "" || nome == string.Empty || nome == null){
            
                if(File.Exists(local+"BackUpBirks.7z")){
                    int np = Directory.GetFiles(local).Length;
                    nome = "BackUpBirks_"+np+".7z";
                }else{ nome = "BackUpBirks.7z"; }
                
            }else{ nome = nome.Split(".")[0]+".7z";}
            ProcessStartInfo zp = new ProcessStartInfo(Directory.GetCurrentDirectory()+@"\7z\7za.exe ");
            zp.UseShellExecute = false;
            zp.Arguments = @"a "+local+@"\"+nome+" "+arquivo+" -mx0 ";

            Process.Start(zp).WaitForExit();

            Console.WriteLine("\nArquivo de BackUp gerado com Sucesso!!!");
            Console.WriteLine("Local do arquivo:"+ local+nome +"\n");
        }

    }

/*
    Class para manipulação das pastas, Listar, Criar e nevegar entre elas
*/    class Pasta{
        public string caminho = Directory.GetCurrentDirectory();
        static public void echo(string valor){ Console.WriteLine(valor);}
        public bool subPasta = false;

         static public void discos(){
                echo(@"    --> Lista de Unidades Logica:");
                ManagementObjectSearcher busca = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk ");
                foreach(ManagementObject und in busca.Get()){
                    echo("\n"+@" Unidade logica -> "+ und.GetPropertyValue("DeviceID"));
                }
                echo("\n --------------------------------------");
         }

        List<string> ptsSalve = new List<string>();
        public List<string> Lista(){
            echo("----------------------------------------");
            echo(@" --> Lista de Pastas: "+ caminho );
            int nu = 0;
            
            List<string> pasta = ltPasta(caminho);
               foreach(string lts in pasta)
               {
                        echo(@" ["+nu+"] Pasta -> "+lts);
                        ptsSalve.Add(lts);
                        nu++;
                        if(subPasta){
                            if(ltPasta(lts).Count>0)
                            {
                                foreach(string slts in ltPasta(lts))
                                {
                                    echo(@"   ["+nu+"] SubPasta -> "+ slts);
                                    ptsSalve.Add(lts);
                                    nu++;
                                }
                            } 
                        }
                            
               }  
             
             
            echo("-----------------------------------------\n");
            return ptsSalve;        
        }

         List<string> ltPasta(string lugar,bool todasPasta = false, string busca = "*")
        {    IEnumerable<string> past  ;
            try{
                 past = Directory.EnumerateDirectories(lugar);

                 if(todasPasta==true)
                {
                    past = Directory.EnumerateDirectories(lugar,"*",SearchOption.AllDirectories);   
                }
            
                List<string> lsPasta = new List<string>();
                foreach(string ls in past){ lsPasta.Add(ls);}
                return lsPasta; 
            }
            catch(UnauthorizedAccessException){
                Console.WriteLine(@"    DIRETORIO RESTRITO!");
                List<string> sss = new List<string>();
                return sss;
            }catch(ArgumentException e){ List<string> sss = new List<string>();
                return sss; }
            catch(IOException e){List<string> sss = new List<string>();
                return sss;}
             
        }

    }
   
   class BirksBackUp
   {
       string[] args;
       string rp1;
       List<string> diretorioSalvo = new List<string>();
       List<string> tempListaPasta = new List<string>();
        void echo(string valor){ Console.WriteLine(valor);}
        void read(){ rp1 = Console.ReadLine(); }
        public void inicio(){
            echo("################ Sistema BirksBackUp #################");
            echo(@"
    Sistema para BackUp simples e facil via linha
    Comandos: -ajuda -lista -salvelista -backup e sair
            ");
             read();
            do {
                    if(rp1.ToLower() == "-ajuda") // menu ajuda
                    { 
                        echo("  ---> AJUDA BirksBackUp \n");
                        echo(@"    -> Comando -lista "+"\n");
                        echo(@" O comando -lista serve para mostrar uma lista de diretorios de um caminho.");
                        echo(@"Essa lista e numerada para ser usado no comando -salvelista");
                        echo(@"ao usar o comando -lista podemos observar um numero entre couchetes [?] antes das pastas.");
                        echo(@"podemos usar esse numero para adicionar a pasta a uma lista de pastas para ser salva.");
                        echo(@"Exemplo: apos usar o comando ( -lista caminho=c:\ ) use ( -salvelista n=0 )");
                        echo(@"Sintax <-lista> [ -subpasta  [caminho=<SeuCaminho>]  [-discos] ]");
                        echo("EXEMPLOS: -lista");
                        echo(@" -lista [ mostara todos os diretorios de c:\users ] ");
                        echo(@" -lista -discos [ mostra as unidades logicas do computador ] ");
                        echo(@" -lista caminho=c:\ [ mostra todos os diretorios de c:\ ]");
                        echo(@" -lista -subpasta [ mostra os diretorios e o 1º nivel de subdiretorios de c:\users ]");
                        
                        echo("\n"+@"    -> Comando -salvelista"+"\n");
                        echo(@" O comando -salvelista cria uma lista de pastas a serem salvas.");
                        echo(@"podemos usar o comando -salvelista de varias formas aqui esta alguns exemplos:");
                        echo(@"Sintax <-salvelista> [addpasta=<caminhoPasta>] [n=<NumerosPatas>] [-exibir] ");
                        echo("EXEMPLOS: -salvelista");
                        echo(@" -salvelista  [ Cria uma lista das pastas padrão da biblioteca do usuario atual ]");
                        echo(@" -salvelista addpasta=c:\teste [ Adicioa a pasta c:\teste se existir a lista para BackUp ]");
                        echo(@" -salvelista -exibir [ Mostra a lista atual para ser gerado o BackUp ]");
                        echo(@" -salvelista n=1  [ Parametro n e baseado no comando -lista  Use -lista para formar lista de pastas numeradas ]");
                        echo("OBSERVAÇÕES: -salvelista");
                        echo(@" Para usar o (-salvelista n=?) primeiro use o comando ( -lista ) para mostra uma listagem de pastas numeradas");
                        echo(@" exemplo: ( [3] Pasta -> c:\users )  onde o [3] e o numero da pasta c:\users ");
                        echo(@" Voce pode usar varios numeros, porem da mesma listagem! exemplo ( -salvelista n=2-5-10-100 ) O Hifen - separa os numeros");

                        echo("\n"+@"    -> Comando -backup"+"\n");
                        echo(@" O comando -backup Cria um arquivo .7z com todas as pastas armazenadas pelo comando -salvelist");
                        echo(@" Por padrão o comando cria uma pasta chamada BackUpbirks em C:\ e salva os backups nessa pasta");
                        echo(@" Podemos modificar o nome e local de destino do nosso arquivo 7z com os parametros -nome -caminho -caminhonome ");
                        echo(@"Sintax <-backup> [-nome] [-caminho] [-caminhonome]");
                        echo(@"EXEMPLOS: -backup");
                        echo(@"  -backup [ Salva o Conteude de -salvelista em c:\BackUpBirks\BackUpBirks.7z ]");
                        echo(@"  -backup -nome [ Salva o Conteude de -salvelista em c:\BackUpBirks\ Com o nome fornecido  ]");
                        echo(@"  -backup -caminho [ Salva o Conteude de -salvelista no diretorio fornecido com o nome BackUpBirks.7z ]");
                        echo(@"  -backup -caminhonome [ Salva o Conteude de -salvelista no diretorio fornecido com o nome fornecido ]");
                        echo("OBSERVAÇÕES: -backup");
                        echo(@"-backup com Parametros -nome -caminho e -caminhonome voce pode fornecer os parametros apos o ENTER");

                        echo("\n"+@"-------------- Final Ajuda BirksBackUp ----------------");

                        read();
                    }
                    else if(rp1.ToLower().StartsWith("-lista")) // Chamadas da class lista
                    {
                        string[] alternativa = rp1.Split(" ");
                        if(alternativa.Length > 0)
                        {   
                            Pasta novaLista = new Pasta();
                           
                            if( alternativa.Length > 1 && alternativa[1] != "" && alternativa[1] != null)
                            {
                                foreach(string sr in alternativa)
                                { 
                                    if(sr.ToLower() == "-subpasta"){
                                            novaLista.subPasta = true;
                                        if(alternativa.Length < 3) {
                                            novaLista.caminho = @"c:\users";
                                            tempListaPasta = novaLista.Lista();
                                            read();
                                        }
                                    }
                                }

                                for(int y=1; y<alternativa.Length;y++)
                                {
                            
                                    if(alternativa[y].StartsWith("caminho"))
                                    {
                                        novaLista.caminho = alternativa[y].Split("=")[1];
                                        tempListaPasta = novaLista.Lista();
                                        read();
                                    }else if(alternativa[y].StartsWith("-subpasta")){}
                                    else if(alternativa[y].StartsWith("-discos")){
                                        Pasta.discos();
                                        read();
                                    }
                                    else{ echo("O comando -lista não possui esse argumento!");read();}

                                }
                            }else{
                                novaLista.caminho = @"c:\users";
                                tempListaPasta = novaLista.Lista();
                                read();
                            }
                        }
                    }else if(rp1.ToLower().StartsWith("-salvelista"))
                    {
                        string[] ptt = rp1.Split(" ");
                        if(ptt.Length>1 && ptt[1] != "" && ptt[1] != null)
                        {
                            for(int y =1; y< ptt.Length; y++)
                            {
                                if(ptt[y].ToLower().StartsWith("n="))
                                {  
                                    
                                   
                                        try{
                                            string[] gtn;
                                            if(ptt[y].Split("=")[1].Split("-").Length>1){  
                                              gtn = ptt[y].Split("=")[1].Split("-");
                                            }else{
                                              gtn = new string[] {ptt[y].Split("=")[1]};
                                            }
                                            if(gtn.Length>1){
                                                    foreach(string vgg in gtn)
                                                    {   int ptn = Convert.ToInt32(vgg);
                                                    if( tempListaPasta[ptn] != "" && tempListaPasta[ptn] != null)
                                                        {
                                                        diretorioSalvo.Add(tempListaPasta[ptn]);
                                                            echo("Diretorio-> "+tempListaPasta[ptn]+"\n Adicionado a Lista do BackUp!\n");
                                                        }else{
                                                            echo("Numero de pasta errado tente colsultar 0 -lista");
                                                        }
                                                    }
                                                    read();
                                            }else{
                                                int ptn = Convert.ToInt32(gtn[0]);
                                                if(tempListaPasta[ptn] != null && tempListaPasta[ptn] != "")
                                                {
                                                    diretorioSalvo.Add(tempListaPasta[ptn]);
                                                    echo("Diretorio-> "+tempListaPasta[ptn]+"\n Adicionado a Lista do BackUp!");
                                                    read();

                                                }else{ echo(" Numero de Pasta invalido tente consultar o -lista"); read();}
                                            }
                                        }
                                        catch(ArgumentException e){ echo("Erro com o valor declarado em N Consulte -lista ou -ajuda");}
                                    
                                }else if (ptt[y].ToLower().StartsWith("addpasta"))
                                {
                                    string[] adpasta = ptt[y].Split("=");

                                    if(adpasta.Length>1 && adpasta[1] !="" && adpasta[1] !=null){
                                        if(Directory.Exists(adpasta[1])){
                                            diretorioSalvo.Add(adpasta[1]);
                                            echo("Pasta adicionada a lista do BackUP!");
                                            read();
                                            
                                        }else{ echo("Não existe patas no caminho informado!"); read(); }
                                    }else{ echo("Erro Argumento passado em pasta=? invalido!"); read();}
                                }else if(ptt[y].ToLower().StartsWith("-exibir")){
                                    echo("\n"+@"    -> Lista Para BackUp"+"\n");
                                    foreach(string drs in diretorioSalvo){
                                        echo(@"    Pasta p/ Bkp -> "+ drs);
                                    }
                                    echo(@"---------------------------------------"+"\n");
                                    read();
                                }else if(ptt[y].ToLower().StartsWith("-limpar")){
                                        diretorioSalvo.Clear();
                                        echo("\n"+@"  Lista de pastas para BackUp apagada! "+"\n");
                                        read();
                                }
                                else{
                                    echo("Parametro Invalido o comando -salvelista tem os parametros [n=?] [addpasta=?] [-exibir]");
                                    read();
                                }
                            }

                        }else{ 
                            echo("\nO Comando -salvelista sem parametros ira gerar uma lista");
                            echo("das pastas: Downloads, Documentos, Imagens, Desktop, Videos, Musicas.");
                            echo("do usuario logado atualmente!");
                             echo("\n Deseja Continuar? digite sim para continuar ou outra coisa para sair!");
                             string ds = Console.ReadLine();
                             if(ds == "sim"){
                                 echo("\n -> Gerando Lista de BackUp Padrão ...");
                                 string userPc = Environment.UserName;
                                 string localUser = @"c:\users\"+userPc;
                                    if(Directory.Exists(localUser)){
                                        string[] drs = new string[] {"documents","desktop","videos","pictures","music","downloads"};
                                       
                                        for(int u=0; u<drs.Length;u++){
                                            if(Directory.Exists(localUser+"\\"+drs[u])){
                                            diretorioSalvo.Add(localUser+"\\"+drs[u]);
                                                echo($"Diretorio {drs[u]} Adicionado a lista");
                                            }else{ echo($"Erro ao Adicionar {drs[u]} a lista"); }
                                        }

                                        echo("\n -> Lista acima salva! ");
                                    
                                    }else{
                                        echo("Erro ao achar Diretorio do Usuario tente via -lista");  
                                    }

                                 read();

                             }else{ read(); }

                         }
                    }
                    else if(rp1.ToLower().StartsWith("-backup")){
                        echo("\n"+@"  -> Iniciando BackUp"+"\n");
                        if(diretorioSalvo.Count>0){
                            string[] plt = rp1.Split(" ");
                            
                                string atpt = Directory.GetCurrentDirectory();
                                echo("Lista de pastas que serão salva:");
                                foreach(string lfs in diretorioSalvo){
                                    echo(@" Pasta p/ bkp -> "+lfs);
                                }
                                echo(" ------------------------------- \n");
                                echo(" Digite sim para salvar as pastas listadas acima");
                                string cta = Console.ReadLine();
                                if(cta == "sim"){
                                    using(StreamWriter linha = File.CreateText(atpt+"\\salveLiista.txt"))
                                    {
                                        foreach(string fns in  diretorioSalvo){
                                            linha.WriteLine(fns);
                                        }
                                        echo("Arquivo Lista Gerado! Armazenando....");
                                    }
                                }else{echo("\n"); read();}


                            if(plt.Length > 2 && plt[1] !="" && plt[1] != null ){
                                
                                for(int o=1;o<plt.Length;o++){
                                    if(plt[o].ToLower().StartsWith("-nome"))
                                    {
                                        echo(" Digite um nome para seu arquivo de BackUp:");
                                        string nmr ="";
                                        do { 
                                                if(nmr == ""){ echo("O nome não pode ser vaziu");}
                                                nmr = Console.ReadLine();
                                         } while(nmr != "" && nmr != null && nmr != string.Empty);
                                         Zipa zp = new Zipa("@"+atpt+"\\salveLiista.txt");
                                         zp.nome = nmr;
                                         zp.criarArquivo();
                                         read();

                                    }else if(plt[o].ToLower().StartsWith("-caminho"))
                                    {
                                        echo(" Digite um caminho para seu arquivo de BackUp:");
                                        string nmr ="";
                                        do { 
                                                if(nmr == ""){ echo("O caminho não pode ser vaziu");}
                                                nmr = Console.ReadLine();
                                         } while(nmr != "" && nmr != null && nmr != string.Empty);
                                         Zipa zp = new Zipa("@"+atpt+"\\salveLiista.txt");
                                         zp.local = nmr;
                                         zp.criarArquivo();
                                         read();

                                    }else if(plt[o].ToLower().StartsWith("-caminhonome"))
                                    {
                                            echo(" Digite o Caminho ESPAÇO Nome do BackUp:");
                                            echo(@" Exemplo: C:\minhapasta BeckupFelipe "+"\n");
                                        string nmr ="";
                                        do { 
                                                if(nmr == ""){ echo("O nome não pode ser vaziu");}
                                                nmr = Console.ReadLine();
                                         } while(nmr != "" && nmr != null && nmr != string.Empty);
                                         Zipa zp = new Zipa("@"+atpt+"\\salveLiista.txt");
                                         zp.local = nmr.Split(" ")[0];
                                         zp.nome = nmr.Split(" ")[1];
                                         zp.criarArquivo();
                                         read();

                                    }else{
                                        echo(" Esse Parametro não existe no comando -backup \n");
                                        read();
                                    }
                                }

                            }else{
                                            Zipa zp = new Zipa("@"+atpt+"\\salveLiista.txt");
                                            zp.criarArquivo();
                                            read();
                            }

                        }else{
                            echo("Nenhuma Pasta adicionada a lista de BackUp use o comando -salvelista");
                            read();
                        }
                    }
                    else if (rp1 == "sair"){}
                    else{echo("Comando não existe tente outro! \nComandos: -ajuda ou sair!"); read();}
            }while(rp1.ToLower() != "sair");
        }
            
   }
       class Program
    {
        static void Main(string[] args)
        {
            BirksBackUp novo = new BirksBackUp();
            novo.inicio();
        }
    }
}
