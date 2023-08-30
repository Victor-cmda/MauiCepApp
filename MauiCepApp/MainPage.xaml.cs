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
        }

        public void AlterSearch(object sender, CheckedChangedEventArgs e)
        {
            if (selectCep.IsChecked)
            {
                AlterLabel.Text = "Consulte seu CEP";
                cepEntry.Placeholder = "Digite seu CEP aqui";
            }
            else if (selectRua.IsChecked)
            {
                AlterLabel.Text = "Consulte sua Rua";
                cepEntry.Placeholder = "Digite sua Rua aqui";

            }
        }

        public async void ButtonClicked(object sender, EventArgs e)
        {
            string cep = cepEntry.Text;

            using HttpClient client = new HttpClient();

            string requestUrl = "";
            if (selectCep.IsChecked)
                requestUrl = $"{ViaCepBaseUrl}{cep}/json/";
            else
            {
                cep = cep.Replace(",", "/");
                cep = cep.Replace(" ", "%20");
                requestUrl = $"{ViaCepBaseUrl}{cep}/json/";
            }

            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (selectRua.IsChecked)
                    {

                        List<ViaCepResponse> viaCepResponse = JsonSerializer.Deserialize<List<ViaCepResponse>>(responseBody);

                        listViewOfResponse.ItemsSource = viaCepResponse;
                    }
                    else
                    {
                        ViaCepResponse viaCepResponse = JsonSerializer.Deserialize<ViaCepResponse>(responseBody);

                        List<ViaCepResponse> listOfViaCepResponse = new List<ViaCepResponse>();
                        listOfViaCepResponse.Add(viaCepResponse);

                        listViewOfResponse.ItemsSource = listOfViaCepResponse;
                    }
                    notFound.IsVisible = false;
                }
                else
                {
                    notFound.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
