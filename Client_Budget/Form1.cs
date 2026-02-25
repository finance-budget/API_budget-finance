using System.Net.Http.Json;
namespace Client_Budget
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // 1. L'adresse de ton API (Vérifie que le port 7195 est le bon !)
            string url = "https://localhost:7195/api/Auth/login";

            // 2. On prépare les données à envoyer (le paquet)
            var loginData = new
            {
                Username = txtPseudo.Text,
                Password = txtPassword.Text
            };

            // 3. On crée le facteur (HttpClient)
            using (var client = new HttpClient())
            {
                try
                {
                    // 4. On envoie le paquet et on attend la réponse
                    var response = await client.PostAsJsonAsync(url, loginData);

                    // 5. On vérifie la réponse
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Connexion réussie ! Bienvenue.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // C'est ici qu'on ouvrira la prochaine fenêtre plus tard
                    }
                    else
                    {
                        MessageBox.Show("Pseudo ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible de contacter le serveur : " + ex.Message, "Erreur Technique");
                }
            }
        }
    }
}
