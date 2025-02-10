sc create "YTG Temp Manager Service" binPath= "%~dp0YTG.TempManager.exe" start= delayed-auto

sc description "YTG Temp Manager Service" "Manage date style folders in the temp directory."

pause
