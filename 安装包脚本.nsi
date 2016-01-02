; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "长三角商品现货行情客户端"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "Yrdce"
!define PRODUCT_WEB_SITE "http://www.Yrdce.com"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"


SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "..\..\Pictures\logo.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!define MUI_FINISHPAGE_RUN_TEXT "运行 长三角商品现货行情客户端"
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"


; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!insertmacro MUI_PAGE_FINISH



; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS



;安装函数定义


; ------ MUI 现代界面定义结束 ------



Section
SectionEnd

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "长三角商品现货行情客户端安装包.exe"
InstallDir "$PROGRAMFILES\长三角商品现货行情客户端"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File /r "D:\YrdceClient\*.*"
  CreateDirectory "$SMPROGRAMS\长三角商品现货行情客户端"
  CreateShortCut "$SMPROGRAMS\长三角商品现货行情客户端\长三角商品现货行情客户端.lnk" "$INSTDIR\YrdceTradeApp.exe"
  
  CreateShortCut "$desktop\长三角商品现货行情客户端.lnk" "$INSTDIR\YrdceTradeApp.exe"
 
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\长三角商品现货行情客户端\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\长三角商品现货行情客户端\Uninstall.lnk" "$INSTDIR\uninst.exe"


SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd



Function LaunchLink
    ExecShell "" "$INSTDIR\YrdceTradeApp.exe"
FunctionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  

  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"

  Delete "$SMPROGRAMS\长三角商品现货行情客户端\Uninstall.lnk"
  Delete "$SMPROGRAMS\长三角商品现货行情客户端\Website.lnk"
  Delete "$SMPROGRAMS\长三角商品现货行情客户端\长三角商品现货行情客户端.lnk"
	Delete  "$desktop\长三角商品现货行情客户端.lnk"

  RMDir "$SMPROGRAMS\长三角商品现货行情客户端"

  RMDir /r "$INSTDIR\log"
  RMDir /r "$INSTDIR\images"
  RMDir /r "$INSTDIR\Function"
  RMDir /r "$INSTDIR\ErrLog"
	RMDir /r /REBOOTOK $INSTDIR

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
