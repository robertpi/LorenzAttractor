REM run me as admin

REM See here for more details: http://msdn.microsoft.com/en-us/library/ms733768(v=vs.110).aspx

netsh http delete urlacl url=http://+:1000/
netsh http add urlacl url=http://+:1000/ user=%USERDOMAIN%\%USERNAME%

pause