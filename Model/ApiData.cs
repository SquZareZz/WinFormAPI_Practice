using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormAPI_Practice.Model
{
    public class ApiData
    {
        // "https://data.moi.gov.tw/MoiOD/System/DownloadFile.aspx?DATA=C65202A6-92EE-4533-A415-786B74665BF2"
        //[JsonProperty("年別")]
        public string responseCode { get; set; } = "";

        public string responseMessage { get; set; } = "";

        public string totalPage { get; set; } = "";
        public string totalDataSize { get; set; } = "";
        public string page { get; set; } = "";
        public string pageDataSize { get; set; } = "";
        public List<ApiSubData> responseData { get; set; } = new List<ApiSubData>();
    }
    public class ApiSubData
    {
        // "https://data.moi.gov.tw/MoiOD/System/DownloadFile.aspx?DATA=C65202A6-92EE-4533-A415-786B74665BF2"
        //[JsonProperty("年別")]
        public string statistic_yyymm { get; set; } = "";

        public string district_code { get; set; } = "";

        public string site_id { get; set; } = "";
        public string village { get; set; } = "";
        public string marriage_type { get; set; } = "";
        public string sex { get; set; } = "";
        public string nation { get; set; } = "";
        public string registration { get; set; } = "";
        public string marry_count { get; set; } = "";
    }
}
