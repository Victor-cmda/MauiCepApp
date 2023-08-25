using MauiCepApp.Class;
using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text.Json;

namespace MauiCepApp
{
    public partial class MainPage : ContentPage
    {
        private const string ViaCepBaseUrl = "https://viacep.com.br/ws/";

        public MainPage()
        {
            InitializeComponent();



            cepStatic.IsVisible = false;
            logradouroStatic.IsVisible = false;
            bairroStatic.IsVisible = false;
            localidadeStatic.IsVisible = false;
        }

        public async void ButtonClicked(object sender, EventArgs e)
        {
            string cep = cepEntry.Text;

            if (!string.IsNullOrWhiteSpace(cep) && cep.Length == 8)
            {
                using HttpClient client = new HttpClient();

                string requestUrl = $"{ViaCepBaseUrl}{cep}/json/";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        ViaCepResponse viaCepResponse = JsonSerializer.Deserialize<ViaCepResponse>(responseBody);

                        cepLabel.Text = viaCepResponse.cep;
                        logradouroLabel.Text = viaCepResponse.logradouro;
                        bairroLabel.Text = viaCepResponse.bairro;
                        localidadeLabel.Text = viaCepResponse.localidade;

                        notFound.IsVisible = false;
                        cepStatic.IsVisible = true;
                        logradouroStatic.IsVisible = true;
                        bairroStatic.IsVisible = true;
                        localidadeStatic.IsVisible = true;

                        if(viaCepResponse.cep == null)
                        {
                            notFound.IsVisible = true;
                            cepStatic.IsVisible = false;
                            logradouroStatic.IsVisible = false;
                            bairroStatic.IsVisible = false;
                            localidadeStatic.IsVisible = false;
                        }
                    }
                    else
                    {
                        cepLabel.Text = "CEP não encontrado";
                        logradouroLabel.Text = "";
                        bairroLabel.Text = "";
                        localidadeLabel.Text = "";

                        cepStatic.IsVisible = false;
                        logradouroStatic.IsVisible = false;
                        bairroStatic.IsVisible = false;
                        localidadeStatic.IsVisible = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                    cepStatic.IsVisible = false;
                    logradouroStatic.IsVisible = false;
                    bairroStatic.IsVisible = false;
                    localidadeStatic.IsVisible = false;
                }
            }
        }
    }
}
