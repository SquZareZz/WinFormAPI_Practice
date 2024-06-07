
# WinFormAPI_Practice

## 程式背景

練習把 WebAPI 上的資料串接到自己寫的 WinForm 上。
具備篩選資料功能
其他功能持續練習與發想中

### 介紹

#### Form1
初始化主要畫面的物件相關（InitializeComponent()）

#### 畫面按鈕事件

##### GetData_Click
取得資料按紐，對應到事件 [GetDataEvent()](#getdataevent)

##### buttonResetComboBox_Click

重置下拉選單按鈕按紐。

##### buttonResetAll_Click

清除資料按紐。

#### 非同步事件

##### GetDataEvent()

取得 API 上的資料，為 json 格式，事前先對資料類型建好模型，在 ApiData.cs 裡面。另外要撈的資料來源 URL 也在此處規範

#### 一般事件

##### SetupComboBoxTitleFilter()

初始化 Title 的 ComboBox

##### comboBoxTitleFilter_SelectedIndexChanged

Title 的 ComboBox 數值變化發生的事件

##### comboBoxValueFilter_SelectedIndexChanged

 Value 的 ComboBox 數值變化發生的事件

