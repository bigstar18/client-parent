; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "��������Ʒ�ֻ�����ͻ���"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "Yrdce"
!define PRODUCT_WEB_SITE "http://www.Yrdce.com"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"


SetCompressor lzma

; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"

; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "..\..\Pictures\logo.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!define MUI_FINISHPAGE_RUN_TEXT "���� ��������Ʒ�ֻ�����ͻ���"
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"


; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!insertmacro MUI_PAGE_FINISH



; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS



;��װ��������


; ------ MUI �ִ����涨����� ------



Section
SectionEnd

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "��������Ʒ�ֻ�����ͻ��˰�װ��.exe"
InstallDir "$PROGRAMFILES\��������Ʒ�ֻ�����ͻ���"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File /r "D:\YrdceClient\*.*"
  CreateDirectory "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���"
  CreateShortCut "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���\��������Ʒ�ֻ�����ͻ���.lnk" "$INSTDIR\YrdceTradeApp.exe"
  
  CreateShortCut "$desktop\��������Ʒ�ֻ�����ͻ���.lnk" "$INSTDIR\YrdceTradeApp.exe"
 
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���\Uninstall.lnk" "$INSTDIR\uninst.exe"


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
 *  �����ǰ�װ�����ж�ز���  *
 ******************************/

Section Uninstall
  

  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"

  Delete "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���\Uninstall.lnk"
  Delete "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���\Website.lnk"
  Delete "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���\��������Ʒ�ֻ�����ͻ���.lnk"
	Delete  "$desktop\��������Ʒ�ֻ�����ͻ���.lnk"

  RMDir "$SMPROGRAMS\��������Ʒ�ֻ�����ͻ���"

  RMDir /r "$INSTDIR\log"
  RMDir /r "$INSTDIR\images"
  RMDir /r "$INSTDIR\Function"
  RMDir /r "$INSTDIR\ErrLog"
	RMDir /r /REBOOTOK $INSTDIR

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  SetAutoClose true
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷʵҪ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش����ļ�����Ƴ���"
FunctionEnd
