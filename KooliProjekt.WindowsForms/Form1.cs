using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Api; // Make sure this points to your new Ingredient class

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadData();
        }

        private async Task LoadData()
        {
            // Update the port (e.g., 5086) to match your launchSettings.json
            var url = "http://localhost:5086/api/Ingredients/List?page=1&pageSize=10";

            try
            {
                using var client = new HttpClient();
                var response = await client.GetFromJsonAsync<OperationResult<PagedResult<Ingredient>>>(url);

                if (response != null && response.Value != null)
                {
                    // Bind the list of results to the DataGridView
                    dataGridView1.DataSource = response.Value.Results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to API: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Leave this empty or delete the link via Option 1 later
        }
    }
}