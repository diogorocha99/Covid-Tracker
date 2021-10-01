//Serviços REST
// 23/12/2020
//Controller da classe pessoa
//
//Criado por Diogo Rocha nº16966






using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Npgsql;

using TP2.Model;
using System.Collections;
using NpgsqlTypes;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TP2.Controllers
{
    [ApiController]
    [Route("Infetados")]
    public class PessoaController : Controller
    {


        //faz a ligação ao server AZURE que está associado a um localhost do pc
        //conexao ao azure que por ter limite de orçamento foi substituido por conexao ao Heroku

        //NpgsqlConnection connection = new NpgsqlConnection((@"Server=isi2020.postgres.database.azure.com;Database=Pandemia;Port=5432;User Id=isi2020@isi2020;Password=Qwerty1234;Ssl Mode=Require;"));
        NpgsqlConnection connection = new NpgsqlConnection((@"Server= ec2-176-34-114-78.eu-west-1.compute.amazonaws.com; Port=5432;User Id =zopdtoykgnswxp; Password=850126216a32bbcc33906fb0dcec51c709d637e4cc7f1012adcae57216e88bf9;Database=d1lonqns55uvq;sslmode=Require;Trust Server Certificate=true"));


        //[AllowAnonymous]
        //[HttpPost("login")]
        //public AuthenticateResponse Login(AuthenticateRequest loginDetalhes)
        //{
        //    bool result = ValidarUtilizador(loginDetalhes);
        //    if (result)
        //    {
        //        var tokenString = GerarToken();

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        
        

        //private bool ValidarUtilizador(AuthenticateRequest a)
        //{
        //    int contador;
        //    connection.Open();
        //    string query = @"Select * from seguranca where (nome = @nome) AND (password = @password)";
        //    NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
        //    cmd.Parameters.AddWithValue("@nome", a.username);
        //    cmd.Parameters.AddWithValue("@idade", a.password);
        //    contador = cmd.ExecuteNonQuery();
        //    if (contador == 1) return true;
        //    return false;
        //}


        //string GerarToken()
        //{
        //    var issuer = _config["Jwt:Issuer"];
        //    var audience = _config["Jwt:Audience"];
        //    var expiry = DateTime.Now.AddMinutes(120);  //válido por 2 horas
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var stringToken = tokenHandler.WriteToken(token);
        //    return stringToken;
        //}


        //Um post para adicionar um infetado
        [HttpPost("adicionaInfetado")]
        //public bool AddPessoa(Pessoa x)
        public bool AddPessoa(Pessoa x)
        {
            bool resultado = true;
            int contador;
            connection.Open();
            try
            {
                //insere estes parametros
                
                connection.Open();
                string query = @"INSERT INTO pessoa (id, nome, idade, nif, email, nacionalidade, sintomas, contacto, genero, concelho, area_tratamento) VALUES ((SELECT MAX(id) FROM pessoa) + 1 ,@nome, @idade, @nif, @email, @nacionalidade, @sintomas, @contacto, @genero, @concelho, @area_tratamento)";

                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nome", x.nome);
                cmd.Parameters.AddWithValue("@idade", x.idade);
                cmd.Parameters.AddWithValue("@nif", x.nif);
                cmd.Parameters.AddWithValue("@email", x.email);
                cmd.Parameters.AddWithValue("@nacionalidade", x.nacionalidade);
                cmd.Parameters.AddWithValue("@sintomas", x.sintomas);
                cmd.Parameters.AddWithValue("@contacto", x.contacto);
                cmd.Parameters.AddWithValue("@genero", x.genero);
                cmd.Parameters.AddWithValue("@concelho", x.concelho);
                cmd.Parameters.AddWithValue("@area_tratamento", x.area_tratamento);


                //executa a query
                contador = cmd.ExecuteNonQuery();

                connection.Close();
                //Se houver 1 query executada então retornará true, se não false
                if (contador == 1) resultado = true;
                resultado = false;
            }
            catch(Exception e)
            {
                throw new Exception("Erro" + e.Message);
            }
            finally
            {
                connection.Close();
            }
                return resultado;
            
        }
        //remove um infetado
        [HttpDelete("removeInfetado/{id}")]
        public bool DeletePessoa(int id)
        {
            int n;

            connection.Open();

            string query = "DELETE FROM pessoa WHERE id = @id";

            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            //Adicionar o parametro id e compara-o com o id inserido, se for igual remove a pessoa com esse ID
            cmd.Parameters.AddWithValue("@id", id);
       
            n = cmd.ExecuteNonQuery();
            //fecha a conexao
            connection.Close();
            //Mesmo que em cima, uma query associada a 1
            if (n >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Um get que vê os infetados
        //[HttpGet("veInfetados")]
        [HttpGet("/VeInfetados")]


        public string getInfetdos()
        {

            connection.Open();
            try
            {
                //seleciona todos os atributos da pessoa
                string query = "Select * FROM pessoa";
                
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, connection);
                //Cria um dataset
                //DataSet ds = new DataSet();  //Com o dataset o json não conseguia converter de novo para a classe devolvendo null por isso optamos por datatable
                
                DataTable dt = new DataTable();
                //Associa a query "da" à dataset vazia ds
                da.Fill(dt);
                //Recebe o "ds" e converte para JSON
                string json = JsonConvert.SerializeObject(dt);
                return json;
            }
            //exceção 
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }
        //É um get que fornece a informação onde os infetaados estão a ser tratados, que na base de dados está definido como em "Casa", "Hospital" e "Cuidados Intensivos" sendo estas
        //as únicas opçoes disponiveis quando se insere uma area_tratamento
        [HttpGet("Area/Tratamento/{area_tratamento}")]
        public string Tratamento(string area_tratamento)
        {
            DataTable dt = new DataTable();
            //abrir conexao
            connection.Open();

            string query = @"Select * from pessoa where (area_tratamento = @area_tratamento)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

            cmd.Parameters.Add("@area_tratamento", NpgsqlDbType.Varchar).Value = area_tratamento;
            //Executa a query
            cmd.ExecuteNonQuery();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            da.Fill(dt);

            string json = JsonConvert.SerializeObject(dt);

            connection.Close();
            return json;

        }


        [HttpGet("Idosos" )]
        public string CasosIdosos()
        {
            connection.Open();
            try
            {
                string query = @"Select * from pessoa where (@idade > 65)";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, connection);
                //Cria um dataset
                DataTable dt = new DataTable();
                //Associa a query "da" à dataset vazia ds
                da.Fill(dt);
                //Recebe o "ds" e converte para JSON
                string json = JsonConvert.SerializeObject(dt);
                return json;
            }
            //exceção 
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        [HttpGet("Casos/Concelho/{concelho}")]
        public string CasosConcelho(string concelho)
        {
            DataTable dt = new DataTable();
            //abrir conexao
            connection.Open();
   
            string query = @"Select * from pessoa where concelho = @concelho";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

            cmd.Parameters.Add("@concelho", NpgsqlDbType.Varchar).Value = concelho;
            //Executa a query
            cmd.ExecuteNonQuery();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            da.Fill(dt);

            string json = JsonConvert.SerializeObject(dt);

            connection.Close();
            return json;

        }

        //infetados num determinado dia


        [HttpGet("Casos/Diarios/{data}")]
        public string InfetadosDia(DateTime data) {

            connection.Open();
            try
            {
                DataTable dt = new DataTable();
                //abrir conexao

                string query = @"Select * from pessoa where data = @data";
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.Add("@data", NpgsqlDbType.Date).Value = data;
                //Executa a query
                cmd.ExecuteNonQuery();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                da.Fill(dt);

                string json = JsonConvert.SerializeObject(dt);

                connection.Close();
                return json;
            }
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        [HttpPost("AdicionaRecuperados/{id}")]
        public bool AdicionaRecuperado(int id)
        {
            int cont;
            connection.Open();
            try
            {
                string query = "INSERT INTO recuperado SELECT* FROM pessoa Where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cont = cmd.ExecuteNonQuery();

                connection.Close();
                //Se houver 1 query executada então retornará true, se não false
                if (cont == 1) return true;
                return false;
            }
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        [HttpGet("VeRecuperados")]
        public string getRecuperados()
        {

            connection.Open();
            try
            {
                DataTable dt = new DataTable();
                //seleciona todos os atributos da pessoa
                string query = "Select * FROM recuperado";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, connection);
                //Associa a query "da" à dataset vazia ds
                da.Fill(dt);
                //Recebe o "ds" e converte para JSON
                string json = JsonConvert.SerializeObject(dt);
                return json;
            }
            //exceção 
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }


        [HttpDelete("ApagaRecuperado")]
        public bool DeleteRecuperado(int id)
        {
            int n;

            connection.Open();

            string query = "DELETE FROM recuperado WHERE id = @id";

            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            //Adicionar o parametro id e compara-o com o id inserido, se for igual remove a pessoa com esse ID
            cmd.Parameters.AddWithValue("@id", id);

            n = cmd.ExecuteNonQuery();
            //fecha a conexao
            connection.Close();
            //Mesmo que em cima, uma query associada a 1
            if (n >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [HttpGet("Recuperadosdia/{data}")]
        public string RecuperadosDia(DateTime data)
        {

            connection.Open();
            try
            {
                DataTable dt = new DataTable();
                //abrir conexao

                string query = @"Select * from recuperado where data = @data";
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.Add("@data", NpgsqlDbType.Date).Value = data;
                //Executa a query
                cmd.ExecuteNonQuery();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                da.Fill(dt);

                string json = JsonConvert.SerializeObject(dt);

                connection.Close();
                return json;
            }
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        [HttpPost("AdicionaObitos/{id}")]
        public bool AdicionaObito(int id)
        {
            int cont;
            connection.Open();
            try
            {
                string query = "INSERT INTO morto SELECT* FROM pessoa Where id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cont = cmd.ExecuteNonQuery();

                connection.Close();
                //Se houver 1 query executada então retornará true, se não false
                if (cont == 1) return true;
                return false;
            }
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        [HttpGet("VeObitos")]
        public string getObitos()
        {

            connection.Open();
            try
            {
                //seleciona todos os atributos da pessoa
                string query = "Select * FROM morto";

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, connection);
                //Cria um dataset
                DataSet ds = new DataSet();
                //Associa a query "da" à dataset vazia ds
                da.Fill(ds, "morto");
                //Recebe o "ds" e converte para JSON
                string json = JsonConvert.SerializeObject(ds);
                return json;
            }
            //exceção 
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }


        [HttpDelete("ApagaObito")]
        public bool DeleteObito(int id)
        {
            int n;

            connection.Open();

            string query = "DELETE FROM morto WHERE id = @id";

            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            //Adicionar o parametro id e compara-o com o id inserido, se for igual remove a pessoa com esse ID
            cmd.Parameters.AddWithValue("@id", id);

            n = cmd.ExecuteNonQuery();
            //fecha a conexao
            connection.Close();
            //Mesmo que em cima, uma query associada a 1
            if (n >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //obitos por data
        [HttpGet("Obitosdia/{data}")]
        public string ObitosDia(DateTime data)
        {

            connection.Open();
            try
            {
                DataTable dt = new DataTable();
                //abrir conexao

                string query = @"Select * from morto where data = @data";
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

                cmd.Parameters.Add("@data", NpgsqlDbType.Date).Value = data;
                //Executa a query
                cmd.ExecuteNonQuery();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                da.Fill(dt);

                string json = JsonConvert.SerializeObject(dt);

                connection.Close();
                return json;
            }
            catch (Exception error)
            {
                throw new Exception("Erro" + error.Message);
            }
            finally
            {
                connection.Close();
            }

        }




    }
}
