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
                    // �M�ű������C���������Y�]Accept Headers�^
                    client.DefaultRequestHeaders.Accept.Clear();
                    // �s�W�������C���������Y�]Accept Headers�^
                    // ��L�� text/plain�G�¤�r�榡�Bapplication/xml�GXML �榡�B
                    // application/x-www-form-urlencoded�GURL �s�X�榡�A�q�`�Ω��洣��B
                    // application/octet-stream�G�G�i���ơBmultipart/form-data�G�Ω��ɮפW�Ǫ������
                    // �o�ǳ����U�۹������ഫ�k
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
                        comboBoxTitleFilter.Enabled = true;
                        buttonResetAll.Enabled = true;
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
            SpiderDataGridView.DataSource = apiResponse.responseData;
            //�� ComboBox �T�w�b����ܪ��a��
            comboBoxTitleFilter.SelectedIndex = -1;
            comboBoxValueFilter.SelectedIndex = -1;
            //���m���s Disable
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
                    //GetType().GetProperty() => ���o���w�Ѽƪ��W��
                    //.GetValue(x, null)���o�ȰѼƥ���(�����a�ѼƭȡA�ҥH�ĤG�� null e.g. x=1 =>(x,1))
                    //�� HashSet() �T�O�����Ƹ�T
                    var selectedValues = apiResponse.responseData.Select(x => x.GetType().GetProperty(selectedTitle)
                    .GetValue(x, null).ToString()).ToHashSet().ToList();
                    comboBoxValueFilter.DataSource = selectedValues;
                    //�ҥΤ������ ValueFilter
                    comboBoxValueFilter.Enabled = true;
                    //�ҥΤ���������s
                    buttonResetAll.Enabled = true;
                    buttonResetComboBox.Enabled = true;
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
                    // ���o�i�H����ݩʭȪ�����
                    PropertyInfo property = typeof(ApiSubData).GetProperty(comboBoxTitleFilter.SelectedItem.ToString());
                    // �v����Ū�A�p�GŪ����w���ݩ�(�W��)�A��o���ƨ��X��
                    var Target = apiResponse.responseData.Where(x => property.GetValue(x).ToString() == selectedValue).ToList();
                    SpiderDataGridView.DataSource = Target;
                    //�ҥΤ���������s
                    buttonResetAll.Enabled = true;
                    buttonResetComboBox.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���`���p{ex.Message}", "��ܲ��`");
            }
        }

        private void buttonResetComboBox_Click(object sender, EventArgs e)
        {
            try
            {
                // ��l�ƫe���Ѱ��ƥ�j�w
                comboBoxTitleFilter.SelectedIndexChanged -= comboBoxTitleFilter_SelectedIndexChanged;
                comboBoxTitleFilter.SelectedIndexChanged -= comboBoxValueFilter_SelectedIndexChanged;
                //�� ComboBox �T�w�b����ܪ��a��
                comboBoxTitleFilter.SelectedIndex = -1;
                comboBoxValueFilter.SelectedIndex = -1;
                comboBoxValueFilter.Enabled = false;
                SpiderDataGridView.DataSource = apiResponse.responseData;
                // ��_�ƥ�j�w
                comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;
                comboBoxTitleFilter.SelectedIndexChanged += comboBoxValueFilter_SelectedIndexChanged;
                // �������Ы��A���D����ŦX
                buttonResetComboBox.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���`���p{ex.Message}", "��ܲ��`");
            }
        }

        private void buttonResetAll_Click(object sender, EventArgs e)
        {
            // ��l�ƫe���Ѱ��ƥ�j�w
            comboBoxTitleFilter.SelectedIndexChanged -= comboBoxTitleFilter_SelectedIndexChanged;
            comboBoxTitleFilter.SelectedIndexChanged -= comboBoxValueFilter_SelectedIndexChanged;
            //��l�ưѼ�
            apiResponse = new ApiData();
            //ComboBox
            //Title ���e���ܡA�����m
            comboBoxTitleFilter.Enabled = false;
            comboBoxValueFilter.Enabled = false;
            comboBoxValueFilter.DataSource = null;
            //DataGridView
            SpiderDataGridView.DataSource = null;
            SpiderDataGridView.DataSource = apiResponse.responseData;
            //Button
            buttonResetComboBox.Enabled = false;
            buttonResetAll.Enabled = false;            
            // ��_�ƥ�j�w
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxValueFilter_SelectedIndexChanged;
        }
    }
}