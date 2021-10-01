//Serviços REST
// 23/12/2020
//Classe pessoa
//
//Criado por Diogo Rocha nº16966





using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TP2.Model
{

    public class Pessoa
    {

        public Pessoa()
        {

            this.nome = default(string);
            this.idade = default(int);
            this.nif = default(int);
            this.email = default(string);
            this.nacionalidade = default(string);
            this.sintomas = default(string);
            this.contacto = default(int);
            this.genero = default(string);
            this.concelho = default(string);
            this.area_tratamento = default(string);

        }


        public Pessoa(string nome, int idade, int nif, string email, string nacionalidade, string sintomas, int contacto, string genero, string concelho, string area_tratamento)
        {
            this.nome = nome;
            this.idade = idade;
            this.nif = nif;
            this.email = email;
            this.nacionalidade = nacionalidade;
            this.sintomas = sintomas;
            this.contacto = contacto;
            this.genero = genero;
            this.concelho = concelho;
            this.area_tratamento = area_tratamento;
        }

        public string nome { get; set; }
        public int idade { get; set; }
        public int nif { get; set; }
        public string email { get; set; }
        public string nacionalidade { get; set; }
        public string sintomas { get; set; }
        public int contacto { get; set; }
        public string concelho { get; set; }
        public string genero { get; set; }
        public string area_tratamento { get; set; }


    }





}




