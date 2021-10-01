//Serviços REST
// 23/12/2020
//Cliente REST
//
//Criado por Diogo Rocha nº16966




using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Runtime.Serialization.Json;
//using Newtonsoft.Json;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// url : https://localhost:44383/Infetados/adicionaInfetado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdicionarInfetado(object sender, EventArgs e)
        {

            string url = "https://pandemia-api.azure-api.net/Infetados/[ACTION]";
            //verificar se foi introduzio alguma coisa
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && comboBox1.Text != "" && textBox9.Text != "" && comboBox2.Text != "")
            {
                url = url.Replace("[ACTION]", "adicionaInfetado");
                //Vai criar o objeto para preencher
                Pessoa person = new Pessoa(default(int), (textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text), (textBox4.Text), (textBox5.Text), (textBox6.Text), int.Parse(textBox7.Text), (comboBox1.Text), (textBox9.Text), (comboBox2.ToString()), default(string));


                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri(url);
                //string jsonString = JsonConvert.SerializeObject(person);
                string jsonString = JsonSerializer.Serialize(person);
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                var request = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = cliente.PostAsync(url, request).Result;
                var result = response.Content.ReadAsStringAsync();

                MessageBox.Show("Adicionou mais um infetado com sucesso!!!");

            }
            else
            {
                MessageBox.Show("Tem de definir valores em todos os campos"); return;
            }

        }

        private void Ver_Click(object sender, EventArgs e)
        {
            //get para ver infetados

            string url = "https://pandemia-api.azure-api.net/VeInfetados";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);


            foreach (Pessoa p in h)

            {
                string[] row = new string[] { p.id.ToString(), p.nome.ToString(), p.idade.ToString(), p.nif.ToString(), p.email.ToString(), p.nacionalidade.ToString(), p.sintomas.ToString(), p.contacto.ToString(), p.concelho.ToString(), p.genero.ToString(), p.area_tratamento.ToString(), p.data.ToString() };
                dataGridView1.Rows.Add(row);
            }



        }

        //botao para apagar infetados
        private void ApagaInfetado(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                string url = "https://pandemia-api.azure-api.net/Infetados/removeInfetado/" + textBox12.Text;

                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri(url);
                string jsonString = JsonSerializer.Serialize(url);
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                var request = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = cliente.PostAsync(url, request).Result;
                var result = response.Content.ReadAsStringAsync();



                MessageBox.Show("Morto adicionado!!!");

            }
            else
            {
                MessageBox.Show("Tem de definir valores em todos os campos"); return;
            }

        }


        //botao para ver idosos
        private void button3_Click_1(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/Idosos";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            foreach (Pessoa p in h)

            {
                string[] row = new string[] { p.id.ToString(), p.nome.ToString(), p.idade.ToString(), p.nif.ToString(), p.email.ToString(), p.nacionalidade.ToString(), p.sintomas.ToString(), p.contacto.ToString(), p.concelho.ToString(), p.genero.ToString(), p.area_tratamento.ToString(), p.data.ToString() };
                dataGridView1.Rows.Add(row);
            }

        }

        //Selecionar local
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/Area/Tratamento/" + comboBox3.Text;
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            textBox10.Text = h.Count.ToString();
        }

        //conta infetados no dia
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/Casos/Diarios/" + comboBox4.Text;
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            textBox10.Text = h.Count.ToString();

        }
        //casos por concelho
        private void button6_Click(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/Casos/Concelho/" + textBox11.Text;
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            textBox10.Text = h.Count.ToString();

        }
        //adicionar morto
        private void button8_Click(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/AdicionaObitos/" + textBox14.Text;
            //verificar se foi introduzio alguma coisa
            if (textBox14.Text != "")
            {
                //Vai criar o objeto para preencher

                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri(url);
                string jsonString = JsonSerializer.Serialize(url);
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                var request = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = cliente.PostAsync(url, request).Result;
                var result = response.Content.ReadAsStringAsync();

                MessageBox.Show("Adicionou mais um morto!!!");

            }
            else
            {
                MessageBox.Show("Tem de definir valores em todos os campos"); return;
            }
        }


        //adicionar recuperado

        private void button7_Click(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/AdicionaRecuperados/" + textBox13.Text;
            //verificar se foi introduzio alguma coisa
            if (textBox13.Text != "")
            {
                //Vai criar o objeto para preencher

                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri(url);
                string jsonString = JsonSerializer.Serialize(url);
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                var request = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = cliente.PostAsync(url, request).Result;
                var result = response.Content.ReadAsStringAsync();

                MessageBox.Show("Adicionou mais um recuperado com sucesso!!!");

            }
            else
            {
                MessageBox.Show("Tem de definir valores em todos os campos"); return;
            }


        }
        //recuperados dia
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/Obitosdia/" + comboBox6.Text;
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            textBox10.Text = h.Count.ToString();

        }
        //obitos do dia
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
        
            string url = "https://pandemia-api.azure-api.net/Infetados/Recuperadosdia/" + comboBox5.Text;
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            textBox10.Text = h.Count.ToString();

        }
        //ver todos os recuperados
        private void button9_Click(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/VeRecuperados";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Recuperado>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Recuperado> h = (List<Recuperado>)jsonSerializer.ReadObject(ms);

            foreach (Recuperado p in h)

            {
                string[] row = new string[] { p.id.ToString(), p.nome.ToString(), p.idade.ToString(), p.nif.ToString(), p.email.ToString(), p.nacionalidade.ToString(), p.sintomas.ToString(), p.contacto.ToString(), p.concelho.ToString(), p.genero.ToString(), p.area_tratamento.ToString(), p.data.ToString() };
                dataGridView1.Rows.Add(row);
            }

        }
        //ver todos os mortos
        private void button10_Click(object sender, EventArgs e)
        {
            string url = "https://pandemia-api.azure-api.net/Infetados/VeObitos";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Pessoa>));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            List<Pessoa> h = (List<Pessoa>)jsonSerializer.ReadObject(ms);

            foreach (Morto p in h)

            {
                string[] row = new string[] { p.id.ToString(), p.nome.ToString(), p.idade.ToString(), p.nif.ToString(), p.email.ToString(), p.nacionalidade.ToString(), p.sintomas.ToString(), p.contacto.ToString(), p.concelho.ToString(), p.genero.ToString(), p.area_tratamento.ToString(), p.data.ToString() };
                dataGridView1.Rows.Add(row);
            }

        }
        //serviço externo que mostra os casos acumulados por concelho 
        private void button9_Click_1(object sender, EventArgs e)
        {

            string url = "https://pandemia-api.azure-api.net/COVID";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Root));
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            Root c = (Root)jsonSerializer.ReadObject(ms);
            foreach (Feature f in c.features)

            {
                string[] row = new string[] { f.attributes.Concelho, f.attributes.ConfirmadosAcumulado.ToString() };
                dataGridView2.Rows.Add(row);
            }
        }



    }

}














