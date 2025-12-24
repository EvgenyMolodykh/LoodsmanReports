<?xml version="1.0" encoding="windows-1251"?>
<?component error="false" debug="false"?>
<component> 

<registration description="EventHandler" progid="progid" version="1.00" classid="{5A5136B9-52E3-407C-8F6A-59BF5A632069}">
</registration>

<public>

   	<method name="Run" dispid="1">
		<PARAMETER name="las"/>
		<PARAMETER name="params"/>
		<PARAMETER name="returnCode"/>
		<PARAMETER name="errorMessage"/>
		<PARAMETER name="flowDirectionCode"/>
	</method>

</public>
 
<script language="VBScript">
<![CDATA[

' при создании документа в ЛОЦМАН, его атрибуты «Дата создания», «Разработал» заполняются автоматически
function Run(las, params, returnCode, errorMessage, flowDirectionCode)  
  Const p_name = 0, p_value = 1
  Dim ds, ds2
  Set ds = CreateObject("DataProvider.Dataset")
  ds.Data = las.GetInfoAboutType(params(1, p_value), 1, returnCode, errorMessage)
   
  ' Сначала посмотрим, можно ли созданному объекту задать данные атрибуты
  If ds.Locate("_NAME", "Разработал", False, False) or ds.Locate("_NAME", "Дата создания", False, False) Then
    ' Поищем существующие атрибуты, уже заданные атрибуты затирать не будем
    Set ds2 = CreateObject("DataProvider.Dataset")
    ds2.Data = las.GetInfoAboutVersion("", "", "", params(0, p_value), 2, returnCode, errorMessage)
    ' Дата создания
    If ds.Locate("_NAME", "Дата создания", False, False) and not ds2.Locate("_NAME", "Дата создания", False, False) Then
      las.UpAttrValueById params(0, p_value), "Дата создания", Date, "", False, returnCode, errorMessage
    End If
    ' Разработал
    If ds.Locate("_NAME", "Разработал", False, False) and not ds2.Locate("_NAME", "Разработал", False, False) Then
      ds2.Data = las.GetInfoAboutCurrentUser(returnCode, errorMessage)
      If ds2.RecordCount > 0 Then
        If ds2.FieldValue("_FULLNAME") <> "" Then
	  las.UpAttrValueById params(0, p_value), "Разработал", ds2.FieldValue("_FULLNAME"), "", False, returnCode, errorMessage
	Else
          las.UpAttrValueById params(0, p_value), "Разработал", ds2.FieldValue("_NAME"), "", False, returnCode, errorMessage		
	End If
      End If
    End If
  End If
end function
 
]]>
</script>
 
</component>
