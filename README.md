# DeployTool
遠端部署工具

這個工具可以將本機資料夾的檔案透過壓縮上傳到遠端主機後，自動的解壓縮及覆蓋目錄。

在覆蓋過程中會停止web site及app pool後，再次啟動。

# 環境

* 在 Windows Server 2012 x64 及 PowerShell 50上可以正常使用。

# How to use

* 在 releaseFile中，已預先編譯好檔案，下載該部份即可。

* 執行主程式前，請先分別設定.Config檔。
  * Client端主程式 『DeploySenderFramework』
    * DeploySenderFramework.exe.config
  * Server端主程式 『DeployService』
    * DeployService.exe.config
  
* 在Server上直接執行Server主程式開始進行監聽，預設為 http://*:82
* 可針對Client主程式進行捷徑設定，以方便使用，每次執行Client主程式，輸入說明後，開始進行部署

# Config Setting

* Client
  * LocalFileFolder：本機要壓縮的資料夾
  * LocalSaveZipFolder：本機暫存要部署的zip檔 (永遠覆蓋zip檔案)
  * ServerSaveZipFolder：Server要暫存zip檔的資料夾 (在server 端會套上 guid 防止覆蓋，注意要定時清理)
  * serverDeployFolder：Server解壓縮zip檔後要發佈的資料夾
  * ZipName：設定zip檔的名稱
  * DeployUri：Server的web api位址
  * DeployKey：金鑰，必需與server端的一致
  * DeleteFiles_BeforeZip：壓縮時要排除的檔案
* Server
  * WebSite：佈署前先停止及部署後啟動的 Web Site
  * AppPoolName：佈署前先停止及部署後啟動的 App Pool
  * AllowDeployKey：金鑰，必需與client端的一致
  * ListenUrl：要監聽的 url

