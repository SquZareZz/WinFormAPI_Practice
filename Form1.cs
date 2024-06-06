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
            // 初始化前先解除事件綁定
            comboBoxTitleFilter.SelectedIndexChanged -= comboBoxTitleFilter_SelectedIndexChanged;
            SetupComboBoxTitleFilter();

            // 重新綁定事件
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;

        }

        private async void GetData_Click(object sender, EventArgs e)
        {
            await GetDataEvent();
        }
        private async Task GetDataEvent()
        {
            // 設定API的URL
            //string apiUrl = "你的API的URL";
            string apiUrl = "https://data.moi.gov.tw/MoiOD/System/DownloadFile.aspx?DATA=C65202A6-92EE-4533-A415-786B74665BF2";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 設定Accept header為application/json
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // 發送GET請求到API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // 檢查請求是否成功
                    if (response.IsSuccessStatusCode)
                    {
                        // 解析JSON回應為指定的C#類別
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        // 使用 JsonConvert 來反序列化 JSON 回應
                        apiResponse = JsonConvert.DeserializeObject<ApiData>(jsonResponse);
                        if (apiResponse != null)
                        {
                            SpiderDataGridView.DataSource = apiResponse.responseData;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"請求失敗，狀態碼: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"發生例外: {ex.Message}");
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
                MessageBox.Show($"異常狀況{ex.Message}", "選擇異常");
            }

        }

        private void comboBoxValueFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxValueFilter.SelectedIndex != -1 && apiResponse != null && apiResponse.responseData.Count != 0)
                {
                    string? selectedValue = comboBoxValueFilter.SelectedItem.ToString();
                    // 使用反射獲取屬性值
                    PropertyInfo property = typeof(ApiSubData).GetProperty(comboBoxTitleFilter.SelectedItem.ToString());
                    var Target = apiResponse.responseData.Where(x => property.GetValue(x).ToString() == selectedValue).ToList();
                    SpiderDataGridView.DataSource = Target;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"異常狀況{ex.Message}", "選擇異常");
            }
        }
    }
}