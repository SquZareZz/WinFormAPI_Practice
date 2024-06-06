using WinFormAPI_Practice.Model;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using System.Reflection;

namespace WinFormAPI_Practice
{
    public partial class Form1 : Form
    {
        private ApiData? apiResponse = new ApiData();
        public Form1()
        {
            InitializeComponent();
            // ��l�ƫe���Ѱ��ƥ�j�w
            comboBoxTitleFilter.SelectedIndexChanged -= comboBoxTitleFilter_SelectedIndexChanged;
            SetupComboBoxTitleFilter();

            // ���s�j�w�ƥ�
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;

        }

        private async void GetData_Click(object sender, EventArgs e)
        {
            await GetDataEvent();
        }
        private async Task GetDataEvent()
        {
            // �]�wAPI��URL
            //string apiUrl = "�A��API��URL";
            string apiUrl = "https://data.moi.gov.tw/MoiOD/System/DownloadFile.aspx?DATA=C65202A6-92EE-4533-A415-786B74665BF2";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // �]�wAccept header��application/json
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // �o�eGET�ШD��API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // �ˬd�ШD�O�_���\
                    if (response.IsSuccessStatusCode)
                    {
                        // �ѪRJSON�^�������w��C#���O
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        // �ϥ� JsonConvert �ӤϧǦC�� JSON �^��
                        apiResponse = JsonConvert.DeserializeObject<ApiData>(jsonResponse);
                        if (apiResponse != null)
                        {
                            SpiderDataGridView.DataSource = apiResponse.responseData;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"�ШD���ѡA���A�X: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"�o�ͨҥ~: {ex.Message}");
                }
            }
        }

        private void SetupComboBoxTitleFilter()
        {
            var titles = new List<string> { "statistic_yyymm", "district_code", "site_id", "village", "marriage_type", "sex", "nation", "registration", "marry_count" };

            comboBoxTitleFilter.DataSource = titles;
            comboBoxTitleFilter.SelectedIndex = -1;
            comboBoxValueFilter.SelectedIndex = -1;
        }

        private void comboBoxTitleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string? selectedTitle = comboBoxTitleFilter.SelectedItem.ToString();
                if (comboBoxTitleFilter.SelectedIndex != -1 && apiResponse.responseData.Count != 0)
                {
                    var selectedValues = apiResponse.responseData.Select(x => x.GetType().GetProperty(selectedTitle)
                    .GetValue(x, null).ToString()).ToHashSet().ToList();
                    comboBoxValueFilter.DataSource = selectedValues;
                    comboBoxValueFilter.SelectedIndex = -1;
                    comboBoxValueFilter.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���`���p{ex.Message}", "��ܲ��`");
            }

        }

        private void comboBoxValueFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxValueFilter.SelectedIndex != -1 && apiResponse != null && apiResponse.responseData.Count != 0)
                {
                    string? selectedValue = comboBoxValueFilter.SelectedItem.ToString();
                    // �ϥΤϮg����ݩʭ�
                    PropertyInfo property = typeof(ApiSubData).GetProperty(comboBoxTitleFilter.SelectedItem.ToString());
                    var Target = apiResponse.responseData.Where(x => property.GetValue(x).ToString() == selectedValue).ToList();
                    SpiderDataGridView.DataSource = Target;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���`���p{ex.Message}", "��ܲ��`");
            }
        }
    }
}