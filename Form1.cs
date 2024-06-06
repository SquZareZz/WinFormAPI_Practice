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
        private ApiData apiResponse = new ApiData();
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
                    // 清空接受的媒體類型標頭（Accept Headers）
                    client.DefaultRequestHeaders.Accept.Clear();
                    // 新增接受的媒體類型標頭（Accept Headers）
                    // 其他像 text/plain：純文字格式、application/xml：XML 格式、
                    // application/x-www-form-urlencoded：URL 編碼格式，通常用於表單提交、
                    // application/octet-stream：二進位資料、multipart/form-data：用於檔案上傳的表單資料
                    // 這些都有各自對應的轉換法
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
                        comboBoxTitleFilter.Enabled = true;
                        buttonResetAll.Enabled = true;
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
            SpiderDataGridView.DataSource = apiResponse.responseData;
            //讓 ComboBox 固定在未選擇的地方
            comboBoxTitleFilter.SelectedIndex = -1;
            comboBoxValueFilter.SelectedIndex = -1;
            //重置按鈕 Disable
            buttonResetComboBox.Enabled = false;
            buttonResetAll.Enabled = false;
        }

        private void comboBoxTitleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string? selectedTitle = comboBoxTitleFilter.SelectedItem.ToString();
                if (comboBoxTitleFilter.SelectedIndex != -1 && apiResponse.responseData.Count != 0)
                {
                    //GetType().GetProperty() => 取得指定參數的名稱
                    //.GetValue(x, null)取得值參數本身(但不帶參數值，所以第二項 null e.g. x=1 =>(x,1))
                    //轉 HashSet() 確保不重複資訊
                    var selectedValues = apiResponse.responseData.Select(x => x.GetType().GetProperty(selectedTitle)
                    .GetValue(x, null).ToString()).ToHashSet().ToList();
                    comboBoxValueFilter.DataSource = selectedValues;
                    //啟用不能按的 ValueFilter
                    comboBoxValueFilter.Enabled = true;
                    //啟用不能按的按鈕
                    buttonResetAll.Enabled = true;
                    buttonResetComboBox.Enabled = true;
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
                    // 取得可以獲取屬性值的物件
                    PropertyInfo property = typeof(ApiSubData).GetProperty(comboBoxTitleFilter.SelectedItem.ToString());
                    // 逐行資料讀，如果讀到指定的屬性(名稱)，把這行資料取出來
                    var Target = apiResponse.responseData.Where(x => property.GetValue(x).ToString() == selectedValue).ToList();
                    SpiderDataGridView.DataSource = Target;
                    //啟用不能按的按鈕
                    buttonResetAll.Enabled = true;
                    buttonResetComboBox.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"異常狀況{ex.Message}", "選擇異常");
            }
        }

        private void buttonResetComboBox_Click(object sender, EventArgs e)
        {
            try
            {
                // 初始化前先解除事件綁定
                comboBoxTitleFilter.SelectedIndexChanged -= comboBoxTitleFilter_SelectedIndexChanged;
                comboBoxTitleFilter.SelectedIndexChanged -= comboBoxValueFilter_SelectedIndexChanged;
                //讓 ComboBox 固定在未選擇的地方
                comboBoxTitleFilter.SelectedIndex = -1;
                comboBoxValueFilter.SelectedIndex = -1;
                comboBoxValueFilter.Enabled = false;
                SpiderDataGridView.DataSource = apiResponse.responseData;
                // 恢復事件綁定
                comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;
                comboBoxTitleFilter.SelectedIndexChanged += comboBoxValueFilter_SelectedIndexChanged;
                // 不給反覆按，除非條件符合
                buttonResetComboBox.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"異常狀況{ex.Message}", "選擇異常");
            }
        }

        private void buttonResetAll_Click(object sender, EventArgs e)
        {
            // 初始化前先解除事件綁定
            comboBoxTitleFilter.SelectedIndexChanged -= comboBoxTitleFilter_SelectedIndexChanged;
            comboBoxTitleFilter.SelectedIndexChanged -= comboBoxValueFilter_SelectedIndexChanged;
            //初始化參數
            apiResponse = new ApiData();
            //ComboBox
            //Title 內容不變，不重置
            comboBoxTitleFilter.Enabled = false;
            comboBoxValueFilter.Enabled = false;
            comboBoxValueFilter.DataSource = null;
            //DataGridView
            SpiderDataGridView.DataSource = null;
            SpiderDataGridView.DataSource = apiResponse.responseData;
            //Button
            buttonResetComboBox.Enabled = false;
            buttonResetAll.Enabled = false;            
            // 恢復事件綁定
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxValueFilter_SelectedIndexChanged;
        }
    }
}